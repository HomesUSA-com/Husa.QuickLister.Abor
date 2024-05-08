namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingSaleStatusFieldsInfo : ListingStatusFieldsInfo, IProvideSaleStatusFields
    {
        public bool HasContingencyInfo { get; set; }

        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.HasContingencyInfo;
            yield return this.ContingencyInfo;
            yield return this.SaleTerms;
            yield return this.PendingDate;
            yield return this.SellConcess;
        }
    }
}
