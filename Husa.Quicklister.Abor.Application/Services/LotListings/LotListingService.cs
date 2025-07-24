namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Extensions.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services;

    public class LotListingService : ExtensionsServices.ListingService<LotListing, ILotListingRepository>, ILotListingService
    {
        private readonly IMapper mapper;
        private readonly ICommunitySaleRepository communityRepository;
        private readonly ILotListingMediaService listingMediaService;
        private readonly Husa.Quicklister.Extensions.Crosscutting.FeatureFlags featureFlags;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        public LotListingService(
            ILotListingRepository lotListingRepository,
            ICommunitySaleRepository communityRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IUserContextProvider userContextProvider,
            ILotListingMediaService listingMediaService,
            ILotListingLockService unlockService,
            IOptions<ApplicationOptions> applicationOptions,
            IMapper mapper,
            ILogger<LotListingService> logger)
             : base(lotListingRepository, logger, userContextProvider, unlockService)
        {
            this.communityRepository = communityRepository ?? throw new ArgumentNullException(nameof(communityRepository));
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.featureFlags = applicationOptions?.Value?.FeatureFlags ?? throw new ArgumentNullException(nameof(applicationOptions));
        }

        private static IEnumerable<MarketStatuses> StatusesThatAllowDuplicates => new[] { MarketStatuses.Canceled, MarketStatuses.Closed };

        public async Task<CommandSingleResult<Guid, string>> CreateAsync(QuickCreateListingDto lotListing)
        {
            var importFromListing = lotListing.ListingIdToImport.HasValue && !lotListing.ListingIdToImport.Equals(Guid.Empty);
            var commandResult = await this.QuickCreateAsync(lotListing, importFromListing);
            if (commandResult.HasErrors())
            {
                return CommandSingleResult<Guid, string>.Error(commandResult.Message);
            }

            var listingResult = await this.ListingRepository.AddAsync(commandResult.Result);
            if (importFromListing)
            {
                await this.listingMediaService.CopyMediaAsync(lotListing.ListingIdToImport.Value, listingResult.Id);
            }

            this.Logger.LogInformation("ABOR Lot Listing successfully created Id: {listingId}", listingResult.Id);

            return CommandSingleResult<Guid, string>.Success(listingResult.Id);
        }

        public async Task<CommandResult<LotListing>> QuickCreateAsync(QuickCreateListingDto lotListing, bool importFromListing)
        {
            this.Logger.LogInformation("ABOR Lot Listing Service starting create listing with Address : {StreetNumber} {StreetName}", lotListing.StreetNumber, lotListing.StreetName);
            var existingListings = await this.ListingRepository.GetListingsByAddress(lotListing, StatusesThatAllowDuplicates);

            if (existingListings.Any())
            {
                var formalAddress = lotListing.GetFormalAddress();
                this.Logger.LogInformation("listing {address} already exists!", formalAddress);
                return CommandResult<LotListing>.Error($"listing {formalAddress} already exists!", existingListings);
            }

            var company = await this.serviceSubscriptionClient.Company.GetCompany(lotListing.CompanyId);
            var lotListingEntity = new LotListing(
                lotListing.MlsStatus,
                lotListing.StreetName,
                lotListing.StreetNumber,
                lotListing.City,
                lotListing.State,
                lotListing.ZipCode,
                company.Id,
                company.Name,
                lotListing.County,
                lotListing.CommunityId)
            {
                LegacyId = lotListing.LegacyId,
            };

            if (importFromListing)
            {
                await this.ImportDataFromListingAsync(lotListingEntity, lotListing.ListingIdToImport.Value);
            }
            else
            {
                await this.ImportDataFromCommunityAsync(lotListingEntity, lotListing.CommunityId);

                if (lotListing.County.HasValue)
                {
                    lotListingEntity.AddressInfo.County = lotListing.County;
                }
            }

            return CommandResult<LotListing>.Success(lotListingEntity);
        }

        public async Task UpdateListing(Guid listingId, LotListingDto listingDto)
        {
            this.Logger.LogInformation("Starting update sale listing with id {listingId}", listingId);
            var existingListings = await this.ListingRepository.GetListingsByAddress(listingDto.AddressInfo, StatusesThatAllowDuplicates);

            if (existingListings.Any() && (existingListings.Count() > 1 || existingListings.Single().Id != listingId))
            {
                var formalAddress = listingDto.AddressInfo.GetFormalAddress();
                this.Logger.LogInformation("{address} already exists!", formalAddress);
                throw new InvalidOperationException($"{formalAddress} already exists!");
            }

            var currentUser = this.UserContextProvider.GetCurrentUser();
            var lotListing = await this.ListingRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<LotListing>(listingId);

            lotListing.UpdateBaseListingInfo(
                listingDto.ListType,
                listingDto.ListPrice,
                listingDto.ExpirationDate,
                listingDto.ListDate,
                listingDto.MlsStatus,
                LockedStatus.LockedNotSubmitted,
                currentUser.Id);
            lotListing.UpdateManuallyManagement(listingDto.IsManuallyManaged);

            var property = this.mapper.Map<LotPropertyInfo>(listingDto.PropertyInfo);
            lotListing.UpdatePropertyInfo(property);
            var address = this.mapper.Map<LotAddressInfo>(listingDto.AddressInfo);
            lotListing.UpdateAddressInfo(address);
            var features = this.mapper.Map<LotFeaturesInfo>(listingDto.FeaturesInfo);
            lotListing.UpdateFeatures(features);
            var financial = this.mapper.Map<LotFinancialInfo>(listingDto.FinancialInfo);
            lotListing.UpdateFinancial(financial);
            var schools = this.mapper.Map<LotSchoolsInfo>(listingDto.SchoolsInfo);
            lotListing.UpdateSchools(schools);
            var showing = this.mapper.Map<LotShowingInfo>(listingDto.ShowingInfo);
            lotListing.UpdateShowing(showing);
            var statusFieldsInfo = this.mapper.Map<ListingStatusFieldsInfo>(listingDto.StatusFieldsInfo);
            lotListing.UpdateStatusFieldsInfo(statusFieldsInfo);

            lotListing.OwnerName = listingDto.OwnerName;

            await this.ListingRepository.SaveChangesAsync(lotListing);
        }

        public async Task AssignMlsNumberAsync(Guid listingId, string mlsNumber, MarketStatuses requestStatus, ActionType actionType)
        {
            var lotListing = await this.ListingRepository.GetById(listingId, filterByCompany: true) ?? throw new NotFoundException<LotListing>(listingId);
            var listingWithMlsNumber = await this.ListingRepository.GetListingByMlsNumber(mlsNumber);

            if (listingWithMlsNumber is not null && listingWithMlsNumber.Id != listingId)
            {
                throw new DomainException($"Duplicate MLS # {mlsNumber}. It is already assigned to {listingWithMlsNumber.AddressInfo.FormalAddress}");
            }

            lotListing.CompleteListingRequest(mlsNumber, this.UserContextProvider.GetCurrentUserId(), requestStatus, actionType, this.featureFlags.IsDownloaderEnabled);

            await this.ListingRepository.SaveChangesAsync(lotListing);
        }

        public async Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken cancellationToken = default)
        {
            var lotListing = await this.ListingRepository.GetById(listingId) ?? throw new NotFoundException<LotListing>(listingId);
            lotListing.UpdateActionType(actionType);
            await this.ListingRepository.SaveChangesAsync(lotListing);
        }

        protected override Task UpdatePhotoRequestProperty(LotListing listing)
        {
            throw new NotImplementedException();
        }

        protected override async Task UpdateCommunity(LotListing listing, Guid communityId, bool filterByUserContext = true)
        {
            var community = await this.communityRepository.GetById(communityId, filterByCompany: filterByUserContext) ?? throw new NotFoundException<CommunitySale>(communityId);

            if (listing.CompanyId != community.CompanyId)
            {
                this.Logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{CompanyId}'", communityId, community.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            listing.CommunityId = communityId;
        }

        private async Task ImportDataFromListingAsync(LotListing lotListingEntity, Guid listingIdToImport)
        {
            var listing = await this.ListingRepository.GetById(listingIdToImport) ?? throw new NotFoundException<LotListing>(listingIdToImport);
            lotListingEntity.ImportDataFromListing(listing);
        }

        private async Task ImportDataFromCommunityAsync(LotListing lotListing, Guid? fromCommunityId)
        {
            var communityId = fromCommunityId ?? throw new ArgumentNullException(nameof(fromCommunityId));
            this.Logger.LogInformation("Starting import data from community with id: {communityId} to sale listing with id: {listingId}", communityId, lotListing.Id);
            var communitySale = await this.communityRepository.GetById(communityId, filterByCompany: true) ?? throw new NotFoundException<CommunitySale>(communityId);
            if (lotListing.CompanyId != communitySale.CompanyId)
            {
                this.Logger.LogInformation("The selected Community with id: {communityId} was not found for the company id: '{companyId}'", communityId, communitySale.CompanyId);
                throw new NotFoundException<CommunitySale>(communityId);
            }

            lotListing.ImportDataFromCommunity(communitySale);
        }
    }
}
