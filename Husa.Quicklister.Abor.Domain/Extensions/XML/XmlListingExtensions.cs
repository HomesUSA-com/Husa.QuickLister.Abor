namespace Husa.Quicklister.Abor.Domain.Extensions.XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Extensions.Listing;
    using Husa.Xml.Api.Contracts.Response;

    public static class XmlListingExtensions
    {
        public static readonly IEnumerable<MarketStatuses> PendingAndCanceledStatuses = MarketStatusesExtensions.PendingAndCanceledStatuses;
        public static void UpdateFromXml(this SaleListing listing, XmlListingDetailResponse xmlListing, bool ignoreRequestByCompletionDate = false, bool ignoreRequestByDescription = false)
        {
            ArgumentNullException.ThrowIfNull(xmlListing);
            if (xmlListing.Price.HasValue && listing.ListPrice.HasValue && xmlListing.Price.Value != listing.ListPrice.Value && !PendingAndCanceledStatuses.Contains(listing.MlsStatus))
            {
                listing.ListPrice = xmlListing.Price;
            }

            listing.XmlListingId = xmlListing.Id;
            listing.SaleProperty.PropertyInfo.UpdateFromXml(xmlListing, ignoreRequestByCompletionDate);
            listing.SaleProperty.FeaturesInfo.UpdateFromXml(xmlListing, ignoreRequestByDescription);
        }
    }
}
