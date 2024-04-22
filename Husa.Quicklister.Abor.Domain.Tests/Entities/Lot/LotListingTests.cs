namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
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

        [Fact]
        public void CloneListing_ClonedListingHasSamePropertiesAsSourceListing()
        {
            // Arrange
            var sourceListing = new LotListing();

            // Set properties of sourceListing here
            var targetListing = new LotListing();

            // Act
            targetListing.CloneListing(sourceListing);

            // Assert
            Assert.Equal(sourceListing.CommunityId, targetListing.CommunityId);
            Assert.Equal(sourceListing.CompanyId, targetListing.CompanyId);
            Assert.Equal(sourceListing.OwnerName, targetListing.OwnerName);
            Assert.Equal(sourceListing.AddressInfo, targetListing.AddressInfo);
            Assert.Equal(sourceListing.PropertyInfo, targetListing.PropertyInfo);
            Assert.Equal(sourceListing.FeaturesInfo, targetListing.FeaturesInfo);
            Assert.Equal(sourceListing.FinancialInfo, targetListing.FinancialInfo);
            Assert.Equal(sourceListing.SchoolsInfo, targetListing.SchoolsInfo);
            Assert.Equal(sourceListing.ShowingInfo, targetListing.ShowingInfo);
        }

        [Fact]
        public void ImportDataFromCommunity_ImportedListingHasSamePropertiesAsCommunitySale()
        {
            // Arrange
            var communitySale = new CommunitySale(Guid.NewGuid(), "CommunityName", "CompanyName")
            {
                Utilities = new()
                {
                    WaterSource = new[] { WaterSource.Public },
                },
            };

            // Set properties of communitySale here
            var listing = new LotListing();

            // Act
            listing.ImportDataFromCommunity(communitySale);

            // Assert
            Assert.Equal(communitySale.Id, listing.CommunityId);
            Assert.Equal(communitySale.Utilities.WaterSource, listing.FeaturesInfo.WaterSource);
        }
    }
}
