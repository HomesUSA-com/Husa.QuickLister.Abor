namespace Husa.Quicklister.Abor.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.ShowingTime.Models;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using CompanyServiceSubscriptionFilter = Husa.CompanyServicesManager.Api.Contracts.Request.FilterServiceSubscriptionRequest;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.SaleListings;

    public class SaleListingService : ExtensionsServices.SaleListingService<SaleListing, IListingSaleRepository, ICommunitySaleRepository, SaleProperty, CommunitySale>, ISaleListingService
    {
        private readonly IPlanRepository planRepository;
        private readonly ExtensionsInterfaces.ISaleListingMediaService listingMediaService;
        private readonly ISaleListingPhotoService saleListingPhotoService;
        private readonly IXmlClient xmlClient;
        private readonly Husa.Quicklister.Extensions.Crosscutting.FeatureFlags featureFlags;

        private readonly IEnumerable<MarketStatuses> statusAllowedForReleaseXmlListing = new[]
        {
            MarketStatuses.Closed,
            MarketStatuses.Canceled,
        };

        public SaleListingService(
            IListingSaleRepository listingSaleRepository,
            ICommunitySaleRepository communitySaleRepository,
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            ExtensionsInterfaces.ISaleListingMediaService listingMediaService,
            ISaleListingPhotoService saleListingPhotoService,
            ExtensionsInterfaces.ILockLegacyListingService lockLegacyListingService,
            IXmlClient xmlClient,
            ExtensionsInterfaces.ISaleListingLockService unlockService,
            IOptions<ApplicationOptions> applicationOptions,
            IMapper mapper,
            ILogger<SaleListingService> logger)
            : base(listingSaleRepository, communitySaleRepository, lockLegacyListingService, serviceSubscriptionClient, unlockService, logger, userContextProvider, mapper)
        {
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.saleListingPhotoService = saleListingPhotoService ?? throw new ArgumentNullException(nameof(saleListingPhotoService));
            this.featureFlags = applicationOptions?.Value?.FeatureFlags ?? throw new ArgumentNullException(nameof(applicationOptions));
        }

        private static IEnumerable<MarketStatuses> StatusesThatAllowDuplicates => new[] { MarketStatuses.Canceled, MarketStatuses.Closed };

        public async Task<CommandSingleResult<Guid, string>> CreateAsync(QuickCreateListingDto listingSale)
        {
            var companyServices = await this.ServiceSubscriptionClient.Company.GetCompanyServices(
                listingSale.CompanyId,
                new CompanyServiceSubscriptionFilter { ServiceCode = new[] { ServiceCode.XMLImport } });

            if (companyServices.Total > 0 && !listingSale.IsManuallyManaged)
            {
                return CommandSingleResult<Guid, string>.Error("Listings are managed by company's XML data.");
            }

            var importFromListing = listingSale.ListingIdToImport.HasValue && !listingSale.ListingIdToImport.Equals(Guid.Empty);
            var commandResult = await this.QuickCreateAsync(listingSale, importFromListing);
            if (commandResult.HasErrors())
            {
                return CommandSingleResult<Guid, string>.Error(commandResult.Message);
            }

            var listingResult = await this.ListingRepository.AddAsync(commandResult.Result);
            if (importFromListing)
            {
                await this.listingMediaService.CopyMediaAsync(listingSale.ListingIdToImport.Value, listingResult.Id);
            }

            this.Logger.LogInformation("ABOR Listing Sale successfully created Id: {listingId}", listingResult.Id);

            return CommandSingleResult<Guid, string>.Success(listingResult.Id);
        }

        public async Task<CommandResult<SaleListing>> QuickCreateAsync(QuickCreateListingDto listingSale, bool importFromListing)
        {
            this.Logger.LogInformation("ABOR Listing Sale Service starting create listing with Address : {StreetNumber} {StreetName}", listingSale.StreetNumber, listingSale.StreetName);
            var listing = await this.ListingRepository.GetListing(listingSale.StreetNumber, listingSale.StreetName, listingSale.City, listingSale.ZipCode, listingSale.UnitNumber);

            if (listing is not null && !StatusesThatAllowDuplicates.Contains(listing.MlsStatus))
            {
                this.Logger.LogInformation("listing {address} already exists!", listing.SaleProperty.AddressInfo.FormalAddress);
                return CommandResult<SaleListing>.Error($"listing {listing.SaleProperty.AddressInfo.FormalAddress} already exists!", listing);
            }

            var company = await this.ServiceSubscriptionClient.Company.GetCompany(listingSale.CompanyId);
            var listingSaleEntity = new SaleListing(
                listingSale.MlsStatus,
                listingSale.StreetName,
                listingSale.StreetNumber,
                listingSale.UnitNumber,
                listingSale.City,
                listingSale.State,
                listingSale.ZipCode,
                listingSale.County,
                listingSale.StreetType,
                listingSale.ConstructionCompletionDate,
                company.Id,
                company.Name,
                listingSale.CommunityId,
                listingSale.PlanId,
                listingSale.IsManuallyManaged)
            {
                LegacyId = listingSale.LegacyId,
            };

            if (importFromListing)
            {
                await this.ImportDataFromListingAsync(listingSaleEntity, listingSale.ListingIdToImport.Value);
            }
            else
            {
                await this.ImportDataFromCommunityAndPlan(listingSaleEntity, listingSale);
            }

            return CommandResult<SaleListing>.Success(listingSaleEntity);
        }

        public async Task UpdateListing(Guid listingId, SaleListingDto listingDto, bool migrateFullListing = true)
        {
            this.Logger.LogInformation("Starting update sale listing with id {listingId}", listingId);
            var listingAddress = listingDto.SaleProperty.AddressInfo;
            var existingListing = await this.ListingRepository.GetListing(
                listingAddress.StreetNumber,
                listingAddress.StreetName,
                listingAddress.City,
                listingAddress.ZipCode,
                listingAddress.UnitNumber);

            if (existingListing is not null && existingListing.Id != listingId && !StatusesThatAllowDuplicates.Contains(existingListing.MlsStatus))
            {
                this.Logger.LogInformation("{address} already exists!", existingListing.SaleProperty.AddressInfo.FormalAddress);
                throw new InvalidOperationException($"{existingListing.SaleProperty.AddressInfo.FormalAddress} already exists!");
            }

            var listingSale = await this.ListingRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var company = await this.ServiceSubscriptionClient.Company.GetCompany(listingSale.CompanyId) ?? throw new NotFoundException<SaleListing>(listingSale.CompanyId);
            await this.UpdateBaseListingInfo(listingDto, Guid.Empty, listingSale, migrateFullListing);

            var statusFieldsInfo = this.mapper.Map<ListingStatusFieldsInfo>(listingDto.StatusFieldsInfo);
            listingSale.SetMigrateFullListing(migrateFullListing);
            listingSale.SaleProperty.SetMigrateFullListing(migrateFullListing);
            listingSale.UpdateStatusFieldsInfo(statusFieldsInfo);
            listingSale.UseShowingTime = listingDto.UseShowingTime;

            if (!migrateFullListing)
            {
                var salepropertyInfo = this.mapper.Map<SalePropertyValueObject>(listingDto.SaleProperty);
                listingSale.SaleProperty.FillSalesPropertyInformation(salepropertyInfo);
            }
            else
            {
                await this.UpdateListingSaleProperty(listingSale, listingDto.SaleProperty, isBlockedSquareFootage: company.MlsInfo.BlockSquareFootage);
            }

            await this.UpdateRooms(listingDto.SaleProperty.Rooms, entity: listingSale);
            await this.UpdateOpenHouse(listingDto.SaleProperty.OpenHouses, entity: listingSale);
            await this.UpdateShowingTime(listingDto.ShowingTime, entity: listingSale);
            await this.listingMediaService.ResizeMedia(listingDto.Id, [ImageSize.HD]);

            await this.ListingRepository.UpdateAsync(listingSale);
        }

        public async Task UpdateShowingTime(ShowingTimeDto showingTimeDto, SaleListing entity = null)
        {
            entity = await this.GetEntity(entity);
            this.Logger.LogInformation("Starting update open house information for listing with id {listingId}", entity.Id);
            if (entity.UseShowingTime && showingTimeDto is not null)
            {
                var showingTime = this.mapper.Map<ShowingTime>(showingTimeDto);
                entity.UpdateShowingTime(showingTime);
            }
        }

        public async Task UpdateBaseListingInfo(SaleListingDto saleListingDto, Guid listingId = default, SaleListing entity = null, bool migrateFullListing = true)
        {
            this.Logger.LogInformation("Starting update base sale listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);
            var currentUser = this.UserContextProvider.GetCurrentUser();

            if (migrateFullListing)
            {
                entity.UpdateBaseListingInfo(
                    saleListingDto.ListType,
                    saleListingDto.ListPrice,
                    saleListingDto.ExpirationDate,
                    saleListingDto.ListDate,
                    saleListingDto.MlsStatus,
                    LockedStatus.LockedNotSubmitted,
                    currentUser.Id);
            }

            entity.UpdateManuallyManagement(saleListingDto.IsManuallyManaged);
        }

        public async Task UpdatePropertyInfo(PropertyDto propertyDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update property information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var property = this.mapper.Map<PropertyInfo>(propertyDto);
            entity.SaleProperty.UpdatePropertyInfo(property);
        }

        public async Task UpdateAddressInfo(SaleAddressDto addressDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update address information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var address = this.mapper.Map<SaleAddressInfo>(addressDto);
            entity.SaleProperty.UpdateAddressInfo(address);
        }

        public async Task UpdateFeaturesInfo(FeaturesDto featuresDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update features information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var features = this.mapper.Map<FeaturesInfo>(featuresDto);
            entity.SaleProperty.UpdateFeatures(features);
        }

        public async Task UpdateFinancialInfo(FinancialDto financialDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update financial information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var financial = this.mapper.Map<FinancialInfo>(financialDto);
            entity.SaleProperty.UpdateFinancial(financial);
        }

        public async Task UpdateSchoolsInfo(Models.SchoolsDto schoolsDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update schools information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var schools = this.mapper.Map<SchoolsInfo>(schoolsDto);
            entity.SaleProperty.UpdateSchools(schools);
        }

        public async Task UpdateSpacesDimensionsInfo(SpacesDimensionsDto spacesDimensionsDto, Guid listingId = default, SaleListing entity = null, bool isBlockedSquareFootage = false)
        {
            this.Logger.LogInformation("Starting update spaces and dimensions  information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var spacesDimensions = this.mapper.Map<SpacesDimensionsInfo>(spacesDimensionsDto);
            entity.SaleProperty.UpdateSpacesDimensions(spacesDimensions, !isBlockedSquareFootage || this.UserContextProvider.GetCurrentUser().IsMLSAdministrator);
        }

        public async Task UpdateShowingInfo(ShowingDto showingDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting update showing information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var showing = this.mapper.Map<ShowingInfo>(showingDto);
            entity.SaleProperty.UpdateShowing(showing);
        }

        public async Task UpdateRooms(IEnumerable<RoomDto> roomDto, Guid listingId = default, SaleListing entity = null)
        {
            this.Logger.LogInformation("Starting rooms information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var rooms = this.mapper.Map<ICollection<ListingSaleRoom>>(roomDto);
            entity.SaleProperty.UpdateRooms(rooms);
        }

        public async Task UpdateOpenHouse(IEnumerable<OpenHouseDto> openHouseDto, SaleListing entity = null)
        {
            entity = await this.GetEntity(entity);
            this.Logger.LogInformation("Starting update open house information for listing with id {listingId}", entity.Id);

            var openHouses = this.mapper.Map<ICollection<SaleListingOpenHouse>>(openHouseDto);
            entity.SaleProperty.UpdateOpenHouse(openHouses);
        }

        public async Task<SaleListing> GetEntity(SaleListing entity = null, Guid listingId = default)
        {
            if (entity is not null || listingId == Guid.Empty)
            {
                return entity;
            }

            return await this.ListingRepository.GetById(listingId, filterByCompany: true);
        }

        public async Task AssignMlsNumberAsync(Guid listingId, string mlsNumber, MarketStatuses requestStatus, ActionType actionType)
        {
            var listingSale = await this.ListingRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var listingWithMlsNumber = await this.ListingRepository.GetListingByMlsNumber(listingId, mlsNumber);
            var xmlListingId = listingSale.XmlListingId ?? listingSale.XmlDiscrepancyListingId ?? Guid.Empty;
            if (this.statusAllowedForReleaseXmlListing.Contains(listingSale.MlsStatus) && xmlListingId != Guid.Empty)
            {
                listingSale.XmlListingId = null;
                listingSale.XmlDiscrepancyListingId = null;
                await this.xmlClient.Listing.ReleaseListingRelationship(xmlListingId);
            }

            if (listingWithMlsNumber is not null)
            {
                throw new DomainException($"Duplicate MLS # {mlsNumber}. It is already assigned to {listingWithMlsNumber.SaleProperty.AddressInfo.FormalAddress}");
            }

            var mlsNumberWasEmpty = string.IsNullOrWhiteSpace(listingSale.MlsNumber);
            listingSale.CompleteListingRequest(mlsNumber, this.UserContextProvider.GetCurrentUserId(), requestStatus, actionType, this.featureFlags.IsDownloaderEnabled);

            await this.ListingRepository.SaveChangesAsync(listingSale);

            if (mlsNumberWasEmpty)
            {
                await this.UpdatePhotoRequestProperty(listingSale);
            }
        }

        public async Task<SaleListing> SaveListingChanges(Guid listingId, ListingSaleRequestDto listingSaleDto)
        {
            var listingSale = await this.ListingRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);

            var statusFieldsInfo = this.mapper.Map<ListingStatusFieldsInfo>(listingSaleDto.StatusFieldsInfo);

            listingSale.UpdateBaseListingInfo(
                listingSaleDto.ListType,
                listingSaleDto.ListPrice,
                listingSaleDto.ExpirationDate,
                listingSaleDto.ListDate,
                listingSaleDto.MlsStatus,
                LockedStatus.LockedNotSubmitted,
                this.UserContextProvider.GetCurrentUserId());
            listingSale.UpdateStatusFieldsInfo(statusFieldsInfo);
            listingSale.UseShowingTime = listingSaleDto.UseShowingTime;

            await this.UpdateListingSaleProperty(listingSale, listingSaleDto.SaleProperty);
            await this.UpdateRooms(listingSaleDto.SaleProperty.Rooms, entity: listingSale);
            await this.UpdateOpenHouse(listingSaleDto.SaleProperty.OpenHouses, entity: listingSale);
            await this.UpdateShowingTime(listingSaleDto.ShowingTime, entity: listingSale);

            await this.ListingRepository.SaveChangesAsync(listingSale);
            return listingSale;
        }

        public override async Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken cancellationToken = default)
        {
            var listing = await this.ListingRepository.GetById(listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            listing.UpdateActionType(actionType);
            await this.ListingRepository.SaveChangesAsync(listing);
        }

        protected override void ImportListingInfoToPlan(SaleListing listingSale)
        {
            var plan = listingSale.SaleProperty.Plan;
            if (plan is null)
            {
                this.Logger.LogInformation("Plan not found for listing sale with Id: {listingSaleId}. Skipping info import.", listingSale.Id);
                return;
            }

            plan.ImportFromSaleProperty(listingSale.SaleProperty, updateRooms: false, overwriteFieldsOnlyIfNull: true);
        }

        protected override async Task UpdatePhotoRequestProperty(SaleListing listing)
        {
            if (listing.LastPhotoRequestCreationDate.HasValue)
            {
                await this.saleListingPhotoService.SendUpdatePropertiesMessages(new[] { listing });
            }
        }

        protected override async Task UpdateCommunity(SaleListing listing, Guid communityId, bool filterByUserContext = true)
        {
            var community = await this.communityRepository.GetById(communityId, filterByCompany: filterByUserContext) ?? throw new NotFoundException<CommunitySale>(communityId);

            if (listing.SaleProperty.CompanyId != community.CompanyId)
            {
                this.Logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{CompanyId}'", communityId, community.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            listing.SaleProperty.CommunityId = communityId;
        }

        protected override async Task UpdatePlan(SaleListing listing, Guid planId, bool updateRooms = false, bool filterByUserContext = true)
        {
            var plan = await this.planRepository.GetById(planId, filterByCompany: filterByUserContext) ?? throw new NotFoundException<Plan>(planId);

            if (listing.SaleProperty.CompanyId != plan.CompanyId)
            {
                this.Logger.LogInformation("The selected Plan with id: {planId} was not found for the company id: '{companyId}'", planId, plan.CompanyId);
                throw new NotFoundException<Plan>(planId);
            }

            listing.SaleProperty.UpdateRoomsInfoFromPlan(plan, listing.Id, updateRooms);

            listing.SaleProperty.PlanId = planId;
        }

        protected override Task AutomaticReverseProspect()
        {
            throw new NotImplementedException();
        }

        private async Task ImportDataFromCommunityAndPlan(SaleListing listingSaleEntity, QuickCreateListingDto listingSale)
        {
            await this.ImportCommunityDataAsync(listingSaleEntity, listingSale.CommunityId);

            if (listingSale.County.HasValue)
            {
                listingSaleEntity.SaleProperty.AddressInfo.County = listingSale.County;
            }

            if (listingSale.PlanId != null && !listingSale.PlanId.Equals(Guid.Empty))
            {
                await this.ImportPlanDataAsync(listingSaleEntity, listingSale.PlanId.Value);
            }
        }

        private async Task UpdateListingSaleProperty(SaleListing listing, SalePropertyDetailDto salePropertyDto, bool isBlockedSquareFootage = false)
        {
            listing.SaleProperty.UpdateBaseInfo(salePropertyDto.SalePropertyInfo?.OwnerName);
            await this.UpdatePropertyInfo(salePropertyDto.PropertyInfo, entity: listing);
            await this.UpdateAddressInfo(salePropertyDto.AddressInfo, entity: listing);
            await this.UpdateShowingInfo(salePropertyDto.ShowingInfo, entity: listing);
            await this.UpdateSchoolsInfo(salePropertyDto.SchoolsInfo, entity: listing);
            await this.UpdateFeaturesInfo(salePropertyDto.FeaturesInfo, entity: listing);
            await this.UpdateFinancialInfo(salePropertyDto.FinancialInfo, entity: listing);
            await this.UpdateSpacesDimensionsInfo(salePropertyDto.SpacesDimensionsInfo, entity: listing, isBlockedSquareFootage: isBlockedSquareFootage);
        }

        private async Task ImportDataFromListingAsync(SaleListing listingSaleEntity, Guid listingIdToImport)
        {
            var listing = await this.ListingRepository.GetById(listingIdToImport) ?? throw new NotFoundException<SaleListing>(listingIdToImport);
            listingSaleEntity.CloneListing(listing);
        }

        private async Task ImportCommunityDataAsync(SaleListing listingSale, Guid? fromCommunityId)
        {
            var communityId = fromCommunityId ?? throw new ArgumentNullException(nameof(fromCommunityId));
            this.Logger.LogInformation("Starting import data from community with id: {communityId} to sale listing with id: {listingId}", communityId, listingSale.Id);
            var communitySale = await this.communityRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            if (listingSale.SaleProperty.CompanyId != communitySale.CompanyId)
            {
                this.Logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{companyId}'", communityId, communitySale.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            listingSale.SaleProperty.ImportDataFromCommunity(communitySale);
            listingSale.ImportShowingTimeInfo(communitySale);
        }

        private async Task ImportPlanDataAsync(SaleListing listingSale, Guid planId)
        {
            this.Logger.LogInformation("Starting import data from plan with id: {planId} to sale listing with id: {listingId}", planId, listingSale.Id);
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);

            if (listingSale.SaleProperty.CompanyId != plan.CompanyId)
            {
                this.Logger.LogInformation("The selected Plan with id: {planId} was not found for the company id: '{companyId}'", planId, plan.CompanyId);
                throw new NotFoundException<Plan>(planId);
            }

            listingSale.SaleProperty.ImportDataFromPlan(plan);
        }
    }
}
