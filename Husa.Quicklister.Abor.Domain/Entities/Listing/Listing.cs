namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using ExtensionsEntities = Husa.Quicklister.Extensions.Domain.Entities.Listing;

    public abstract class Listing : ExtensionsEntities.XmlListing
    {
        public static readonly IEnumerable<ActionType> RelistAndComparable = new[] { ActionType.Relist, ActionType.Comparable };

        protected Listing()
            : base()
        {
        }

        public virtual int? CDOM { get; set; }

        public virtual int? DOM { get; set; }

        public virtual decimal? ListPrice { get; set; }

        public virtual ListType ListType { get; protected set; }

        public virtual DateTime? MarketModifiedOn { get; set; }

        public virtual string MarketUniqueId { get; set; }

        public virtual MarketStatuses MlsStatus { get; set; }

        public virtual Guid? XmlDiscrepancyListingId { get; set; }

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
            this.CDOM = listingValue.CDOM;
            this.DOM = listingValue.DOM;
            this.ListDate = listingValue.ListDate;
            this.ListPrice = listingValue.ListPrice;
            this.ListType = listingValue.ListType;
            this.MarketModifiedOn = listingValue.MarketModifiedOn;
            this.MlsNumber = listingValue.MlsNumber;
            this.MlsStatus = listingValue.MlsStatus;
        }
    }
}
