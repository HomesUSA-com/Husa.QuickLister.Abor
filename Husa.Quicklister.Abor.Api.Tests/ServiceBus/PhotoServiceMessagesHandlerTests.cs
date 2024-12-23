namespace Husa.Quicklister.Abor.Api.Tests.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Downloader.Sabor.ServiceBus.Contracts;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class PhotoServiceMessagesHandlerTests
    {
        private readonly Mock<IPhotoServiceSubscriber> subscriberMock = new();
        private readonly Mock<ISubscriptionClient> subscriptionClientMock = new();
        private readonly Mock<ISaleListingPhotoService> listingPhotoServiceMock = new();
        private readonly Mock<ICommunityPhotoService> communityPhotoServiceMock = new();
        private readonly Mock<IPlanPhotoService> planPhotoServiceMock = new();
        private readonly Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        private readonly Mock<ILogger<PhotoServiceMessagesHandler>> loggerMock = new();

        [Fact]
        public async Task HandlerAsync_Residential()
        {
            // Arrange
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            var photoRequest = new PhotoRequestCreatedMessage
            {
                Id = Guid.NewGuid(),
                SysCreatedOn = DateTime.Now,
                PropertyId = Guid.NewGuid(),
                Type = PhotoRequestType.Residential,
            };
            var message = ApplicationServicesFixture.BuildBusMessage(photoRequest);

            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.listingPhotoServiceMock.Verify(s => s.AssignLatestPhotoRequest(photoRequest.PropertyId, photoRequest.Id, photoRequest.SysCreatedOn), Times.Once);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        [Fact]
        public async Task HandlerAsync_Community()
        {
            // Arrange
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            var photoRequest = new PhotoRequestCreatedMessage
            {
                Id = Guid.NewGuid(),
                SysCreatedOn = DateTime.Now,
                PropertyId = Guid.NewGuid(),
                Type = PhotoRequestType.Community,
            };
            var message = ApplicationServicesFixture.BuildBusMessage(photoRequest);

            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, default);

            // Assert
            this.communityPhotoServiceMock.Verify(s => s.AssignLatestPhotoRequest(photoRequest.PropertyId, photoRequest.Id, photoRequest.SysCreatedOn), Times.Once);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        [Fact]
        public async Task HandlerAsync_Plan()
        {
            // Arrange
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            var photoRequest = new PhotoRequestCreatedMessage
            {
                Id = Guid.NewGuid(),
                SysCreatedOn = DateTime.Now,
                PropertyId = Guid.NewGuid(),
                Type = PhotoRequestType.Plan,
            };
            var message = ApplicationServicesFixture.BuildBusMessage(photoRequest);

            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, default);

            // Assert
            this.planPhotoServiceMock.Verify(s => s.AssignLatestPhotoRequest(photoRequest.PropertyId, photoRequest.Id, photoRequest.SysCreatedOn), Times.Once);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        [Fact]
        public async Task ProcessUnmanagedMessageTypeIsIgnoredCorrectlyTestAsync()
        {
            // Arrange
            const string mlsNumber = "1122334455";
            const string mediaTitle = "some-title";
            var mediaId = Guid.NewGuid();
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
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

            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.communityPhotoServiceMock.Verify(s => s.AssignLatestPhotoRequest(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();

            serviceCollection.AddSingleton(this.listingPhotoServiceMock.Object);
            serviceCollection.AddSingleton(this.communityPhotoServiceMock.Object);
            serviceCollection.AddSingleton(this.planPhotoServiceMock.Object);
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

        private PhotoServiceMessagesHandler GetSut() => new(
            this.subscriberMock.Object,
            this.serviceScopeFactoryMock.Object,
            this.loggerMock.Object);
    }
}
