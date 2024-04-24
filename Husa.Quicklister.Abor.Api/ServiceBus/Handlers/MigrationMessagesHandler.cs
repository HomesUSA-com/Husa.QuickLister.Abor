namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Services;
    using Husa.Quicklister.Abor.Api.ServiceBus.Helpers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Application.Models.Migration;
    using Husa.Quicklister.Extensions.ServiceBus.Contracts;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    public class MigrationMessagesHandler : MessagesHandler<MigrationMessagesHandler>, IMigrationMessagesHandler
    {
        private readonly DownloaderUserSettings downloaderUserOptions;
        public MigrationMessagesHandler(
            IMigrationSubscriber migrationSubscriber,
            IServiceScopeFactory serviceProvider,
            IOptions<DownloaderUserSettings> options,
            ILogger<MigrationMessagesHandler> logger)
            : base(migrationSubscriber, serviceProvider, logger)
        {
            this.downloaderUserOptions = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        public override async Task ProcessMessageAsync(Message message, IServiceScope scope, CancellationToken cancellationToken)
        {
            HandlerHelper.ConfigureUserAgent(scope);
            HandlerHelper.ConfigureContext(message, this.downloaderUserOptions, scope);
            this.Logger.LogDebug("Deserializing message {messageId}.", message.MessageId);
            var receivedMessage = message.DeserializeMessage();
            switch (receivedMessage)
            {
                case MigrateListingMessage migrateListingMessage:
                    this.Logger.LogInformation("Processing message to migrate listing with mls number {mlsNumber}", migrateListingMessage.MlsNumber);
                    var listingMigrationService = scope.ServiceProvider.GetRequiredService<ISaleListingMigrationService>();
                    await listingMigrationService.MigrateListings(migrateListingMessage.CompanyId, new MigrateListingFilter
                    {
                        MlsNumber = migrateListingMessage.MlsNumber,
                        UpdateListing = migrateListingMessage.UpdateListing,
                        MigrateFullListing = migrateListingMessage.MigrateFullListing,
                    });
                    return;
                default:
                    this.Logger.LogWarning("Message type not recognized for message {messageId}.", message.MessageId);
                    break;
            }
        }
    }
}
