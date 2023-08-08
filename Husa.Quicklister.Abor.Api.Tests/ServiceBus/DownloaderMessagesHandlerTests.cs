namespace Husa.Quicklister.Abor.Api.Tests.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Application.Models.Listing;
    using Husa.Quicklister.Abor.Application.Models.Office;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Application.Models;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using DownloaderConstructionStage = Husa.Downloader.Sabor.Domain.Enums.ConstructionStage;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class DownloaderMessagesHandlerTests
    {
        private readonly Mock<IDownloaderSubscriber> subscriberMock = new();
        private readonly Mock<ISubscriptionClient> subscriptionClientMock = new();

        private readonly Mock<IAgentService> agentServiceMock = new();
        private readonly Mock<IOfficeService> officeServiceMock = new();
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
            const string loginName = "fake-agent";
            const string agentId = "122344";
            var agentMessage = new AgentMessage
            {
                Id = Guid.NewGuid(),
                AgentId = agentId,
                LoginName = loginName,
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
                    It.Is<AgentDto>(a => a.LoginName.Equals(loginName) && a.MarketUniqueId.Equals(agentId))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessOfficeMessageSuccessTestAsync()
        {
            // Arrange
            const string officeName = "fake-office";
            const string officeId = "122344";
            var officeMessage = new OfficeMessage
            {
                Id = Guid.NewGuid(),
                OfficeId = officeId,
                Name = officeName,
                City = "Austin",
                State = "TX",
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
        public async Task ProcessResidentialMessageSuccessTestAsync()
        {
            // Arrange
            const string mlsNumber = "1122334455";
            const string sellingAgent = "122344";
            var residentialMessage = new ResidentialMessage
            {
                Id = Guid.NewGuid(),
                ResidentialValue = new()
                {
                    MlsNumber = mlsNumber,
                    BuilderName = "Builder Name",
                    SellingAgent = sellingAgent,
                    Status = MarketStatuses.PriceChange.ToStringFromEnumMember(),
                },
                LegacyResidentialInfo = new()
                {
                    ConstructionCompletionDate = DateTime.UtcNow,
                    ConstructionStage = DownloaderConstructionStage.Complete,
                    EmailForRealtors = "test.email@test.com",
                    OwnerAlternatePhone = "9991116666",
                },
                Rooms = Array.Empty<RoomMessage>(),
                Hoas = Array.Empty<HoaMessage>(),
            };
            var message = ApplicationServicesFixture.BuildBusMessage(residentialMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.downloaderServiceMock.Verify(
                ds => ds.ProcessDataFromDownloaderAsync(
                    It.Is<FullListingSaleDto>(l => l.MlsNumber.Equals(mlsNumber) && l.MlsStatus == MarketStatuses.PriceChange),
                    It.IsAny<IEnumerable<RoomDto>>(),
                    It.IsAny<IEnumerable<HoaDto>>(),
                    It.Is<string>(agentId => agentId.Equals(sellingAgent))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessResidentialOpenHousesMessageSuccessTestAsync()
        {
            // Arrange
            const string mlsNumber = "1122334455";
            var openHouseStartingDate = new DateTime(2022, 10, 19, 8, 0, 0, DateTimeKind.Utc);
            var openHouseMessage = new ResidentialOpenHousesMessage
            {
                Id = Guid.NewGuid(),
                MlsNumber = mlsNumber,
                OpenHouses = new List<OpenHouseMessage>
                {
                    new()
                    {
                        MlsNumber = mlsNumber,
                        OpenHouseType = "Residential",
                        StartTime = openHouseStartingDate,
                        EndTime = openHouseStartingDate.AddHours(12),
                        Lunch = true,
                        Refreshments = true,
                    },
                    new()
                    {
                        MlsNumber = mlsNumber,
                        OpenHouseType = "Residential",
                        StartTime = openHouseStartingDate.AddDays(1),
                        EndTime = openHouseStartingDate.AddHours(12).AddDays(1),
                        Lunch = true,
                        Refreshments = true,
                    },
                },
            };

            var message = ApplicationServicesFixture.BuildBusMessage(openHouseMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.downloaderServiceMock.Verify(
                ds => ds.ProcessOpenHouseFromDownloaderAsync(
                    It.Is<string>(mlsnum => mlsnum.Equals(mlsNumber)),
                    It.Is<IEnumerable<OpenHouseDto>>(openHouses => openHouses.Any(oh => oh.StartTime == openHouseStartingDate.TimeOfDay))),
                Times.Once);
        }

        [Fact]
        public async Task ProcessResidentialMediaMessageSuccessTestAsync()
        {
            // Arrange
            const string mlsNumber = "1122334455";
            const string mediaTitle = "some-title";
            var mediaId = Guid.NewGuid();
            var mediaMessage = new ResidentialMediaMessage
            {
                Id = Guid.NewGuid(),
                MlsNumber = mlsNumber,
                Media = new List<MediaMessage>
                {
                    new()
                    {
                        MediaId = mediaId.ToString(),
                        MlsNumber = mlsNumber,
                        Title = mediaTitle,
                        UploadKey = mediaId.ToString(),
                    },
                },
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
            this.downloaderServiceMock.Verify(
                ds => ds.ProcessMediaFromDownloaderAsync(
                    It.Is<string>(mlsnum => mlsnum.Equals(mlsNumber)),
                    It.Is<IEnumerable<ListingSaleMediaDto>>(media => media.Any(oh => oh.MediaId == mediaId.ToString()))),
                Times.Once);
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
            this.downloaderServiceMock.Verify(s => s.ProcessDataFromDownloaderAsync(It.IsAny<FullListingSaleDto>(), It.IsAny<IEnumerable<RoomDto>>(), It.IsAny<IEnumerable<HoaDto>>(), It.IsAny<string>()), Times.Never);
            this.downloaderServiceMock.Verify(s => s.ProcessOpenHouseFromDownloaderAsync(It.IsAny<string>(), It.IsAny<IEnumerable<OpenHouseDto>>()), Times.Never);
            this.downloaderServiceMock.Verify(s => s.ProcessMediaFromDownloaderAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ListingSaleMediaDto>>()), Times.Never);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();
            serviceCollection.AddSingleton(this.agentServiceMock.Object);
            serviceCollection.AddSingleton(this.officeServiceMock.Object);
            serviceCollection.AddSingleton(this.downloaderServiceMock.Object);
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
