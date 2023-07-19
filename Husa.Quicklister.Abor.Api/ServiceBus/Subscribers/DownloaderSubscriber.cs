namespace Husa.Quicklister.Abor.Api.ServiceBus.Subscribers
{
    using System;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Options;

    public class DownloaderSubscriber : IDownloaderSubscriber
    {
        public DownloaderSubscriber(IOptions<ServiceBusSettings> options)
        {
            var busOptions = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
            this.Client = new SubscriptionClient(busOptions.ConnectionString, busOptions.DownloaderService.TopicName, busOptions.DownloaderService.SubscriptionName);
        }

        public ISubscriptionClient Client { get; set; }
    }
}
