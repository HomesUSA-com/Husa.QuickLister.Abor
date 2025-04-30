namespace Husa.Quicklister.Abor.Api.Tests.ServiceBus
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.PhotoService.Domain.Enums;
    using Husa.PhotoService.ServiceBus.Messages;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Api.ServiceBus.Subscribers;
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

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();

            serviceCollection.AddSingleton(this.listingPhotoServiceMock.Object);
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
