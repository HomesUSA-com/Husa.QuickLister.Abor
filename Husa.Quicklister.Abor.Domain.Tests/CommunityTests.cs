namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class CommunityTests
    {
        public CommunityTests()
            : base()
        {
        }

        [Fact]
        public void ImportFromListing_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.AddressInfo.ZipCode = "11111";

            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            community.Property.ZipCode = "22222";

            // Act
            community.ImportFromListing(listing);

            // Assert
            Assert.Equal(listing.SaleProperty.AddressInfo.ZipCode, community.Property.ZipCode);
        }

        [Fact]
        public void UpdatePropertyHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            var property = new Property()
            {
                Subdivision = "test",
                ZipCode = "1111",
            };

            community.UpdateProperty(property);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateSalesOfficeHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            var saleOffice = new CommunitySaleOffice()
            {
                StreetName = "StreetName",
                StreetNumber = "StreetNumber",
            };

            community.UpdateSalesOffice(saleOffice);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateSalesOfficeNotHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            community.SaleOffice.IsSalesOffice = false;

            var saleOffice = new CommunitySaleOffice()
            {
                IsSalesOffice = true,
            };

            community.UpdateSalesOffice(saleOffice);

            Assert.Empty(community.Changes);
        }

        [Fact]
        public void UpdateUtilitiesHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            community.Utilities.Fireplaces = 0;

            var saleOffice = new Utilities()
            {
                Fireplaces = 1,
            };

            community.UpdateUtilities(saleOffice);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateFinancialHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            var saleOffice = new CommunityFinancialInfo()
            {
                ProposedTerms = TestModelProvider.GetEnumCollectionRandom<ProposedTerms>(),
            };

            community.UpdateFinancial(saleOffice);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateSchoolsHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            var saleOffice = new SchoolsInfo()
            {
                SchoolDistrict = Faker.Enum.Random<SchoolDistrict>(),
            };

            community.UpdateSchools(saleOffice);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateShowingHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            community.Showing.Directions = "Old Directios";

            var saleOffice = new CommunityShowingInfo()
            {
                Directions = "Directions",
            };

            community.UpdateShowing(saleOffice);

            Assert.NotEmpty(community.Changes);
        }

        [Fact]
        public void UpdateShowingNotHasChanges()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            community.Showing.RealtorContactEmail = "test@test.com";

            var saleOffice = new CommunityShowingInfo()
            {
                RealtorContactEmail = "test@test.com",
            };

            community.UpdateShowing(saleOffice);

            Assert.Empty(community.Changes);
        }
    }
}
