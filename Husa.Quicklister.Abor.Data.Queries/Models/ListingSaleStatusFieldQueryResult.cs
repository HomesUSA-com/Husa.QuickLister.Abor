namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldQueryResult : ListingStatusFieldsQueryResult
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
    }
}
