namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsResponse : ListingStatusFieldsResponse
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public DateTime? ContractDate { get; set; }

        public string SellConcess { get; set; }
    }
}
