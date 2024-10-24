namespace Husa.Quicklister.Abor.Domain.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public static class SalePropertyExtensions
    {
        public static IEnumerable<SaleListing> GetListingsToUpdate(this ICollection<SaleProperty> saleProperties) => saleProperties
                 .Where(property => !property.IsDeleted)
                 .SelectMany(p => p.SaleListings)
                 .Where(listing => !listing.IsDeleted
            && SaleListing.ActiveListingStatuses.Contains(listing.MlsStatus)
            && listing.LockedStatus == LockedStatus.NoLocked);
    }
}
