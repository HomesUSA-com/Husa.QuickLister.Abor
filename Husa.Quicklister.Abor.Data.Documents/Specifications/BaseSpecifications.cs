namespace Husa.Quicklister.Abor.Data.Documents.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Documents.Specifications;
    using ReflectionPropertyInfo = System.Reflection.PropertyInfo;

    public static class BaseSpecifications
    {
        public static IOrderedQueryable<SaleListingRequest> ApplyListingSaleRequestSortByFields(this IQueryable<SaleListingRequest> records, string orderQueryString)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return records as IOrderedQueryable<SaleListingRequest>;
            }

            var attributesList = orderQueryString.Split(",");
            IOrderedQueryable<SaleListingRequest> sortedRecords = null;
            var firstSortColumn = true;

            foreach (var attribute in attributesList)
            {
                if (attribute.StartsWith('-') || attribute.StartsWith('+'))
                {
                    var sortOrder = attribute.First();
                    var attributeName = attribute[1..];
                    var propertyInfo = typeof(SaleListingRequest).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    Expression<Func<SaleListingRequest, object>> lambda = null;
                    if (propertyInfo == null)
                    {
                        if (attributeName.ToLower() == "address")
                        {
                            sortedRecords = HandleOrderByAddressAttributes(records, sortedRecords, firstSortColumn, sortOrder);
                            continue;
                        }

                        sortedRecords = HandleSalePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, attributeName);
                        continue;
                    }

                    lambda = GetLambdaExpression(string.Empty, propertyInfo);
                    sortedRecords = AddPropertyToOrder(sortedRecords, lambda, sortOrder, records, firstSortColumn);
                    firstSortColumn = false;
                }
            }

            return sortedRecords;
        }

        public static IQueryable<SaleListingRequest> FilterByQuery(this IQueryable<SaleListingRequest> records, SaleListingRequestQueryFilter queryFilter)
        {
            return records.FilterByStatus(queryFilter.RequestState)
                .FilterSaleRequestsByCompany(queryFilter.CompanyId)
                .FilterByListingId(queryFilter.SaleListingId)
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

        public static decimal? GetBuyersAgentCommission(this string buyersAgentCommission)
        {
            if (string.IsNullOrEmpty(buyersAgentCommission))
            {
                return null;
            }

            var cleannedValue = buyersAgentCommission.Contains('$') ?
                   buyersAgentCommission.Replace("$", string.Empty) :
                   buyersAgentCommission.Replace("%", string.Empty);

            decimal decimalValue;
            if (decimal.TryParse(cleannedValue, out decimalValue))
            {
                return decimalValue;
            }

            return null;
        }

        private static IOrderedQueryable<SaleListingRequest> HandleOrderByAddressAttributes(
            IQueryable<SaleListingRequest> records,
            IOrderedQueryable<SaleListingRequest> sortedRecords,
            bool firstSortColumn,
            char sortOrder)
        {
            sortedRecords = HandleSalePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, "StreetNumber");
            sortedRecords = HandleSalePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, "StreetName");
            return sortedRecords;
        }

        private static IOrderedQueryable<SaleListingRequest> HandleSalePropertyAttributes(
            IQueryable<SaleListingRequest> records,
            IOrderedQueryable<SaleListingRequest> sortedRecords,
            bool firstSortColumn,
            char sortOrder,
            string attributeName)
        {
            var columnName = "SaleProperty.AddressInfo.";
            ReflectionPropertyInfo propertyInfo;

            if (attributeName.ToLower() == "ownername")
            {
                propertyInfo = typeof(SaleProperty).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                columnName = "SaleProperty.";
            }
            else
            {
                propertyInfo = typeof(AddressInfo).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }

            if (propertyInfo == null)
            {
                throw new ArgumentException("ApplyOrderDirection: The associated attribute to the given attribute could not by resolved", attributeName);
            }

            var lambda = GetLambdaExpression(columnName, propertyInfo);
            sortedRecords = AddPropertyToOrder(sortedRecords, lambda, sortOrder, records, firstSortColumn);
            return sortedRecords;
        }

        private static Expression<Func<SaleListingRequest, object>> GetLambdaExpression(string innerObjectName, ReflectionPropertyInfo propertyInfo)
        {
            var parameterExpression = Expression.Parameter(typeof(SaleListingRequest), "root");
            var memberExpression = CreateExpression(typeof(SaleListingRequest), innerObjectName + propertyInfo.Name);
            var lambda = Expression.Lambda<Func<SaleListingRequest, object>>(Expression.Convert(memberExpression, typeof(object)), parameterExpression);
            return lambda;
        }

        private static MemberExpression CreateExpression(Type type, string propertyName)
        {
            Expression body = Expression.Parameter(type, "root");
            foreach (var member in propertyName.Split("."))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return (MemberExpression)body;
        }

        private static IOrderedQueryable<SaleListingRequest> AddPropertyToOrder(
            IOrderedQueryable<SaleListingRequest> sortedRecords,
            Expression<Func<SaleListingRequest, object>> lambda,
            char sortOrder,
            IQueryable<SaleListingRequest> records,
            bool firstSortColumn)
        {
            if (firstSortColumn)
            {
                return sortOrder == '+' ? records.OrderBy(lambda) : records.OrderByDescending(lambda);
            }

            return sortOrder == '+' ? sortedRecords.ThenBy(lambda) : sortedRecords.ThenByDescending(lambda);
        }
    }
}
