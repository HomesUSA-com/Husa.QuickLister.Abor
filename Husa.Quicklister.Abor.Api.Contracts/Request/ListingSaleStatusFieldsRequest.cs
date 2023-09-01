namespace Husa.Quicklister.Abor.Api.Contracts.Request
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsRequest : ListingStatusFieldsRequest
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public DateTime? ContractDate { get; set; }

        public string SellConcess { get; set; }
    }
}
