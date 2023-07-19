namespace Husa.Quicklister.Abor.Api.ServiceBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class Worker : IHostedService
    {
        private readonly IPhotoServiceSubscriber photoSubscriber;
        private readonly IPhotoServiceMessagesHandler photoMessagesHandler;
        private readonly IDownloaderSubscriber downloaderSubscriber;
        private readonly IDownloaderMessagesHandler downloaderMessagesHandler;
        private readonly IXmlSubscriber xmlSubscriber;
        private readonly IXmlMessagesHandler xmlMessagesHandler;
        private readonly FeatureFlags featureFlags;
        private readonly ILogger<Worker> logger;

        public Worker(
            IPhotoServiceSubscriber photoSubscriber,
            IPhotoServiceMessagesHandler photoMessagesHandler,
            IDownloaderSubscriber downloaderSubscriber,
            IDownloaderMessagesHandler downloaderMessagesHandler,
            IXmlSubscriber xmlSubscriber,
            IXmlMessagesHandler xmlMessagesHandler,
            IOptions<ApplicationOptions> options,
            ILogger<Worker> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.downloaderSubscriber = downloaderSubscriber ?? throw new ArgumentNullException(nameof(downloaderSubscriber));
            this.featureFlags = options?.Value?.FeatureFlags ?? throw new ArgumentNullException(nameof(options));
            this.downloaderMessagesHandler = downloaderMessagesHandler ?? throw new ArgumentNullException(nameof(downloaderMessagesHandler));
            this.photoSubscriber = photoSubscriber ?? throw new ArgumentNullException(nameof(photoSubscriber));
            this.photoMessagesHandler = photoMessagesHandler ?? throw new ArgumentNullException(nameof(photoMessagesHandler));
            this.xmlSubscriber = xmlSubscriber ?? throw new ArgumentNullException(nameof(xmlSubscriber));
            this.xmlMessagesHandler = xmlMessagesHandler ?? throw new ArgumentNullException(nameof(xmlMessagesHandler));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this.logger.LogInformation("Registering handler client: {subscriber}", nameof(this.photoMessagesHandler));
                this.photoSubscriber.ConfigureClient(this.photoMessagesHandler);

                if (this.featureFlags.IsDownloaderEnabled)
                {
                    this.logger.LogInformation("Registering handler client: {subscriber}", nameof(this.downloaderSubscriber));
                    this.downloaderSubscriber.ConfigureClient(this.downloaderMessagesHandler);
                }

                if (this.featureFlags.IsXmlBusHandlerEnabled)
                {
                    this.logger.LogInformation("Registering handler client: {subscriber}", nameof(this.xmlMessagesHandler));
                    this.xmlSubscriber.ConfigureClient(this.xmlMessagesHandler);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to register the client handlers");
                throw;
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Closing subcription client connection of {subscriber}", nameof(this.photoSubscriber));
            await this.photoSubscriber.CloseClient();

            if (this.featureFlags.IsDownloaderEnabled)
            {
                this.logger.LogInformation("Closing subcription client connection of {subscriber}", nameof(this.downloaderSubscriber));
                await this.downloaderSubscriber.CloseClient();
            }

            if (this.featureFlags.IsXmlBusHandlerEnabled)
            {
                this.logger.LogInformation("Closing subcription client connection of {subscriber}", nameof(this.xmlSubscriber));
                await this.xmlSubscriber.CloseClient();
            }
        }
    }
}
