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
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Extensions;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.SaleListings;
    using XmlContract = Husa.Xml.Domain.Enums;

    public class SaleListingXmlService : ExtensionsServices.SaleListingXmlService<
        SaleListing,
        CommunitySale,
        IListingSaleRepository,
        ICommunitySaleRepository>, ISaleListingXmlService
    {
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IMapper mapper;
        private readonly ApplicationOptions options;
        private readonly ISaleListingService listingSaleService;
        private readonly ISaleListingRequestService saleListingRequestService;
        private readonly ISaleListingXmlMediaService xmlMediaService;

        private readonly IEnumerable<MarketStatuses> notAlowedStatusForRequest = new[]
        {
            MarketStatuses.Canceled,
        };

        public SaleListingXmlService(
            IXmlClient xmlClient,
            IListingSaleRepository listingSaleRepository,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            ILogger<SaleListingXmlService> logger,
            ISaleListingXmlMediaService xmlMediaService,
            ISaleListingService listingSaleService,
            ISaleListingRequestService saleListingRequestService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IOptions<ApplicationOptions> options,
            IMapper mapper)
            : base(listingSaleRepository, communityRepository, userContextProvider, xmlClient, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.xmlMediaService = xmlMediaService ?? throw new ArgumentNullException(nameof(xmlMediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
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
            var listing = await this.ListingSaleRepository.GetListingByLocationAsync(null, xmlListing.StreetNum, xmlListing.StreetName, xmlListing.Zip);
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

                listing.UpdateFromXml(
                    xmlListing,
                    userId: currentUser.Id,
                    ignoreRequestByCompletionDate: companyDetail.SettingInfo.IgnoreRequestByCompletionDate);
                if (this.ListingSaleRepository.HasXmlChanges(listing))
                {
                    listing.LockByUser(currentUser.Id);
                }
            }

            await this.ListingSaleRepository.SaveChangesAsync(listing);
            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = listAction.ToXmlListActionType(), ImportMedia = importMedia || !companyDetail.SettingInfo.StopXMLMediaSyncOfExistingListings });
            return listing.Id;
        }

        public async Task UpdateListingFromXmlAsync(Guid xmlListingId)
        {
            var listing = await this.ListingSaleRepository.GetListingByXmlListingId(xmlListingId) ?? throw new NotFoundException<SaleListing>(xmlListingId);
            var skipPlanAndCommunity = listing.XmlDiscrepancyListingId != null && listing.XmlDiscrepancyListingId.Value != Guid.Empty;
            var xmlListing = await this.GetListingFromXml(skipPlanAndCommunity ? listing.XmlDiscrepancyListingId.Value : xmlListingId, skipPlanAndCommunity: skipPlanAndCommunity);
            var currentUser = this.UserContextProvider.GetCurrentUser();
            this.Logger.LogInformation("Updating the xml listing with id {xmlListingId}", xmlListingId);
            var hasOpenRequest = await this.saleListingRequestService.HasOpenRequest(listing.Id);
            if (listing.IsManuallyManaged)
            {
                this.Logger.LogWarning("The listing: {listingId} is configured to be manually managed, skipping", listing.Id);
                return;
            }

            if (!listing.IsInMls || hasOpenRequest)
            {
                this.Logger.LogWarning("The listing could not be updated because there is an open listing request {listingId}", listing.Id);
                return;
            }

            if (this.notAlowedStatusForRequest.Contains(listing.MlsStatus))
            {
                this.Logger.LogWarning("The listing could not be updated because is an {listing.MlsStatus} listing {listingId}", listing.MlsStatus.ToString(), listing.Id);
                return;
            }

            var companyDetail = await this.serviceSubscriptionClient.Company.GetCompany(xmlListing.CompanyId.Value) ?? throw new NotFoundException<CompanyDetail>(xmlListing.CompanyId.Value);
            var shouldProcessNewMedia = !companyDetail.SettingInfo.StopXMLMediaSyncOfExistingListings;
            listing.UpdateFromXml(
                xmlListing,
                currentUser.Id,
                ignoreRequestByCompletionDate: companyDetail.SettingInfo.IgnoreRequestByCompletionDate);
            var mediaHasChanges = false;

            if (shouldProcessNewMedia)
            {
                var newMediaFromXml = await this.XmlClient.Listing.Media(xmlListingId, excludeImported: true);
                if (newMediaFromXml != null && newMediaFromXml.Any())
                {
                    await this.xmlMediaService.ImportListingMedia(
                        xmlListingId,
                        checkMediaLimit: true,
                        maxImagesAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia,
                        useServiceBus: false);
                    mediaHasChanges = true;
                }
            }

            if (this.ListingSaleRepository.HasXmlChanges(listing) || mediaHasChanges)
            {
                listing.LockByUser(currentUser.Id);
                await this.ListingSaleRepository.SaveChangesAsync(listing);
                var requestResult = listing.GenerateRequest(currentUser.Id);
                if (!requestResult.Errors.Any())
                {
                    await this.saleListingRequestService.GenerateRequestFromXmlAsync(requestResult.Result);
                }
                else
                {
                    this.Logger.LogWarning("The listing request could not be created due to the following: {@errors}", requestResult.Errors);
                    var errors = string.Join(", ", requestResult.Errors.Select(x => x.ErrorMessage));
                    await this.ListingSaleRepository.AddXmlRequestError(listing.Id, $"The listing request could not be created due to the following: {errors}");
                    return;
                }
            }

            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = XmlContract.ListActionType.ListUpdate });
        }
    }
}
