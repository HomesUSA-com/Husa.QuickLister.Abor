namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Xunit;
    using CompanyExtensions = Husa.CompanyServicesManager.Api.Contracts.Response;

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
        public void UpdateCompanyEmailLeadsWhenEmailLeadsIsNull()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);

            // Act
            var exception = Record.Exception(() => community.UpdateCompanyEmailLeads(null));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateCompanyEmailLeadsWhenEmailLeadsIsEmpty()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            var emailLeads = Enumerable.Empty<CompanyExtensions.EmailLead>();

            // Act
            var exception = Record.Exception(() => community.UpdateCompanyEmailLeads(emailLeads));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateCompanyEmailLeadsWhenEmailLeadsWithValidEmailLeads()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            var emailLeads = new List<CompanyExtensions.EmailLead>
            {
                new CompanyExtensions.EmailLead { EmailPriority = EmailPriority.One, EntityType = EmailEntityType.Sale, Email = "principal@example.com" },
                new CompanyExtensions.EmailLead { EmailPriority = EmailPriority.Two, EntityType = EmailEntityType.Sale, Email = "secondary@example.com" },
                new CompanyExtensions.EmailLead { EmailPriority = EmailPriority.Three, EntityType = EmailEntityType.Sale, Email = "third@example.com" },
                new() { EmailPriority = EmailPriority.Four, EntityType = EmailEntityType.Sale, Email = "fourth@example.com" },
                new() { EmailPriority = EmailPriority.Five, EntityType = EmailEntityType.Sale, Email = "fifth@example.com" },
                new() { EmailPriority = EmailPriority.Six, EntityType = EmailEntityType.Sale, Email = "other@example.com" },
            };

            // Act
            community.UpdateCompanyEmailLeads(emailLeads);

            // Assert
            Assert.Equal("principal@example.com", community.EmailLead.EmailLeadPrincipal);
            Assert.Equal("secondary@example.com", community.EmailLead.EmailLeadSecondary);
            Assert.Equal("third@example.com", community.EmailLead.EmailLeadThird);
            Assert.Equal("fourth@example.com", community.EmailLead.EmailLeadFourth);
            Assert.Equal("fifth@example.com", community.EmailLead.EmailLeadFifth);
            Assert.Equal("other@example.com", community.EmailLead.EmailLeadOther);
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
                HoaIncludes = TestModelProvider.GetEnumCollectionRandom<HoaIncludes>(),
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
    }
}
