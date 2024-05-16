namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
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
            this.PublishInfo = new();
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

        public virtual PublishInfo PublishInfo { get; set; }

        public virtual ListingStatusFieldsInfo StatusFieldsInfo { get; set; }

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

        public virtual void UpdateStatusFieldsInfo(ListingStatusFieldsInfo listingSaleStatusFields)
        {
            if (listingSaleStatusFields is null)
            {
                throw new ArgumentNullException(nameof(listingSaleStatusFields));
            }

            this.CopyInformationFromValueObject(listingSaleStatusFields);
        }

        protected void CopyInformationFromValueObject(ListingStatusFieldsInfo listingSaleStatusFields)
        {
            if (!this.isMarketUpdate)
            {
                this.StatusFieldsInfo.AgentId = listingSaleStatusFields.AgentId;
                this.StatusFieldsInfo.AgentIdSecond = listingSaleStatusFields.AgentIdSecond;
                this.StatusFieldsInfo.HasSecondBuyerAgent = listingSaleStatusFields.HasSecondBuyerAgent;
                this.StatusFieldsInfo.HasContingencyInfo = listingSaleStatusFields.HasContingencyInfo;
                this.StatusFieldsInfo.ContingencyInfo = listingSaleStatusFields.ContingencyInfo;
                this.StatusFieldsInfo.CancelledReason = listingSaleStatusFields.CancelledReason;

                if (!this.migrateFullListing)
                {
                    return;
                }
            }

            if (this.processFullListing)
            {
                this.StatusFieldsInfo.SellConcess = listingSaleStatusFields.SellConcess;
                this.StatusFieldsInfo.ClosePrice = listingSaleStatusFields.ClosePrice;
                this.StatusFieldsInfo.HasBuyerAgent = listingSaleStatusFields.HasBuyerAgent;
                this.StatusFieldsInfo.PendingDate = listingSaleStatusFields.PendingDate;
                this.StatusFieldsInfo.ClosedDate = listingSaleStatusFields.ClosedDate;
                this.StatusFieldsInfo.BackOnMarketDate = listingSaleStatusFields.BackOnMarketDate;
                this.StatusFieldsInfo.OffMarketDate = listingSaleStatusFields.OffMarketDate;
                this.StatusFieldsInfo.SaleTerms = listingSaleStatusFields.SaleTerms;
            }

            this.StatusFieldsInfo.EstimatedClosedDate = listingSaleStatusFields.EstimatedClosedDate;
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
