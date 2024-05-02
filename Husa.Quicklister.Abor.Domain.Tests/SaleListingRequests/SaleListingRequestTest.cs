namespace Husa.Quicklister.Abor.Domain.Tests.SaleListingRequests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Common;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class SaleListingRequestTest
    {
        public SaleListingRequestTest()
        {
        }

        [Fact]
        public void GetSummaryWhenTheListPriceChangedSuccess()
        {
            // Arrange
            const int expectedFields = 2;
            var creationDateTime = new DateTime(2022, 11, 4, 0, 0, 0, DateTimeKind.Utc);
            var oldCompleteRequest = GetListingRequest(creationDateTime);
            oldCompleteRequest.SetupGet(s => s.ListPrice).Returns(120000);

            var sut = GetListingRequest(creationDateTime);
            sut.SetupGet(s => s.ListPrice).Returns(150000);
            sut.Setup(s => s.GetSummary(It.IsAny<SaleListingRequest>())).CallBase();
            sut.Setup(s => s.GetRequestSummary(It.IsAny<SaleListingRequest>())).CallBase();

            var propertyRecord = new Mock<SalePropertyRecord>();
            propertyRecord
                .Setup(p => p.GetSummarySections(It.Is<SalePropertyRecord>(record => record == null)))
                .Returns(GetSalePropertySummary())
                .Verifiable();

            sut.SetupGet(s => s.SaleProperty).Returns(propertyRecord.Object);

            var statusFieldsRecord = new Mock<SaleStatusFieldsRecord>();
            statusFieldsRecord
                .Setup(p => p.GetSummary(It.Is<SaleStatusFieldsRecord>(p => p == null), It.IsAny<MarketStatuses>()))
                .Returns((SummarySection)null)
                .Verifiable();
            sut.SetupGet(s => s.StatusFieldsInfo).Returns(statusFieldsRecord.Object);

            // Act
            var summaryResult = sut.Object.GetSummary(oldCompleteRequest.Object);

            // Assert
            var section = Assert.Single(summaryResult);
            Assert.Equal(expectedFields, section.Fields.Count());
        }

        [Fact]
        public void GetSummaryWhenThereAreNoChangesSuccess()
        {
            // Arrange
            var creationDateTime = DateTime.UtcNow;
            var oldCompleteRequest = GetListingRequest(creationDateTime);

            var sut = GetListingRequest(creationDateTime);
            sut.Setup(s => s.GetSummary(It.IsAny<SaleListingRequest>())).CallBase();

            var propertyRecord = new Mock<SalePropertyRecord>();
            propertyRecord
                .Setup(p => p.GetSummarySections(It.Is<SalePropertyRecord>(p => p == null)))
                .Returns(GetSalePropertySummary())
                .Verifiable();

            sut.SetupGet(s => s.SaleProperty).Returns(propertyRecord.Object);

            var statusFieldsRecord = new Mock<SaleStatusFieldsRecord>();
            statusFieldsRecord
                .Setup(p => p.GetSummary(It.Is<SaleStatusFieldsRecord>(p => p == null), It.IsAny<MarketStatuses>()))
                .Returns((SummarySection)null)
                .Verifiable();
            sut.SetupGet(s => s.StatusFieldsInfo).Returns(statusFieldsRecord.Object);

            // Act
            var summaryResult = sut.Object.GetSummary(oldCompleteRequest.Object);

            // Assert
            Assert.Empty(summaryResult);
        }

        [Fact]
        public void GetSummaryWhenNoPreviousRequestExistsSuccess()
        {
            // Arrange
            const int expectedSummarySections = 10;
            var creationDateTime = DateTime.UtcNow;
            var sut = GetListingRequest(creationDateTime);
            sut.Setup(s => s.GetSummary(It.IsAny<SaleListingRequest>())).CallBase();

            var propertyRecord = new Mock<SalePropertyRecord>();
            propertyRecord
                .Setup(p => p.GetSummarySections(It.Is<SalePropertyRecord>(p => p == null)))
                .Returns(GetSalePropertySummary())
                .Verifiable();

            sut.SetupGet(s => s.SaleProperty).Returns(propertyRecord.Object);

            var statusFieldsRecord = new Mock<SaleStatusFieldsRecord>();
            statusFieldsRecord
                .Setup(p => p.GetSummary(It.Is<SaleStatusFieldsRecord>(p => p == null), It.IsAny<MarketStatuses>()))
                .Returns((SummarySection)null)
                .Verifiable();
            sut.SetupGet(s => s.StatusFieldsInfo).Returns(statusFieldsRecord.Object);

            // Act
            var summaryResult = sut.Object.GetSummary((SaleListingRequest)null);

            // Assert
            Assert.NotEmpty(summaryResult);
            Assert.Equal(expectedSummarySections, summaryResult.Count());
            propertyRecord.Verify();
            statusFieldsRecord.Verify();
        }

        [Fact]
        public void GetSummaryWhenNoPreviousRequestExistsWithStatusSummarySuccess()
        {
            // Arrange
            const int expectedSummarySections = 11;
            var creationDateTime = DateTime.UtcNow;
            var sut = GetListingRequest(creationDateTime);
            sut.Setup(s => s.GetSummary(It.IsAny<SaleListingRequest>())).CallBase();

            var propertyRecord = new Mock<SalePropertyRecord>();
            propertyRecord
                .Setup(p => p.GetSummarySections(It.Is<SalePropertyRecord>(p => p == null)))
                .Returns(GetSalePropertySummary())
                .Verifiable();

            sut.SetupGet(s => s.SaleProperty).Returns(propertyRecord.Object);

            var statusFieldsRecord = new Mock<SaleStatusFieldsRecord>();
            statusFieldsRecord
                .Setup(p => p.GetSummary(It.Is<SaleStatusFieldsRecord>(p => p == null), It.IsAny<MarketStatuses>()))
                .Returns(GetStatusFieldSummary())
                .Verifiable();
            sut.SetupGet(s => s.StatusFieldsInfo).Returns(statusFieldsRecord.Object);

            // Act
            var summaryResult = sut.Object.GetSummary((SaleListingRequest)null);

            // Assert
            Assert.NotEmpty(summaryResult);
            Assert.Equal(expectedSummarySections, summaryResult.Count());
            propertyRecord.Verify();
            statusFieldsRecord.Verify();
        }

        [Fact]
        public void CreateSalePropertyRecordAndCopySalesOfficeInfoSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true);
            listing.SaleProperty.SalesOfficeInfo = new SalesOffice
            {
                StreetNumber = "123",
                StreetName = "StreetName",
                StreetSuffix = "Suffix",
                SalesOfficeCity = Faker.Enum.Random<Cities>(),
                SalesOfficeZip = "11111",
            };

            var saleListingRequest = new SaleListingRequest(listing, userId);

            Assert.Equal(listing.SaleProperty.SalesOfficeInfo.StreetName, saleListingRequest.SaleProperty.SalesOfficeInfo.StreetName);
            Assert.Equal(listing.SaleProperty.SalesOfficeInfo.StreetNumber, saleListingRequest.SaleProperty.SalesOfficeInfo.StreetNumber);
            Assert.Equal(listing.SaleProperty.SalesOfficeInfo.SalesOfficeCity, saleListingRequest.SaleProperty.SalesOfficeInfo.SalesOfficeCity);
            Assert.Equal(listing.SaleProperty.SalesOfficeInfo.SalesOfficeZip, saleListingRequest.SaleProperty.SalesOfficeInfo.SalesOfficeZip);
            Assert.Equal(listing.SaleProperty.SalesOfficeInfo.StreetSuffix, saleListingRequest.SaleProperty.SalesOfficeInfo.StreetSuffix);
        }

        [Fact]
        public void GetSummaryAndCheckSalesOfficeInfoSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            var newRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid());
            newRequest.ListingSaleId = listingId;
            newRequest.SaleProperty.SalesOfficeInfo = new SalesOfficeRecord
            {
                StreetName = "newRequest",
            };

            var oldRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid());
            oldRequest.ListingSaleId = listingId;
            oldRequest.SaleProperty.SalesOfficeInfo = new SalesOfficeRecord
            {
                StreetName = "oldRequest",
            };

            // Act
            var summary = newRequest.SaleProperty.GetSummarySections(oldRequest.SaleProperty);

            // Assert
            var summarySection = Assert.Single(summary, sc => sc != null && sc.Name == SalesOfficeRecord.SummarySection);
            Assert.Single(summarySection.Fields);
            Assert.Contains(summarySection.Fields, x =>
                x.FieldName == "StreetName" &&
                x.NewValue.ToString() == newRequest.SaleProperty.SalesOfficeInfo.StreetName &&
                x.OldValue.ToString() == oldRequest.SaleProperty.SalesOfficeInfo.StreetName);
        }

        [Fact]
        public void CloneRecordSuccess()
        {
            // Arrange
            var listing = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);
            var saleListingRequest = new SaleListingRequest(listing, Guid.NewGuid())
            {
                RequestState = ListingRequestState.Completed,
            };

            var requestCloned = saleListingRequest.Clone();
            Assert.NotEqual(saleListingRequest.Id, requestCloned.Id);
            Assert.NotEqual(saleListingRequest.RequestState, requestCloned.RequestState);
            Assert.Equal(saleListingRequest.SaleProperty.FinancialInfo.HOARequirement, requestCloned.SaleProperty.FinancialInfo.HOARequirement);
            Assert.Equal(saleListingRequest.SaleProperty.ShowingInfo.ShowingInstructions, requestCloned.SaleProperty.ShowingInfo.ShowingInstructions);
            Assert.Equal(saleListingRequest.SaleProperty.SchoolsInfo.HighSchool, requestCloned.SaleProperty.SchoolsInfo.HighSchool);
        }

        [Fact]
        public void CreatePropertyRecordCollectionFail()
        {
            // Arrange
            var property = new PropertyInfo()
            {
                ConstructionCompletionDate = DateTime.UtcNow,
                ConstructionStage = ConstructionStage.Complete,
                LegalDescription = "LegalDescription",
                TaxId = "TaxId",
                LotSize = "LotSize",
                ConstructionStartYear = 2023,
                TaxLot = "TaxLot",
                LotDescription = new List<LotDescription>(),
                PropertyType = PropertySubType.SingleFamilyResidence,
                UpdateGeocodes = false,
                IsXmlManaged = false,
            };

            var propertyRecord = PropertyRecord.CreateRecord(property);
            var errors = ValidatePropertiesAttribute.GetErrors(propertyRecord);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, x => x.MemberNames.Any(name => name == nameof(property.LotDescription)));
        }

        [Theory]
        [InlineData(WaterfrontFeatures.Creek)]
        [InlineData(WaterfrontFeatures.LakePrivileges)]
        [InlineData(WaterfrontFeatures.WaterFront)]
        [InlineData(WaterfrontFeatures.RiverFront)]
        [InlineData(WaterfrontFeatures.LakeFront)]
        [InlineData(WaterfrontFeatures.CanalFront)]
        public void CreateFeaturesRecord_WaterBodyName_Fail(WaterfrontFeatures waterfrontFeatures)
        {
            // Arrange
            var features = ListingTestProvider.GetFeaturesInfo();
            features.WaterBodyName = null;
            features.WaterfrontFeatures = new[] { waterfrontFeatures };

            var featuresRecord = FeaturesRecord.CreateRecord(features);
            var errors = ValidatePropertiesAttribute.GetErrors(featuresRecord);

            Assert.NotEmpty(errors);
            Assert.Contains(errors, x => x.MemberNames.Any(name => name == nameof(featuresRecord.WaterBodyName)));
        }

        [Fact]
        public void CreateFeaturesRecord_WaterBodyName_Success()
        {
            // Arrange
            var features = ListingTestProvider.GetFeaturesInfo();
            features.WaterBodyName = null;
            features.WaterfrontFeatures = new[] { WaterfrontFeatures.None };

            var featuresRecord = FeaturesRecord.CreateRecord(features);
            var errors = ValidatePropertiesAttribute.GetErrors(featuresRecord);

            Assert.Empty(errors);
        }

        [Fact]
        public void CreateFeaturesRecord_GarageDescriptionNull_GarageSpacesZero_Success()
        {
            // Arrange
            var features = ListingTestProvider.GetFeaturesInfo();
            features.GarageDescription = null;
            features.GarageSpaces = 0;

            var featuresRecord = FeaturesRecord.CreateRecord(features);
            var errors = ValidatePropertiesAttribute.GetErrors(featuresRecord);

            Assert.Empty(errors);
        }

        [Theory]
        [InlineData(new GarageDescription[0])]
        [InlineData(null)]
        public void CreateFeaturesRecord_GarageDescriptionNull_GarageSpacesGreaterThanZero_Fail(ICollection<GarageDescription> garageDescription)
        {
            // Arrange
            var features = ListingTestProvider.GetFeaturesInfo();
            features.GarageDescription = garageDescription;
            features.GarageSpaces = 1;

            var featuresRecord = FeaturesRecord.CreateRecord(features);
            var errors = ValidatePropertiesAttribute.GetErrors(featuresRecord);

            Assert.NotEmpty(errors);
        }

        private static Mock<SaleListingRequest> GetListingRequest(DateTime? creationDate)
        {
            var creationDateTime = creationDate ?? DateTime.UtcNow;
            var listingRequest = new Mock<SaleListingRequest>();
            listingRequest.SetupAllProperties();
            listingRequest.SetupGet(s => s.SysCreatedOn).Returns(creationDateTime);
            listingRequest.SetupGet(s => s.SysModifiedOn).Returns(creationDateTime);
            listingRequest.SetupGet(s => s.SysTimestamp).Returns(creationDateTime);

            return listingRequest;
        }

        private static IEnumerable<SummarySection> GetSalePropertySummary()
        {
            yield return new SummarySection { Name = SalePropertyRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(SalePropertyRecord.OwnerName), NewValue = "some-owner", OldValue = null } } };
            yield return new SummarySection { Name = PropertyRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(PropertyRecord.LegalDescription), NewValue = "some-legal-description", OldValue = null } } };
            yield return new SummarySection { Name = AddressRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(AddressRecord.Subdivision), NewValue = "some-subdivision", OldValue = null } } };
            yield return new SummarySection { Name = FeaturesRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(FeaturesRecord.PropertyDescription), NewValue = "some-property-description", OldValue = null } } };
            yield return new SummarySection { Name = SpacesDimensionsRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(SpacesDimensionsRecord.StoriesTotal), NewValue = Stories.One, OldValue = null } } };
            yield return new SummarySection { Name = FinancialRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(FinancialRecord.TitleCompany), NewValue = "some-title-company", OldValue = null } } };
            yield return new SummarySection { Name = SchoolRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(SchoolRecord.SchoolDistrict), NewValue = "some-school-district", OldValue = null } } };
            yield return new SummarySection { Name = ShowingRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(ShowingRecord.AgentPrivateRemarks), NewValue = "some-private-remarks", OldValue = null } } };
            yield return new SummarySection { Name = SalesOfficeRecord.SummarySection, Fields = new[] { new SummaryField { FieldName = nameof(SalesOfficeRecord.SalesOfficeCity), NewValue = "some-sales-office-city", OldValue = null } } };
        }

        private static SummarySection GetStatusFieldSummary() => new()
        {
            Name = SaleStatusFieldsRecord.SummarySection,
            Fields = new[]
            {
                new SummaryField
                {
                    FieldName = nameof(SaleStatusFieldsRecord.CancelledReason),
                    NewValue = "some-cancelled-reason",
                    OldValue = null,
                },
            },
        };
    }
}
