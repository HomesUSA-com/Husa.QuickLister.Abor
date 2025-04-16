namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions.XML;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Extensions;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.SaleListings;
    using XmlContract = Husa.Xml.Domain.Enums;

    public class SaleListingXmlService : ExtensionsServices.SaleListingXmlService<
        SaleListing,
        CommunitySale,
        IListingSaleRepository,
        ICommunitySaleRepository>, ISaleListingXmlService
    {
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly ExtensionsInterfaces.ISaleListingMediaService saleListingMediaService;
        private readonly IMapper mapper;
        private readonly ApplicationOptions options;
        private readonly ISaleListingService listingSaleService;
        private readonly IListingRequestXmlService<XmlListingDetailResponse> saleListingRequestService;
        private readonly ISaleListingXmlMediaService xmlMediaService;
        private readonly IRequestErrorRepository requestErrorRepository;

        private readonly IEnumerable<MarketStatuses> notAlowedStatusForRequest = new[]
        {
            MarketStatuses.Canceled,
        };

        public SaleListingXmlService(
            IXmlClientWithoutToken xmlClient,
            IListingSaleRepository listingSaleRepository,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            ILogger<SaleListingXmlService> logger,
            ISaleListingXmlMediaService xmlMediaService,
            ISaleListingService listingSaleService,
            IListingRequestXmlService<XmlListingDetailResponse> saleListingRequestService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ExtensionsInterfaces.ISaleListingMediaService saleListingMediaService,
            IRequestErrorRepository requestErrorRepository,
            IOptions<ApplicationOptions> options,
            IMapper mapper)
            : base(listingSaleRepository, communityRepository, userContextProvider, xmlClient, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.saleListingMediaService = saleListingMediaService ?? throw new ArgumentNullException(nameof(saleListingMediaService));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.xmlMediaService = xmlMediaService ?? throw new ArgumentNullException(nameof(xmlMediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.requestErrorRepository = requestErrorRepository ?? throw new ArgumentNullException(nameof(requestErrorRepository));
        }

        public async Task<Guid> ProcessListingAsync(Guid xmlListingId, ImportActionType listAction)
        {
            var xmlListing = await this.GetListingFromXml(xmlListingId);
            this.Logger.LogInformation("Process xml listing with id {xmlListingId}, action type: {actionType}", xmlListingId, listAction);
            var currentUser = this.UserContextProvider.GetCurrentUser();
            if (!currentUser.IsMLSAdministrator && xmlListing.CompanyId != currentUser.CompanyId)
            {
                throw new DomainException($"The user '{currentUser.Name}' cannot import the listing {xmlListing.Street1} because the current company doesn't match the listing's company");
            }

            var importMedia = false;
            var listing = await this.ListingSaleRepository.GetListingByLocationAsync(null, xmlListing.StreetNum, xmlListing.StreetName, xmlListing.Zip, xmlListing.UnitIndicator);
            var community = await this.CommunitySaleRepository.GetById(xmlListing.CommunityId.Value, filterByCompany: false) ?? throw new NotFoundException<CommunitySale>(xmlListing.CommunityId.Value);
            var companyDetail = await this.serviceSubscriptionClient.Company.GetCompany(xmlListing.CompanyId.Value) ?? throw new NotFoundException<CompanyDetail>(xmlListing.CompanyId);

            if (listing == null || xmlListing.ImportWithoutRematch)
            {
                var listingSaleDto = this.mapper.Map<QuickCreateListingDto>(xmlListing);
                var quickCreateResult = await this.listingSaleService.QuickCreateAsync(listingSaleDto, importFromListing: false);

                if (quickCreateResult.Code == ResponseCode.Error)
                {
                    throw new DomainException(quickCreateResult.Message);
                }

                listing = quickCreateResult.Results.Single();
                this.ListingSaleRepository.Attach(listing);
                importMedia = true;
                listing.ImportFromXml(xmlListing, companyName: companyDetail.Name, listAction, currentUser.Id, community);
            }
            else
            {
                if (listing.MlsStatus == MarketStatuses.Closed)
                {
                    this.Logger.LogWarning("The listing could not be updated because is an sold listing {listingId}", listing.Id);
                    return listing.Id;
                }

                listing.UpdateFromXml(xmlListing, ignoreRequestByCompletionDate: companyDetail.SettingInfo.IgnoreRequestByCompletionDate);
                listing.LockByUser(currentUser.Id);
            }

            await this.ListingSaleRepository.SaveChangesAsync(listing);
            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = listAction.ToXmlListActionType(), ImportMedia = importMedia || !companyDetail.SettingInfo.StopXMLMediaSyncOfExistingListings });
            return listing.Id;
        }

        public async Task UpdateListingFromXmlAsync(Guid xmlListingId)
        {
            var mediaLimitAllowed = this.options.MediaAllowed.SaleListingMaxAllowedMedia;
            var listing = await this.ListingSaleRepository.GetListingByXmlListingId(xmlListingId) ?? throw new NotFoundException<SaleListing>(xmlListingId);
            var skipPlanAndCommunity = listing.XmlDiscrepancyListingId != null && listing.XmlDiscrepancyListingId.Value != Guid.Empty;
            var xmlListing = await this.GetListingFromXml(skipPlanAndCommunity ? listing.XmlDiscrepancyListingId.Value : xmlListingId, skipPlanAndCommunity: skipPlanAndCommunity);
            var currentUser = this.UserContextProvider.GetCurrentUser();
            var companyDetail = await this.serviceSubscriptionClient.Company.GetCompany(xmlListing.CompanyId.Value) ?? throw new NotFoundException<CompanyDetail>(xmlListing.CompanyId.Value);
            this.Logger.LogInformation("Updating the xml listing with id {xmlListingId}", xmlListingId);

            if (listing.IsManuallyManaged)
            {
                this.Logger.LogWarning("The listing: {listingId} is configured to be manually managed, skipping", listing.Id);
                return;
            }

            if (!listing.IsInMls)
            {
                this.Logger.LogWarning("The listing could not be updated because there is an open listing request {listingId}", listing.Id);
                return;
            }

            if (this.notAlowedStatusForRequest.Contains(listing.MlsStatus))
            {
                this.Logger.LogWarning("The listing could not be updated because is an {listing.MlsStatus} listing {listingId}", listing.MlsStatus.ToString(), listing.Id);
                return;
            }

            var shouldProcessNewMedia = !companyDetail.SettingInfo.StopXMLMediaSyncOfExistingListings;
            if (shouldProcessNewMedia)
            {
                var currentListingMedia = await this.saleListingMediaService.MediaClient.GetResources(listing.Id, MediaType.Residential);
                var newMediaFromXml = await this.XmlClient.Listing.Media(xmlListingId, excludeImported: true);
                if (newMediaFromXml != null && newMediaFromXml.Any() && currentListingMedia.Media.Count() < mediaLimitAllowed)
                {
                    await this.xmlMediaService.ImportListingMedia(
                        xmlListingId,
                        checkMediaLimit: true,
                        maxImagesAllowed: mediaLimitAllowed,
                        useServiceBus: true);
                }
            }

            listing.UpdateFromXml(xmlListing, ignoreRequestByCompletionDate: companyDetail.SettingInfo.IgnoreRequestByCompletionDate);

            var requestResult = listing.GenerateRequest(currentUser.Id);
            if (requestResult.Errors.Any())
            {
                listing.LockUnsubmitted(currentUser.Id);
                await this.ListingSaleRepository.SaveChangesAsync(listing);
                this.Logger.LogWarning("The listing request could not be created due to the following: {@errors}", requestResult.Errors);
                var errors = string.Join(", ", requestResult.Errors.Select(x => x.ErrorMessage));
                await this.requestErrorRepository.AddRequestError(listing.Id, $"The listing request could not be created due to the following: {errors}", ImportSource.Xml);
                return;
            }

            var requestResponse = await this.saleListingRequestService.CreateRequestAsync(listing, xmlListing, ignoreRequestByCompletionDate: companyDetail.SettingInfo.IgnoreRequestByCompletionDate);
            if (requestResponse.Code == ResponseCode.Success)
            {
                listing.LockByUser(currentUser.Id);
                await this.ListingSaleRepository.SaveChangesAsync(listing);
            }

            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = XmlContract.ListActionType.ListUpdate });
        }
    }
}
