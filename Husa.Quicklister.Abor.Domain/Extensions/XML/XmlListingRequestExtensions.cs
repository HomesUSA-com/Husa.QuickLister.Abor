namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using System;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlListingRequestExtensions
    {
        public static void UpdateXromXml(
            this SaleListingRequest listing,
            XmlListingDetailResponse xmlListing,
            ImportActionType? listAction = null,
            bool ignoreRequestByCompletionDate = false,
            bool ignoreRequestByDescription = false)
        {
            ArgumentNullException.ThrowIfNull(xmlListing);
            if (xmlListing.Price.HasValue && listing.ListPrice.HasValue && xmlListing.Price.Value != listing.ListPrice.Value && !listing.MlsStatus.IsAlowedStatusXmlForRequest())
            {
                listing.ListPrice = xmlListing.Price;
            }

            listing.SaleProperty.PropertyInfo.UpdateFromXml(xmlListing, ignoreRequestByCompletionDate);
            listing.SaleProperty.FeaturesInfo.UpdateFromXml(xmlListing, ignoreRequestByDescription);
        }
    }
}
