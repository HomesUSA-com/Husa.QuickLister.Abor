namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
    using Xunit;
    using static Microsoft.Azure.Amqp.Serialization.SerializableType;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class ListingTests
    {
        [Fact]
        public void UpdateBaseListingInfo_Added_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingEntity(listingId);
            var listingMock = Mock.Get(listing);
            var userId = Guid.NewGuid();

            listingMock
                .Setup(x => x.UpdateBaseListingInfo(ListType.Residential, new decimal(2.33), DateTime.UtcNow, DateTime.UtcNow, MarketStatuses.Active, LockedStatus.LockedByUser, userId))
                .CallBase()
                .Verifiable();

            // Act
            listing.UpdateBaseListingInfo(ListType.Residential, new decimal(2.33), DateTime.UtcNow, DateTime.UtcNow, MarketStatuses.Active, LockedStatus.LockedByUser, userId);

            // Assert
            listingMock.Verify(r => r.UpdateBaseListingInfo(ListType.Residential, new decimal(2.33), It.IsAny<DateTime>(), It.IsAny<DateTime>(), MarketStatuses.Active, LockedStatus.LockedByUser, It.IsAny<Guid>()), Times.Once);
        }

        [Theory]
        [InlineData(LockedStatus.LockedByUser)]
        [InlineData(LockedStatus.LockedBySystem)]
        public void UpdateBaseListingInfo_AwaitingMlsUpdate_Error(LockedStatus lockedStatus)
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.LockedStatus = lockedStatus;
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() => listing.UpdateBaseListingInfo(ListType.Residential, new decimal(2.33), DateTime.UtcNow, DateTime.UtcNow, MarketStatuses.Active, LockedStatus.LockedByUser, userId));
        }

        [Fact]
        public void UpdateBaseListingInfo_LockedButNotSubmitted_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.LockedStatus = LockedStatus.LockedNotSubmitted;
            listing.LockedBy = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<DomainException>(() => listing.UpdateBaseListingInfo(ListType.Residential, new decimal(2.33), DateTime.UtcNow, DateTime.UtcNow, MarketStatuses.Active, LockedStatus.LockedByUser, userId));
        }

        [Fact]
        public void UpdateBaseListingInfo_LockedButNotSubmitted_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.LockedStatus = LockedStatus.LockedNotSubmitted;
            var userId = Guid.NewGuid();
            listing.LockedBy = userId;
            var newListPrice = Faker.RandomNumber.Next(50000, 2000000);

            // Act
            listing.UpdateBaseListingInfo(ListType.Residential, newListPrice, DateTime.UtcNow, DateTime.UtcNow, MarketStatuses.Active, LockedStatus.LockedByUser, userId);

            // Assert
            Assert.Equal(newListPrice, listing.ListPrice);
        }

        [Fact]
        public void UpdatePhotosDeclineInformation_Declined_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingEntity(listingId);
            var listingMock = Mock.Get(listing);
            listingMock.Setup(x => x.DeclinePhotos(userId)).CallBase();

            // Act
            listingMock.Object.DeclinePhotos(userId);

            // Assert
            listingMock.Verify();
        }

        [Fact]
        public void HasInMlsTrue()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingEntity(listingId);
            var listingMock = Mock.Get(listing);
            listingMock.Setup(x => x.MlsNumber).Returns("12345678");
            listingMock.Setup(x => x.IsInMls).CallBase();

            // Act
            var result = listingMock.Object.IsInMls;

            // Assert
            listingMock.Verify();
            Assert.True(result);
        }

        [Fact]
        public void HasInMlsFalse()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingEntity(listingId);
            var listingMock = Mock.Get(listing);
            listingMock.Setup(x => x.MlsNumber).Returns(string.Empty);
            listingMock.Setup(x => x.IsInMls).CallBase();

            // Act
            var result = listingMock.Object.IsInMls;

            // Assert
            listingMock.Verify();
            Assert.False(result);
        }
    }
}
