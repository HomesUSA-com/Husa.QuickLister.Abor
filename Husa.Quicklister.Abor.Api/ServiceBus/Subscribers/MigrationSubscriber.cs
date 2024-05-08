namespace Husa.Quicklister.Abor.Api.ServiceBus.Subscribers
{
    using System;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Options;
    public class MigrationSubscriber : IMigrationSubscriber
    {
        public MigrationSubscriber(IOptions<ServiceBusSettings> options)
        {
            var busOptions = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
            this.Client = new SubscriptionClient(busOptions.ConnectionString, busOptions.MigrationService.TopicName, busOptions.MigrationService.SubscriptionName);
        }

        public ISubscriptionClient Client { get; set; }
    }
}
