namespace Husa.Quicklister.Abor.Domain.Tests.Providers
{
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Moq;

    public static class TestProviderLotRequest
    {
        public static Mock<LotListingRequest> GetLotListingRequestMock()
        {
            var request = new Mock<LotListingRequest>();
            request.Setup(x => x.SchoolsInfo).Returns(new LotSchoolRecord());
            request.Setup(x => x.FinancialInfo).Returns(new LotFinancialRecord());
            request.Setup(x => x.ShowingInfo).Returns(new LotShowingRecord());
            request.Setup(x => x.FeaturesInfo).Returns(new LotFeaturesRecord());
            request.Setup(x => x.AddressInfo).Returns(new LotAddressRecord());
            request.Setup(x => x.PropertyInfo).Returns(new LotPropertyRecord());
            request.Setup(x => x.SalesOfficeInfo).Returns(new SalesOfficeRecord());
            request.Setup(x => x.PublishInfo).Returns(new PublishFieldsRecord());
            request.Setup(x => x.StatusFieldsInfo).Returns(new LotStatusFieldsRecord());
            return request;
        }
    }
}
