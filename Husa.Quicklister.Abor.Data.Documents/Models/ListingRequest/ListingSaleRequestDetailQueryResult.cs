namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;

    public class ListingSaleRequestDetailQueryResult : ListingRequestQueryResult
    {
        public Guid ListingSaleId { get; set; }

        public ListingRequestSalePropertyQueryResult SaleProperty { get; set; }

        public ListingRequestPublishInfoQueryResult PublishInfo { get; set; }

        public ListingRequestStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
