namespace Husa.Quicklister.Abor.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using CompanyServiceSubscriptionFilter = Husa.CompanyServicesManager.Api.Contracts.Request.FilterServiceSubscriptionRequest;

    public class SaleListingService : ISaleListingService
    {
        private readonly IMapper mapper;
        private readonly ISaleListingRequestRepository saleRequestRepository;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ICommunitySaleRepository communitySaleRepository;
        private readonly IPlanRepository planRepository;
        private readonly ILogger<SaleListingService> logger;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IUserContextProvider userContextProvider;
        private readonly ISaleListingMediaService listingMediaService;
        private readonly ISaleListingPhotoService saleListingPhotoService;
        private readonly FeatureFlags featureFlags;

        public SaleListingService(
            ISaleListingRequestRepository saleRequestRepository,
            IListingSaleRepository listingSaleRepository,
            ICommunitySaleRepository communitySaleRepository,
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            ISaleListingMediaService listingMediaService,
            ISaleListingPhotoService saleListingPhotoService,
            IOptions<ApplicationOptions> applicationOptions,
            IMapper mapper,
            ILogger<SaleListingService> logger)
        {
            this.saleRequestRepository = saleRequestRepository ?? throw new ArgumentNullException(nameof(saleRequestRepository));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.communitySaleRepository = communitySaleRepository ?? throw new ArgumentNullException(nameof(communitySaleRepository));
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.saleListingPhotoService = saleListingPhotoService ?? throw new ArgumentNullException(nameof(saleListingPhotoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.featureFlags = applicationOptions?.Value?.FeatureFlags ?? throw new ArgumentNullException(nameof(applicationOptions));
        }

        public async Task<CommandSingleResult<Guid, string>> CreateAsync(ListingSaleDto listingSale)
        {
            var companyServices = await this.serviceSubscriptionClient.Company.GetCompanyServices(
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

            var listingResult = await this.listingSaleRepository.AddAsync(commandResult.Result);
            if (importFromListing)
            {
                await this.listingMediaService.CopyMediaAsync(listingSale.ListingIdToImport.Value, listingResult.Id);
            }

            this.logger.LogInformation("ABOR Listing Sale successfully created Id: {listingId}", listingResult.Id);

            return CommandSingleResult<Guid, string>.Success(listingResult.Id);
        }

        public async Task<CommandResult<SaleListing>> QuickCreateAsync(ListingSaleDto listingSale, bool importFromListing)
        {
            this.logger.LogInformation("ABOR Listing Sale Service starting create listing with Address : {StreetNumber} {StreetName}", listingSale.StreetNumber, listingSale.StreetName);
            var listing = await this.listingSaleRepository.GetListing(listingSale.StreetNumber, listingSale.StreetName, listingSale.City, listingSale.ZipCode, listingSale.UnitNumber);

            if (listing is not null)
            {
                this.logger.LogInformation("listing {address} already exists!", listing.SaleProperty.AddressInfo.FormalAddress);
                return CommandResult<SaleListing>.Error($"listing {listing.SaleProperty.AddressInfo.FormalAddress} already exists!", listing);
            }

            var company = await this.serviceSubscriptionClient.Company.GetCompany(listingSale.CompanyId);
            var listingSaleEntity = new SaleListing(
                listingSale.MlsStatus,
                listingSale.StreetName,
                listingSale.StreetNumber,
                listingSale.UnitNumber,
                listingSale.City,
                listingSale.State,
                listingSale.ZipCode,
                listingSale.County,
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

        public async Task UpdateListing(Guid listingId, SaleListingDto listingDto)
        {
            this.logger.LogInformation("Starting update sale listing with id {listingId}", listingId);
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var listingAddress = listingDto.SaleProperty.AddressInfo;
            var listing = await this.listingSaleRepository.GetListing(
                listingAddress.StreetNumber,
                listingAddress.StreetName,
                listingAddress.City,
                listingAddress.ZipCode,
                listingAddress.UnitNumber);

            if (listing is not null && listing.Id != listingSale.Id)
            {
                this.logger.LogInformation("{address} already exists!", listing.SaleProperty.AddressInfo.FormalAddress);
                throw new InvalidOperationException($"{listing.SaleProperty.AddressInfo.FormalAddress} already exists!");
            }

            await this.UpdateBaseListingInfo(listingDto, Guid.Empty, listingSale);

            var statusFieldsInfo = this.mapper.Map<ListingSaleStatusFieldsInfo>(listingDto.StatusFieldsInfo);
            listingSale.UpdateStatusFieldsInfo(statusFieldsInfo);

            await this.UpdatePropertyInfo(listingDto.SaleProperty.PropertyInfo, entity: listingSale);
            await this.UpdateAddressInfo(listingDto.SaleProperty.AddressInfo, entity: listingSale);
            await this.UpdateShowingInfo(listingDto.SaleProperty.ShowingInfo, entity: listingSale);
            await this.UpdateSchoolsInfo(listingDto.SaleProperty.SchoolsInfo, entity: listingSale);
            await this.UpdateFeaturesInfo(listingDto.SaleProperty.FeaturesInfo, entity: listingSale);
            await this.UpdateFinancialInfo(listingDto.SaleProperty.FinancialInfo, entity: listingSale);
            await this.UpdateSpacesDimensionsInfo(listingDto.SaleProperty.SpacesDimensionsInfo, entity: listingSale);
            await this.UpdateRooms(listingDto.SaleProperty.Rooms, entity: listingSale);
            await this.UpdateOpenHouse(listingDto.SaleProperty.OpenHouses, entity: listingSale);

            await this.listingSaleRepository.UpdateAsync(listingSale);
        }

        public async Task DeleteListing(Guid listingId)
        {
            this.logger.LogInformation("Starting delete sale listing with id {listingId}", listingId);
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            listingSale.Delete(this.userContextProvider.GetCurrentUserId(), false);
            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task UpdateBaseListingInfo(SaleListingDto saleListingDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update base sale listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);
            var currentUser = this.userContextProvider.GetCurrentUser();

            entity.UpdateBaseListingInfo(
                saleListingDto.ListType,
                saleListingDto.ListPrice,
                saleListingDto.ExpirationDate,
                saleListingDto.ListDate,
                saleListingDto.MlsStatus,
                LockedStatus.LockedNotSubmitted,
                currentUser.Id);

            entity.UpdateManuallyManagement(saleListingDto.IsManuallyManaged);
        }

        public async Task UpdatePropertyInfo(PropertyDto propertyDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update property information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var property = this.mapper.Map<PropertyInfo>(propertyDto);
            entity.SaleProperty.UpdatePropertyInfo(property);
        }

        public async Task UpdateAddressInfo(AddressDto addressDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update address information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var address = this.mapper.Map<AddressInfo>(addressDto);
            entity.SaleProperty.UpdateAddressInfo(address);
        }

        public async Task UpdateFeaturesInfo(FeaturesDto featuresDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update features information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var features = this.mapper.Map<FeaturesInfo>(featuresDto);
            entity.SaleProperty.UpdateFeatures(features);
        }

        public async Task UpdateFinancialInfo(FinancialDto financialDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update financial information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var financial = this.mapper.Map<FinancialInfo>(financialDto);
            entity.SaleProperty.UpdateFinancial(financial);
        }

        public async Task UpdateSchoolsInfo(Models.SchoolsDto schoolsDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update schools information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var schools = this.mapper.Map<SchoolsInfo>(schoolsDto);
            entity.SaleProperty.UpdateSchools(schools);
        }

        public async Task UpdateSpacesDimensionsInfo(SpacesDimensionsDto spacesDimensionsDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update spaces and dimensions  information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var spacesDimensions = this.mapper.Map<SpacesDimensionsInfo>(spacesDimensionsDto);
            entity.SaleProperty.UpdateSpacesDimensions(spacesDimensions);
        }

        public async Task UpdateShowingInfo(ShowingDto showingDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting update showing information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var showing = this.mapper.Map<ShowingInfo>(showingDto);
            entity.SaleProperty.UpdateShowing(showing);
        }

        public async Task UpdateRooms(IEnumerable<RoomDto> roomDto, Guid listingId = default, SaleListing entity = null)
        {
            this.logger.LogInformation("Starting rooms information for listing with id {listingId}", listingId);
            entity = await this.GetEntity(entity, listingId);

            var rooms = this.mapper.Map<ICollection<ListingSaleRoom>>(roomDto);
            entity.SaleProperty.UpdateRooms(rooms);
        }

        public async Task UpdateOpenHouse(IEnumerable<OpenHouseDto> openHouseDto, SaleListing entity = null)
        {
            entity = await this.GetEntity(entity);
            this.logger.LogInformation("Starting update open house information for listing with id {listingId}", entity.Id);

            var openHouses = this.mapper.Map<ICollection<SaleListingOpenHouse>>(openHouseDto);
            entity.SaleProperty.UpdateOpenHouse(openHouses);
        }

        public async Task<SaleListing> GetEntity(SaleListing entity = null, Guid listingId = default)
        {
            if (entity is not null || listingId == Guid.Empty)
            {
                return entity;
            }

            return await this.listingSaleRepository.GetById(listingId, filterByCompany: true);
        }

        public async Task ChangeCommunity(Guid listingId, Guid communityId)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var community = await this.communitySaleRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);

            if (listingSale.SaleProperty.CompanyId != community.CompanyId)
            {
                this.logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{CompanyId}'", communityId, community.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            listingSale.SaleProperty.CommunityId = communityId;

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task ChangePlan(Guid listingId, Guid planId, bool updateRooms = false)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);

            if (listingSale.SaleProperty.CompanyId != plan.CompanyId)
            {
                this.logger.LogInformation("The selected Plan with id: {planId} was not found for the company id: '{companyId}'", planId, plan.CompanyId);
                throw new NotFoundException<Plan>(planId);
            }

            listingSale.SaleProperty.UpdateRoomsInfoFromPlan(plan, listingId, updateRooms);

            listingSale.SaleProperty.PlanId = planId;

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task AssignMlsNumberAsync(Guid listingId, string mlsNumber, MarketStatuses requestStatus, ActionType actionType)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);
            var listingWithMlsNumber = await this.listingSaleRepository.GetListingByMlsNumber(listingId, mlsNumber);

            if (listingWithMlsNumber is not null)
            {
                throw new DomainException($"Duplicate MLS # {mlsNumber}. It is already assigned to {listingWithMlsNumber.SaleProperty.AddressInfo.FormalAddress}");
            }

            var mlsNumberWasEmpty = string.IsNullOrWhiteSpace(listingSale.MlsNumber);
            listingSale.CompleteListingRequest(mlsNumber, this.userContextProvider.GetCurrentUserId(), requestStatus, actionType, this.featureFlags.IsDownloaderEnabled);

            if (mlsNumberWasEmpty && !listingSale.LastPhotoRequestCreationDate.HasValue)
            {
                await this.saleListingPhotoService.SendUpdatePropertiesMessages(new[] { listingSale });
            }

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task<SaleListing> SaveListingChanges(Guid listingId, ListingSaleRequestDto listingSaleDto)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);

            var statusFieldsInfo = this.mapper.Map<ListingSaleStatusFieldsInfo>(listingSaleDto.StatusFieldsInfo);

            listingSale.UpdateBaseListingInfo(
                listingSaleDto.ListType,
                listingSaleDto.ListPrice,
                listingSaleDto.ExpirationDate,
                listingSaleDto.ListDate,
                listingSaleDto.MlsStatus,
                LockedStatus.LockedNotSubmitted,
                this.userContextProvider.GetCurrentUserId());
            listingSale.UpdateStatusFieldsInfo(statusFieldsInfo);
            await this.UpdatePropertyInfo(listingSaleDto.SaleProperty.PropertyInfo, entity: listingSale);
            await this.UpdateAddressInfo(listingSaleDto.SaleProperty.AddressInfo, entity: listingSale);
            await this.UpdateShowingInfo(listingSaleDto.SaleProperty.ShowingInfo, entity: listingSale);
            await this.UpdateSchoolsInfo(listingSaleDto.SaleProperty.SchoolsInfo, entity: listingSale);
            await this.UpdateFeaturesInfo(listingSaleDto.SaleProperty.FeaturesInfo, entity: listingSale);
            await this.UpdateFinancialInfo(listingSaleDto.SaleProperty.FinancialInfo, entity: listingSale);
            await this.UpdateSpacesDimensionsInfo(listingSaleDto.SaleProperty.SpacesDimensionsInfo, entity: listingSale);
            await this.UpdateRooms(listingSaleDto.SaleProperty.Rooms, entity: listingSale);
            await this.UpdateOpenHouse(listingSaleDto.SaleProperty.OpenHouses, entity: listingSale);

            await this.listingSaleRepository.SaveChangesAsync(listingSale);
            return listingSale;
        }

        public async Task<CommandResult<string>> UnlockListing(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Trying to unlock Listing sale with id {listingId}.", listingId);
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);

            var currentUser = this.userContextProvider.GetCurrentUser();
            if (!listingSale.CanUnlock(currentUser))
            {
                this.logger.LogInformation("Listing sale {listingId} cannot be unlocked.", listingId);
                throw new DomainException($"Listing sale {listingId} cannot be unlocked.");
            }

            var existingRequest = await this.saleRequestRepository.CheckFirstListingRequestExistAsync(listingId, cancellationToken);

            if (!currentUser.IsMLSAdministrator && existingRequest)
            {
                this.logger.LogInformation("The sale listing {listingId} has an open request, cannot be unlocked.", listingId);
                return CommandResult<string>.Error($"The sale listing {listingId} has an open request, cannot be unlocked.");
            }

            listingSale.Unlock(this.featureFlags.AllowManualListingUnlock);
            await this.listingSaleRepository.SaveChangesAsync(listingSale);
            return CommandResult<string>.Success($"Unlocked listing sale with id {listingId}.");
        }

        public async Task LockListingByUser(Guid listingId)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<SaleListing>(listingId);

            listingSale.LockByUser(this.userContextProvider.GetCurrentUserId());
            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task DeclinePhotos(Guid listingId, CancellationToken cancellationToken = default)
        {
            var listingSale = await this.listingSaleRepository.GetById(listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            var currentUser = this.userContextProvider.GetCurrentUser();
            listingSale.DeclinePhotos(currentUser.Id);
            await this.listingSaleRepository.SaveChangesAsync(listingSale);
        }

        public async Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken cancellationToken = default)
        {
            var listing = await this.listingSaleRepository.GetById(listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            listing.UpdateActionType(actionType);
            await this.listingSaleRepository.SaveChangesAsync(listing);
        }

        private async Task ImportDataFromCommunityAndPlan(SaleListing listingSaleEntity, ListingSaleDto listingSale)
        {
            await this.ImportCommunityDataAsync(listingSaleEntity, listingSale.CommunityId);

            if (listingSale.PlanId != null && !listingSale.PlanId.Equals(Guid.Empty))
            {
                await this.ImportPlanDataAsync(listingSaleEntity, listingSale.PlanId.Value);
            }
        }

        private async Task ImportDataFromListingAsync(SaleListing listingSaleEntity, Guid listingIdToImport)
        {
            var listing = await this.listingSaleRepository.GetById(listingIdToImport) ?? throw new NotFoundException<SaleListing>(listingIdToImport);
            listingSaleEntity.CloneListing(listing);
        }

        private async Task ImportCommunityDataAsync(SaleListing listingSale, Guid? fromCommunityId)
        {
            var communityId = fromCommunityId ?? throw new ArgumentNullException(nameof(fromCommunityId));
            this.logger.LogInformation("Starting import data from community with id: {communityId} to sale listing with id: {listingId}", communityId, listingSale.Id);
            var communitySale = await this.communitySaleRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            if (listingSale.SaleProperty.CompanyId != communitySale.CompanyId)
            {
                this.logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{companyId}'", communityId, communitySale.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            listingSale.SaleProperty.ImportDataFromCommunity(communitySale);
        }

        private async Task ImportPlanDataAsync(SaleListing listingSale, Guid planId)
        {
            this.logger.LogInformation("Starting import data from plan with id: {planId} to sale listing with id: {listingId}", planId, listingSale.Id);
            var plan = await this.planRepository.GetById(planId, filterByCompany: true) ?? throw new NotFoundException<Plan>(planId);

            if (listingSale.SaleProperty.CompanyId != plan.CompanyId)
            {
                this.logger.LogInformation("The selected Plan with id: {planId} was not found for the company id: '{companyId}'", planId, plan.CompanyId);
                throw new NotFoundException<Plan>(planId);
            }

            listingSale.SaleProperty.ImportDataFromPlan(plan);
        }
    }
}
