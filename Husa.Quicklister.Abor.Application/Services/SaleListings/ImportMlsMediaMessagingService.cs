namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ImportMlsMediaMessagingService : MessagingServiceBusBase, IImportMlsMediaMessagingService
    {
        public ImportMlsMediaMessagingService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            ServiceBusClient busClient,
            ILogger<ImportMlsMediaMessagingService> logger)
          : base(logger, busClient, serviceBusSettings.Value.ImportMlsMedia.TopicName)
        {
        }
    }
}
