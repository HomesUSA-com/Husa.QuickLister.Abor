namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsResponse : ListingStatusFieldsResponse
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
    }
}
