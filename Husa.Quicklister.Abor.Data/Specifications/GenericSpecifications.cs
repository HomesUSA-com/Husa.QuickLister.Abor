namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Linq;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public static class GenericSpecifications
    {
        public static IQueryable<T> FilterById<T>(this IQueryable<T> query, Guid id)
            where T : Entity
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return query.Where(p => p.Id == id);
        }

        public static IQueryable<T> FilterNotDeleted<T>(this IQueryable<T> query)
            where T : Entity
        {
            return query.Where(p => !p.IsDeleted);
        }

        public static IQueryable<T> FilterMatchingAddress<T>(
            this IQueryable<T> listings,
            string streetName,
            string streetNumber,
            string zipCode,
            Guid companyId,
            bool partialMatch = false)
            where T : SaleListing
        {
            if (string.IsNullOrEmpty(streetNumber))
            {
                throw new ArgumentNullException(nameof(streetNumber));
            }

            if (string.IsNullOrEmpty(streetName))
            {
                throw new ArgumentNullException(nameof(streetName));
            }

            listings.Where(l => l.CompanyId == companyId &&
            l.SaleProperty.AddressInfo.StreetNumber.ToString() == streetNumber);

            if (partialMatch)
            {
                return listings.Where(l => streetName.Contains(l.SaleProperty.AddressInfo.StreetName.Split(" ", StringSplitOptions.None)[0]) &&
                !l.IsManuallyManaged);
            }

            if (string.IsNullOrEmpty(zipCode))
            {
                throw new ArgumentNullException(nameof(zipCode));
            }

            return listings.Where(l =>
                    l.SaleProperty.AddressInfo.StreetName.Contains(streetName) &&
                    l.SaleProperty.AddressInfo.ZipCode.Length >= 4 &&
                    l.SaleProperty.AddressInfo.ZipCode.Contains(zipCode));
        }

        public static IQueryable<T> FilterByCompany<T>(this IQueryable<T> query, Guid? companyId, bool applyFilter = true)
            where T : Entity
        {
            if (applyFilter && (!companyId.HasValue || companyId.Value.Equals(Guid.Empty)))
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return applyFilter ? query.Where(p => p.CompanyId.Equals(companyId)) : query;
        }

        public static IQueryable<T> FilterByCompany<T>(this IQueryable<T> query, IUserContext userContext)
            where T : Entity
        {
            return query.FilterByCompany(userContext.CompanyId, userContext.UserRole == UserRole.User || userContext.CompanyId.HasValue);
        }

        public static IQueryable<T> FilterByAddress<T>(this IQueryable<T> listings, string streetName, string streetNumber, string zipCode)
            where T : SaleListing
        {
            if (string.IsNullOrEmpty(streetName))
            {
                throw new ArgumentNullException(nameof(streetName));
            }

            if (string.IsNullOrEmpty(streetNumber))
            {
                throw new ArgumentNullException(nameof(streetNumber));
            }

            if (string.IsNullOrEmpty(zipCode))
            {
                throw new ArgumentNullException(nameof(zipCode));
            }

            return listings.Where(l => l.SaleProperty.AddressInfo.StreetName.Contains(streetName) &&
                                            l.SaleProperty.AddressInfo.StreetNumber.ToString().Contains(streetNumber) &&
                                            l.SaleProperty.AddressInfo.ZipCode.Contains(zipCode));
        }

        public static IQueryable<T> FilterByImportStatus<T>(this IQueryable<T> query, XmlStatus? xmlStatus)
            where T : IXmlProfileInfo
        {
            return xmlStatus.HasValue
                ? query.Where(p => p.XmlStatus == xmlStatus)
                : query.Where(p => p.XmlStatus == XmlStatus.Approved || p.XmlStatus == XmlStatus.NotFromXml);
        }
    }
}
