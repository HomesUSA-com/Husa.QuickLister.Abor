namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldQueryResult : ListingStatusFieldsQueryResult
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
    }
}
