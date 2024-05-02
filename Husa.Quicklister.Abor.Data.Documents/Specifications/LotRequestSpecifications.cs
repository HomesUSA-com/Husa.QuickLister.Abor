namespace Husa.Quicklister.Abor.Data.Documents.Specifications
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Husa.Extensions.Document.Specifications.Document;
    using Husa.Quicklister.Abor.Data.Documents.Specifications.RequestsCommon;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Documents.Specifications;
    using ReflectionPropertyInfo = System.Reflection.PropertyInfo;

    public static class LotRequestSpecifications
    {
        public static IOrderedQueryable<LotListingRequest> ApplyLotListingRequestSortByFields(this IQueryable<LotListingRequest> records, string orderQueryString)
        {
            return records.ApplyRequestSortByFields(orderQueryString, GetLotPropertyAttribute);
        }

        public static IQueryable<LotListingRequest> FilterByQuery(this IQueryable<LotListingRequest> records, ListingRequestQueryFilter queryFilter)
        {
            return records.FilterByStatus(queryFilter.RequestState)
                .FilterLotRequestsByCompany(queryFilter.CompanyId)
                .FilterByEntityId(queryFilter.ListingId)
                .ApplySearchByFilter(queryFilter.SearchFilter)
                .ApplyLotListingRequestSortByFields(queryFilter.SortBy);
        }

        public static IQueryable<LotListingRequest> FilterLotRequestsByCompany(this IQueryable<LotListingRequest> records, Guid companyId)
        {
            return companyId.Equals(Guid.Empty) ? records : records.Where(x => x.CompanyId == companyId);
        }

        public static IQueryable<LotListingRequest> ApplySearchByFilter(this IQueryable<LotListingRequest> query, string searchByFilter)
        {
            if (string.IsNullOrEmpty(searchByFilter))
            {
                return query;
            }

            var words = searchByFilter.Trim().Split(' ').ToList();
            foreach (var search in words)
            {
                query = query.Where(x => x.MlsNumber.Contains(search)
                    || x.AddressInfo.StreetNumber.ToString().Contains(search)
                    || x.AddressInfo.StreetName.Contains(search)
                    || x.AddressInfo.Subdivision.Contains(search));
            }

            return query;
        }

        private static Tuple<string, ReflectionPropertyInfo> GetLotPropertyAttribute(this string attributeName)
        {
            var columnName = "AddressInfo.";
            ReflectionPropertyInfo propertyInfo = typeof(AddressInfo).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException("ApplyOrderDirection: The associated attribute to the given attribute could not by resolved", attributeName);
            }

            return new(columnName, propertyInfo);
        }
    }
}
