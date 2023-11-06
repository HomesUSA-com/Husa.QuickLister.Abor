namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Xml;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using XmlContract = Husa.Xml.Domain.Enums;

    public class SaleListingXmlService : ISaleListingXmlService
    {
        private readonly IXmlClient xmlClient;
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;
        private readonly IMapper mapper;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ISaleListingService listingSaleService;
        private readonly IUserContextProvider userContextProvider;
        private readonly ICommunitySaleRepository communitySaleRepository;
        private readonly ISaleListingRequestService saleListingRequestService;
        private readonly IXmlMediaService xmlMediaService;
        private readonly ILogger<SaleListingXmlService> logger;

        public SaleListingXmlService(
            IMapper mapper,
            IXmlClient xmlClient,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IListingSaleRepository listingSaleRepository,
            ISaleListingService listingSaleService,
            ICommunitySaleRepository communitySaleRepository,
            ISaleListingRequestService saleListingRequestService,
            IUserContextProvider userContextProvider,
            IXmlMediaService xmlMediaService,
            ILogger<SaleListingXmlService> logger)
        {
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.communitySaleRepository = communitySaleRepository ?? throw new ArgumentNullException(nameof(communitySaleRepository));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.saleListingRequestService = saleListingRequestService ?? throw new ArgumentNullException(nameof(saleListingRequestService));
            this.xmlMediaService = xmlMediaService ?? throw new ArgumentNullException(nameof(xmlMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> ProcessListingAsync(Guid xmlListingId, ListActionType listAction)
        {
            var xmlListing = await this.GetListingFromXml(xmlListingId);
            this.logger.LogInformation("Process xml listing with id {xmlListingId}, action type: {actionType}", xmlListingId, listAction);
            var currentUser = this.userContextProvider.GetCurrentUser();
            if (!currentUser.IsMLSAdministrator && xmlListing.CompanyId != currentUser.CompanyId)
            {
                throw new DomainException($"The user '{currentUser.Name}' cannot import the listing {xmlListing.Street1} because the current company doesn't match the listing's company");
            }

            var importMedia = false;
            var listing = await this.listingSaleRepository.GetListingByLocationAsync(null, xmlListing.StreetNum, xmlListing.StreetName, xmlListing.Zip);
            if (listing == null)
            {
                var listingSaleDto = this.mapper.Map<ListingSaleDto>(xmlListing);
                var quickCreateResult = await this.listingSaleService.QuickCreateAsync(listingSaleDto, importFromListing: false);

                if (quickCreateResult.Code == ResponseCode.Error)
                {
                    throw new DomainException(quickCreateResult.Message);
                }

                listing = quickCreateResult.Results.Single();
                this.listingSaleRepository.Attach(listing);
                importMedia = true;
                var companyDetail = await this.serviceSubscriptionClient.Company.GetCompany(xmlListing.CompanyId.Value);
                listing.ImportFromXml(xmlListing, companyName: companyDetail.Name, listAction, currentUser.Id);
            }
            else
            {
                if (listing.MlsStatus == MarketStatuses.Closed)
                {
                    this.logger.LogWarning("The listing could not be updated because is an sold listing {listingId}", listing.Id);
                    return listing.Id;
                }

                listing.UpdateFromXml(xmlListing, userId: currentUser.Id);
            }

            await this.listingSaleRepository.SaveChangesAsync(listing);
            await this.xmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = GetXmlListActionType(listAction), ImportMedia = importMedia });
            return listing.Id;
        }

        public async Task UpdateListingFromXmlAsync(Guid xmlListingId)
        {
            var listing = await this.listingSaleRepository.GetListingByXmlListingId(xmlListingId) ?? throw new NotFoundException<SaleListing>(xmlListingId);
            var skipPlanAndCommunity = listing.XmlDiscrepancyListingId != null && listing.XmlDiscrepancyListingId.Value != Guid.Empty;
            var xmlListing = await this.GetListingFromXml(skipPlanAndCommunity ? listing.XmlDiscrepancyListingId.Value : xmlListingId, skipPlanAndCommunity: skipPlanAndCommunity);
            var currentUser = this.userContextProvider.GetCurrentUser();
            this.logger.LogInformation("Updating the xml listing with id {xmlListingId}", xmlListingId);
            var hasOpenRequest = await this.saleListingRequestService.HasOpenRequest(listing.Id);
            if (listing.IsManuallyManaged)
            {
                this.logger.LogWarning("The listing: {listingId} is configured to be manually managed, skipping", listing.Id);
                return;
            }

            if (!listing.IsInMls || hasOpenRequest)
            {
                this.logger.LogWarning("The listing could not be updated because there is an open listing request {listingId}", listing.Id);
                return;
            }

            if (listing.MlsStatus == MarketStatuses.Closed)
            {
                this.logger.LogWarning("The listing could not be updated because is an sold listing {listingId}", listing.Id);
                return;
            }

            listing.UpdateFromXml(xmlListing, currentUser.Id);

            await this.xmlMediaService.ImportListingMedia(xmlListingId, checkMediaLimit: true, useServiceBus: false);

            var requestResult = listing.GenerateRequest(currentUser.Id);
            if (requestResult.Errors.Any())
            {
                this.logger.LogWarning("The listing request could not be created due to the following: {@errors}", requestResult.Errors);
                return;
            }

            await this.listingSaleRepository.SaveChangesAsync(listing);
            await this.saleListingRequestService.GenerateRequestFromXmlAsync(requestResult.Result);
            await this.xmlClient.Listing.ProcessListing(xmlListingId, request: new() { ListingId = listing.Id, Type = XmlContract.ListActionType.ListUpdate });
        }

        public async Task AutoMatchListingFromXmlAsync(Guid xmlListingId)
        {
            var currentUser = this.userContextProvider.GetCurrentUser();
            var xmlListing = await this.GetListingFromXml(xmlListingId, skipPlanAndCommunity: true);
            var saleListing = await this.listingSaleRepository.GetListingByXmlListingId(xmlListingId);

            if (saleListing != null)
            {
                this.logger.LogInformation("Xml Listing with id: {xmlListingId} was already imported in Quicklister DB with id: {listingId}", xmlListingId, saleListing.Id);
                return;
            }

            var isMatched = await ExactMatch();
            if (isMatched != MatchStatus.Matched)
            {
                isMatched = await PartialMatch();
            }

            await this.listingSaleRepository.SaveChangesAsync();
            await this.xmlClient.Listing.ChangeMatchStatus(xmlListingId, request: new() { Status = GetXmlMatchStatus(isMatched) });

            async Task<MatchStatus> ExactMatch()
            {
                this.logger.LogInformation("starting full match listings for listing with id: {xmlListingId}", xmlListingId);
                var matchingListings = await this.listingSaleRepository
                    .GetAutmaticMatchingListingsAsync(xmlListing.StreetName, xmlListing.StreetNum, xmlListing.Zip, xmlListing.CompanyId.Value, partialMatch: false);
                var isMatched = MatchStatus.NotMatched;

                if (matchingListings != null && matchingListings.Any())
                {
                    var matchedListing = matchingListings.First();
                    this.logger.LogInformation("Match found between xmlListingId: {xmlListingId} with {listingId}:", xmlListingId, matchedListing.Id);
                    matchedListing.MatchFromXml(xmlListingId, currentUser.Id);
                    isMatched = MatchStatus.Matched;
                }

                return isMatched;
            }

            async Task<MatchStatus> PartialMatch()
            {
                this.logger.LogInformation("Making partial match listings for listing with id: {xmlListingId}", xmlListingId);
                var matchingListings = await this.listingSaleRepository
                    .GetAutmaticMatchingListingsAsync(xmlListing.StreetName, xmlListing.StreetNum, xmlListing.Zip, xmlListing.CompanyId.Value, partialMatch: true);
                if (matchingListings != null && matchingListings.Any())
                {
                    this.logger.LogInformation("Partial match for {total} listings was found for xmlListingId: {xmlListingId}", matchingListings.Count(), xmlListingId);
                    foreach (var listing in matchingListings)
                    {
                        listing.LockByUser(currentUser.Id);
                    }
                }

                return MatchStatus.NotMatched;
            }
        }

        public async Task ListLaterAsync(Guid xmlListingId, DateTime listOn)
        {
            this.logger.LogInformation("Mark listing with id {xmlListingId} as list later xml listing", xmlListingId);
            var xmlListing = await this.GetListingFromXml(xmlListingId);
            await this.xmlClient.Listing.ProcessListing(xmlListing.Id, new ListActionRequest
            {
                Type = XmlContract.ListActionType.ListLater,
                ListOn = listOn,
            });
        }

        public async Task DeleteListingAsync(Guid xmlListingId)
        {
            this.logger.LogInformation("Delete xml listing with id {xmlListingId}", xmlListingId);
            var xmlListing = await this.GetListingFromXml(xmlListingId);
            await this.xmlClient.Listing.ProcessListing(xmlListing.Id, new ListActionRequest
            {
                Type = XmlContract.ListActionType.ListNever,
            });
        }

        public async Task RestoreListingAsync(Guid xmlListingId)
        {
            this.logger.LogInformation("Restore xml listing with id {xmlListingId}", xmlListingId);
            var xmlListing = await this.GetListingFromXml(xmlListingId);
            await this.xmlClient.Listing.RestoreListing(xmlListing.Id);
        }

        private static XmlContract.ListActionType GetXmlListActionType(ListActionType type) => type switch
        {
            ListActionType.ListNow => XmlContract.ListActionType.ListNow,
            ListActionType.ListCompare => XmlContract.ListActionType.ListCompare,
            ListActionType.ListLater => XmlContract.ListActionType.ListLater,
            ListActionType.ListNever => XmlContract.ListActionType.ListNever,
            ListActionType.ListUpdate => XmlContract.ListActionType.ListUpdate,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };

        private static XmlContract.MatchStatus GetXmlMatchStatus(MatchStatus type) => type switch
        {
            MatchStatus.Matched => XmlContract.MatchStatus.Matched,
            MatchStatus.NotMatched => XmlContract.MatchStatus.NotMatched,
            MatchStatus.AwaitingMatch => XmlContract.MatchStatus.AwaitingMatch,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };

        private async Task<XmlListingDetailResponse> GetListingFromXml(Guid xmlListingId, bool skipPlanAndCommunity = false)
        {
            var xmlListing = await this.xmlClient.Listing.GetByIdAsync(xmlListingId, skipPlanAndCommunity) ?? throw new NotFoundException<XmlListingDetailResponse>(xmlListingId);
            var currentUser = this.userContextProvider.GetCurrentUser();

            if (xmlListing.Market != MarketCode.Austin)
            {
                throw new DomainException($"Listing {xmlListing.Street1} is not for '{MarketCode.Austin}' market");
            }

            if (xmlListing.CompanyId == null)
            {
                throw new DomainException($"Listing {xmlListing.Street1} does not have company id");
            }

            if (!currentUser.IsMLSAdministrator && xmlListing.CompanyId != currentUser.CompanyId)
            {
                throw new DomainException($"The user '{currentUser.Name}' cannot perform this action on the listing {xmlListing.Street1} because the user's company doesn't match the listing's company");
            }

            if (currentUser.UserRole == UserRole.User && currentUser.EmployeeRole == RoleEmployee.SalesEmployee &&
                (!xmlListing.CommunityId.HasValue || !this.communitySaleRepository.IsCommunityEmployee(currentUser.Id, xmlListing.CommunityId.Value)))
            {
                throw new InvalidOperationException($"The user '{currentUser.Name}' cannot perform this action on the xml listing {xmlListingId}");
            }

            return xmlListing;
        }
    }
}
