namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Common;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class SaleListingRequest : ListingRequest
    {
        private Guid? companyId;

        public SaleListingRequest(SaleListing saleListing, Guid userId)
            : base(
                  saleListing.MlsStatus,
                  saleListing.MlsNumber,
                  saleListing.ListPrice.GetValueOrDefault(),
                  saleListing.ListDate,
                  saleListing.ExpirationDate)
        {
            this.Id = Guid.NewGuid();
            this.ListingSaleId = saleListing.Id;
            this.StatusFieldsInfo = SaleStatusFieldsRecord.CreateRecord(saleListing.StatusFieldsInfo);
            this.SaleProperty = SalePropertyRecord.CreateRecord(saleListing.SaleProperty);
            this.PublishInfo = PublishFieldsRecord.CreateRecord(saleListing.PublishInfo);
            this.UpdateTrackValues(userId, isNewRecord: true);
            this.CompanyId = this.SaleProperty.CompanyId;
        }

        protected SaleListingRequest()
            : base()
        {
            this.StatusFieldsInfo = new();
            this.SaleProperty = new();
            this.PublishInfo = new();
        }

        [Required]
        [ValidateProperties]
        public virtual SalePropertyRecord SaleProperty { get; set; }

        public virtual SaleStatusFieldsRecord StatusFieldsInfo { get; set; }

        public virtual PublishFieldsRecord PublishInfo { get; set; }

        public override Guid CompanyId
        {
            get { return this.companyId ?? this.SaleProperty.CompanyId; }
            set { this.companyId = value; }
        }

        public virtual SaleListingRequest Clone()
        {
            var clonedRequest = (SaleListingRequest)this.MemberwiseClone();
            clonedRequest.Id = Guid.NewGuid();
            clonedRequest.SysCreatedOn = DateTime.UtcNow;
            clonedRequest.SysTimestamp = DateTime.UtcNow;
            clonedRequest.RequestState = ListingRequestState.Pending;
            clonedRequest.SaleProperty = this.SaleProperty.CloneRecord();
            clonedRequest.StatusFieldsInfo = this.StatusFieldsInfo.CloneRecord();
            clonedRequest.PublishInfo = this.PublishInfo.CloneRecord();

            return clonedRequest;
        }

        public void UpdateRequestInformation(decimal listPrice, SaleProperty saleProperty)
        {
            this.ListPrice = listPrice;
            this.SetStatus(ListingRequestState.Pending);
            this.SaleProperty = SalePropertyRecord.CreateRecord(saleProperty);
        }

        public virtual void UpdateRequestInformation(ListingRequestValueObject listingRequestValue, ListingStatusFieldsInfo statusFieldsInfo, SalePropertyValueObject salePropertyValue)
        {
            this.ExpirationDate = listingRequestValue.ExpirationDate;
            this.ListDate = listingRequestValue.ListDate;
            this.ListPrice = listingRequestValue.ListPrice;
            this.MlsStatus = listingRequestValue.MlsStatus;
            this.StatusFieldsInfo.UpdateInformation(statusFieldsInfo);
            this.SaleProperty.UpdateInformation(salePropertyValue);
        }

        public override IEnumerable<ValidationResult> IsValidForSubmit()
        {
            var validationResults = base.IsValidForSubmit();
            var propertyResults = ValidateListingProperty<PropertyRecord>.GetErrors(this.MlsStatus, this.SaleProperty.PropertyInfo);

            if (propertyResults != null)
            {
                validationResults = validationResults.Concat(new[] { propertyResults });
            }

            return validationResults;
        }

        public override IEnumerable<SummarySection> GetSummary<TListingRequest>(TListingRequest previousRequest)
            => this.GetSummary(previousRequest as SaleListingRequest);

        public void UpdateLegacyInformation(Guid userId, int requestLegacyId, SaleListing listing)
        {
            this.LegacyId = requestLegacyId;
            this.SysModifiedBy = userId;
            this.SysCreatedBy = userId;
            this.ListingSaleId = listing.Id;
            this.CompanyId = listing.CompanyId;
            this.SaleProperty.Id = listing.SalePropertyId;
            this.SaleProperty.CompanyId = listing.CompanyId;
            this.SaleProperty.CommunityId = listing.SaleProperty.CommunityId;
            this.SaleProperty.PlanId = listing.SaleProperty.PlanId;
            this.SaleProperty.SysCreatedOn = listing.SaleProperty.SysCreatedOn;
            this.SaleProperty.SysTimestamp = listing.SaleProperty.SysTimestamp;
            this.SaleProperty.SysCreatedBy = listing.SaleProperty.SysCreatedBy;
            this.SaleProperty.Address = listing.SaleProperty.Address;
            this.SaleProperty.SysModifiedOn = this.SysModifiedOn;
            this.SaleProperty.SysModifiedBy = userId;
        }

        private IEnumerable<SummarySection> GetSummary(SaleListingRequest previousRequest)
        {
            var summarySections = new List<SummarySection>();
            if (previousRequest is null || !this.SaleProperty.Equals(previousRequest.SaleProperty))
            {
                summarySections.AddRange(this.SaleProperty.GetSummarySections(previousRequest?.SaleProperty));
            }

            summarySections.Add(this.StatusFieldsInfo.GetSummary(previousRequest?.StatusFieldsInfo, this.MlsStatus));
            summarySections = summarySections.Where(summary => summary != null).ToList();

            var rootFieldChanges = this.GetRequestSummary(previousRequest);

            if (summarySections.Count == 0 && !rootFieldChanges.Any())
            {
                return Array.Empty<SummarySection>();
            }

            if (!rootFieldChanges.Any(x => x.FieldName == nameof(this.MlsStatus)))
            {
                rootFieldChanges = rootFieldChanges.Append(new()
                {
                    FieldName = nameof(this.MlsStatus),
                    NewValue = this.MlsStatus,
                    OldValue = this.MlsStatus,
                });
            }

            return new List<SummarySection> { new() { Name = SummarySection, Fields = rootFieldChanges } }.Concat(summarySections);
        }
    }
}
