namespace Husa.Quicklister.Abor.Domain.Tests.Extensions.Lot
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions.Lot;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ImportDataExtensionsTests
    {
        [Fact]
        public void CloneListing_ClonedListingHasSamePropertiesAsSourceListing()
        {
            // Arrange
            var sourceListing = new LotListing();

            // Set properties of sourceListing here
            var targetListing = new LotListing();

            // Act
            targetListing.ImportDataFromListing(sourceListing);

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
