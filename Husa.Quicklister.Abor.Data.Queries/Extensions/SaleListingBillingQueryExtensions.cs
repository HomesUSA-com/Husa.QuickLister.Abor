namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Data.Queries.Models;

    public static class SaleListingBillingQueryExtensions
    {
        public static IEnumerable<ListingSaleBillingQueryResult> FilterBySearch(this IEnumerable<ListingSaleBillingQueryResult> listings, string searchBy)
        {
            if (string.IsNullOrEmpty(searchBy))
            {
                return listings;
            }

            searchBy = searchBy.Trim();

            var splitted = searchBy.Split(' ', 2);
            if (splitted.Length > 1)
            {
                return listings.Where(l => (l.StreetName.StartsWith(splitted[1], System.StringComparison.CurrentCultureIgnoreCase) && l.StreetNum.Equals(splitted[0])) ||
                                    l.Subdivision.Contains(searchBy, System.StringComparison.CurrentCultureIgnoreCase) ||
                                    l.CreatedBy.Contains(searchBy, System.StringComparison.CurrentCultureIgnoreCase) ||
                                    l.OwnerName.Contains(searchBy, System.StringComparison.CurrentCultureIgnoreCase));
            }

            var cleanSearch = searchBy.TrimStart('$');
            if (decimal.TryParse(cleanSearch, out _))
            {
                cleanSearch = string.Join(string.Empty, cleanSearch.Split(','));
            }

            if (cleanSearch.All(c => c >= '0' && c <= '9'))
            {
                return listings.Where(l =>
                    l.MlsNumber.Contains(cleanSearch) ||
                    l.ZipCode.Contains(cleanSearch) ||
                    l.StreetNum.Contains(cleanSearch) ||
                    ((int)l.ListFee.GetValueOrDefault()).ToString() == cleanSearch);
            }

            return listings.Where(l => l.StreetName.StartsWith(searchBy, System.StringComparison.CurrentCultureIgnoreCase) ||
                l.Subdivision.StartsWith(searchBy, System.StringComparison.CurrentCultureIgnoreCase) ||
                l.OwnerName.Contains(searchBy, System.StringComparison.CurrentCultureIgnoreCase) ||
                l.CreatedBy.Contains(searchBy, System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
