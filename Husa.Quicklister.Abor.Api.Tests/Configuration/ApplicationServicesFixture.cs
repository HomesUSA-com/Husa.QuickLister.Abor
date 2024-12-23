namespace Husa.Quicklister.Abor.Api.Tests.Configuration
{
    using System;
    using AutoMapper;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.Azure.ServiceBus;
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
                    IsDownloaderEnabled = false,
                    IsXmlBusHandlerEnabled = false,
                },
                MediaAllowed = new()
                {
                    SaleListingMaxAllowedMedia = 40,
                    CommunityMaxAllowedMedia = 60,
                    PlanMaxAllowedMedia = 60,
                },
            });

            this.DownloaderUserInfo = new Mock<IOptions<DownloaderUserSettings>>();
            this.DownloaderUserInfo.Setup(o => o.Value).Returns(new DownloaderUserSettings
            {
                Id = new Guid("eb117a3e-503b-4c34-876d-07f7d5386ec7"),
                Email = "downloaderuser@homesusa.com",
                Name = "downloader user",
                MLSAdministrator = true,
            });

            this.XmlUserSettings = new Mock<IOptions<XmlUserSettings>>();
            this.XmlUserSettings.Setup(o => o.Value).Returns(new XmlUserSettings
            {
                Id = new Guid("79196fe3-52ee-4501-80ec-5307ba1451df"),
                Email = "xml@test.com",
                Name = "xml name",
                MLSAdministrator = true,
            });

            this.Mapper = Bootstrapper.ConfigureAutoMapper();
        }

        public Mock<IOptions<ApplicationOptions>> Options { get; set; }

        public Mock<IOptions<DownloaderUserSettings>> DownloaderUserInfo { get; set; }

        public Mock<IOptions<XmlUserSettings>> XmlUserSettings { get; set; }

        public IMapper Mapper { get; }

        public static Message BuildBusMessage<T>(T unserializedMessage)
            where T : IProvideBusEvent
        {
            var message = new Message
            {
                Body = unserializedMessage.SerializeMessage<T>(),
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString(),
            };

            message.UserProperties.Add("AssemblyName", typeof(T).AssemblyQualifiedName);
            message.UserProperties.Add("UserId", Guid.NewGuid());

            return message;
        }
    }
}
