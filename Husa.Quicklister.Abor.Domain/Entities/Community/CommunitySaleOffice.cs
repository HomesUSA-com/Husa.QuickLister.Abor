namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Xml.Api.Contracts.Response;

    public class CommunitySaleOffice : SalesOffice
    {
        public bool IsSalesOffice { get; set; }

        public static CommunitySaleOffice ImportFromXml(SubdivisionResponse subdivision, CommunitySaleOffice salesOffice)
        {
            var importedSalesOffice = new CommunitySaleOffice();
            if (salesOffice != null)
            {
                importedSalesOffice = (CommunitySaleOffice)salesOffice.Clone();
                importedSalesOffice.IsSalesOffice = true;
            }

            importedSalesOffice.StreetNumber = subdivision.SaleOffice.StreetNum;
            importedSalesOffice.StreetName = subdivision.SaleOffice.StreetName;
            importedSalesOffice.StreetSuffix = subdivision.SaleOffice.StreetSuffix;
            importedSalesOffice.SalesOfficeCity = subdivision.SaleOffice.City.ToCity(isExactValue: false);
            importedSalesOffice.SalesOfficeZip = subdivision.SaleOffice.Zip;

            return importedSalesOffice;
        }

        public virtual CommunitySaleOffice UpdateFromXml(SalesOfficeResponse salesOffice)
        {
            var clonnedSalesOffice = (CommunitySaleOffice)this.Clone();

            clonnedSalesOffice.IsSalesOffice = this.IsSalesOffice;

            if (!string.IsNullOrEmpty(salesOffice.StreetNum))
            {
                clonnedSalesOffice.StreetNumber = salesOffice.StreetNum;
            }

            if (!string.IsNullOrEmpty(salesOffice.StreetName))
            {
                clonnedSalesOffice.StreetName = salesOffice.StreetName;
            }

            if (!string.IsNullOrEmpty(salesOffice.StreetSuffix))
            {
                clonnedSalesOffice.StreetSuffix = salesOffice.StreetSuffix;
            }

            var city = salesOffice.City.ToCity(isExactValue: false);
            if (city.HasValue)
            {
                clonnedSalesOffice.SalesOfficeCity = city;
            }

            if (!string.IsNullOrEmpty(salesOffice.Zip))
            {
                clonnedSalesOffice.SalesOfficeZip = salesOffice.Zip;
            }

            return clonnedSalesOffice;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StreetNumber;
            yield return this.StreetName;
            yield return this.StreetSuffix;
            yield return this.SalesOfficeCity;
            yield return this.SalesOfficeZip;
            yield return this.IsSalesOffice;
        }
    }
}
