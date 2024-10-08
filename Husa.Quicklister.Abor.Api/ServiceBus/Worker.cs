namespace Husa.Quicklister.Abor.Api.ServiceBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Extensions.Api.ServiceBus.Subscribers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class Worker : Husa.Quicklister.Extensions.Api.ServiceBus.Worker
    {
        private readonly IPhotoServiceSubscriber photoSubscriber;
        private readonly IPhotoServiceMessagesHandler photoMessagesHandler;
        private readonly IDownloaderSubscriber downloaderSubscriber;
        private readonly IDownloaderMessagesHandler downloaderMessagesHandler;
        private readonly IXmlSubscriber xmlSubscriber;
        private readonly IXmlMessagesHandler xmlMessagesHandler;
        private readonly IMigrationSubscriber migrationSubscriber;
        private readonly IMigrationMessagesHandler migrationMessagesHandler;
        private readonly FeatureFlags featureFlags;

        public Worker(
            IPhotoServiceSubscriber photoSubscriber,
            IPhotoServiceMessagesHandler photoMessagesHandler,
            IDownloaderSubscriber downloaderSubscriber,
            IDownloaderMessagesHandler downloaderMessagesHandler,
            IXmlSubscriber xmlSubscriber,
            IXmlMessagesHandler xmlMessagesHandler,
            IMigrationSubscriber migrationSubscriber,
            IMigrationMessagesHandler migrationMessagesHandler,
            ICompanyServiceSubscriber companySubscriber,
            ICompanyServiceMessagesHandler companyMessagesHandler,
            IJsonImportSubscriber jsonImportSubscriber,
            IJsonImportMessagesHandler jsonImportMessagesHandler,
            IOptions<ApplicationOptions> options,
            ILogger<Worker> logger)
            : base(companySubscriber, companyMessagesHandler, jsonImportSubscriber, jsonImportMessagesHandler, logger)
        {
            this.downloaderSubscriber = downloaderSubscriber ?? throw new ArgumentNullException(nameof(downloaderSubscriber));
            this.featureFlags = options?.Value?.FeatureFlags ?? throw new ArgumentNullException(nameof(options));
            this.downloaderMessagesHandler = downloaderMessagesHandler ?? throw new ArgumentNullException(nameof(downloaderMessagesHandler));
            this.photoSubscriber = photoSubscriber ?? throw new ArgumentNullException(nameof(photoSubscriber));
            this.photoMessagesHandler = photoMessagesHandler ?? throw new ArgumentNullException(nameof(photoMessagesHandler));
            this.xmlSubscriber = xmlSubscriber ?? throw new ArgumentNullException(nameof(xmlSubscriber));
            this.xmlMessagesHandler = xmlMessagesHandler ?? throw new ArgumentNullException(nameof(xmlMessagesHandler));
            this.migrationSubscriber = migrationSubscriber ?? throw new ArgumentNullException(nameof(migrationSubscriber));
            this.migrationMessagesHandler = migrationMessagesHandler ?? throw new ArgumentNullException(nameof(migrationMessagesHandler));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.Logger.LogInformation(RegisterHandlerMsg, nameof(this.photoMessagesHandler));
                this.photoSubscriber.ConfigureClient(this.photoMessagesHandler);

                this.Logger.LogInformation(RegisterHandlerMsg, nameof(this.migrationMessagesHandler));
                this.migrationSubscriber.ConfigureClient(this.migrationMessagesHandler);

                if (this.featureFlags.IsDownloaderEnabled)
                {
                    this.Logger.LogInformation(RegisterHandlerMsg, nameof(this.downloaderSubscriber));
                    this.downloaderSubscriber.ConfigureClient(this.downloaderMessagesHandler);
                }

                if (this.featureFlags.IsXmlBusHandlerEnabled)
                {
                    this.Logger.LogInformation(RegisterHandlerMsg, nameof(this.xmlMessagesHandler));
                    this.xmlSubscriber.ConfigureClient(this.xmlMessagesHandler);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Failed to register the client handlers");
                throw;
            }

            base.StartAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this.Logger.LogInformation(CloseSubscriptionClientMsg, nameof(this.photoSubscriber));
            await this.photoSubscriber.CloseClient();

            this.Logger.LogInformation(CloseSubscriptionClientMsg, nameof(this.migrationSubscriber));
            await this.migrationSubscriber.CloseClient();

            if (this.featureFlags.IsDownloaderEnabled)
            {
                this.Logger.LogInformation(CloseSubscriptionClientMsg, nameof(this.downloaderSubscriber));
                await this.downloaderSubscriber.CloseClient();
            }

            if (this.featureFlags.IsXmlBusHandlerEnabled)
            {
                this.Logger.LogInformation(CloseSubscriptionClientMsg, nameof(this.xmlSubscriber));
                await this.xmlSubscriber.CloseClient();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
