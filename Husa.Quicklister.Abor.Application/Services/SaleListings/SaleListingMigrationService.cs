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
    using PhotoRequest = Husa.PhotoService.Api.Contracts.Request;

    public class SaleListingMigrationService : ExtensionsServices.SaleListingMigrationService<SaleListing, IListingSaleRepository, ICommunitySaleRepository, IPlanRepository, ISaleListingPhotoService>, ISaleListingMigrationService
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
            ISaleListingPhotoService photoService,
            ILogger<SaleListingMigrationService> logger,
            IMapper mapper)
            : base(listingRepository, serviceSubscriptionClient, migrationClient, communityRepository, planRepository, photoService, logger)
        {
            this.saleListingService = saleListingService ?? throw new ArgumentNullException(nameof(saleListingService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }

        protected override MigrationMarketType MigrationMarket => MigrationMarketType.Austin;

        public async override Task UpdateListing(SaleListing listing, SaleListingResponse legacyListing, bool migrateFullListing = true)
        {
            var listingDto = this.mapper.Map<SaleListingDto>(legacyListing);
            listingDto.Id = listing.Id;
            listingDto.ListType = listing.ListType;
            listingDto.MarketModifiedOn = listing.MarketModifiedOn;
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

            if (listingDto.SaleProperty.PropertyInfo.ConstructionCompletionDate == null)
            {
                listingDto.SaleProperty.PropertyInfo.ConstructionCompletionDate = listing.SaleProperty.PropertyInfo.ConstructionCompletionDate;
            }

            if (listingDto.SaleProperty.PropertyInfo.ConstructionStage == null)
            {
                listingDto.SaleProperty.PropertyInfo.ConstructionStage = listing.SaleProperty.PropertyInfo.ConstructionStage;
            }

            if (listingDto.SaleProperty.PropertyInfo.ConstructionStartYear == null)
            {
                listingDto.SaleProperty.PropertyInfo.ConstructionStartYear = listing.SaleProperty.PropertyInfo.ConstructionStartYear;
            }

            if (listingDto.ExpirationDate == null)
            {
                listingDto.ExpirationDate = listing.ExpirationDate;
            }

            await this.saleListingService.UpdateListing(listing.Id, listingDto);
            if (string.IsNullOrWhiteSpace(listing.MlsNumber) && !string.IsNullOrWhiteSpace(listingDto.MlsNumber))
            {
                await this.saleListingService.AssignMlsNumberAsync(listing.Id, listingDto.MlsNumber, listingDto.MlsStatus, ActionType.NewListing);
            }

            await this.saleListingService.UnlockListing(listing.Id);
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

        protected override PhotoRequest.Property GetPhotoRequestProperty(SaleListing listing)
        {
            return new PhotoRequest.Property()
            {
                Id = listing.Id,
                MlsNumber = listing.MlsNumber,
                Type = PhotoService.Domain.Enums.PropertyType.Residential,
                StreetName = listing.SaleProperty.AddressInfo.StreetName,
                StreetNum = listing.SaleProperty.AddressInfo.StreetNumber,
                UnitNumber = listing.SaleProperty.AddressInfo.UnitNumber,
                StreetType = listing.SaleProperty.AddressInfo.StreetType?.ToString(),
                Zip = listing.SaleProperty.AddressInfo.ZipCode,
                City = listing.SaleProperty.AddressInfo.City.ToString(),
                ReadableCity = listing.SaleProperty.AddressInfo.ReadableCity,
                Subdivision = listing.SaleProperty.AddressInfo.Subdivision,
            };
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
