namespace Husa.Quicklister.Abor.Domain.Tests.Entities.LotRequest
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
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
    }
}
