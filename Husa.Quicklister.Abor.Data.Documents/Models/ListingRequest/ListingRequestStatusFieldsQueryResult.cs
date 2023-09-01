namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingRequestStatusFieldsQueryResult : ListingStatusFieldsQueryResult
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public DateTime? ContractDate { get; set; }

        public string SellConcess { get; set; }
    }
}
