namespace Husa.Quicklister.Abor.Application.Tests.Extensions
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Application.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Moq;
    using Xunit;

    public class ListingExtensionsTests
    {
        [Fact]
        public void ToUnlockedListingDto_ShouldMapAllProperties_WhenGivenValidSaleListing()
        {
            // Arrange
            var ownerName = "Test Owner";

            var community = new Mock<CommunitySale>();
            community.Setup(x => x.Id).Returns(Guid.NewGuid());
            community.Setup(x => x.ProfileInfo).Returns(new ProfileInfo("Test Community", ownerName));

            var saleProperty = new Mock<SaleProperty>();
            saleProperty.Setup(x => x.Community).Returns(community.Object);
            saleProperty.Setup(x => x.OwnerName).Returns(ownerName);

            var listingMock = new Mock<SaleListing>();
            listingMock.Setup(x => x.MlsNumber).Returns("12345");
            listingMock.Setup(x => x.Address).Returns("123 Test Street");
            listingMock.Setup(x => x.CompanyId).Returns(Guid.NewGuid());
            listingMock.Setup(x => x.SaleProperty).Returns(saleProperty.Object);
            var listing = listingMock.Object;

            // Act
            var result = ListingDtoExtensions.ToUnlockedListingDto(listing);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listing.MlsNumber, result.MlsNumber);
            Assert.Equal(listing.Address, result.Address);
            Assert.Equal(listing.SaleProperty.Community.Id, result.CommunityId);
            Assert.Equal(listing.SaleProperty.Community.ProfileInfo.Name, result.CommunityName);
            Assert.Equal(listing.CompanyId, result.CompanyId);
            Assert.Equal(listing.SaleProperty.OwnerName, result.CompanyName);
            Assert.Equal(MarketCode.Austin, result.MarketCode);
        }

        [Fact]
        public void ToUnlockedListingDto_ShouldHandleNullValues_AppropriatelyWithoutException()
        {
            var saleProperty = new Mock<SaleProperty>();

            var listingMock = new Mock<SaleListing>();
            listingMock.Setup(x => x.SaleProperty).Returns(saleProperty.Object);
            var listing = listingMock.Object;

            // Act & Assert
            var result = ListingDtoExtensions.ToUnlockedListingDto(listing);

            // We're just asserting that no exception is thrown and the mapping completes
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.CommunityId);
            Assert.Null(result.CommunityName);
            Assert.Null(result.CompanyName);
        }

        [Fact]
        public void ToUnlockedLotDto_ShouldMapAllProperties_WhenGivenValidSaleListing()
        {
            // Arrange
            var ownerName = "Test Owner";

            var community = new Mock<CommunitySale>();
            community.Setup(x => x.Id).Returns(Guid.NewGuid());
            community.Setup(x => x.ProfileInfo).Returns(new ProfileInfo("Test Community", ownerName));

            var listingMock = new Mock<LotListing>();
            listingMock.Setup(x => x.MlsNumber).Returns("12345");
            listingMock.Setup(x => x.Address).Returns("123 Test Street");
            listingMock.Setup(x => x.CompanyId).Returns(Guid.NewGuid());
            listingMock.Setup(x => x.Community).Returns(community.Object);
            listingMock.Setup(x => x.OwnerName).Returns(ownerName);
            var listing = listingMock.Object;

            // Act
            var result = ListingDtoExtensions.ToUnlockedLotDto(listing);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listing.MlsNumber, result.MlsNumber);
            Assert.Equal(listing.Address, result.Address);
            Assert.Equal(listing.Community.Id, result.CommunityId);
            Assert.Equal(listing.Community.ProfileInfo.Name, result.CommunityName);
            Assert.Equal(listing.CompanyId, result.CompanyId);
            Assert.Equal(listing.OwnerName, result.CompanyName);
            Assert.Equal(MarketCode.Austin, result.MarketCode);
        }

        [Fact]
        public void ToUnlockedLotDto_ShouldHandleNullValues_AppropriatelyWithoutException()
        {
            var listingMock = new Mock<LotListing>();
            var listing = listingMock.Object;

            // Act & Assert
            var result = ListingDtoExtensions.ToUnlockedLotDto(listing);

            // We're just asserting that no exception is thrown and the mapping completes
            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.CommunityId);
            Assert.Null(result.CommunityName);
            Assert.Null(result.CompanyName);
        }
    }
}
