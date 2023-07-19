namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class SaleListingTests
    {
        [Fact]
        public void UpdatePropertyInfo_UpdateComplete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            var propertyInfo = TestModelProvider.GetPropertyInfo();
            var listingMock = Mock.Get(listing);

            listingMock
                .Setup(x => x.SaleProperty.UpdatePropertyInfo(It.IsAny<PropertyInfo>()))
                .CallBase()
                .Verifiable();

            // Act
            listing.SaleProperty.UpdatePropertyInfo(propertyInfo);

            // Assert
            listingMock.Verify(r => r.SaleProperty.UpdatePropertyInfo(propertyInfo), Times.Once);
        }

        [Fact]
        public void UpdatePropertyInfo_ParameterNull_Fail()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            PropertyInfo propertyInfo = null;
            var listingMock = Mock.Get(listing);

            listingMock
                .Setup(x => x.SaleProperty.UpdatePropertyInfo(It.IsAny<PropertyInfo>()))
                .CallBase()
                .Verifiable();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => listing.SaleProperty.UpdatePropertyInfo(propertyInfo));

            listingMock.Verify(r => r.SaleProperty.UpdatePropertyInfo(propertyInfo), Times.Once);
        }

        [Fact]
        public void ListingSaleRequestIsNotValidForSubmit()
        {
            var requestId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleRequestEntity(requestId);

            var result = listing.IsValidForSubmit();

            Assert.NotNull(result);
        }

        [Fact]
        public void AssignMlsNumberToListingSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            const string mlsNumber = "mls1234578";
            var sut = new Mock<SaleListing>();
            sut.SetupAllProperties();
            sut.Setup(ls => ls.CompleteListingRequest(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<MarketStatuses>(), It.IsAny<ActionType>(), It.IsAny<bool>())).CallBase();

            // Act
            sut.Object.CompleteListingRequest(mlsNumber, userId, MarketStatuses.Active, ActionType.NewListing, false);

            // Assert
            Assert.Equal(mlsNumber, sut.Object.MlsNumber);
            Assert.Equal(LockedStatus.LockedBySystem, sut.Object.LockedStatus);
        }

        [Fact]
        public void NoAssignMlsNumberToListingWithDifferentMlsNumber()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentMlsNumber = "mls87654321";
            const string mlsNumber = "mls1234578";
            var sut = new Mock<SaleListing>();
            sut.SetupGet(ls => ls.Id).Returns(userId);
            sut.SetupGet(ls => ls.MlsNumber).Returns(mlsNumber);
            sut.SetupGet(ls => ls.LockedStatus).Returns(LockedStatus.LockedBySystem);
            sut.Setup(ls => ls.CompleteListingRequest(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<MarketStatuses>(), It.IsAny<ActionType>(), It.IsAny<bool>())).CallBase();

            // Act
            sut.Object.CompleteListingRequest(currentMlsNumber, userId, MarketStatuses.Active, ActionType.NewListing, false);

            // Assert
            Assert.Equal(LockedStatus.LockedBySystem, sut.Object.LockedStatus);
        }

        [Fact]
        public void AssignMlsNumberWithEmptyMlsNumber()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mlsNumber = string.Empty;
            var sut = new Mock<SaleListing>();
            sut.SetupGet(ls => ls.Id).Returns(userId);
            sut.SetupGet(ls => ls.MlsNumber).Returns(mlsNumber);
            sut.Setup(ls => ls.CompleteListingRequest(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<MarketStatuses>(), It.IsAny<ActionType>(), It.IsAny<bool>())).CallBase();

            // Act && Assert
            Assert.Throws<DomainException>(() => sut.Object.CompleteListingRequest(mlsNumber, userId, MarketStatuses.Active, ActionType.NewListing, false));
        }

        [Fact]
        public void AssignMlsNumberWithNullMlsNumber()
        {
            // Arrange
            var userId = Guid.NewGuid();
            const string mlsNumber = null;
            var sut = new Mock<SaleListing>();
            sut.SetupGet(ls => ls.Id).Returns(userId);
            sut.SetupGet(ls => ls.MlsNumber).Returns(mlsNumber);
            sut.Setup(ls => ls.CompleteListingRequest(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<MarketStatuses>(), It.IsAny<ActionType>(), It.IsAny<bool>())).CallBase();

            // Act && Assert
            Assert.Throws<DomainException>(() => sut.Object.CompleteListingRequest(mlsNumber, userId, MarketStatuses.Active, ActionType.NewListing, false));
        }

        [Fact]
        public void LockByUserSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            listing.SetupProperty(l => l.LockedBy, initialValue: null);
            listing.SetupProperty(l => l.LockedOn, initialValue: null);
            listing.SetupProperty(l => l.LockedStatus, initialValue: LockedStatus.NoLocked);
            listing.Setup(ls => ls.LockByUser(It.IsAny<Guid>())).CallBase();

            // Act
            listing.Object.LockByUser(userId);

            // Assert
            Assert.Equal(userId, listing.Object.LockedBy);
            Assert.NotNull(listing.Object.LockedOn);
            Assert.Equal(LockedStatus.LockedByUser, listing.Object.LockedStatus);
        }

        [Fact]
        public void LockAndReturnNoActionWhenLockedByIsNotSet()
        {
            // Arrange
            var sut = new Mock<SaleListing>();
            sut.SetupAllProperties();
            sut.SetupProperty(l => l.LockedBy, initialValue: null);

            sut.Setup(ls => ls.LockAndReturn()).CallBase();

            // Act
            sut.Object.LockAndReturn();

            // Assert
            sut.VerifySet(l => l.LockedStatus = It.Is<LockedStatus>(status => status == LockedStatus.ReturnedByMlsAdmin), Times.Never);
        }

        [Fact]
        public void LockAndReturnSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupAllProperties();
            sut.SetupProperty(l => l.LockedBy, initialValue: userId);
            sut.SetupProperty(l => l.LockedStatus, initialValue: LockedStatus.LockedBySystem);

            sut.Setup(ls => ls.LockAndReturn()).CallBase();

            // Act
            sut.Object.LockAndReturn();

            // Assert
            Assert.Equal(LockedStatus.ReturnedByMlsAdmin, sut.Object.LockedStatus);
        }

        [Fact]
        public void UnlockListingWhenIsLockedBySystemAndDownloaderEnabledThrowsDomainException()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupGet(l => l.LockedStatus).Returns(LockedStatus.LockedBySystem);

            sut.Setup(ls => ls.Unlock(It.IsAny<bool>())).CallBase();

            // Assert
            Assert.Throws<DomainException>(() => sut.Object.Unlock(allowUnlock: false));
        }

        [Theory]
        [InlineData(LockedStatus.LockedByUser)]
        [InlineData(LockedStatus.LockedNotSubmitted)]
        [InlineData(LockedStatus.ReturnedByMlsAdmin)]
        [InlineData(LockedStatus.NoLocked)]
        public void UnlockListingWhenDownloaderEnabledSuccess(LockedStatus lockedStatus)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupProperty(l => l.LockedStatus, initialValue: lockedStatus);
            sut.SetupProperty(l => l.LockedBy, initialValue: userId);
            sut.Setup(ls => ls.Unlock(It.IsAny<bool>())).CallBase();

            // Assert
            sut.Object.Unlock(allowUnlock: true);

            Assert.Null(sut.Object.LockedBy);
            Assert.Null(sut.Object.LockedOn);
            Assert.Equal(LockedStatus.NoLocked, sut.Object.LockedStatus);
        }

        [Theory]
        [InlineData(false, true, RoleEmployee.CompanyAdmin)]
        [InlineData(false, true, RoleEmployee.All)]
        [InlineData(true, true, RoleEmployee.SalesEmployee)]
        [InlineData(false, false, RoleEmployee.Photographer)]
        public void CanUnlockWhenCurrentUserIsSameAsLockUserSuccess(bool lockedByUser, bool isMlsAdministrator, RoleEmployee employeeRole)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupGet(l => l.LockedBy).Returns(userId);
            sut.SetupGet(l => l.IsLockedByUser).Returns(lockedByUser);
            sut.Setup(ls => ls.CanUnlock(It.IsAny<IUserContext>())).CallBase();

            var userContext = new Mock<IUserContext>();
            userContext.SetupGet(l => l.Id).Returns(userId);
            userContext.SetupGet(l => l.IsMLSAdministrator).Returns(isMlsAdministrator);
            userContext.SetupGet(l => l.EmployeeRole).Returns(employeeRole);

            // Act
            var canUnlock = sut.Object.CanUnlock(userContext.Object);

            // Assert
            Assert.True(canUnlock);
        }

        [Theory]
        [InlineData(false, true, RoleEmployee.CompanyAdmin)]
        [InlineData(false, true, RoleEmployee.All)]
        [InlineData(true, true, RoleEmployee.Photographer)]
        [InlineData(false, false, RoleEmployee.Photographer)]
        public void CanUnlockWhenCurrentUserIsDifferentThanLockUserSuccess(bool lockedByUser, bool isMlsAdministrator, RoleEmployee employeeRole)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentUserId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupGet(l => l.LockedBy).Returns(userId);
            sut.SetupGet(l => l.IsLockedByUser).Returns(lockedByUser);
            sut.Setup(ls => ls.CanUnlock(It.IsAny<IUserContext>())).CallBase();

            var userContext = new Mock<IUserContext>();
            userContext.SetupGet(l => l.Id).Returns(currentUserId);
            userContext.SetupGet(l => l.IsMLSAdministrator).Returns(isMlsAdministrator);
            userContext.SetupGet(l => l.EmployeeRole).Returns(employeeRole);

            // Act
            var canUnlock = sut.Object.CanUnlock(userContext.Object);

            // Assert
            Assert.True(canUnlock);
        }

        [Theory]
        [InlineData(LockedStatus.LockedBySystem)]
        [InlineData(LockedStatus.LockedByUser)]
        public void IsLockedByUserSuccess(LockedStatus lockedStatus)
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupGet(l => l.LockedStatus).Returns(lockedStatus);
            sut.SetupGet(l => l.IsLockedByUser).CallBase();

            // Act && Assert
            Assert.True(sut.Object.IsLockedByUser);
        }

        [Theory]
        [InlineData(LockedStatus.NoLocked)]
        [InlineData(LockedStatus.LockedNotSubmitted)]
        [InlineData(LockedStatus.ReturnedByMlsAdmin)]
        public void IsNotLockedByUserSuccess(LockedStatus lockedStatus)
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var sut = new Mock<SaleListing>();
            sut.SetupGet(l => l.Id).Returns(listingId);
            sut.SetupGet(l => l.LockedStatus).Returns(lockedStatus);
            sut.SetupGet(l => l.IsLockedByUser).CallBase();

            // Act && Assert
            Assert.False(sut.Object.IsLockedByUser);
        }

        [Fact]
        public void GenerateRequestFromCommunitySuccess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var community = CommunityTestProvider.GetCommunityEntity(communityId);
            community.Property.MlsArea = MlsArea.HundredOne;
            var changes = new List<string> { nameof(community.Property.MlsArea) };
            community.UpdateChanges(nameof(community.Property), changes);

            var listing = ListingTestProvider.GetListingEntity(listingId);
            listing.SaleProperty = TestModelProvider.GetFullSalePropertyWithStaticValues(listing.SalePropertyId);
            listing.SaleProperty.Community = community;

            var newSaleListingRequest = new Mock<SaleListingRequest>();
            newSaleListingRequest.SetupAllProperties();

            var lastSaleListingRequest = new Mock<SaleListingRequest>();
            lastSaleListingRequest.SetupAllProperties();
            lastSaleListingRequest.Setup(x => x.Clone()).Returns(newSaleListingRequest.Object);

            // Act
            var result = listing.GenerateRequestFromCommunity(lastSaleListingRequest.Object, userId);

            // Assert
            Assert.Equal(ResponseCode.Success, result.Code);
        }
    }
}
