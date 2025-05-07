namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using System;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlListingExtensions
    {
        public static void UpdateFromXml(this SaleListing listing, XmlListingDetailResponse xmlListing, bool ignoreRequestByCompletionDate = false, bool ignoreRequestByDescription = false)
        {
            ArgumentNullException.ThrowIfNull(xmlListing);
            if (xmlListing.Price.HasValue && listing.ListPrice.HasValue && xmlListing.Price.Value != listing.ListPrice.Value && !listing.MlsStatus.IsAlowedStatusXmlForRequest())
            {
                listing.ListPrice = xmlListing.Price;
            }

            listing.XmlListingId = xmlListing.Id;
            listing.SaleProperty.PropertyInfo.UpdateFromXml(xmlListing, ignoreRequestByCompletionDate);
            listing.SaleProperty.FeaturesInfo.UpdateFromXml(xmlListing, ignoreRequestByDescription);
        }
    }
}
