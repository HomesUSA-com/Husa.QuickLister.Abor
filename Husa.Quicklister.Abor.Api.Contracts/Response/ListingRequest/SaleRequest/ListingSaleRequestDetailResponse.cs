namespace Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;

    public class ListingSaleRequestDetailResponse : ListingRequestDetailResponse, IListingRequestDetailResponse
    {
        public Guid ListingSaleId { get; set; }

        public ListingSaleStatusFieldsResponse StatusFieldsInfo { get; set; }

        public ListingSalePublishInfoResponse PublishInfo { get; set; }

        public SalePropertyDetailResponse SaleProperty { get; set; }
    }
}
