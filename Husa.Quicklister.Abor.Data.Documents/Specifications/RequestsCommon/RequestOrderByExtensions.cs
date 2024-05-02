namespace Husa.Quicklister.Abor.Data.Documents.Specifications.RequestsCommon
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using ReflectionPropertyInfo = System.Reflection.PropertyInfo;

    public static class RequestOrderByExtensions
    {
        public static IOrderedQueryable<TListingRequest> ApplyRequestSortByFields<TListingRequest>(
            this IQueryable<TListingRequest> records,
            string orderQueryString,
            Func<string, Tuple<string, ReflectionPropertyInfo>> getPropertyAttribute)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return records as IOrderedQueryable<TListingRequest>;
            }

            var attributesList = orderQueryString.Split(",");
            IOrderedQueryable<TListingRequest> sortedRecords = null;
            var firstSortColumn = true;

            foreach (var attribute in attributesList)
            {
                if (attribute.StartsWith('-') || attribute.StartsWith('+'))
                {
                    var sortOrder = attribute.First();
                    var attributeName = attribute[1..];
                    var propertyInfo = typeof(TListingRequest).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    Expression<Func<TListingRequest, object>> lambda = null;
                    if (propertyInfo == null)
                    {
                        if (attributeName.Equals("address", StringComparison.CurrentCultureIgnoreCase))
                        {
                            sortedRecords = HandlePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, getPropertyAttribute("StreetNumber"));
                            sortedRecords = HandlePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, getPropertyAttribute("StreetName"));
                            continue;
                        }

                        sortedRecords = HandlePropertyAttributes(records, sortedRecords, firstSortColumn, sortOrder, getPropertyAttribute(attributeName));
                        continue;
                    }

                    lambda = GetLambdaExpression<TListingRequest>(string.Empty, propertyInfo);
                    sortedRecords = AddPropertyToOrder(sortedRecords, lambda, sortOrder, records, firstSortColumn);
                    firstSortColumn = false;
                }
            }

            return sortedRecords;
        }

        private static IOrderedQueryable<TListingRequest> AddPropertyToOrder<TListingRequest>(
            IOrderedQueryable<TListingRequest> sortedRecords,
            Expression<Func<TListingRequest, object>> lambda,
            char sortOrder,
            IQueryable<TListingRequest> records,
            bool firstSortColumn)
        {
            if (firstSortColumn)
            {
                return sortOrder == '+' ? records.OrderBy(lambda) : records.OrderByDescending(lambda);
            }

            return sortOrder == '+' ? sortedRecords.ThenBy(lambda) : sortedRecords.ThenByDescending(lambda);
        }

        private static Expression<Func<TListingRequest, object>> GetLambdaExpression<TListingRequest>(string innerObjectName, ReflectionPropertyInfo propertyInfo)
        {
            var parameterExpression = Expression.Parameter(typeof(TListingRequest), "root");
            var memberExpression = CreateExpression(typeof(TListingRequest), innerObjectName + propertyInfo.Name);
            var lambda = Expression.Lambda<Func<TListingRequest, object>>(Expression.Convert(memberExpression, typeof(object)), parameterExpression);
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

        private static IOrderedQueryable<TListingRequest> HandlePropertyAttributes<TListingRequest>(
            IQueryable<TListingRequest> records,
            IOrderedQueryable<TListingRequest> sortedRecords,
            bool firstSortColumn,
            char sortOrder,
            Tuple<string, ReflectionPropertyInfo> propertyAttribute)
        {
            var lambda = GetLambdaExpression<TListingRequest>(propertyAttribute.Item1, propertyAttribute.Item2);
            sortedRecords = AddPropertyToOrder(sortedRecords, lambda, sortOrder, records, firstSortColumn);
            return sortedRecords;
        }
    }
}
