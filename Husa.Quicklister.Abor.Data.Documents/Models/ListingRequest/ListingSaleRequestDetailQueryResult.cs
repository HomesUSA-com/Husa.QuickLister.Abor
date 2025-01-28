namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;
    using Husa.Quicklister.Extensions.Data.Queries.Models.ShowingTime;

    public class ListingSaleRequestDetailQueryResult : ListingRequestDetailQueryResult
    {
        public Guid ListingSaleId { get; set; }

        public ListingRequestSalePropertyQueryResult SaleProperty { get; set; }

        public ListingRequestStatusFieldsQueryResult StatusFieldsInfo { get; set; }

        public ShowingTimeQueryResult ShowingTime { get; set; }
    }
}
