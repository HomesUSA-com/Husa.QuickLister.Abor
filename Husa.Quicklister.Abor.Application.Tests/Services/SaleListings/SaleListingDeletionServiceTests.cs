namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class SaleListingDeletionServiceTests
    {
        private readonly Mock<IListingSaleRepository> listingRepositoryMock = new();
        private readonly Mock<ILogger<SaleListingDeletionService>> loggerMock = new();
        private readonly Mock<IUserContextProvider> userContextProviderMock = new();
        private readonly Mock<IEmailService> emailServiceMock = new();
        private readonly Mock<ISaleListingMediaService> saleListingMediaService = new();
        private readonly SaleListingDeletionService sut;

        public SaleListingDeletionServiceTests()
        {
            this.sut = new SaleListingDeletionService(
                this.listingRepositoryMock.Object,
                this.loggerMock.Object,
                this.userContextProviderMock.Object,
                this.emailServiceMock.Object,
                this.saleListingMediaService.Object);
        }

        [Fact]
        public void DraftListingDtoProjection_ShouldMapCorrectly()
        {
            // Arrange & Act
            var properties = typeof(SaleListingDeletionService).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            var propertyInfo = properties.FirstOrDefault(p =>
                p.Name == "DraftListingDtoProjection" &&
                p.DeclaringType == typeof(SaleListingDeletionService));

            var projectionDelegate = propertyInfo.GetValue(this.sut) as Func<SaleListing, DraftListingDto>;

            // Assert
            Assert.NotNull(projectionDelegate);
            var testListing = TestModelProvider.GetListingSaleEntity(companyId: Guid.NewGuid(), createStub: true);
            var result = projectionDelegate(testListing);

            Assert.NotNull(result);
            Assert.Equal(testListing.CompanyId, result.CompanyId);
        }
    }
}
