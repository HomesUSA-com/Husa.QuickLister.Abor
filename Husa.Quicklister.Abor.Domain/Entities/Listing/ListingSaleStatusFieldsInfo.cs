namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingSaleStatusFieldsInfo : ListingStatusFieldsInfo, IProvideSaleStatusFields
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.ContingencyInfo;
            yield return this.SaleTerms2nd;
            yield return this.ContractDate;
            yield return this.ExpiredDateOption;
            yield return this.KickOutInformation;
            yield return this.HowSold;
            yield return this.SellPoints;
            yield return this.SellConcess;
            yield return this.SellerConcessionDescription;
        }
    }
}
