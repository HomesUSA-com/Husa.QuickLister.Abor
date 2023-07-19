namespace Husa.Quicklister.Abor.Application.Media
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MediaMessagingService : MessagingServiceBusBase, IMediaMessagingService
    {
        public MediaMessagingService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            ServiceBusClient busClient,
            ILogger<MediaMessagingService> logger)
         : base(logger, busClient, serviceBusSettings.Value.MediaService.TopicName)
        {
        }
    }
}
