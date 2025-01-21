namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Extensions.Application.Interfaces.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;
    using MediaRequest = Husa.MediaService.Api.Contracts.Request;
    using ServiceBusOptions = Husa.Extensions.ServiceBus.Attributes.ServiceBusOptions;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public abstract class MediaServiceTests<TMediaService>
        where TMediaService : IMediaService
    {
        protected readonly Mock<IOptions<ServiceBusSettings>> busOptions;
        protected readonly Mock<IMediaServiceClient> mediaServiceClient;
        protected readonly Mock<ServiceBusClient> busClient;
        protected readonly Mock<ServiceBusSender> busSender;
        protected readonly Mock<IProvideTraceId> traceIdProvider;
        protected readonly ApplicationServicesFixture fixture;
        private readonly string topicName = "topic-media";

        protected MediaServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.mediaServiceClient = new Mock<IMediaServiceClient>();

            this.traceIdProvider = new Mock<IProvideTraceId>();
            this.traceIdProvider.Setup(u => u.TraceId).Returns(Guid.NewGuid().ToString());

            this.busOptions = new Mock<IOptions<ServiceBusSettings>>();
            var serviceBusSettings = new ServiceBusSettings { MediaService = new ServiceBusOptions { TopicName = this.topicName } };
            this.busOptions.Setup(u => u.Value).Returns(serviceBusSettings);

            this.busSender = new Mock<ServiceBusSender>();
            var serviceBusMessageBatch = ServiceBusModelFactory.ServiceBusMessageBatch(1, new List<ServiceBusMessage>());
            this.busSender.Setup(u => u.CreateMessageBatchAsync(It.IsAny<CancellationToken>())).ReturnsAsync(serviceBusMessageBatch);

            this.busClient = new Mock<ServiceBusClient>();
            this.busClient.Setup(u => u.CreateSender(It.IsAny<string>())).Returns(this.busSender.Object);
        }

        public TMediaService Sut { get; set; }

        [Fact]
        public async Task GetAsync_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mediaList = new List<MediaDetail>
            {
                TestModelProvider.GetMediaDetail(Guid.NewGuid()),
                TestModelProvider.GetMediaDetail(Guid.NewGuid()),
            };
            var virtualTourList = new List<VirtualTourDetail> { };
            var resourceResponse = new ResourceResponse { Media = mediaList, VirtualTour = virtualTourList };

            this.SetupValidEntityAndUser(entityId, userId);
            this.mediaServiceClient
                .Setup(x => x.GetResources(It.IsAny<Guid>(), It.IsAny<MediaType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resourceResponse)
                .Verifiable();

            // Act
            var result = await this.Sut.GetAsync(entityId);

            // Assert
            Assert.NotNull(result);
            this.mediaServiceClient.Verify(x => x.GetResources(It.Is<Guid>(id => id == entityId), It.Is<MediaType>(type => type == this.Sut.MediaType), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetResourceById_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var mediaDetail = TestModelProvider.GetMediaDetail(mediaId);

            this.SetupValidEntityAndUser(entityId, userId);
            this.mediaServiceClient
                .Setup(x => x.GetMediaById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var result = await this.Sut.Resource.GetById(entityId, mediaId);

            // Assert
            Assert.NotNull(result);
            var media = Assert.IsAssignableFrom<MediaDetail>(result);
            Assert.Equal(media.Id, mediaId);
            this.mediaServiceClient.Verify(x => x.GetMediaById(It.Is<Guid>(id => id == entityId), It.Is<Guid>(id => id == mediaId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteResourceById_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            this.SetupValidEntityAndUser(entityId, userId);
            this.mediaServiceClient
                .Setup(x => x.DeleteMediaById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            // Act
            await this.Sut.Resource.DeleteByIdAsync(entityId, mediaId, 25);

            // Assert
            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteResources_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(entityId, userId);

            this.mediaServiceClient
                .Setup(x => x.DeleteResources(It.IsAny<Guid>(), It.IsAny<MediaType>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            // Act
            await this.Sut.Resource.DeleteAsync(entityId);

            // Assert
            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateResource_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var simpleMedia = TestModelProvider.MediaDetail(mediaId);

            this.SetupValidEntityAndUser(entityId, userId);
            this.mediaServiceClient
                .Setup(x => x.UpdateMedia(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<MediaRequest.SimpleMedia>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            // Act
            await this.Sut.Resource.UpdateAsync(entityId, mediaId, simpleMedia);

            // Assert
            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateVirtualTour_Success()
        {
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(entityId, userId);
            var virtualTour = TestModelProvider.VirtualTour(entityId);

            await this.Sut.VirtualTour.CreateAsync(entityId, virtualTour);

            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteVirtualTour_Success()
        {
            var entityId = Guid.NewGuid();
            var virtualTourId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(entityId, userId);

            await this.Sut.VirtualTour.DeleteById(entityId, virtualTourId);

            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CopyMediaAsync_Success()
        {
            var fromEntityId = Guid.NewGuid();
            var toEntityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(toEntityId, userId);

            await this.Sut.CopyMediaAsync(fromEntityId, toEntityId);

            this.busClient.Verify(t => t.CreateSender(this.topicName), Times.Once);
            this.busSender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        protected abstract void SetupValidEntityAndUser(Guid entityId, Guid userId);
    }
}
