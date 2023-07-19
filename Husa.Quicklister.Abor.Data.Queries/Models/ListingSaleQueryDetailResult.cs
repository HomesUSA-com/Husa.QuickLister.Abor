namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    public class ListingSaleQueryDetailResult : ListingQueryResult
    {
        public ListingSaleStatusFieldQueryResult StatusFieldsInfo { get; set; }

        public ListingSalePublishInfoQueryResult PublishInfo { get; set; }

        public SalePropertyDetailQueryResult SaleProperty { get; set; }

        public EmailLeadQueryResult EmailLead { get; set; }
    }
}
