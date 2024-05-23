namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    public class ListingSaleQueryDetailResult : ListingDetailsQueryResult
    {
        public ListingSaleStatusFieldQueryResult StatusFieldsInfo { get; set; }

        public SalePropertyDetailQueryResult SaleProperty { get; set; }
    }
}
