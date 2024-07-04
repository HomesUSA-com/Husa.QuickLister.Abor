namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using System.Diagnostics.CodeAnalysis;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class CallForwardServiceTests
    {
        private readonly Mock<IListingSaleRepository> listingRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ILogger<CallForwardServiceTestClass>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        [Fact]
        public void GetCommunityPhone_ReturnsOfficePhone_WhenOfficePhoneIsNotEmpty()
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity(createCommunity: true);
            listing.SaleProperty.Community.ProfileInfo.OfficePhone = "123456789";
            listing.SaleProperty.Community.ProfileInfo.BackupPhone = "987654321";
            var sut = this.GetSut();

            // Act
            var result = sut.PublicGetCommunityPhone(listing);

            // Assert
            Assert.Equal("123456789", result);
        }

        [Fact]
        public void GetCommunityPhone_ReturnsBackupPhone_WhenOfficePhoneIsEmpty()
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity(createCommunity: true);
            listing.SaleProperty.Community.ProfileInfo.OfficePhone = string.Empty;
            listing.SaleProperty.Community.ProfileInfo.BackupPhone = "987654321";
            var sut = this.GetSut();

            // Act
            var result = sut.PublicGetCommunityPhone(listing);

            // Assert
            Assert.Equal("987654321", result);
        }

        [Fact]
        public void GetCommunityPhone_ReturnsEmpty_WhenCommunityIsNull()
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity();
            var sut = this.GetSut();

            // Act
            var result = sut.PublicGetCommunityPhone(listing);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(ListType.Residential)]
        [InlineData(ListType.Lease)]
        [InlineData(ListType.Lots)]
        public void GetCentralizedPhone_ReturnsCorrectValues_ForResidentialListing(ListType listType)
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity(listType);
            var company = new CompanyDetail
            {
                PhoneLeadInfo = new PhoneLeadInfoResponse
                {
                    IsCentralizedForSale = listType == ListType.Residential,
                    CentralizeLeadPhone = "123456789",
                    IsCentralizedForLease = listType == ListType.Lease,
                    CentralizeLeadPhoneForLease = "987654321",
                    IsCentralizedForLot = listType == ListType.Lots,
                    CentralizeLeadPhoneForLot = "555555555",
                },
            };
            var sut = this.GetSut();

            // Act
            var result = sut.PublicGetCentralizedPhone(listing, company);

            // Assert
            Assert.True(result.Centralized);
            if (listType == ListType.Residential)
            {
                Assert.Equal("123456789", result.Phone);
            }
            else if (listType == ListType.Lease)
            {
                Assert.Equal("987654321", result.Phone);
            }
            else if (listType == ListType.Lots)
            {
                Assert.Equal("555555555", result.Phone);
            }
        }

        [Fact]
        public void GetCentralizedPhone_ReturnsFalseAndEmpty_ForUnknownListingType()
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity((ListType)999);
            var company = new CompanyDetail();
            var sut = this.GetSut();

            // Act
            var result = sut.PublicGetCentralizedPhone(listing, company);

            // Assert
            Assert.False(result.Centralized);
            Assert.Equal(string.Empty, result.Phone);
        }

        private CallForwardServiceTestClass GetSut()
            => new(
                this.listingRepository.Object,
                this.userContextProvider.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object);
    }
}
