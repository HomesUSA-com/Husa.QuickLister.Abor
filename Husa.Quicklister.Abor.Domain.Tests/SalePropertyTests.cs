namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class SalePropertyTests
    {
        [Fact]
        public void UpdateFeatures_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var features = new FeaturesInfo();

            listingMock
                .Setup(x => x.SaleProperty.UpdateFeatures(It.IsAny<FeaturesInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateFeatures(features);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateFeatures(features), Times.Once);
        }

        [Fact]
        public void UpdateFeatures_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            FeaturesInfo features = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFeatures(It.IsAny<FeaturesInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateFeatures(features));
            listingMock.Verify(r => r.SaleProperty.UpdateFeatures(features), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateFinancial(financial);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            FinancialInfo financial = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_BuyersAgentCommissionInvalid_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.BuyersAgentCommission = -1;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_BuyersAgentCommissionInvalidRangePercent_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.BuyersAgentCommission = 9;
            financial.BuyersAgentCommissionType = CommissionType.Percent;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_BuyersAgentCommissionInvalidRangeAmmount_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.HasAgentBonus = true;
            financial.BuyersAgentCommission = 51000;
            financial.BuyersAgentCommissionType = CommissionType.Amount;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_AgentBonusAmountInvalid_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.AgentBonusAmount = -1;
            financial.HasAgentBonus = true;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_AgentBonusAmountInvalidRangePercent_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.HasAgentBonus = true;
            financial.AgentBonusAmount = 9;
            financial.AgentBonusAmountType = CommissionType.Percent;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateFinancial_AgentBonusAmountInvalidRangeAmmount_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var financial = new FinancialInfo();
            financial.HasAgentBonus = true;
            financial.AgentBonusAmount = 51000;
            financial.AgentBonusAmountType = CommissionType.Amount;

            listingMock
                .Setup(x => x.SaleProperty.UpdateFinancial(It.IsAny<FinancialInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<DomainException>(() => listing.SaleProperty.UpdateFinancial(financial));
            listingMock.Verify(r => r.SaleProperty.UpdateFinancial(financial), Times.Once);
        }

        [Fact]
        public void UpdateShowing_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var showing = new ShowingInfo();

            listingMock
                .Setup(x => x.SaleProperty.UpdateShowing(It.IsAny<ShowingInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateShowing(showing);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateShowing(showing), Times.Once);
        }

        [Fact]
        public void UpdateShowing_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            ShowingInfo showing = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateShowing(It.IsAny<ShowingInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateShowing(showing));
            listingMock.Verify(r => r.SaleProperty.UpdateShowing(showing), Times.Once);
        }

        [Fact]
        public void UpdateSchools_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var schools = new SchoolsInfo();

            listingMock
                .Setup(x => x.SaleProperty.UpdateSchools(It.IsAny<SchoolsInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateSchools(schools);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateSchools(schools), Times.Once);
        }

        [Fact]
        public void UpdateSchools_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            SchoolsInfo schools = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateSchools(It.IsAny<SchoolsInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateSchools(schools));
            listingMock.Verify(r => r.SaleProperty.UpdateSchools(schools), Times.Once);
        }

        [Fact]
        public void UpdateSpacesDimensions_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var spacesDimensions = new SpacesDimensionsInfo();

            listingMock
                .Setup(x => x.SaleProperty.UpdateSpacesDimensions(It.IsAny<SpacesDimensionsInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateSpacesDimensions(spacesDimensions);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateSpacesDimensions(spacesDimensions), Times.Once);
        }

        [Fact]
        public void UpdateSpacesDimensions_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            SpacesDimensionsInfo spacesDimensions = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateSpacesDimensions(It.IsAny<SpacesDimensionsInfo>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateSpacesDimensions(spacesDimensions));
            listingMock.Verify(r => r.SaleProperty.UpdateSpacesDimensions(spacesDimensions), Times.Once);
        }

        [Fact]
        public void UpdateRooms_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var rooms = new Mock<List<ListingSaleRoom>>();
            rooms.Object.Add(new ListingSaleRoom(Guid.NewGuid(), RoomType.MasterBedroom, RoomLevel.Main));

            listingMock
                .Setup(x => x.SaleProperty.UpdateRooms(It.IsAny<List<ListingSaleRoom>>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateRooms(rooms.Object);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateRooms(rooms.Object), Times.Once);
        }

        [Fact]
        public void UpdateRooms_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            List<ListingSaleRoom> rooms = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateRooms(It.IsAny<List<ListingSaleRoom>>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateRooms(rooms));
            listingMock.Verify(r => r.SaleProperty.UpdateRooms(rooms), Times.Once);
        }

        [Fact]
        public void UpdateHoas_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            var hoas = new List<SaleListingHoa>
            {
                HoaTestProvider.GetListingSaleHoaEntity(listingId),
            };

            listingMock
                .Setup(x => x.SaleProperty.UpdateHoas(It.IsAny<List<SaleListingHoa>>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdateHoas(hoas);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdateHoas(hoas), Times.Once);
        }

        [Fact]
        public void UpdateHoas_Failed_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingMock = Mock.Get(listing);
            List<SaleListingHoa> hoas = null;

            listingMock
                .Setup(x => x.SaleProperty.UpdateHoas(It.IsAny<List<SaleListingHoa>>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdateHoas(hoas));
            listingMock.Verify(r => r.SaleProperty.UpdateHoas(hoas), Times.Once);
        }

        [Fact]
        public void UpdateFinancialInfo_CloneUsed_Success()
        {
            var financial = new FinancialInfo
            {
                AgentBonusAmount = new decimal(3.44),
                HasAgentBonus = true,
            };

            var financialClonned = financial.ImportFinancialFromCommunity(new CommunityFinancialInfo());

            Assert.NotEqual(financial, financialClonned);
        }

        [Fact]
        public void UpdateFeaturesInfo_CloneUsed_Success()
        {
            // Arrange
            var features = new FeaturesInfo
            {
                FireplaceDescription = TestModelProvider.GetEnumCollectionRandom<FireplaceDescription>(),
                RoofDescription = TestModelProvider.GetEnumCollectionRandom<RoofDescription>(),
                CoolingSystem = TestModelProvider.GetEnumCollectionRandom<CoolingSystem>(),
            };

            // Act
            var featuresClonned = features.ImportFeaturesFromCommunity(new Utilities());

            // Assert
            Assert.NotEqual(features, featuresClonned);
        }

        [Fact]
        public void UpdateSchoolsInfo_CloneUsed_Success()
        {
            // Arrange
            var schools = new SchoolsInfo
            {
                SchoolDistrict = SchoolDistrict.LaGrange,
                ElementarySchool = ElementarySchool.Gattis,
                HighSchool = HighSchool.Genesis,
                MiddleSchool = MiddleSchool.LagoVista,
            };

            // Act
            var schoolsClonned = schools.ImportSchools(new SchoolsInfo());

            // Assert
            Assert.NotEqual(schools, schoolsClonned);
        }

        [Fact]
        public void HasNewChangesFromCommunity_ListingHasChanges_ReturnsTrue()
        {
            // Arrange
            var communityId = Guid.NewGuid();

            var community = CommunityTestProvider.GetCommunityEntity(communityId);
            community.Property.ZipCode = "string";
            community.Property.MlsArea = MlsArea.WW;
            var changes = new List<string> { nameof(community.Property.ZipCode), nameof(community.Property.MlsArea) };
            community.UpdateChanges(nameof(community.Property), changes);

            var saleProperty = ListingTestProvider.GetSalePropertyEntity(communityId: communityId);
            saleProperty.Community = community;

            // Act
            var result = saleProperty.UpdateListingFromCommunitySubmit();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasNewChangesFromCommunity_ListingHasChanges_ReturnsFalse()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = CommunityTestProvider.GetCommunityEntity(communityId);
            community.Changes?.Clear();
            var saleProperty = ListingTestProvider.GetSalePropertyEntity(communityId: communityId);
            saleProperty.Community = community;

            // Act
            var result = saleProperty.UpdateListingFromCommunitySubmit();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateListingFromCommunityChangesSaleOfficeSuccess()
        {
            // Arrange
            var salePropertyId = Guid.NewGuid();
            var saleProperty = TestModelProvider.GetFullSalePropertyWithStaticValues(salePropertyId);
            saleProperty.SalesOfficeInfo.StreetName = "SalePropertySaleOffice";
            saleProperty.Community.SaleOffice.StreetName = "CommunitySaleOffice";
            saleProperty.Community.UpdateChanges(nameof(saleProperty.Community.SaleOffice), new string[] { nameof(saleProperty.Community.SaleOffice.StreetName) });

            // Act
            var result = saleProperty.UpdateListingFromCommunitySubmit();

            // Assert
            Assert.True(result);
            Assert.Equal(saleProperty.SalesOfficeInfo.StreetName, saleProperty.SalesOfficeInfo.StreetName);
        }

        [Fact]
        public void CloneSalePropertyFailsWhenSalesInfoIsNull()
        {
            // Arrange
            const string ownerName = "fake-owner";
            var companyId = Guid.NewGuid();
            var communityProfileId = Guid.NewGuid();
            var planProfileId = Guid.NewGuid();
            var addressInfo = TestModelProvider.GetDefaultAddressInfo();
            var propertyInfo = TestModelProvider.GetDefaultPropertyInfo();
            var sut = new SaleProperty(
                addressInfo.StreetName,
                addressInfo.StreetNumber,
                addressInfo.City,
                addressInfo.State,
                addressInfo.ZipCode,
                addressInfo.County,
                propertyInfo.ConstructionCompletionDate,
                companyId,
                ownerName,
                communityProfileId,
                planProfileId);

            // Act
            var clonedProperty = sut.Clone();

            // Assert
            Assert.NotNull(clonedProperty);
            Assert.Equal(addressInfo.StreetName, clonedProperty.AddressInfo.StreetName);
            Assert.Equal(addressInfo.StreetNumber, clonedProperty.AddressInfo.StreetNumber);
            Assert.Equal(addressInfo.City, clonedProperty.AddressInfo.City);
            Assert.Equal(addressInfo.State, clonedProperty.AddressInfo.State);
        }

        [Fact]
        public void SetDeleted_AlreadyDeleted_Fail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listing = new Mock<SaleProperty>();
            listing.SetupAllProperties();
            listing.Setup(x => x.IsDeleted).Returns(true);
            listing.Setup(x => x.Delete(userId, false)).CallBase();

            // Act
            listing.Object.Delete(userId, false);

            // Assert
            listing.Verify();
        }

        [Fact]
        public void SetDeleted_Deleted_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listing = new Mock<SaleProperty>();
            listing.SetupAllProperties();
            listing.Setup(x => x.IsDeleted).Returns(false);
            listing.Setup(x => x.Delete(userId, false)).CallBase();

            // Act
            listing.Object.Delete(userId, false);

            // Assert
            listing.Verify();
        }

        [Fact]
        public void AddNewListingThrowsArgumentNullExceptionWhenListingInfoIsNotProvided()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.AddListing(
                    It.IsAny<ListingValueObject>(),
                    It.IsAny<ListingSaleStatusFieldsInfo>(),
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>(),
                    It.IsAny<Guid>()))
                .CallBase();
            var listingStatusInfo = new Mock<ListingSaleStatusFieldsInfo>();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.AddListing(
                listingInfo: null,
                listingStatusInfo: listingStatusInfo.Object,
                salePropertyInfo: salePropertyInfo.Object,
                rooms: Array.Empty<ListingSaleRoom>(),
                hoas: Array.Empty<SaleListingHoa>(),
                companyId));
        }

        [Fact]
        public void AddNewListingThrowsArgumentNullExceptionWhenListingStatusInfoIsNotProvided()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.AddListing(
                    It.IsAny<ListingValueObject>(),
                    It.IsAny<ListingSaleStatusFieldsInfo>(),
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>(),
                    It.IsAny<Guid>()))
                .CallBase();
            var listingInfo = new Mock<ListingValueObject>();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.AddListing(
                listingInfo: listingInfo.Object,
                listingStatusInfo: null,
                salePropertyInfo: salePropertyInfo.Object,
                rooms: Array.Empty<ListingSaleRoom>(),
                hoas: Array.Empty<SaleListingHoa>(),
                companyId));
        }

        [Fact]
        public void AddNewListingThrowsArgumentNullExceptionWhenSalePropertyIsNotProvided()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.AddListing(
                    It.IsAny<ListingValueObject>(),
                    It.IsAny<ListingSaleStatusFieldsInfo>(),
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>(),
                    It.IsAny<Guid>()))
                .CallBase();
            var listingInfo = new Mock<ListingValueObject>();
            var listingStatusInfo = new Mock<ListingSaleStatusFieldsInfo>();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.AddListing(
                listingInfo: listingInfo.Object,
                listingStatusInfo: listingStatusInfo.Object,
                salePropertyInfo: null,
                rooms: Array.Empty<ListingSaleRoom>(),
                hoas: Array.Empty<SaleListingHoa>(),
                companyId));
        }

        [Fact]
        public void AddNewListingWhenStatusIsNotInProgress()
        {
            // Arrange
            const string mlsNumber = "12312312";
            const int cdom = 180;
            const decimal price = 230000;
            var referenceDate = new DateTime(2021, 11, 4, 0, 0, 0, DateTimeKind.Utc);
            var companyId = Guid.NewGuid();
            var salePropertyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupGet(sp => sp.Id).Returns(salePropertyId);
            saleProperty
                .Setup(sp => sp.AddListing(
                    It.IsAny<ListingValueObject>(),
                    It.IsAny<ListingSaleStatusFieldsInfo>(),
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>(),
                    It.IsAny<Guid>()))
                .CallBase();

            saleProperty.SetupProperty(sp => sp.SaleListings, initialValue: new List<SaleListing>());

            var listingInfo = new Mock<ListingValueObject>();
            listingInfo.SetupGet(sp => sp.CDOM).Returns(cdom);
            listingInfo.SetupGet(sp => sp.DOM).Returns(cdom);
            listingInfo.SetupGet(sp => sp.ListDate).Returns(referenceDate);
            listingInfo.SetupGet(sp => sp.ListPrice).Returns(price);
            listingInfo.SetupGet(sp => sp.ListType).Returns(ListType.Residential);
            listingInfo.SetupGet(sp => sp.MarketModifiedOn).Returns(referenceDate.AddMonths(12));
            listingInfo.SetupGet(sp => sp.MlsNumber).Returns(mlsNumber);
            listingInfo.SetupGet(sp => sp.MlsStatus).Returns(MarketStatuses.Cancelled);

            var listingStatusInfo = new Mock<ListingSaleStatusFieldsInfo>();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act
            var addedListing = saleProperty.Object.AddListing(
                listingInfo: listingInfo.Object,
                listingStatusInfo: listingStatusInfo.Object,
                salePropertyInfo: salePropertyInfo.Object,
                rooms: Array.Empty<ListingSaleRoom>(),
                hoas: Array.Empty<SaleListingHoa>(),
                companyId);

            // Assert
            Assert.NotNull(addedListing);
            saleProperty.Verify(
                sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()),
                Times.Never);
            Assert.Equal(LockedStatus.NoLocked, addedListing.LockedStatus);
            var propertyListing = Assert.Single(saleProperty.Object.SaleListings);
            Assert.Equal(addedListing.MlsNumber, propertyListing.MlsNumber);
        }

        [Fact]
        public void AddNewListingWhenStatusIsInProgress()
        {
            // Arrange
            const string mlsNumber = "12312312";
            const int cdom = 180;
            const decimal price = 230000;
            var referenceDate = new DateTime(2021, 11, 4, 0, 0, 0, DateTimeKind.Utc);
            var companyId = Guid.NewGuid();
            var salePropertyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupGet(sp => sp.Id).Returns(salePropertyId);
            saleProperty
                .Setup(sp => sp.AddListing(
                    It.IsAny<ListingValueObject>(),
                    It.IsAny<ListingSaleStatusFieldsInfo>(),
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>(),
                    It.IsAny<Guid>()))
                .CallBase();

            saleProperty.SetupProperty(sp => sp.SaleListings, initialValue: new List<SaleListing>());

            var listingInfo = new Mock<ListingValueObject>();
            listingInfo.SetupGet(sp => sp.CDOM).Returns(cdom);
            listingInfo.SetupGet(sp => sp.DOM).Returns(cdom);
            listingInfo.SetupGet(sp => sp.ListDate).Returns(referenceDate);
            listingInfo.SetupGet(sp => sp.ListPrice).Returns(price);
            listingInfo.SetupGet(sp => sp.ListType).Returns(ListType.Residential);
            listingInfo.SetupGet(sp => sp.MarketModifiedOn).Returns(referenceDate.AddMonths(12));
            listingInfo.SetupGet(sp => sp.MlsNumber).Returns(mlsNumber);
            listingInfo.SetupGet(sp => sp.MlsStatus).Returns(MarketStatuses.Active);

            var listingStatusInfo = new Mock<ListingSaleStatusFieldsInfo>();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act
            var addedListing = saleProperty.Object.AddListing(
                listingInfo: listingInfo.Object,
                listingStatusInfo: listingStatusInfo.Object,
                salePropertyInfo: salePropertyInfo.Object,
                rooms: Array.Empty<ListingSaleRoom>(),
                hoas: Array.Empty<SaleListingHoa>(),
                companyId);

            // Assert
            Assert.NotNull(addedListing);
            Assert.Equal(LockedStatus.NoLocked, addedListing.LockedStatus);
            saleProperty.Verify();
            saleProperty.Verify(
                sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()),
                Times.Once);
            var propertyListing = Assert.Single(saleProperty.Object.SaleListings);
            Assert.Equal(addedListing.MlsNumber, propertyListing.MlsNumber);
        }

        [Fact]
        public void ApplyMarketUpdateThrowsArgumentNullExceptionWhenSalePropertyInfoIsNotProvided()
        {
            // Arrange
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()))
                .CallBase();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.ApplyMarketUpdate(
                salePropertyInfo: null,
                roomsInfo: Array.Empty<ListingSaleRoom>(),
                hoasInfo: Array.Empty<SaleListingHoa>()));
        }

        [Fact]
        public void ApplyMarketUpdateThrowsArgumentNullExceptionWhenRoomsInfoIsNotProvided()
        {
            // Arrange
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()))
                .CallBase();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.ApplyMarketUpdate(
                salePropertyInfo: salePropertyInfo.Object,
                roomsInfo: null,
                hoasInfo: Array.Empty<SaleListingHoa>()));
        }

        [Fact]
        public void ApplyMarketUpdateThrowsArgumentNullExceptionWhenHoaInfoIsNotProvided()
        {
            // Arrange
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()))
                .CallBase();
            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.ApplyMarketUpdate(
                salePropertyInfo: salePropertyInfo.Object,
                roomsInfo: Array.Empty<ListingSaleRoom>(),
                hoasInfo: null));
        }

        [Fact]
        public void ApplyMarketUpdateSuccess()
        {
            // Arrange
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.ApplyMarketUpdate(
                    It.IsAny<SalePropertyValueObject>(),
                    It.IsAny<IEnumerable<ListingSaleRoom>>(),
                    It.IsAny<IEnumerable<SaleListingHoa>>()))
                .CallBase();

            var salePropertyInfo = new Mock<SalePropertyValueObject>();

            // Act
            saleProperty.Object.ApplyMarketUpdate(
                salePropertyInfo: salePropertyInfo.Object,
                roomsInfo: Array.Empty<ListingSaleRoom>(),
                hoasInfo: Array.Empty<SaleListingHoa>());

            // Assert
            saleProperty.Verify(sp => sp.FillSalesPropertyInformation(It.IsAny<SalePropertyValueObject>()), Times.Once);
            saleProperty.Verify(sp => sp.UpdateRooms(It.IsAny<IEnumerable<ListingSaleRoom>>()), Times.Once);
            saleProperty.Verify(sp => sp.UpdateHoas(It.IsAny<IEnumerable<SaleListingHoa>>()), Times.Once);
        }

        [Fact]
        public void FillSalesPropertyInformationThrowsArgumentNullException()
        {
            // Arrange
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(sp => sp.FillSalesPropertyInformation(It.IsAny<SalePropertyValueObject>()))
                .CallBase();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.FillSalesPropertyInformation(salePropertyInfo: null));
        }

        [Theory]
        [InlineData(7, 3)]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(5, 2)]
        public void FillSalesPropertyInformationSuccess(int fireplaces, int newFireplaces)
        {
            // Arrange
            const int maxFireplaces = 3;
            var expectedFireplaces = ((fireplaces > maxFireplaces && newFireplaces == 3) || newFireplaces == fireplaces) ? fireplaces : newFireplaces;
            const string owner = "some-owner";
            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupAllProperties();
            saleProperty.SetupProperty(sp => sp.OwnerName, initialValue: "old-owner");
            saleProperty.SetupProperty(sp => sp.FeaturesInfo.Fireplaces, initialValue: fireplaces);
            saleProperty
                .Setup(sp => sp.FillSalesPropertyInformation(It.IsAny<SalePropertyValueObject>()))
                .CallBase();
            var featuresInfo = new Mock<FeaturesInfo>();
            featuresInfo.SetupGet(sg => sg.Fireplaces).Returns(newFireplaces);
            var salePropertyInfo = new Mock<SalePropertyValueObject>();
            salePropertyInfo.SetupGet(sp => sp.OwnerName).Returns(owner);
            salePropertyInfo.SetupGet(sp => sp.PropertyInfo).Returns(new Mock<PropertyInfo>().Object);
            salePropertyInfo.SetupGet(sp => sp.AddressInfo).Returns(new Mock<AddressInfo>().Object);
            salePropertyInfo.SetupGet(sp => sp.FeaturesInfo).Returns(featuresInfo.Object);
            salePropertyInfo.SetupGet(sp => sp.SchoolsInfo).Returns(new Mock<SchoolsInfo>().Object);
            salePropertyInfo.SetupGet(sp => sp.ShowingInfo).Returns(new Mock<ShowingInfo>().Object);
            salePropertyInfo.SetupGet(sp => sp.SpacesDimensionsInfo).Returns(new Mock<SpacesDimensionsInfo>().Object);
            salePropertyInfo.SetupGet(sp => sp.FinancialInfo).Returns(new Mock<FinancialInfo>().Object);

            // Act
            saleProperty.Object.FillSalesPropertyInformation(salePropertyInfo: salePropertyInfo.Object);

            // Assert
            Assert.Equal(owner, saleProperty.Object.OwnerName);
            Assert.Equal(expectedFireplaces, saleProperty.Object.FeaturesInfo.Fireplaces);
        }

        [Fact]
        public void ImportOpenHouseInfoFromMarketWhenMarketOpenHouseIsNullThrowsArgumentNullException()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupGet(sp => sp.Id).Returns(propertyId);

            saleProperty
                .Setup(x => x.ImportOpenHouseInfoFromMarket(It.IsAny<List<SaleListingOpenHouse>>()))
                .CallBase()
                .Verifiable();

            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => saleProperty.Object.ImportOpenHouseInfoFromMarket(openHouses: null));
        }

        [Fact]
        public void ImportOpenHouseInfoFromMarketWhenOpenHouseDataExistsStopsProcess()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            saleProperty.SetupGet(sp => sp.Id).Returns(propertyId);

            var propertyOpenHouse = new Mock<SaleListingOpenHouse>();
            saleProperty
                .SetupGet(sp => sp.OpenHouses)
                .Returns(new[] { propertyOpenHouse.Object });

            saleProperty
                .Setup(x => x.ImportOpenHouseInfoFromMarket(It.IsAny<List<SaleListingOpenHouse>>()))
                .CallBase()
                .Verifiable();

            // Act
            var isOpenHouseImported = saleProperty.Object.ImportOpenHouseInfoFromMarket(openHouses: Array.Empty<SaleListingOpenHouse>());

            // Assert
            Assert.False(isOpenHouseImported);
        }

        [Fact]
        public void ImportOpenHouseInfoFromMarketWhenNoOpenHouseDataExistsTheOpenHouseIsAdded()
        {
            // Arrange
            var propertyId = Guid.NewGuid();
            var saleProperty = new Mock<SaleProperty>();
            var date = new DateTime(2022, 11, 7, 0, 0, 0, DateTimeKind.Utc);
            saleProperty.SetupGet(sp => sp.Id).Returns(propertyId);
            saleProperty
                .SetupGet(sp => sp.OpenHouses)
                .Returns(new List<SaleListingOpenHouse>());

            var showingInfo = new Mock<ShowingInfo>();

            saleProperty
                .SetupGet(sp => sp.ShowingInfo)
                .Returns(showingInfo.Object);

            saleProperty
                .Setup(x => x.ImportOpenHouseInfoFromMarket(It.IsAny<IEnumerable<SaleListingOpenHouse>>()))
                .CallBase();

            var openHouseInfo = new SaleListingOpenHouse(
                propertyId,
                type: OpenHouseType.Monday,
                startTime: date.AddHours(8).TimeOfDay,
                endTime: date.AddHours(18).TimeOfDay,
                refreshments: true,
                lunch: true);

            // Act
            var isOpenHouseImported = saleProperty.Object.ImportOpenHouseInfoFromMarket(openHouses: new[] { openHouseInfo });

            // Assert
            Assert.True(isOpenHouseImported);
            showingInfo.Verify(sp => sp.EnableOpenHouse(It.Is<bool>(showOpenHouseWhenPending => !showOpenHouseWhenPending)), Times.Once);
        }
    }
}
