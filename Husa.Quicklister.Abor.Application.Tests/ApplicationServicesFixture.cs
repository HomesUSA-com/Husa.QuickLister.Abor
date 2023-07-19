namespace Husa.Quicklister.Abor.Application.Tests
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Options;
    using Moq;

    public class ApplicationServicesFixture
    {
        public const string TestTopicName = "topic-photo-local";

        public ApplicationServicesFixture()
        {
            this.Options = new Mock<IOptions<ApplicationOptions>>();
            this.Options.Setup(o => o.Value).Returns(new ApplicationOptions
            {
                FeatureFlags = new FeatureFlags
                {
                    IsXmlBusHandlerEnabled = false,
                    IsDownloaderEnabled = false,
                },
                ListingRequest = new()
                {
                    MinRequiredMedia = 4,
                    MaxAllowedMedia = 25,
                },
                MediaAllowed = new MediaAllowedSettings()
                {
                    PlanMaxAllowedMedia = 25,
                    SaleListingMaxAllowedMedia = 25,
                    SaleCommunityMaxAllowedMedia = 60,
                },
            });

            this.BusOptions = new Mock<IOptions<ServiceBusSettings>>();
            this.BusOptions.Setup(o => o.Value).Returns(new ServiceBusSettings
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

        public Mock<IOptions<ServiceBusSettings>> BusOptions { get; set; }

        public IMapper Mapper { get; }
    }
}
