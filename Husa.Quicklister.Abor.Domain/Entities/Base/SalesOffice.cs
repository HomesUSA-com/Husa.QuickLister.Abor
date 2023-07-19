namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;

    public class SalesOffice : ValueObject
    {
        public SalesOffice(string streetNumber, string streetName, string streetSuffix, Cities? city, string zip)
        {
            this.StreetNumber = streetNumber;
            this.StreetName = streetName;
            this.StreetSuffix = streetSuffix;
            this.SalesOfficeCity = city;
            this.SalesOfficeZip = zip;
        }

        public SalesOffice()
        {
        }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string StreetSuffix { get; set; }

        public Cities? SalesOfficeCity { get; set; }

        public string SalesOfficeZip { get; set; }

        public static SalesOffice ImportFromXml(XmlListingDetailResponse listing, SalesOffice salesOffice)
        {
            var importedSalesOffice = new SalesOffice();
            if (salesOffice != null)
            {
                importedSalesOffice = salesOffice.Clone();
            }

            return importedSalesOffice;
        }

        public virtual SalesOffice Clone()
        {
            return (SalesOffice)this.MemberwiseClone();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StreetNumber;
            yield return this.StreetName;
            yield return this.StreetSuffix;
            yield return this.SalesOfficeCity;
            yield return this.SalesOfficeZip;
        }
    }
}
