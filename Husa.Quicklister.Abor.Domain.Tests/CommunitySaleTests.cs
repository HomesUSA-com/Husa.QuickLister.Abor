namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Contracts.Response;
    using Husa.Xml.Domain.Enums;
    using Moq;
    using Moq.Protected;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Domain.Test")]
    public class CommunitySaleTests
    {
        [Fact]
        public void AddCommunityEmployee()
        {
            // Arrange
            var communitySale = new Mock<CommunitySale>();
            var employees = new List<CommunityEmployee>();

            communitySale
                .SetupGet(c => c.Employees)
                .Returns(employees)
                .Verifiable();
            communitySale
                .Setup(c => c.AddCommunityEmployee(It.IsAny<Guid>()))
                .CallBase()
                .Verifiable();

            communitySale.Object.AddCommunityEmployee(Guid.NewGuid());
            communitySale.Verify();

            // Assert
            Assert.Single(communitySale.Object.Employees);
        }

        [Fact]
        public void DeleteCommunity_DeleteFailed_HasActiveListing_Exception()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var communitySale = new Mock<CommunitySale>();

            communitySale
                .Setup(c => c.Delete(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .CallBase()
                .Verifiable();
            communitySale
                .SetupGet(c => c.Id)
                .Returns(communityId)
                .Verifiable();

            communitySale
                .SetupGet(c => c.CanBeDeleted)
                .Returns(false)
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => communitySale.Object.Delete(communityId, deleteChildren: false));
        }

        [Fact]
        public void DeleteCommunity_DeleteSuccessDontDeleteChildren()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var communitySale = new Mock<CommunitySale>();

            communitySale
                .Setup(c => c.Delete(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .CallBase()
                .Verifiable();
            communitySale
                .SetupGet(c => c.Id)
                .Returns(communityId);

            communitySale
                .SetupGet(c => c.CanBeDeleted)
                .Returns(true)
                .Verifiable();

            communitySale.SetupProperty(c => c.IsDeleted, initialValue: false);

            // Act
            communitySale.Object.Delete(userId, deleteChildren: false);

            // Assert
            communitySale.Verify();
            communitySale.VerifyGet(c => c.SaleProperties, Times.Never);
            Assert.True(communitySale.Object.IsDeleted);
        }

        [Fact]
        public void DeleteCommunity_DeleteSuccessDeletesChildren()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var communitySale = new Mock<CommunitySale>();

            communitySale
                .Setup(c => c.Delete(
                    It.IsAny<Guid>(),
                    It.IsAny<bool>()))
                .CallBase()
                .Verifiable();
            communitySale
                .SetupGet(c => c.Id)
                .Returns(communityId);

            communitySale
                .SetupGet(c => c.CanBeDeleted)
                .Returns(true)
                .Verifiable();

            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupProperty(c => c.CommunityId, initialValue: communityId);

            communitySale.SetupGet(c => c.SaleProperties).Returns(new[] { saleProperty.Object });
            communitySale
                .Protected()
                .Setup("DeleteChildren", exactParameterMatch: true, userId)
                .CallBase();

            communitySale.SetupProperty(c => c.IsDeleted, initialValue: false);

            // Act
            communitySale.Object.Delete(userId, deleteChildren: true);

            // Assert
            communitySale.Verify();
            communitySale.VerifyGet(c => c.SaleProperties, Times.Once);
            Assert.True(communitySale.Object.IsDeleted);
            saleProperty.Verify(c => c.Delete(It.Is<Guid>(id => id == userId), It.IsAny<bool>()), Times.Once);
            saleProperty.Verify(c => c.UpdateTrackValues(It.Is<Guid>(id => id == userId), It.IsAny<bool>()), Times.Once);
            Assert.Null(saleProperty.Object.CommunityId);
        }

        [Fact]
        public void ImportFromXmlToCommunitySuccess()
        {
            // Arrange
            const string subdivisionName = "Shawnee Creek Estates";
            const string companyName = "ABC Homes";
            var communityId = Guid.NewGuid();
            var subdivisionId = Guid.NewGuid();
            var communitySale = new Mock<CommunitySale>();
            communitySale.SetupGet(x => x.OpenHouses).Returns(new List<CommunityOpenHouse>());

            var subdivisionFromXml = XmlTestProvider.GetSubdivisionResponse(subdivisionId);

            communitySale
                .Setup(c => c.ImportFromXml(
                    It.IsAny<SubdivisionResponse>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .CallBase()
                .Verifiable();

            communitySale.SetupGet(c => c.Id).Returns(communityId);

            communitySale.SetupProperty(c => c.ProfileInfo, initialValue: SetupProfileInfo(subdivisionName, companyName));
            communitySale.SetupProperty(c => c.SaleOffice, initialValue: SetupSalesOffice());
            communitySale.SetupProperty(c => c.Property, initialValue: SetupProperty(subdivisionName));
            communitySale.SetupProperty(c => c.Financial, initialValue: SetupFinancialInfo());
            communitySale.SetupProperty(c => c.SchoolsInfo, initialValue: SetupSchoolsInfo());
            communitySale.SetupProperty(c => c.Showing, initialValue: SetupShowingInfo());

            // Act
            communitySale.Object.ImportFromXml(subdivisionFromXml, companyName);

            // Assert
            communitySale.Verify();
            communitySale.Verify(c => c.UpdateAddressInfo(It.IsAny<Cities?>(), It.IsAny<Counties?>()), Times.Once);
            communitySale.Verify(c => c.UpdateProfile(It.IsAny<ProfileInfo>()), Times.Once);
            communitySale.Verify(c => c.UpdateSalesOffice(It.IsAny<CommunitySaleOffice>()), Times.Once);
            communitySale.Verify(c => c.UpdateProperty(It.IsAny<Property>()), Times.Once);
            communitySale.Verify(c => c.UpdateFinancial(It.IsAny<CommunityFinancialInfo>()), Times.Once);
            communitySale.Verify(c => c.UpdateSchools(It.IsAny<SchoolsInfo>()), Times.Once);
            communitySale.Verify(c => c.UpdateShowing(It.IsAny<CommunityShowingInfo>()), Times.Once);
            communitySale.Verify(c => c.UpdateUtilities(It.IsAny<Utilities>()), Times.Once);
            communitySale.Verify(c => c.UpdateEmailLeads(It.IsAny<EmailLead>()), Times.Once);
        }

        [Fact]
        public void ImportFromXmlToProfileSuccess()
        {
            // Arrange
            const string subdivisionName = "Shawnee Creek Estates";
            const string companyName = "ABC Homes";
            var communityId = Guid.NewGuid();
            var subdivisionId = Guid.NewGuid();
            var profileInfo = SetupProfileInfo(subdivisionName, companyName);
            var subdivisionResponse = XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId);

            // Act
            var result = ProfileInfo.ImportFromXml(subdivisionResponse, companyName, profileInfo);

            // Assert
            Assert.Equal(result.UseLatLong, profileInfo.UseLatLong);
            Assert.Equal(subdivisionResponse.Name, result.Name);
            Assert.Equal(subdivisionResponse.Latitude, result.Latitude);
            Assert.Equal(subdivisionResponse.Longitude, result.Longitude);
            Assert.Equal(subdivisionResponse.SaleOffice.Phone.CleanPhoneValue(), result.OfficePhone);
            Assert.Equal(subdivisionResponse.SaleOffice.Fax, result.Fax);
            Assert.Equal(result.OwnerName, profileInfo.OwnerName);
        }

        [Fact]
        public void ImportFromXmlToProfileWithNullProfileSuccess()
        {
            // Arrange
            const string companyName = "ABC Homes";
            var communityId = Guid.NewGuid();
            var subdivisionId = Guid.NewGuid();
            var subdivisionResponse = XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId);

            // Act
            var result = ProfileInfo.ImportFromXml(subdivisionResponse, companyName, profile: null);

            // Assert
            Assert.True(result.UseLatLong);
            Assert.Equal(subdivisionResponse.Name, result.Name);
            Assert.Equal(subdivisionResponse.Latitude, result.Latitude);
            Assert.Equal(subdivisionResponse.Longitude, result.Longitude);
            Assert.Equal(subdivisionResponse.SaleOffice.Phone.CleanPhoneValue(), result.OfficePhone);
            Assert.Equal(subdivisionResponse.SaleOffice.Fax, result.Fax);
            Assert.Equal(companyName, result.OwnerName);
        }

        [Fact]
        public void ImportFromXmlToSchoolsProfileSuccess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var subdivisionId = Guid.NewGuid();
            var schoolInfo = SetupSchoolsInfo();
            var subdivisionResponse = XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId);

            // Act
            var result = SchoolsInfo.ImportFromXml(subdivisionResponse, schoolInfo);

            // Assert
            var schoolDistrict = subdivisionResponse.SchoolDistrict.Single();
            Assert.Equal(schoolDistrict.Name, result.SchoolDistrict.Value.ToStringFromEnumMember(), ignoreCase: true);
            Assert.All(schoolDistrict.School, school =>
            {
                switch (school.Type)
                {
                    case SchoolType.Elementary:
                        Assert.Equal(school.Name, result.ElementarySchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                    case SchoolType.Middle:
                        Assert.Equal(school.Name, result.MiddleSchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                    case SchoolType.High:
                        Assert.Equal(school.Name, result.HighSchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                }
            });
        }

        [Fact]
        public void ImportFromXmlToSchoolsWithNullProfileSuccess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var subdivisionId = Guid.NewGuid();
            var subdivisionResponse = XmlTestProvider.GetSubdivisionResponse(subdivisionId, communityId);

            // Act
            var result = SchoolsInfo.ImportFromXml(subdivisionResponse, schoolsInfo: null);

            // Assert
            var schoolDistrict = subdivisionResponse.SchoolDistrict.Single();
            Assert.Equal(schoolDistrict.Name, result.SchoolDistrict.Value.ToStringFromEnumMember(), ignoreCase: true);
            Assert.All(schoolDistrict.School, school =>
            {
                switch (school.Type)
                {
                    case SchoolType.Elementary:
                        Assert.Equal(school.Name, result.ElementarySchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                    case SchoolType.Middle:
                        Assert.Equal(school.Name, result.MiddleSchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                    case SchoolType.High:
                        Assert.Equal(school.Name, result.HighSchool.Value.ToStringFromEnumMember(), ignoreCase: true);
                        break;
                }
            });
        }

        [Fact]
        public void ImportEmailLeadFromSaleOfficeInfoInXmlSuccess()
        {
            // Arrange
            var community = TestModelProvider.GetCommunitySaleEntity(Guid.NewGuid());
            var subdivisionFromXml = XmlTestProvider.GetSubdivisionResponse(Guid.NewGuid());
            subdivisionFromXml.LeadsEmails = null;
            subdivisionFromXml.SaleOffice.Email = Faker.Internet.Email();

            // Act
            var leads = EmailLead.ImportFromXml(subdivisionFromXml, community.EmailLead);

            // Assert
            Assert.Equal(subdivisionFromXml.SaleOffice.Email, leads.EmailLeadPrincipal);
        }

        [Fact]
        public void GetActiveLotListings_Success()
        {
            // Arrange
            var community = TestModelProvider.GetCommunitySaleEntity(Guid.NewGuid());
            community.LotListings = new LotListing[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LockedStatus = LockedStatus.NoLocked,
                    MlsNumber = "54444",
                    MlsStatus = MarketStatuses.Active,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    LockedStatus = LockedStatus.LockedBySystem,
                    MlsNumber = "1231431",
                    MlsStatus = MarketStatuses.Active,
                },
            };

            // Act
            var listings = community.GetActiveLotListings();

            // Assert
            Assert.Equal(2, listings.Count());
        }

        private static ProfileInfo SetupProfileInfo(string subdivisionName, string companyName) => new()
        {
            Name = subdivisionName,
            OwnerName = companyName,
            Latitude = Faker.RandomNumber.Next(),
            Longitude = Faker.RandomNumber.Next(),
            UseLatLong = true,
            BackupPhone = Faker.Phone.Number(),
            Fax = Faker.Phone.Number(),
            OfficePhone = Faker.Phone.Number(),
            EmailMailViolationsWarnings = new[] { Faker.Internet.Email() },
        };

        private static CommunitySaleOffice SetupSalesOffice() => new()
        {
            IsSalesOffice = true,
            StreetName = Faker.Address.StreetName(),
            StreetNumber = Faker.RandomNumber.Next().ToString(),
            StreetSuffix = Faker.Address.StreetSuffix(),
            SalesOfficeCity = Faker.Enum.Random<Cities>(),
            SalesOfficeZip = Faker.Address.ZipCode()[..5],
        };

        private static Property SetupProperty(string subdivisionName) => new()
        {
            Subdivision = subdivisionName,
            City = Faker.Enum.Random<Cities>(),
            County = Faker.Enum.Random<Counties>(),
            ZipCode = Faker.Address.ZipCode()[..5],
        };

        private static CommunityFinancialInfo SetupFinancialInfo() => new()
        {
            TaxRate = 12.5M,
            BuyersAgentCommission = 3,
            BuyersAgentCommissionType = CommissionType.Percent,
            TitleCompany = Faker.Company.Name(),
            HoaIncludes = TestModelProvider.GetEnumCollectionRandom<HoaIncludes>(),
        };

        private static SchoolsInfo SetupSchoolsInfo() => new()
        {
            SchoolDistrict = SchoolDistrict.Harper,
            ElementarySchool = ElementarySchool.Harper,
            MiddleSchool = MiddleSchool.Harper,
            HighSchool = HighSchool.Harper,
        };

        private static CommunityShowingInfo SetupShowingInfo() => new()
        {
            OccupantPhone = Faker.Phone.Number(),
            ContactPhone = Faker.Phone.Number(),
            Directions = Faker.Lorem.Sentence(300),
        };
    }
}
