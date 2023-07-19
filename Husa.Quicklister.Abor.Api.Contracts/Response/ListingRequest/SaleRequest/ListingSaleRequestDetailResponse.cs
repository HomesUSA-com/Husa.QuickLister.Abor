namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;

    public class ListingSaleRequestDetailResponse : ListingRequestDetailResponse
    {
        public Guid ListingSaleId { get; set; }

        public ListingSaleStatusFieldsResponse StatusFieldsInfo { get; set; }

        public ListingSalePublishInfoResponse PublishInfo { get; set; }

        public SalePropertyDetailResponse SaleProperty { get; set; }
    }
}
