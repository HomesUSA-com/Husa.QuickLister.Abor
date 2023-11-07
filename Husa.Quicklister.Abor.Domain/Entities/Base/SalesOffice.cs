namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;
    using ExtensionDomain = Husa.Quicklister.Extensions.Domain.Entities.Base;

    public class SalesOffice : ExtensionDomain.SalesOffice<Cities>
    {
        public SalesOffice(string streetNumber, string streetName, string streetSuffix, Cities? city, string zip)
            : base(streetNumber, streetName, streetSuffix, city, zip)
        {
        }

        public SalesOffice()
        {
        }

        public static SalesOffice ImportFromXml(XmlListingDetailResponse listing, SalesOffice salesOffice)
        {
            var importedSalesOffice = new SalesOffice();
            if (salesOffice != null)
            {
                importedSalesOffice = salesOffice.Clone();
            }

            return importedSalesOffice;
        }

        public override SalesOffice Clone()
        {
            return (SalesOffice)this.MemberwiseClone();
        }
    }
}
