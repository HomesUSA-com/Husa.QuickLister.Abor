namespace Husa.Quicklister.Abor.Application.Tests.Providers
{
    using System;
    using Husa.Extensions.ShowingTime.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Moq;
    using ShowingTimeRecord = Husa.Quicklister.Extensions.Domain.Entities.Request.ShowingTimeRecord;

    public static class ListingRequestProviders
    {
        public static Mock<SaleListingRequest> GetSaleListingRequestMock(Guid requestId, Guid listingId, Guid? companyId = null, ListingRequestState? requestState = null)
        {
            var saleListingRequest = new Mock<SaleListingRequest>();

            saleListingRequest.SetupGet(sl => sl.Id).Returns(requestId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.EntityId).Returns(listingId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.ListingSaleId).Returns(listingId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.CompanyId).Returns(companyId ?? Guid.NewGuid()).Verifiable();
            saleListingRequest.SetupGet(sl => sl.SysCreatedBy).Returns(Guid.NewGuid()).Verifiable();
            var salePropertyRecord = new SalePropertyRecord()
            {
                AddressInfo = new(),
                FeaturesInfo = new(),
                PropertyInfo = new(),
                SpacesDimensionsInfo = new(),
                ShowingInfo = new(),
                FinancialInfo = new(),
                SchoolsInfo = new(),
                SalesOfficeInfo = new(),
            };
            saleListingRequest.SetupGet(sl => sl.SaleProperty).Returns(salePropertyRecord).Verifiable();
            saleListingRequest.SetupGet(sl => sl.StatusFieldsInfo).Returns(new SaleStatusFieldsRecord()).Verifiable();
            saleListingRequest.SetupGet(sl => sl.PublishInfo).Returns(new PublishFieldsRecord()).Verifiable();
            saleListingRequest.SetupGet(sl => sl.ShowingTimeInfo).Returns(new ShowingTimeRecord()
            {
                AppointmentSettings = new() { AppointmentType = AppointmentType.AppointmentRequiredConfirmWithAll },
                AppointmentRestrictions = new(),
                AdditionalInstructions = new(),
                AccessInformation = new(),
            });
            if (requestState.HasValue)
            {
                saleListingRequest.SetupGet(sl => sl.RequestState).Returns(requestState.Value).Verifiable();
            }

            saleListingRequest.Setup(p => p.GetSummary(It.IsAny<SaleListingRequest>())).CallBase().Verifiable();
            saleListingRequest.SetupProperty(x => x.UseShowingTime, false);

            return saleListingRequest;
        }
    }
}
