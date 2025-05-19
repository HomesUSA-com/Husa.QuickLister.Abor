namespace Husa.Quicklister.Abor.Application.Tests.Services.LotListings
{
    using System;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    public class LotListingPhotoServiceTests
    {
        private readonly Mock<IUserContextProvider> mockUserContextProvider = new();
        private readonly Mock<IPhotoServiceClient> mockPhotoClient = new();
        private readonly Mock<IPhotoBusService> mockBusService = new();
        private readonly Mock<ILotListingRepository> mockLotListingRepository = new();
        private readonly Mock<IServiceSubscriptionClient> mockServiceSubscriptionClient = new();
        private readonly Mock<ILogger<LotListingPhotoService>> mockLogger = new();
        private readonly LotListingPhotoService sut;

        public LotListingPhotoServiceTests()
        {
            this.sut = new LotListingPhotoService(
                 this.mockUserContextProvider.Object,
                 this.mockPhotoClient.Object,
                 this.mockBusService.Object,
                 this.mockLotListingRepository.Object,
                 this.mockServiceSubscriptionClient.Object,
                 this.mockLogger.Object);
        }

        [Fact]
        public async Task GetMigratePropertyMessage_WithValidLotListing_SetsCorrectLotListingName()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var entity = new Mock<LotListing>();
            entity.Setup(entity => entity.Id).Returns(entityId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.MLSAdministrator);

            this.mockUserContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.mockLotListingRepository.Setup(x => x.GetById(entityId, It.IsAny<bool>())).ReturnsAsync(entity.Object);
            this.mockBusService
                .Setup(b => b.GetPropertyBusMessage(entityId, It.IsAny<Request.Property>()))
                .Returns(new Husa.PhotoService.ServiceBus.Messages.Contracts.PropertyBusMessage())
                .Verifiable();

            var property = new Request.Property()
            {
                Id = entityId,
            };

            // Act
            var result = await this.sut.GetMigratePropertyMessage(property, 1, companyId, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyId, result.CompanyId);
            Assert.Equal(PhotoService.Domain.Enums.PropertyType.Lot, result.Property.Type);
        }
    }
}
