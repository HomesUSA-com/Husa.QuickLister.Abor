namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingLockServiceTests
    {
        private readonly Mock<IListingSaleRepository> mockListingRepository = new();
        private readonly Mock<ILogger<SaleListingLockService>> mockLogger = new();
        private readonly Mock<IUserContextProvider> mockUserContextProvider = new();
        private readonly Mock<IEmailService> mockEmailService = new();
        private readonly Mock<ISaleListingNotesService> mockNotesService = new();
        private readonly Mock<IServiceSubscriptionClient> mockServiceSubscriptionClient = new();
        private readonly Mock<ISaleListingRequestRepository> mockListingRequestRepository = new();
        private readonly SaleListingLockService sut;

        public SaleListingLockServiceTests(ApplicationServicesFixture fixture)
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
            var properties = typeof(SaleListingLockService).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            var propertyInfo = properties.FirstOrDefault(p =>
                p.Name == "UnlockedListingDtoProjection" &&
                p.DeclaringType == typeof(SaleListingLockService));

            var projectionDelegate = propertyInfo.GetValue(this.sut) as Func<SaleListing, UnlockedListingDto>;

            // Assert
            Assert.NotNull(projectionDelegate);
            var testListing = TestModelProvider.GetListingSaleEntity(companyId: Guid.NewGuid(), createStub: true);
            var result = projectionDelegate(testListing);

            Assert.NotNull(result);
            Assert.Equal(testListing.CompanyId, result.CompanyId);
        }

        [Fact]
        public void Constructor_ShouldInitializeCorrectly()
        {
            Assert.NotNull(this.sut);
            Assert.IsAssignableFrom<ISaleListingLockService>(this.sut);
        }
    }
}
