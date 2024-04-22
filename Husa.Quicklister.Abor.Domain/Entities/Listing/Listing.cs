namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Attributes;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using ExtensionListing = Husa.Quicklister.Extensions.Domain.Entities.Listing.Listing;

    public abstract class Listing : ExtensionListing
    {
        protected bool isMarketUpdate = false;
        protected bool processFullListing = true;
        protected bool migrateFullListing = true;

        protected Listing()
            : base()
        {
        }

        public virtual int? CDOM { get; set; }

        public virtual int? DOM { get; set; }

        [XmlPropertyUpdate]
        public virtual decimal? ListPrice { get; set; }

        public virtual ListType ListType { get; protected set; } = ListType.Residential;

        public virtual DateTime? MarketModifiedOn { get; set; }

        public virtual string MarketUniqueId { get; set; }

        [XmlPropertyUpdate]
        public virtual MarketStatuses MlsStatus { get; set; }

        public virtual void SetMigrateFullListing(bool migrateFullListing)
        {
            this.migrateFullListing = migrateFullListing;
        }

        public virtual void UpdateBaseListingInfo(
            ListType listType,
            decimal? listPrice,
            DateTime? expirationDate,
            DateTime? listDate,
            MarketStatuses mlsStatus,
            LockedStatus lockedStatus,
            Guid userId)
        {
            this.UpdateBaseListingInfo(expirationDate, listDate, lockedStatus, userId);

            this.ListPrice = listPrice;
            this.ListType = listType;
            this.MlsStatus = mlsStatus;
        }

        public virtual void UpdateBaseListingInformation(ListingValueObject listingValue)
        {
            this.CopyInformationFromValueObject(listingValue);
        }

        protected void CopyInformationFromValueObject(ListingValueObject listingValue)
        {
            if (this.processFullListing)
            {
                this.CDOM = listingValue.CDOM;
                this.DOM = listingValue.DOM;
                this.ExpirationDate = listingValue.ExpirationDate;
                this.ListType = listingValue.ListType;
                this.MlsNumber = listingValue.MlsNumber;
                this.ListDate = listingValue.ListDate;
            }

            this.MarketModifiedOn = listingValue.MarketModifiedOn;
            this.MlsStatus = listingValue.MlsStatus;
            this.ListPrice = listingValue.ListPrice;
        }
    }
}
