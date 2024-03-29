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
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;

    public class ListingRequestMigrationService : ExtensionsServices.ListingRequestMigrationService<SaleListing, IListingSaleRepository>
    {
        private readonly IMapper mapper;
        private readonly ISaleListingRequestService saleListingRequestService;
        private readonly ISaleListingRequestRepository saleListingRequestRepository;
        private readonly IAgentRepository agentRepository;
        private readonly ISaleListingMigrationService listingMigrationService;

        public ListingRequestMigrationService(
            IListingSaleRepository listingRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ISaleListingRequestService saleListingRequestService,
            ISaleListingRequestRepository saleListingRequestRepository,
            IUserContextProvider userContextProvider,
            IAgentRepository agentRepository,
            ISaleListingMigrationService listingMigrationService,
            ILogger<ListingRequestMigrationService> logger,
            IMapper mapper)
            : base(listingRepository, serviceSubscriptionClient, userContextProvider, migrationClient, logger)
        {
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.saleListingRequestRepository = saleListingRequestRepository ?? throw new ArgumentNullException(nameof(saleListingRequestRepository));
            this.listingMigrationService = listingMigrationService ?? throw new ArgumentNullException(nameof(listingMigrationService));
        }

        protected override MigrationMarketType MigrationMarket => MigrationMarketType.Austin;

        protected override async Task GenerateRequestAsync(Guid currentUserId, SaleListing listing, SaleListingRequestResponse legacyRequest, bool updateRequest = false)
        {
            if (!updateRequest &&
                (await this.saleListingRequestRepository.GetByLegacyIdAsync(legacyRequest.LegacyListingRequestId)) != null)
            {
                return;
            }

            var request = this.mapper.Map<SaleListingRequest>(legacyRequest);
            request.Id = Guid.NewGuid();
            request.UpdateLegacyInformation(currentUserId, legacyRequest.LegacyListingRequestId, listing);

            request.StatusFieldsInfo.AgentId = await this.GetAgentIdByMarketUniqueId(legacyRequest.StatusFieldsInfo.AgentId);
            request.StatusFieldsInfo.AgentIdSecond = await this.GetAgentIdByMarketUniqueId(legacyRequest.StatusFieldsInfo.AgentIdSecond);

            var usersIds = await this.GetMigrationUserIds(legacyRequest);
            request.SysCreatedBy = usersIds.SysCreatedBy ?? request.SysCreatedBy;
            request.SysModifiedBy = usersIds.SysModifiedBy ?? request.SysModifiedBy;
            request.PublishInfo.PublishUser = usersIds.PublishUserId ?? request.PublishInfo.PublishUser;

            await this.saleListingRequestService.GenerateRequestFromMigrationAsync(request);
            await this.UpdateListingInformation(listing, legacyRequest);
        }

        protected Task UpdateListingInformation(SaleListing listing, SaleListingRequestResponse legacyRequest)
        {
            if (listing.LockedStatus == LockedStatus.NoLocked && legacyRequest.SysModifiedOn > listing.SysModifiedOn)
            {
                legacyRequest.IsXmlManaged = !listing.IsManuallyManaged;
                return this.listingMigrationService.UpdateListing(listing, legacyRequest);
            }

            return Task.CompletedTask;
        }

        private async Task<Guid?> GetAgentIdByMarketUniqueId(string marketUniqueId)
        {
            if (string.IsNullOrWhiteSpace(marketUniqueId))
            {
                return null;
            }

            var agent = await this.agentRepository.GetAgentByMlsId(marketUniqueId);

            return agent?.Id;
        }
    }
}
