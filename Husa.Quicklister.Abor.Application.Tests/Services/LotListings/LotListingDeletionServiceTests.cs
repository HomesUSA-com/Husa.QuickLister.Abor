namespace Husa.Quicklister.Abor.Application.Tests.Services.LotListings
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests.LotListings;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LotListingDeletionServiceTests
    {
        private readonly Mock<ILotListingRepository> listingRepositoryMock = new();
        private readonly Mock<ILogger<LotListingDeletionService>> loggerMock = new();
        private readonly Mock<IUserContextProvider> userContextProviderMock = new();
        private readonly Mock<IEmailService> emailServiceMock = new();
        private readonly Mock<ILotListingMediaService> lotListingMediaService = new();
        private readonly Mock<IPhotoBusService> photoBusServiceMock = new();
        private readonly LotListingDeletionService sut;

        public LotListingDeletionServiceTests()
        {
            this.sut = new LotListingDeletionService(
                this.listingRepositoryMock.Object,
                this.loggerMock.Object,
                this.userContextProviderMock.Object,
                this.emailServiceMock.Object,
                this.lotListingMediaService.Object,
                this.photoBusServiceMock.Object);
        }

        [Fact]
        public void DraftListingDtoProjection_ShouldMapCorrectly()
        {
            // Arrange & Act
            var properties = typeof(LotListingDeletionService).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

            var propertyInfo = properties.FirstOrDefault(p =>
                p.Name == "DraftListingDtoProjection" &&
                p.DeclaringType == typeof(LotListingDeletionService));

            var projectionDelegate = propertyInfo.GetValue(this.sut) as Func<LotListing, DraftListingDto>;

            // Assert
            Assert.NotNull(projectionDelegate);
            var testListing = LotTestProvider.GetListingEntity(companyId: Guid.NewGuid());
            var result = projectionDelegate(testListing);

            Assert.NotNull(result);
            Assert.Equal(testListing.CompanyId, result.CompanyId);
        }
    }
}
