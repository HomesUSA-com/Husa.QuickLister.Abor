namespace Husa.Quicklister.Abor.Api.Tests.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Downloader.CTX.ServiceBus.Contracts;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class DownloaderMessagesHandlerTests
    {
        private readonly Mock<IDownloaderSubscriber> subscriberMock = new();
        private readonly Mock<ISubscriptionClient> subscriptionClientMock = new();

        private readonly Mock<IAgentService> agentServiceMock = new();
        private readonly Mock<IOfficeService> officeServiceMock = new();
        private readonly Mock<IMediaService> mediaServiceMock = new();
        private readonly Mock<IDownloaderService> downloaderServiceMock = new();

        private readonly Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        private readonly Mock<ILogger<DownloaderMessagesHandler>> loggerMock = new();
        private readonly ApplicationServicesFixture fixture;

        public DownloaderMessagesHandlerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task ProcessAgentMessageSuccessTestAsync()
        {
            // Arrange
            const string agentId = "122344";
            var agentMessage = new AgentMessage
            {
                Id = Guid.NewGuid(),
                EntityKey = agentId,
            };

            var message = ApplicationServicesFixture.BuildBusMessage(agentMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.agentServiceMock.Verify(
                ds => ds.ProcessDataFromDownloaderAsync(
                    It.Is<AgentDto>(a => a.MarketUniqueId.Equals(agentId))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOfficeMessageSuccessTestAsync()
        {
            // Arrange
            const string officeName = "fake-office";
            const string officeId = "122344";
            var officeMessage = new Downloader.CTX.ServiceBus.Contracts.OfficeMessage
            {
                Id = Guid.NewGuid(),
                Address = Faker.Address.StreetName(),
                City = Faker.Enum.Random<Cities>().ToStringFromEnumMember(),
                MlsId = Faker.RandomNumber.Next(99999).ToString(),
                Name = officeName,
                OfficeKey = officeId,
                OfficeUpdateDate = DateTime.UtcNow,
                PhoneNumber = Faker.Phone.Number(),
                StateOrProvince = StateOrProvince.TX,
                Status = OfficeStatus.Active,
                Type = OfficeType.RealEstate,
                ZipCode = Faker.Address.ZipCode(),
                ZipCodeExt = null,
            };

            var message = ApplicationServicesFixture.BuildBusMessage(officeMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.officeServiceMock.Verify(
                ds => ds.ProcessDataFromDownloaderAsync(
                    It.Is<OfficeDto>(a => a.Name.Equals(officeName) && a.MarketUniqueId.Equals(officeId))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessMediaMessageSuccessTestAsync()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mediaMessage = new MediaMessage
            {
                ListingId = listingId,
            };

            var message = ApplicationServicesFixture.BuildBusMessage(mediaMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.mediaServiceMock.Verify(
            ds => ds.ProcessData(It.Is<Guid>(a => a == listingId), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task ProcessUnmanagedMessageTypeIsIgnoredCorrectlyTestAsync()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var propertyId = Guid.NewGuid();
            var photoRequestCreatedOn = new DateTime(2022, 10, 19, 8, 0, 0, DateTimeKind.Utc);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            var photoRequestMessage = new PhotoRequestCreatedMessage
            {
                Id = mediaId,
                SysCreatedOn = photoRequestCreatedOn,
                PropertyId = propertyId,
                Type = PhotoRequestType.Lease,
            };

            var message = ApplicationServicesFixture.BuildBusMessage(photoRequestMessage);

            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.agentServiceMock.Verify(s => s.ProcessDataFromDownloaderAsync(It.IsAny<AgentDto>()), Times.Never);
            this.officeServiceMock.Verify(s => s.ProcessDataFromDownloaderAsync(It.IsAny<OfficeDto>()), Times.Never);
            this.downloaderServiceMock.Verify(s => s.ProcessDataFromDownloaderAsync(It.IsAny<FullListingSaleDto>(), It.IsAny<IEnumerable<RoomDto>>(), It.IsAny<string>()), Times.Never);
            this.downloaderServiceMock.Verify(s => s.ProcessOpenHouseFromDownloaderAsync(It.IsAny<string>(), It.IsAny<IEnumerable<OpenHouseDto>>()), Times.Never);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();
            serviceCollection.AddSingleton(this.agentServiceMock.Object);
            serviceCollection.AddSingleton(this.officeServiceMock.Object);
            serviceCollection.AddSingleton(this.downloaderServiceMock.Object);
            serviceCollection.AddSingleton(this.mediaServiceMock.Object);
            serviceCollection.AddSingleton(userProvider.Object);
            serviceCollection.AddSingleton(new HeaderPropagationValues());

            var configureTraceId = new Mock<IConfigureTraceId>();
            serviceCollection.AddSingleton(configureTraceId.Object);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.SetupGet(sc => sc.ServiceProvider).Returns(serviceCollection.BuildServiceProvider());

            this.serviceScopeFactoryMock
                .Setup(sf => sf.CreateScope())
                .Returns(serviceScope.Object);
        }

        private DownloaderMessagesHandler GetSut() => new(
            this.subscriberMock.Object,
            this.serviceScopeFactoryMock.Object,
            this.fixture.Mapper,
            this.fixture.DownloaderUserInfo.Object,
            this.loggerMock.Object);
    }
}
