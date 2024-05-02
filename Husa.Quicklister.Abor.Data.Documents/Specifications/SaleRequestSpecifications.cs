namespace Husa.Quicklister.Abor.Data.Documents.Specifications
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.Quicklister.Abor.Data.Documents.Specifications.RequestsCommon;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Documents.Specifications;
    using ReflectionPropertyInfo = System.Reflection.PropertyInfo;

    public static class SaleRequestSpecifications
    {
        public static IOrderedQueryable<SaleListingRequest> ApplyListingSaleRequestSortByFields(this IQueryable<SaleListingRequest> records, string orderQueryString)
        {
            return records.ApplyRequestSortByFields(orderQueryString, GetSalePropertyAttribute);
        }

        public static IQueryable<SaleListingRequest> FilterByQuery(this IQueryable<SaleListingRequest> records, ListingRequestQueryFilter queryFilter)
        {
            return records.FilterByStatus(queryFilter.RequestState)
                .FilterSaleRequestsByCompany(queryFilter.CompanyId)
                .FilterByListingId(queryFilter.ListingId)
                .ApplySearchByFilter(queryFilter.SearchFilter)
                .ApplyListingSaleRequestSortByFields(queryFilter.SortBy);
        }

        public static IQueryable<SaleListingRequest> FilterSaleRequestsByCompany(this IQueryable<SaleListingRequest> records, Guid companyId)
        {
            return companyId.Equals(Guid.Empty) ? records : records.Where(x => x.SaleProperty.CompanyId == companyId);
        }

        public static IQueryable<SaleListingRequest> FilterByListingId(this IQueryable<SaleListingRequest> query, Guid? listingSaleId)
        {
            return listingSaleId.HasValue ?
                query.Where(x => x.ListingSaleId == listingSaleId.Value) :
                query;
        }

        public static IQueryable<SaleListingRequest> ApplySearchByFilter(this IQueryable<SaleListingRequest> query, string searchByFilter)
        {
            if (string.IsNullOrEmpty(searchByFilter))
            {
                return query;
            }

            var words = searchByFilter.Trim().Split(' ').ToList();
            foreach (var search in words)
            {
                query = query.Where(x => x.MlsNumber.Contains(search)
                    || x.SaleProperty.AddressInfo.StreetNumber.ToString().Contains(search)
                    || x.SaleProperty.AddressInfo.StreetName.Contains(search)
                    || x.SaleProperty.AddressInfo.Subdivision.Contains(search));
            }

            return query;
        }

        private static Tuple<string, ReflectionPropertyInfo> GetSalePropertyAttribute(this string attributeName)
        {
            var columnName = "SaleProperty.AddressInfo.";
            ReflectionPropertyInfo propertyInfo;

            if (attributeName.Equals("ownername", StringComparison.CurrentCultureIgnoreCase))
            {
                propertyInfo = typeof(SaleProperty).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                columnName = "SaleProperty.";
            }
            else
            {
                propertyInfo = typeof(SaleAddressInfo).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("ApplyOrderDirection: The associated attribute to the given attribute could not by resolved", attributeName);
            }

            return new(columnName, propertyInfo);
        }
    }
}
