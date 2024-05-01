namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class LotListingTests
    {
        [Fact]
        public void CompleteListingRequest_ListingIsUpdatedWithProvidedInformation()
        {
            // Arrange
            var listing = new LotListing();
            var mlsNumber = "12345";
            var userId = Guid.NewGuid();
            var requestStatus = MarketStatuses.Active;
            var actionType = ActionType.ActiveTransfer;
            var isDownloaderEnabled = true;

            // Act
            listing.CompleteListingRequest(mlsNumber, userId, requestStatus, actionType, isDownloaderEnabled);

            // Assert
            Assert.Equal(mlsNumber, listing.MlsNumber);
            Assert.Equal(requestStatus, listing.MlsStatus);
            Assert.Equal(userId, listing.LockedBy);
            Assert.NotNull(listing.PublishInfo);
            Assert.Equal(userId, listing.PublishInfo.PublishUser);
            Assert.NotNull(listing.PublishInfo.PublishDate);
        }
    }
}
