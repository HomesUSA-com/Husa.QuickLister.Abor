namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
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
        private readonly ISaleListingService listingSaleService;
        private readonly ISaleListingRequestService saleListingRequestService;
        private readonly IXmlMediaService xmlMediaService;

        public SaleListingXmlService(
            IXmlClient xmlClient,
            IListingSaleRepository listingSaleRepository,
            ICommunitySaleRepository communityRepository,
            IUserContextProvider userContextProvider,
            ILogger<SaleListingXmlService> logger,
            IXmlMediaService xmlMediaService,
            ISaleListingService listingSaleService,
            ISaleListingRequestService saleListingRequestService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IMapper mapper)
            : base(listingSaleRepository, communityRepository, userContextProvider, xmlClient, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.xmlMediaService = xmlMediaService ?? throw new ArgumentNullException(nameof(xmlMediaService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> ProcessListingAsync(Guid xmlListingId, ListActionType listAction)
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
            if (listing == null)
            {
                var listingSaleDto = this.mapper.Map<ListingSaleDto>(xmlListing);
                var quickCreateResult = await this.listingSaleService.QuickCreateAsync(listingSaleDto, importFromListing: false);

                if (quickCreateResult.Code == ResponseCode.Error)
                {
                    throw new DomainException(quickCreateResult.Message);
                }

                listing = quickCreateResult.Results.Single();
                this.ListingSaleRepository.Attach(listing);
                importMedia = true;
                var companyDetail = await this.serviceSubscriptionClient.Company.GetCompany(xmlListing.CompanyId.Value);
                listing.ImportFromXml(xmlListing, companyName: companyDetail.Name, listAction, currentUser.Id);
            }
            else
            {
                if (listing.MlsStatus == MarketStatuses.Closed)
                {
                    this.Logger.LogWarning("The listing could not be updated because is an sold listing {listingId}", listing.Id);
                    return listing.Id;
                }

                listing.UpdateFromXml(xmlListing, userId: currentUser.Id);
            }

            await this.ListingSaleRepository.SaveChangesAsync(listing);
            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = GetXmlListActionType(listAction), ImportMedia = importMedia });
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

            if (listing.MlsStatus == MarketStatuses.Closed)
            {
                this.Logger.LogWarning("The listing could not be updated because is an sold listing {listingId}", listing.Id);
                return;
            }

            listing.UpdateFromXml(xmlListing, currentUser.Id);

            var newMediaFromXml = await this.XmlClient.Listing.Media(xmlListingId, excludeImported: true);

            if (this.ListingSaleRepository.HasXmlChanges(listing) || newMediaFromXml.Any())
            {
                await this.xmlMediaService.ImportListingMedia(xmlListingId, checkMediaLimit: true, useServiceBus: false);
                await this.ListingSaleRepository.SaveChangesAsync(listing);
                var requestResult = listing.GenerateRequest(currentUser.Id);
                if (!requestResult.Errors.Any())
                {
                    await this.saleListingRequestService.GenerateRequestFromXmlAsync(requestResult.Result);
                }
                else
                {
                    this.Logger.LogWarning("The listing request could not be created due to the following: {@errors}", requestResult.Errors);
                }
            }

            await this.XmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = XmlContract.ListActionType.ListUpdate });
        }

        public override XmlContract.MatchStatus GetXmlMatchStatus(MatchStatus type) => type switch
        {
            MatchStatus.Matched => XmlContract.MatchStatus.Matched,
            MatchStatus.NotMatched => XmlContract.MatchStatus.NotMatched,
            MatchStatus.AwaitingMatch => XmlContract.MatchStatus.AwaitingMatch,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };

        private static XmlContract.ListActionType GetXmlListActionType(ListActionType type) => type switch
        {
            ListActionType.ListNow => XmlContract.ListActionType.ListNow,
            ListActionType.ListCompare => XmlContract.ListActionType.ListCompare,
            ListActionType.ListLater => XmlContract.ListActionType.ListLater,
            ListActionType.ListNever => XmlContract.ListActionType.ListNever,
            ListActionType.ListUpdate => XmlContract.ListActionType.ListUpdate,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
    }
}
