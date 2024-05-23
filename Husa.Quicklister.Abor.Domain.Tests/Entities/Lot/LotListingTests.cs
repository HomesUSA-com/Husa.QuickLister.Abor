namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Tests.Providers;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
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

        [Fact]
        public void GenerateRequestFromCommunity_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listing = new LotListing()
            {
                Community = new(Guid.NewGuid(), "Company", "company"),
            };
            var request = TestProviderLotRequest.GetLotListingRequestMock();
            request.Setup(x => x.Clone()).CallBase().Verifiable();

            // Act
            listing.GenerateRequestFromCommunity(request.Object, userId);

            // Assert
            request.Verify(r => r.UpdateTrackValues(userId, It.IsAny<bool>()), Times.Once);
        }
    }
}
