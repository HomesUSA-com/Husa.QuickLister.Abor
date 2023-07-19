namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingSaleStatusFieldsResponse : ListingStatusFieldsResponse
    {
        public string ContingencyInfo { get; set; }

        public string SaleTerms2nd { get; set; }

        public DateTime? ContractDate { get; set; }

        public DateTime? ExpiredDateOption { get; set; }

        public string KickOutInformation { get; set; }

        public HowSold? HowSold { get; set; }

        public decimal SellPoints { get; set; }

        public string SellConcess { get; set; }

        public ICollection<SellerConcessionDescription> SellerConcessionDescription { get; set; }
    }
}
