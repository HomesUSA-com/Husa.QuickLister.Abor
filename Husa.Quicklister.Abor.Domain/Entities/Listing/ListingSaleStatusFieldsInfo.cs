namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingSaleStatusFieldsInfo : ListingStatusFieldsInfo, IProvideSaleStatusFields
    {
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
            yield return this.ContingencyInfo;
        }
    }
}
