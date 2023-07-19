namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.MediaService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Request = Husa.MediaService.Api.Contracts.Request;
    using Response = Husa.MediaService.Api.Contracts.Response;

    public class ListingRequestMediaService : MessagingServiceBusBase, IListingRequestMediaService
    {
        private readonly IMediaServiceClient mediaServiceClient;
        private readonly IUserContextProvider userContextProvider;
        private readonly ILogger<ListingRequestMediaService> logger;
        private readonly ISaleListingRequestRepository saleRequestRepository;
        private readonly IProvideTraceId traceIdProvider;

        public ListingRequestMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            ISaleListingRequestRepository saleRequestRepository,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient client,
            IProvideTraceId traceIdProvider,
            ILogger<ListingRequestMediaService> logger)
            : base(logger, client, serviceBusSettings.Value.MediaService.TopicName)
        {
            this.mediaServiceClient = mediaServiceClient ?? throw new ArgumentNullException(nameof(mediaServiceClient));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.traceIdProvider = traceIdProvider ?? throw new ArgumentNullException(nameof(traceIdProvider));
            this.saleRequestRepository = saleRequestRepository ?? throw new ArgumentNullException(nameof(saleRequestRepository));
        }

        public static MediaType MediaType => MediaType.ListingRequest;

        public async Task<Response.ResourceResponse> GetResources(Guid entityId, MediaType type = MediaType.ListingRequest)
        {
            if (type == MediaType.ListingRequest)
            {
                await this.ValidateEntityAndUserCompany(entityId);
            }

            this.logger.LogInformation("Starting the process to get the resource for the entity with id {entityId}.", entityId);
            return await this.mediaServiceClient.GetResources(entityId, type);
        }

        public async Task<Response.MediaDetail> GetById(Guid entityId, Guid mediaId)
        {
            await this.ValidateEntityAndUserCompany(entityId);

            this.logger.LogInformation("Starting the process to get the resource with id {mediaId}.", mediaId);

            return await this.mediaServiceClient.GetMediaById(entityId, mediaId);
        }

        public async Task CreateAsync(Guid entityId, Request.SimpleMedia simpleMedia)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to create media with id {entityId}", simpleMedia.EntityId);
            simpleMedia.Market = MarketCode.SanAntonio;
            simpleMedia.EntityId = entityId;
            simpleMedia.Type = MediaType;
            var mediaMessage = new MediaCreateMessage
            {
                Id = simpleMedia.EntityId,
                Media = simpleMedia,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task ReplaceAsync(Guid entityId, Request.SimpleMedia simpleMedia)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to replace media with id {mediaId}.", simpleMedia.Id);
            simpleMedia.Market = MarketCode.SanAntonio;
            simpleMedia.EntityId = entityId;
            simpleMedia.Type = MediaType;
            var mediaMessage = new MediaReplaceMessage
            {
                Id = simpleMedia.EntityId,
                Media = simpleMedia,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task CreateVirtualTourAsync(Guid entityId, Request.VirtualTour virtualTour)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to create virtual tour with id {virtualTourId}.", entityId);
            virtualTour.Market = MarketCode.SanAntonio;
            virtualTour.EntityId = entityId;
            virtualTour.Type = MediaType;
            var mediaMessage = new VirtualTourCreateMessage
            {
                Id = virtualTour.EntityId,
                VirtualTour = virtualTour,
            };
            await this.SendBusMessage(mediaMessage);
        }

        public async Task DeleteById(Guid entityId, Guid mediaId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to delete the media {mediaId}.", mediaId);
            var mediaMessage = new MediaDeleteMessage
            {
                Id = mediaId,
                EntityId = entityId,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task DeleteVirtualTourById(Guid entityId, Guid virtualTourId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to delete virtual tour {virtualTourId}.", virtualTourId);
            var mediaMessage = new VirtualTourDeleteMessage
            {
                Id = virtualTourId,
                EntityId = entityId,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task DeleteResources(Guid entityId)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to delete all resources for the {entityId}.", entityId);
            var mediaMessage = new MediaDeleteAllMessage
            {
                OwnerId = entityId,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task UpdateAsync(Guid entityId, Guid mediaId, Request.SimpleMedia simpleMedia)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to update media with id {mediaId}.", mediaId);
            var mediaMessage = new MediaUpdateMessage
            {
                Id = mediaId,
                EntityId = entityId,
                Media = simpleMedia,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task UpdateResourcesAsync(Guid entityId, IEnumerable<Request.SimpleMedia> simpleMedia)
        {
            await this.ValidateEntityAndUserCompany(entityId);
            this.logger.LogInformation("Starting the process to send message to service bus to update {mediaCount} resources.", simpleMedia.Count());
            var mediaMessage = new MediaBulkUpdateMessage
            {
                EntityId = entityId,
                Media = simpleMedia,
            };

            await this.SendBusMessage(mediaMessage);
        }

        public async Task CreateMediaRequestAsync(Guid listingSaleId, Guid listingRequestId, bool dispose = true)
        {
            await this.ValidateEntityAndUserCompany(listingRequestId);
            this.logger.LogInformation("Starting the process to send message to service bus to create a media request for listingRequest {listingRequestId}.", listingRequestId);
            var mediaMessage = new CloneMediaMessage
            {
                CloneMedia = new Request.CloneMedia
                {
                    FromEntityId = listingSaleId,
                    FromType = MediaType.Residential,
                    ToEntityId = listingRequestId,
                    ToType = ListingRequestMediaService.MediaType,
                },
            };
            await this.SendBusMessage(mediaMessage, dispose);
        }

        public async Task DisposeServiceBus()
        {
            this.logger.LogInformation("Dispose media client.");
            await this.DisposeClient();
        }

        private async Task SendBusMessage<T>(T messages, bool dispose = true)
            where T : IProvideBusEvent
        {
            var userId = this.userContextProvider.GetCurrentUserId();

            await this.SendMessage(
                messages: new[] { messages },
                userId: userId.ToString(),
                dispose: dispose,
                correlationId: this.traceIdProvider.TraceId);
        }

        private async Task ValidateEntityAndUserCompany(Guid listingRequestId)
        {
            var entity = await this.saleRequestRepository.GetListingRequestByIdAsync(listingRequestId) ?? throw new NotFoundException<SaleListingRequest>(listingRequestId);
            var user = this.userContextProvider.GetCurrentUser();
            if (user.UserRole == UserRole.User && !user.CompanyId.Value.Equals(entity.SaleProperty.CompanyId))
            {
                this.logger.LogInformation("The current user is not allowed to get the media from the listing {listingRequestId}", listingRequestId);
                throw new DomainException($"The current user is not allowed to get the media from the listing: {listingRequestId}");
            }
        }
    }
}
