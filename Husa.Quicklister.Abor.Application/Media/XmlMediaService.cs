namespace Husa.Quicklister.Abor.Application.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.MediaService.Domain.Enums;
    using Husa.MediaService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class XmlMediaService : IXmlMediaService
    {
        private readonly IXmlClient xmlClient;
        private readonly ICommunitySaleRepository communitySaleRepository;
        private readonly IPlanRepository planRepository;
        private readonly IUserContextProvider userContextProvider;
        private readonly IMediaMessagingService mediaMessagingService;
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly IProvideTraceId traceIdProvider;
        private readonly ISaleListingMediaService mediaService;
        private readonly ApplicationOptions options;
        private readonly ILogger<XmlMediaService> logger;

        public XmlMediaService(
            IXmlClient xmlClient,
            ICommunitySaleRepository communitySaleRepository,
            IPlanRepository planRepository,
            IUserContextProvider userContextProvider,
            IMediaMessagingService mediaMessagingService,
            IListingSaleRepository listingSaleRepository,
            IProvideTraceId traceIdProvider,
            ISaleListingMediaService mediaService,
            IOptions<ApplicationOptions> options,
            ILogger<XmlMediaService> logger)
        {
            this.xmlClient = xmlClient ?? throw new ArgumentNullException(nameof(xmlClient));
            this.communitySaleRepository = communitySaleRepository ?? throw new ArgumentNullException(nameof(communitySaleRepository));
            this.planRepository = planRepository ?? throw new ArgumentNullException(nameof(planRepository));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.mediaMessagingService = mediaMessagingService ?? throw new ArgumentNullException(nameof(mediaMessagingService));
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.traceIdProvider = traceIdProvider ?? throw new ArgumentNullException(nameof(traceIdProvider));
            this.mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ImportSubdivisionMedia(Guid xmlSubdivisionId)
        {
            var xmlSubdivision = await this.xmlClient.Subdivision.GetByIdAsync(xmlSubdivisionId);
            if (!xmlSubdivision.CommunityProfileId.HasValue)
            {
                throw new DomainException($"Cannot process the media import of the xml subdivision {xmlSubdivisionId} before it's imported");
            }

            var community = await this.communitySaleRepository.GetById(xmlSubdivision.CommunityProfileId.Value) ?? throw new NotFoundException<CommunitySale>(xmlSubdivision.CommunityProfileId.Value);
            this.logger.LogInformation("Processing request to import media from XML for subdivision for community {communityId}", community.Id);
            var subdivisionMedia = await this.xmlClient.Subdivision.Media(xmlSubdivisionId, excludeImported: true);
            if (!subdivisionMedia.Any())
            {
                this.logger.LogInformation("No media was found in the MLS to import for subdivision id {subdivision}", xmlSubdivisionId);
                return;
            }

            await this.BulkCreateAsync(community.Id, mediaType: MediaType.CommunityProfile, subdivisionMedia);
            await this.xmlClient.Subdivision.SubdivisionImagesImported(
                xmlSubdivisionId,
                request: new()
                {
                    ImageIds = subdivisionMedia.Select(m => m.Id),
                });
        }

        public async Task ImportPlanMedia(Guid xmlPlanId)
        {
            var xmlPlan = await this.xmlClient.Plan.GetByIdAsync(xmlPlanId);
            if (!xmlPlan.PlanProfileId.HasValue)
            {
                throw new DomainException($"Cannot process the media import of the xml plan {xmlPlanId} before it's imported");
            }

            var plan = await this.planRepository.GetById(xmlPlan.PlanProfileId.Value) ?? throw new NotFoundException<Plan>(xmlPlan.PlanProfileId.Value);
            this.logger.LogInformation("Processing request to import media from XML for plan {planId}", plan.Id);
            var planMedia = await this.xmlClient.Plan.Media(xmlPlanId, excludeImported: true);
            if (!planMedia.Any())
            {
                this.logger.LogInformation("No media was found in the MLS to import for listing id {listingId}", xmlPlanId);
                return;
            }

            await this.BulkCreateAsync(plan.Id, mediaType: MediaType.PlanProfile, planMedia);
            await this.xmlClient.Plan.PlanImagesImported(
                xmlPlanId,
                request: new()
                {
                    ImageIds = planMedia.Select(m => m.Id),
                });
        }

        public async Task ImportListingMedia(Guid xmlListingId, bool checkMediaLimit = false, bool useServiceBus = true)
        {
            var maxAllowedRemaining = 0;
            var maxImagesAllowed = this.options.ListingRequest.MaxAllowedMedia;

            var listing = await this.listingSaleRepository.GetListingByXmlListingId(xmlListingId) ??
                 throw new DomainException($"Cannot process the media import of the xml listing {xmlListingId} before it's imported");

            this.logger.LogInformation("Processing request to import media from XML for listing {listingId}", listing.Id);
            var listingMedia = await this.xmlClient.Listing.Media(xmlListingId, excludeImported: true);
            if (!listingMedia.Any())
            {
                this.logger.LogInformation("No media was found in the MLS to import for listing id {listingId}", xmlListingId);
                return;
            }

            if (checkMediaLimit)
            {
                maxImagesAllowed = await this.GetListingMediaLimit(listing.Id);
                if (maxImagesAllowed == 0)
                {
                    this.logger.LogWarning("the maximum limit of media allowed for the listing with id: {xmlListingId} has been reached", xmlListingId);
                    return;
                }
            }

            maxAllowedRemaining = checkMediaLimit ? maxImagesAllowed : listingMedia.Count();
            listingMedia = listingMedia.Take(maxAllowedRemaining);
            if (useServiceBus)
            {
                await this.BulkCreateAsync(listing.Id, mediaType: MediaType.Residential, listingMedia);
            }
            else
            {
                await this.BulkCreate(listing.Id, mediaType: MediaType.Residential, listingMedia);
            }

            await this.xmlClient.Listing.ListingImagesImported(
                xmlListingId,
                request: new()
                {
                    ImageIds = listingMedia.Select(m => m.Id),
                });
        }

        private async Task<int> GetListingMediaLimit(Guid listingId)
        {
            var mediaLimit = this.options.ListingRequest.MaxAllowedMedia;
            var currentListingMedia = await this.mediaService.GetAsync(listingId);

            if (currentListingMedia == null)
            {
                return mediaLimit;
            }

            if (currentListingMedia.Media != null && currentListingMedia.Media.Count() >= mediaLimit)
            {
                return 0;
            }

            return mediaLimit - currentListingMedia.Media.Count();
        }

        private async Task BulkCreateAsync(Guid entityId, MediaType mediaType, IEnumerable<ImageResponse> listingSaleMediaDto)
        {
            this.logger.LogInformation("Starting the process to send message to service bus to create media with id {entityId}.", entityId);
            var mediaMessages = listingSaleMediaDto.Select(mediaDto => new MediaCreateMessage
            {
                Id = entityId,
                Media = new()
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    Market = MarketCode.Austin,
                    Order = mediaDto.SequencePosition,
                    Type = mediaType,
                    Title = mediaDto.Title,
                    Uri = new Uri(mediaDto.Reference),
                },
            });

            var userId = this.userContextProvider.GetCurrentUserId();
            await this.mediaMessagingService.SendMessage(mediaMessages, userId: userId.ToString(), correlationId: this.traceIdProvider.TraceId);
        }

        private async Task BulkCreate(Guid entityId, MediaType mediaType, IEnumerable<ImageResponse> listingSaleMediaDto)
        {
            this.logger.LogInformation("Starting the process to to create media with id {entityId}. using http", entityId);
            var resources = listingSaleMediaDto.Select(mediaDto => new SimpleMedia
            {
                Id = Guid.NewGuid(),
                EntityId = entityId,
                Market = MarketCode.Austin,
                Order = mediaDto.SequencePosition,
                Type = mediaType,
                Title = mediaDto.Title,
                Uri = new Uri(mediaDto.Reference),
            });

            await this.mediaService.CreateAsync(resources);
        }
    }
}
