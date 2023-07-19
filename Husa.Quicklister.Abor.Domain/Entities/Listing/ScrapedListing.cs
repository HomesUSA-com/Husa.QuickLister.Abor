namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class ScrapedListing : Entity
    {
        public ScrapedListing(ScrapedListingValueObject scrapedListingValue)
        {
            this.ListingDetails = scrapedListingValue != null ? SetInformation(scrapedListingValue) : throw new ArgumentNullException(nameof(scrapedListingValue));
        }

        protected ScrapedListing()
        {
        }

        public static MarketCode Market => MarketCode.SanAntonio;
        public ScrapedListingValueObject ListingDetails { get; set; }

        public void UpdateInformation(ScrapedListingValueObject scrapedListingValue)
        {
            if (scrapedListingValue is null)
            {
                throw new ArgumentNullException(nameof(scrapedListingValue));
            }

            if (this.ListingDetails == scrapedListingValue)
            {
                return;
            }

            if (scrapedListingValue.Refreshed <= this.ListingDetails.ListDate)
            {
                throw new ArgumentException("Scraped listing value: refreshed date is earlier than list date");
            }

            this.ListingDetails = SetInformation(scrapedListingValue);
        }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.ListingDetails;
        }

        private static ScrapedListingValueObject SetInformation(ScrapedListingValueObject scrapedListingValue)
        {
            if (scrapedListingValue.ListPrice < 0 || scrapedListingValue.Price < 0)
            {
                throw new ArgumentException("Scraped listing value: list price or price is less than 0");
            }

            if (scrapedListingValue.ListPrice == 0 && scrapedListingValue.Price == 0)
            {
                throw new ArgumentException("Scraped listing value: list price and price can't be both 0");
            }

            if (scrapedListingValue.ListPrice == 0)
            {
                scrapedListingValue.Comment = "Not in MLS";
            }
            else if (scrapedListingValue.Price == 0)
            {
                scrapedListingValue.Comment = "Not in Builder Website";
            }

            return scrapedListingValue;
        }
    }
}
