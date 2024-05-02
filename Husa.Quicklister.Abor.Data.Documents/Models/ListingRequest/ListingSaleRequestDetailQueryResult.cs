namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;
    using Husa.Quicklister.Abor.Data.Queries.Models;

    public class ListingSaleRequestDetailQueryResult : ListingRequestQueryResult
    {
        public Guid ListingSaleId { get; set; }

        public ListingRequestSalePropertyQueryResult SaleProperty { get; set; }

        public PublishInfoQueryResult PublishInfo { get; set; }

        public ListingRequestStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
