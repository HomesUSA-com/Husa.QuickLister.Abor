namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using ExtensionsEntities = Husa.Quicklister.Extensions.Domain.Entities.Request;

    public abstract class ListingRequest : ExtensionsEntities.SaleListingRequest, IProvideListingInfo
    {
        protected ListingRequest(ListingRequestValueObject listingRequestValue)
            : this(
                  listingRequestValue.MlsStatus,
                  listingRequestValue.MlsNumber,
                  listingRequestValue.ListPrice,
                  listingRequestValue.ListDate,
                  listingRequestValue.ExpirationDate)
        {
        }

        protected ListingRequest(
            MarketStatuses mlsStatus,
            string mlsNumber,
            decimal listPrice,
            DateTime? listDate,
            DateTime? expirationDate)
            : base(mlsNumber, listDate, expirationDate)
        {
            this.MlsStatus = mlsStatus;
            this.ListPrice = listPrice;
        }

        protected ListingRequest()
            : base()
        {
        }

        public virtual int? CDOM { get; set; }

        public virtual int? DOM { get; set; }

        [Required]
        [Range(100000, 3000000, ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? ListPrice { get; set; }

        public virtual DateTime? MarketModifiedOn { get; }

        public virtual string MarketUniqueId { get; }

        public virtual MarketStatuses MlsStatus { get; set; }

        public virtual void UpdateBaseInformation(ListingRequestValueObject listingRequestValue)
        {
            this.ListDate = listingRequestValue.ListDate;
            this.ExpirationDate = listingRequestValue.ExpirationDate;
            this.ListPrice = listingRequestValue.ListPrice;
            this.MlsStatus = listingRequestValue.MlsStatus;
        }

        public virtual IEnumerable<SummaryField> GetRequestSummary(ListingRequest oldObject) => SummaryExtensions.GetFieldSummary(
            this, oldObject, excludeFields: new string[]
            {
                nameof(this.Id),
                nameof(this.MlsNumber),
                nameof(this.CDOM),
                nameof(this.DOM),
                nameof(this.RequestState),
                nameof(this.SysCreatedOn),
                nameof(this.SysCreatedBy),
                nameof(this.SysModifiedOn),
                nameof(this.SysModifiedBy),
                nameof(this.SysTimestamp),
                nameof(this.LegacyId),
            });
    }
}
