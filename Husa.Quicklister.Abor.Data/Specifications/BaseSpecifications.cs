namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Entities.Property;

    public static class BaseSpecifications
    {
        public static Expression<Func<T, bool>> BaseFilter<T>()
            where T : Entity
        {
            return x => !x.IsDeleted;
        }

        public static IOrderedQueryable<T> ApplySortByNotProjectedFields<T>(this IQueryable<T> records, string orderQueryString)
        {
            if (string.IsNullOrEmpty(orderQueryString))
            {
                return records as IOrderedQueryable<T>;
            }

            if (orderQueryString.Contains("address"))
            {
                orderQueryString = orderQueryString.StartsWith('-') ? "-streetnum,-streetname" : "+streetnum,+streetname";
            }

            var attributes = orderQueryString.Split(",");
            IOrderedQueryable<T> sortedRecords = null;
            var orderRecords = true;
            if (!attributes.Any())
            {
                return records as IOrderedQueryable<T>;
            }

            foreach (var attribute in attributes)
            {
                if (attribute.StartsWith('-') || attribute.StartsWith('+'))
                {
                    var sortOrder = attribute.First();
                    var attributeName = attribute[1..];
                    var propertyInfo = typeof(T).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    Expression<Func<T, object>> lambda = null;
                    if (propertyInfo == null)
                    {
                        sortedRecords = HandleSalePropertyAttributes<T>(records, sortedRecords, orderRecords, sortOrder, attributeName);
                    }

                    var parameterExpression = Expression.Parameter(typeof(T), "x");
                    var memberExpression = Expression.PropertyOrField(parameterExpression, propertyInfo?.Name);
                    var memberExpressionConversion = Expression.Convert(memberExpression, typeof(object));
                    lambda = Expression.Lambda<Func<T, object>>(memberExpressionConversion, parameterExpression);

                    if (orderRecords)
                    {
                        sortedRecords = (sortOrder == '+') ? records.OrderBy(lambda) : records.OrderByDescending(lambda);
                        orderRecords = false;
                        continue;
                    }

                    sortedRecords = (sortOrder == '+') ? sortedRecords.ThenBy(lambda) : sortedRecords.ThenByDescending(lambda);
                }
            }

            return sortedRecords;
        }

        private static IOrderedQueryable<T> HandleSalePropertyAttributes<T>(IQueryable<T> records, IOrderedQueryable<T> sortedRecords, bool flag, char sortOrder, string attributeName)
        {
            var propertyInfo = typeof(SaleProperty).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                throw new ArgumentException("ApplyOrderDirection: The associated Attribute to the given AttributeName could not be resolved", attributeName);
            }

            var lambda = GetLambdaExpression<T>("SaleProperty.", propertyInfo);
            sortedRecords = AddPropertyToOrder<T>(sortedRecords, lambda, sortOrder, records, flag);
            return sortedRecords;
        }

        private static Expression<Func<T, object>> GetLambdaExpression<T>(string innerObjectName, PropertyInfo propertyInfo)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "x");
            var memberExpression = Expression.PropertyOrField(parameterExpression, innerObjectName + propertyInfo.Name);
            var memberExpressionConversion = Expression.Convert(memberExpression, typeof(object));
            var lambda = Expression.Lambda<Func<T, object>>(memberExpressionConversion, parameterExpression);
            return lambda;
        }

        private static IOrderedQueryable<T> AddPropertyToOrder<T>(
            IOrderedQueryable<T> sortedRecords,
            Expression<Func<T, object>> lambda,
            char sortOrder,
            IQueryable<T> records,
            bool flag)
        {
            if (flag)
            {
                sortedRecords = (sortOrder == '+') ? records.OrderBy(lambda) : records.OrderByDescending(lambda);
                return sortedRecords;
            }

            sortedRecords = (sortOrder == '+') ? sortedRecords.ThenBy(lambda) : sortedRecords.ThenByDescending(lambda);
            return sortedRecords;
        }
    }
}
