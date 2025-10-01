namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlSpacesDimensionsExtensions
    {
        public static void UpdateFromXml(this IProvideSpacesDimensions fields, XmlListingDetailResponse xmlListing, bool manageSqft = false)
        {
            if (!manageSqft)
            {
                return;
            }

            fields.SqFtTotal = xmlListing.Sqft ?? fields.SqFtTotal;
        }
    }
}
