namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public static class LotSpecifications
    {
        public static IQueryable<T> FilterByCommunities<T>(this IQueryable<T> listings, ICollection<Guid> communityIds)
           where T : LotListing
        {
            if (communityIds != null && communityIds.Count > 0)
            {
                return listings.Where(p => p.CommunityId.HasValue && communityIds.Contains(p.CommunityId.Value));
            }

            return listings;
        }

        public static IQueryable<T> FilterBySearch<T>(this IQueryable<T> listings, string searchBy)
            where T : LotListing
        {
            if (string.IsNullOrEmpty(searchBy))
            {
                return listings;
            }

            var splitted = searchBy.Split(' ', 2);
            if (splitted.Length > 1)
            {
                return listings.Where(l => (l.AddressInfo.StreetName.StartsWith(splitted[1]) &&
                            l.AddressInfo.StreetNumber.Equals(splitted[0])) ||
                            l.AddressInfo.StreetName.StartsWith(searchBy));
            }

            return listings.Where(l => l.AddressInfo.StreetName.StartsWith(searchBy) ||
                                        l.AddressInfo.StreetNumber.Equals(searchBy) ||
                                        l.AddressInfo.Subdivision.StartsWith(searchBy) ||
                                        l.MlsNumber.Equals(searchBy) ||
                                        l.AddressInfo.ZipCode.Equals(searchBy));
        }

        public static IQueryable<LotListing> FilterByStreetNumber(this IQueryable<LotListing> listings, string streetNumber)
        {
            return !string.IsNullOrWhiteSpace(streetNumber) ?
              listings.Where(listingSale => listingSale.AddressInfo.StreetNumber.StartsWith(streetNumber)) :
              listings;
        }

        public static IQueryable<LotListing> FilterByStreetName(this IQueryable<LotListing> listings, string streetName)
        {
            return !string.IsNullOrWhiteSpace(streetName) ?
              listings.Where(listingSale => listingSale.AddressInfo.StreetName.StartsWith(streetName)) :
              listings;
        }
    }
}
