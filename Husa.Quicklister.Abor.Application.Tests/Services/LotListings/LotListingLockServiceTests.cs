namespace Husa.Quicklister.Abor.Application.Tests.Services.LotListings
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class LotListingLockServiceTests
    {
        private readonly Mock<ILotListingRepository> mockListingRepository = new();
        private readonly Mock<ILogger<LotListingLockService>> mockLogger = new();
        private readonly Mock<IUserContextProvider> mockUserContextProvider = new();
        private readonly Mock<IEmailService> mockEmailService = new();
        private readonly Mock<ILotListingNotesService> mockNotesService = new();
        private readonly Mock<IServiceSubscriptionClient> mockServiceSubscriptionClient = new();
        private readonly Mock<ILotListingRequestRepository> mockListingRequestRepository = new();
        private readonly LotListingLockService sut;

        public LotListingLockServiceTests(ApplicationServicesFixture fixture)
        {
            this.sut = new(
                this.mockListingRepository.Object,
                this.mockLogger.Object,
                this.mockUserContextProvider.Object,
                this.mockEmailService.Object,
                this.mockNotesService.Object,
                this.mockServiceSubscriptionClient.Object,
                this.mockListingRequestRepository.Object,
                fixture.Options.Object);
        }

        [Fact]
        public void UnlockedListingDtoProjection_ShouldBeSetToListingExtensionsToUnlockedListingDto()
        {
            // Arrange & Act
            var properties = typeof(LotListingLockService).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            var propertyInfo = properties.FirstOrDefault(p =>
                p.Name == "UnlockedListingDtoProjection" &&
                p.DeclaringType == typeof(LotListingLockService));

            var projectionDelegate = propertyInfo.GetValue(this.sut) as Func<LotListing, UnlockedListingDto>;

            // Assert
            Assert.NotNull(projectionDelegate);
            var testListing = TestModelProvider.GetLotListingEntity(companyId: Guid.NewGuid(), createStub: true);
            var result = projectionDelegate(testListing);

            Assert.NotNull(result);
            Assert.Equal(testListing.CompanyId, result.CompanyId);
        }

        [Fact]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.NotNull(this.sut);
            Assert.IsAssignableFrom<ILotListingLockService>(this.sut);
        }
    }
}
