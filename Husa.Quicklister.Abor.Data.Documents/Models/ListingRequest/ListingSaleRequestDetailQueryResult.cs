namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;

    public class ListingSaleRequestDetailQueryResult : ListingRequestDetailQueryResult
    {
        public Guid ListingSaleId { get; set; }

        public ListingRequestSalePropertyQueryResult SaleProperty { get; set; }

        public ListingRequestStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
