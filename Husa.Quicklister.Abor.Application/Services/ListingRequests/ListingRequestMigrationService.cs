namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Crosscutting.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;

    public class ListingRequestMigrationService : ExtensionsServices.ListingRequestMigrationService<SaleListing, IListingSaleRepository>
    {
        private readonly IMapper mapper;
        private readonly ISaleListingRequestService saleListingRequestService;
        private readonly IAgentRepository agentRepository;

        public ListingRequestMigrationService(
            IListingSaleRepository listingRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ISaleListingRequestService saleListingRequestService,
            IUserContextProvider userContextProvider,
            IAgentRepository agentRepository,
            ILogger<ListingRequestMigrationService> logger,
            IMapper mapper)
            : base(listingRepository, serviceSubscriptionClient, userContextProvider, migrationClient, logger)
        {
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }

        protected override MigrationMarketType MigrationMarket => MigrationMarketType.Austin;

        protected override async Task GenerateRequestAsync(Guid currentUserId, SaleListing listing, SaleListingRequestResponse legacyRequest)
        {
            var request = this.mapper.Map<SaleListingRequest>(legacyRequest);
            request.Id = Guid.NewGuid();
            request.UpdateLegacyInformation(currentUserId, legacyRequest.LegacyListingRequestId, listing);

            request.StatusFieldsInfo.AgentId = await this.GetAgentIdByMarketUniqueId(legacyRequest.StatusFieldsInfo.AgentId);
            request.StatusFieldsInfo.AgentIdSecond = await this.GetAgentIdByMarketUniqueId(legacyRequest.StatusFieldsInfo.AgentIdSecond);

            var usersIds = await this.GetMigrationUserIds(legacyRequest);
            request.SysCreatedBy = usersIds.SysCreatedBy ?? request.SysCreatedBy;
            request.SysModifiedBy = usersIds.SysModifiedBy ?? request.SysModifiedBy;
            request.PublishInfo.PublishUser = usersIds.PublishUserId ?? request.PublishInfo.PublishUser;

            await this.saleListingRequestService.GenerateRequestFromMigrationAsync(request, currentUserId);
        }

        private async Task<Guid?> GetAgentIdByMarketUniqueId(string marketUniqueId)
        {
            if (string.IsNullOrWhiteSpace(marketUniqueId))
            {
                return null;
            }

            var agent = await this.agentRepository.GetAgentByMarketUniqueId(marketUniqueId);

            return agent?.Id;
        }
    }
}
