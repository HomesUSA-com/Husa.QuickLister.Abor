namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Downloader.CTX.ServiceBus.Contracts;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Api.ServiceBus.Helpers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class DownloaderMessagesHandler : MessagesHandler<DownloaderMessagesHandler>, IDownloaderMessagesHandler
    {
        private readonly DownloaderUserSettings downloaderUserOptions;
        private readonly IMapper mapper;

        public DownloaderMessagesHandler(
            IDownloaderSubscriber downloaderSubscriber,
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IOptions<DownloaderUserSettings> options,
            ILogger<DownloaderMessagesHandler> logger)
            : base(downloaderSubscriber, scopeFactory, logger)
        {
            this.downloaderUserOptions = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task ProcessMessageAsync(Message message, IServiceScope scope, CancellationToken cancellationToken)
        {
            HandlerHelper.ConfigureUserAgent(scope);
            HandlerHelper.ConfigureContext(message, this.downloaderUserOptions, scope);
            this.Logger.LogDebug("Deserializing message {messageId}.", message.MessageId);
            var receivedMessage = message.DeserializeMessage();
            switch (receivedMessage)
            {
                case OfficeMessage officeMessage:
                    await ProcessOfficeMessage(officeMessage);
                    break;
                case AgentMessage agentMessage:
                    await ProcessAgentMessage(agentMessage);
                    break;
                case ResidentialMessage residentialMessage:
                    await ProcessResidentialMessage(residentialMessage);
                    break;
                case MediaMessage mediaMessage:
                    await ProcessResidentialMediaMessage(mediaMessage);
                    break;
                default:
                    this.Logger.LogWarning("Message type not recognized for message {messageId}.", message.MessageId);
                    break;
            }

            Task ProcessOfficeMessage(OfficeMessage officeMessage)
            {
                this.Logger.LogInformation("Processing message for office {officeName} with id {officeId}", officeMessage.Name, officeMessage.OfficeKey);
                var officeService = scope.ServiceProvider.GetRequiredService<IOfficeService>();
                var officeDto = this.mapper.Map<OfficeDto>(officeMessage);
                return officeService.ProcessDataFromDownloaderAsync(officeDto);
            }

            Task ProcessAgentMessage(AgentMessage agentMessage)
            {
                this.Logger.LogInformation("Processing message for agent {agentFullName} with id {agentId} ", agentMessage.FullName, agentMessage.EntityKey);
                var agentDto = this.mapper.Map<AgentDto>(agentMessage);

                var agentService = scope.ServiceProvider.GetRequiredService<IAgentService>();
                return agentService.ProcessDataFromDownloaderAsync(agentDto);
            }

            Task ProcessResidentialMessage(ResidentialMessage residentialMessage)
            {
                this.Logger.LogInformation("Processing message for listing with mls number {mlsNumber}", residentialMessage.EntityKey);

                var residentialService = scope.ServiceProvider.GetRequiredService<IListingService>();
                return residentialService.ProcessData(residentialMessage.EntityKey, residentialMessage.ProcessFullListing);
            }

            Task ProcessResidentialMediaMessage(MediaMessage mediaMessage)
            {
                this.Logger.LogInformation("Processing media for listing with id {ListingId}", mediaMessage.ListingId);
                var mediaService = scope.ServiceProvider.GetRequiredService<IMediaService>();
                return mediaService.ProcessData(mediaMessage.ListingId, mediaMessage.UpdatedOn);
            }
        }
    }
}
