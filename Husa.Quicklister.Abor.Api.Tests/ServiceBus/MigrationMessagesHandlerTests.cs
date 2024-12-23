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
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Models.Migration;
    using Husa.Quicklister.Extensions.ServiceBus.Contracts;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class MigrationMessagesHandlerTests
    {
        private readonly Mock<IMigrationSubscriber> subscriberMock = new();
        private readonly Mock<ISubscriptionClient> subscriptionClientMock = new();

        private readonly Mock<ISaleListingMigrationService> saleListingMigrationServiceMock = new();

        private readonly Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        private readonly Mock<ILogger<MigrationMessagesHandler>> loggerMock = new();
        private readonly ApplicationServicesFixture fixture;

        public MigrationMessagesHandlerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task ProcessMigrateListingMessageSuccessTestAsync()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var mlsNumber = "122344";
            var migrateListingMessage = new MigrateListingMessage
            {
                CompanyId = companyId,
                MlsNumber = mlsNumber,
                UpdateListing = true,
                MigrateFullListing = false,
            };

            var message = ApplicationServicesFixture.BuildBusMessage(migrateListingMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.saleListingMigrationServiceMock.Verify(
                ds => ds.MigrateListings(
                    It.IsAny<Guid>(),
                    It.Is<MigrateListingFilter>(f => f.MlsNumber.Equals(mlsNumber) && f.UpdateListing && !f.MigrateFullListing)),
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
            this.saleListingMigrationServiceMock.Verify(s => s.MigrateListings(It.IsAny<Guid>(), It.IsAny<MigrateListingFilter>()), Times.Never);
            this.subscriptionClientMock.Verify(s => s.CompleteAsync(It.Is<string>(token => string.IsNullOrEmpty(token))), Times.Once);
        }

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();
            serviceCollection.AddSingleton(this.saleListingMigrationServiceMock.Object);
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

        private MigrationMessagesHandler GetSut() => new(
            this.subscriberMock.Object,
            this.serviceScopeFactoryMock.Object,
            this.fixture.DownloaderUserInfo.Object,
            this.loggerMock.Object);
    }
}
