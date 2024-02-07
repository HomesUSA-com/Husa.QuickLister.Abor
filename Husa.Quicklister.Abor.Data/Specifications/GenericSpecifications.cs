namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;

    public static class GenericSpecifications
    {
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

            listings = listings.Where(l => l.CompanyId == companyId &&
            l.SaleProperty.AddressInfo.StreetNumber == streetNumber);

            if (partialMatch)
            {
                var streetNameSplit = streetName.Split(" ");
                return listings.Where(l => l.SaleProperty.AddressInfo.StreetName != null && l.SaleProperty.AddressInfo.StreetName.StartsWith(streetNameSplit[0]) &&
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

        public static IQueryable<T> FilterByInvoice<T>(this IQueryable<T> listings)
            where T : SaleListing
        {
            return listings.Where(l => string.IsNullOrEmpty(l.InvoiceInfo.InvoiceId));
        }
    }
}
