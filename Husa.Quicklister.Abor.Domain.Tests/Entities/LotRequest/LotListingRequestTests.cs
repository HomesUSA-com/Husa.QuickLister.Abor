namespace Husa.Quicklister.Abor.Domain.Tests.Entities.LotRequest
{
    using System;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Moq;
    using Xunit;
    public class LotListingRequestTests
    {
        [Fact]
        public void UpdateRequestInformation_Success()
        {
            var listing = new LotListing() { Id = Guid.NewGuid() };
            var request = new LotListingRequest(listing, Guid.NewGuid());

            var requestValueObject = new ListingRequestValueObject()
            {
                MlsNumber = "87162",
                ListPrice = 866389,
            };
            var statusInfo = new ListingStatusFieldsInfo()
            {
                PendingDate = DateTime.Now,
            };
            var propertyValueObject = new LotPropertyValueObject()
            {
                FeaturesInfo = new()
                {
                    DistanceToWaterAccess = DistanceToWaterAccess.OneTwoMiles,
                },
                SchoolsInfo = new()
                {
                    HighSchool = HighSchool.Akins,
                },
                FinancialInfo = new(),
                ShowingInfo = new(),
                PropertyInfo = new(),
                AddressInfo = new(),
            };

            request.UpdateRequestInformation(requestValueObject, statusInfo, propertyValueObject);

            Assert.Equal(requestValueObject.ListPrice, request.ListPrice);
            Assert.Equal(statusInfo.PendingDate, request.StatusFieldsInfo.PendingDate);
            Assert.Equal(propertyValueObject.FeaturesInfo.DistanceToWaterAccess, request.FeaturesInfo.DistanceToWaterAccess);
            Assert.Equal(propertyValueObject.SchoolsInfo.HighSchool, request.SchoolsInfo.HighSchool);
        }

        [Theory]
        [InlineData(MarketStatuses.Canceled)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        [InlineData(MarketStatuses.Pending)]
        [InlineData(MarketStatuses.Hold)]
        [InlineData(MarketStatuses.Closed)]
        public void IsValidForSubmit_StatusFieldAreRequired(MarketStatuses mlsStatus)
        {
            var listing = new Mock<LotListingRequest>();
            listing.Setup(sp => sp.MlsStatus).Returns(mlsStatus);
            listing.Setup(x => x.StatusFieldsInfo).Returns(new LotStatusFieldsRecord());
            listing.Setup(sp => sp.IsValidForSubmit()).CallBase();

            var result = listing.Object.IsValidForSubmit();

            var statusError = result.FirstOrDefault(x => x.ErrorMessage.Contains("StatusFields"));
            Assert.NotNull(statusError);
        }

        [Fact]
        public void IsValidForSubmit_PendingValidations()
        {
            var mlsStatus = MarketStatuses.Pending;
            var listing = new Mock<LotListingRequest>();
            listing.Setup(sp => sp.MlsStatus).Returns(mlsStatus);
            listing.Setup(sp => sp.IsValidForSubmit()).CallBase();

            var statusFields = new LotStatusFieldsRecord()
            {
                PendingDate = DateTime.Today.AddDays(2),
                EstimatedClosedDate = DateTime.Today.AddDays(-2),
            };
            listing.Setup(x => x.StatusFieldsInfo).Returns(statusFields);

            var result = listing.Object.IsValidForSubmit();

            var statusError = result.FirstOrDefault(x => x.ErrorMessage.Contains("StatusFields"));
            Assert.NotNull(statusError);
        }

        [Fact]
        public void GetSummary_ActiveUnderContractChanges_Success()
        {
            var listing = new LotListing() { Id = Guid.NewGuid() };
            var oldRequest = new LotListingRequest(listing, Guid.NewGuid())
            {
                MlsStatus = MarketStatuses.Active,
            };
            var newRequest = new LotListingRequest(listing, Guid.NewGuid())
            {
                MlsStatus = MarketStatuses.ActiveUnderContract,
                StatusFieldsInfo = new()
                {
                    PendingDate = DateTime.Now,
                    ClosedDate = DateTime.Now,
                },
            };

            var result = newRequest.GetSummary(oldRequest);
            var statusSummary = result.First(x => x.Name == nameof(LotListingRequest.StatusFieldsInfo)).Fields;
            Assert.Equal(2, statusSummary.Count());
            Assert.Contains(statusSummary, x => x.FieldName == nameof(LotStatusFieldsRecord.PendingDate));
            Assert.Contains(statusSummary, x => x.FieldName == nameof(LotStatusFieldsRecord.ClosedDate));
        }
    }
}
