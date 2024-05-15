namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsRequest : ListingStatusFieldsRequest
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
    }
}
