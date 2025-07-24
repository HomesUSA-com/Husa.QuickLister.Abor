namespace Husa.Quicklister.Abor.Application.Tests
{
    using AutoMapper;
    using Husa.Extensions.Quickbooks.Models;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.Extensions.Options;
    using Moq;
    using ExtensionsCrosscutting = Husa.Quicklister.Extensions.Crosscutting;

    public class ApplicationServicesFixture
    {
        public const string TestTopicName = "topic-photo-local";

        public ApplicationServicesFixture()
        {
            this.Options = new Mock<IOptions<ApplicationOptions>>();
            this.Options.Setup(o => o.Value).Returns(new ApplicationOptions
            {
                FeatureFlags = new ExtensionsCrosscutting.FeatureFlags
                {
                    IsXmlBusHandlerEnabled = false,
                    IsDownloaderEnabled = false,
                },
                ListingRequest = new()
                {
                    MinRequiredMedia = 1,
                    MaxAllowedMedia = 40,
                },
                MediaAllowed = new Husa.Quicklister.Extensions.Crosscutting.MediaAllowedSettings()
                {
                    PlanMaxAllowedMedia = 25,
                    SaleListingMaxAllowedMedia = 40,
                    CommunityMaxAllowedMedia = 60,
                },
                InvoiceSettings = new InvoiceSettings()
                {
                    ClassRefId = "3",
                    SalesItemLineId = "24355666",
                    ServiceURL = "www.url.com",
                },
            });

            this.BusOptions = new Mock<IOptions<ExtensionsCrosscutting.ServiceBusSettings>>();
            this.BusOptions.Setup(o => o.Value).Returns(new ExtensionsCrosscutting.ServiceBusSettings
            {
                MediaService = new()
                {
                    TopicName = TestTopicName,
                },
                PhotoService = new()
                {
                    TopicName = TestTopicName,
                },
            });

            this.Mapper = Bootstrapper.ConfigureAutoMapper();
        }

        public Mock<IOptions<ApplicationOptions>> Options { get; set; }

        public Mock<IOptions<ExtensionsCrosscutting.ServiceBusSettings>> BusOptions { get; set; }

        public IMapper Mapper { get; }
    }
}
