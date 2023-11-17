namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Enums;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response.SaleListing;
    using Husa.Migration.Crosscutting.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.Migration;

    public class SaleListingMigrationService : ExtensionsServices.SaleListingMigrationService<SaleListing, IListingSaleRepository, ICommunitySaleRepository, IPlanRepository>
    {
        private readonly IMapper mapper;
        private readonly ISaleListingService saleListingService;
        private readonly IAgentRepository agentRepository;

        public SaleListingMigrationService(
            IListingSaleRepository listingRepository,
            IMigrationClient migrationClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ISaleListingService saleListingService,
            IAgentRepository agentRepository,
            IPlanRepository planRepository,
            ICommunitySaleRepository communityRepository,
            ILogger<SaleListingMigrationService> logger,
            IMapper mapper)
            : base(listingRepository, serviceSubscriptionClient, migrationClient, communityRepository, planRepository, logger)
        {
            this.saleListingService = saleListingService ?? throw new ArgumentNullException(nameof(saleListingService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }

        protected override MigrationMarketType MigrationMarket => MigrationMarketType.Austin;

        protected async override Task UpdateListing(SaleListing listing, SaleListingResponse legacyListing)
        {
            var listingDto = this.mapper.Map<SaleListingDto>(legacyListing);
            listingDto.Id = listing.Id;
            listingDto.ListType = listing.ListType;
            listingDto.MarketModifiedOn = listing.MarketModifiedOn;
            listingDto.IsManuallyManaged = listing.IsManuallyManaged;
            listingDto.SaleProperty.Id = listing.SaleProperty.Id;
            listingDto.SaleProperty.SalePropertyInfo.OwnerName = listing.SaleProperty.OwnerName;
            listingDto.SaleProperty.SalePropertyInfo.CompanyId = listing.SaleProperty.CompanyId;
            listingDto.SaleProperty.SalePropertyInfo.PlanId = listing.SaleProperty.PlanId;
            if (listing.SaleProperty.CommunityId.HasValue)
            {
                listingDto.SaleProperty.SalePropertyInfo.CommunityId = listing.SaleProperty.CommunityId.Value;
            }

            listingDto.StatusFieldsInfo.AgentId = await this.GetAgentIdByMarketUniqueId(legacyListing.StatusFieldsInfo.AgentId);
            listingDto.StatusFieldsInfo.AgentIdSecond = await this.GetAgentIdByMarketUniqueId(legacyListing.StatusFieldsInfo.AgentIdSecond);

            await this.saleListingService.UpdateListing(listing.Id, listingDto);
            if (!string.IsNullOrWhiteSpace(listingDto.MlsNumber))
            {
                await this.saleListingService.AssignMlsNumberAsync(listing.Id, listingDto.MlsNumber, listingDto.MlsStatus, ActionType.NewListing);
            }
        }

        protected async override Task<Guid?> CreateListing(Guid companyId, SaleListingResponse legacyListing)
        {
            var listingDto = await this.GetQuickCreateListingDto<ListingSaleDto, MarketStatuses, Cities, Counties>(companyId, legacyListing);
            var queryResponse = await this.saleListingService.CreateAsync(listingDto);

            if (queryResponse.Code == ResponseCode.Success)
            {
                return queryResponse.Result;
            }

            return null;
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
