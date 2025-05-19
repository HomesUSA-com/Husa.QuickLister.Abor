namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
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

        [Theory]
        [InlineData(MarketStatuses.Closed)]
        public void IsValidForSubmit_PropertyFieldAreRequired(MarketStatuses mlsStatus)
        {
            var requestId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleRequestEntity(requestId);
            listing.Setup(sp => sp.MlsStatus).Returns(mlsStatus);
            listing.Setup(x => x.SaleProperty.PropertyInfo).Returns(new PropertyRecord());
            listing.Setup(sp => sp.IsValidForSubmit(It.IsAny<IUserContextProvider>())).CallBase();

            var result = listing.Object.IsValidForSubmit();

            var saleError = result.FirstOrDefault(x => x.ErrorMessage.Contains("SaleProperty"));
            ValidationResult propertyError = null;
            if (saleError is CompositeValidationResult saleCompositeValidation)
            {
                propertyError = saleCompositeValidation.Results.FirstOrDefault(x => x.ErrorMessage.Contains("PropertyInfo"));
            }

            Assert.NotNull(propertyError);
        }

        [Theory]
        [InlineData(MarketStatuses.Closed)]
        public void IsValidForSubmit_PropertyFieldAreNotRequired(MarketStatuses mlsStatus)
        {
            var requestId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleRequestEntity(requestId);
            listing.Setup(sp => sp.MlsStatus).Returns(mlsStatus);
            listing.Setup(x => x.SaleProperty.PropertyInfo).Returns(new PropertyRecord()
            {
                ConstructionStage = ConstructionStage.Complete,
                ConstructionCompletionDate = DateTime.UtcNow.AddDays(-1),
                ConstructionStartYear = 2024,
                LegalDescription = "Legal descrption test",
                TaxId = "1245",
                LotSize = "5",
                TaxLot = "2",
                LotDescription = new List<LotDescription>() { LotDescription.Agricultural },
                PropertyType = PropertySubType.Condominium,
            });
            listing.Setup(sp => sp.IsValidForSubmit(It.IsAny<IUserContextProvider>())).CallBase();

            var result = listing.Object.IsValidForSubmit();

            var saleError = result.FirstOrDefault(x => x.ErrorMessage.Contains("SaleProperty"));
            ValidationResult propertyError = null;
            if (saleError is CompositeValidationResult saleCompositeValidation)
            {
                propertyError = saleCompositeValidation.Results.FirstOrDefault(x => x.ErrorMessage.Contains("PropertyInfo"));
            }

            Assert.Null(propertyError);
        }

        [Theory]
        [InlineData(MarketStatuses.Closed)]
        public void IsValidForSubmit_ConstructionComplete_Fail(MarketStatuses mlsStatus)
        {
            var requestId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleRequestEntity(requestId);
            listing.Setup(sp => sp.MlsStatus).Returns(mlsStatus);
            listing.Setup(x => x.SaleProperty.PropertyInfo).Returns(new PropertyRecord()
            {
                ConstructionStage = ConstructionStage.Incomplete,
                ConstructionCompletionDate = DateTime.UtcNow.AddDays(1),
                LegalDescription = "Legal descrption test",
                TaxId = "1245",
                LotSize = "5",
                TaxLot = "2",
                LotDescription = new List<LotDescription>() { LotDescription.Agricultural },
                PropertyType = PropertySubType.Condominium,
            });
            listing.Setup(sp => sp.IsValidForSubmit(It.IsAny<IUserContextProvider>())).CallBase();

            var result = listing.Object.IsValidForSubmit();

            var saleError = result.FirstOrDefault(x => x.ErrorMessage.Contains("SaleProperty"));
            ValidationResult propertyError = null;
            if (saleError is CompositeValidationResult saleCompositeValidation)
            {
                propertyError = saleCompositeValidation.Results.FirstOrDefault(x => x.ErrorMessage.Contains("PropertyInfo"));
            }

            Assert.NotNull(propertyError);
        }

        [Fact]
        public void ListingSaleRequestIsNotValidForSubmit()
        {
            var requestId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleRequestEntity(requestId);

            var result = listing.Object.IsValidForSubmit();

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
        public void GenerateRequestFromCommunitySuccess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var community = CommunityTestProvider.GetCommunityEntity(communityId);
            community.Property.MlsArea = MlsArea.LW;
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

        [Fact]
        public void ChangeCompany_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var newCompanyId = Guid.NewGuid();
            var newCompanyName = "New Company Name";
            var listing = ListingTestProvider.GetListingEntity(listingId);

            // Act
            listing.ChangeCompany(newCompanyId, newCompanyName);

            // Assert
            Assert.Equal(newCompanyId, listing.CompanyId);
            Assert.Equal(newCompanyId, listing.SaleProperty.CompanyId);
            Assert.Equal(newCompanyName, listing.SaleProperty.OwnerName);
        }
    }
}
