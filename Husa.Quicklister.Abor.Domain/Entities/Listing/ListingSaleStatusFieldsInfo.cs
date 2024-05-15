namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;

    public class ListingSaleStatusFieldsInfo : ListingStatusFieldsInfo
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return base.GetEqualityComponents();
        }
    }
}
