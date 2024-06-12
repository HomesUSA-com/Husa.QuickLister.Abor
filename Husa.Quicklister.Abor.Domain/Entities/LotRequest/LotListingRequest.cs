namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Common;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using ExtensionsEntities = Husa.Quicklister.Extensions.Domain.Entities.Request;

    public class LotListingRequest : ExtensionsEntities.ListingRequest
    {
        public LotListingRequest(LotListing listing, Guid userId)
            : base(listing.MlsNumber, listing.ListDate, listing.ExpirationDate)
        {
            this.Id = Guid.NewGuid();
            this.EntityId = listing.Id;
            this.CompanyId = listing.CompanyId;
            this.CommunityId = listing.CommunityId;
            this.OwnerName = listing.OwnerName;
            this.MlsStatus = listing.MlsStatus;
            this.ListPrice = listing.ListPrice;
            this.StatusFieldsInfo = LotStatusFieldsRecord.CreateRecord(listing.StatusFieldsInfo);
            this.PublishInfo = PublishFieldsRecord.CreateRecord(listing.PublishInfo);
            this.AddressInfo = LotAddressRecord.CreateRecord(listing.AddressInfo);
            this.PropertyInfo = LotPropertyRecord.CreateRecord(listing.PropertyInfo);
            this.FeaturesInfo = LotFeaturesRecord.CreateRecord(listing.FeaturesInfo);
            this.FinancialInfo = LotFinancialRecord.CreateRecord(listing.FinancialInfo);
            this.ShowingInfo = LotShowingRecord.CreateRecord(listing.ShowingInfo);
            this.SchoolsInfo = LotSchoolRecord.CreateRecord(listing.SchoolsInfo);
            this.UpdateTrackValues(userId, isNewRecord: true);
        }

        protected LotListingRequest()
            : base()
        {
            this.StatusFieldsInfo = new();
            this.PublishInfo = new();
            this.FeaturesInfo = new();
            this.SchoolsInfo = new();
            this.FinancialInfo = new();
            this.ShowingInfo = new();
            this.PropertyInfo = new();
            this.AddressInfo = new();
            this.SalesOfficeInfo = new();
        }

        public virtual Guid? CommunityId { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int? CDOM { get; set; }
        public virtual int? DOM { get; set; }

        [Required]
        [Range(5000, 3000000, ErrorMessage = "{0} must be between {1} and {2}")]
        public virtual decimal? ListPrice { get; set; }

        public virtual DateTime? MarketModifiedOn { get; }
        public virtual string MarketUniqueId { get; }
        public virtual MarketStatuses MlsStatus { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotAddressRecord AddressInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotPropertyRecord PropertyInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotFeaturesRecord FeaturesInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotFinancialRecord FinancialInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotShowingRecord ShowingInfo { get; set; }

        [Required]
        [ValidateProperties]
        public virtual LotSchoolRecord SchoolsInfo { get; set; }

        public virtual SalesOfficeRecord SalesOfficeInfo { get; set; }
        public virtual LotStatusFieldsRecord StatusFieldsInfo { get; set; }
        public virtual PublishFieldsRecord PublishInfo { get; set; }
        public virtual string Address => $"{this.AddressInfo.StreetNumber} {this.AddressInfo.StreetName}";

        public virtual LotListingRequest Clone()
        {
            var clonedRequest = (LotListingRequest)this.MemberwiseClone();
            clonedRequest.Id = Guid.NewGuid();
            clonedRequest.SysCreatedOn = DateTime.UtcNow;
            clonedRequest.SysTimestamp = DateTime.UtcNow;
            clonedRequest.RequestState = ListingRequestState.Pending;
            clonedRequest.AddressInfo = this.AddressInfo.CloneRecord();
            clonedRequest.PropertyInfo = this.PropertyInfo.CloneRecord();
            clonedRequest.FeaturesInfo = this.FeaturesInfo.CloneRecord();
            clonedRequest.FinancialInfo = this.FinancialInfo.CloneRecord();
            clonedRequest.ShowingInfo = this.ShowingInfo.CloneRecord();
            clonedRequest.SchoolsInfo = this.SchoolsInfo.CloneRecord();
            clonedRequest.SalesOfficeInfo = this.SalesOfficeInfo.CloneRecord();
            clonedRequest.StatusFieldsInfo = this.StatusFieldsInfo.CloneRecord();
            clonedRequest.PublishInfo = this.PublishInfo.CloneRecord();

            return clonedRequest;
        }

        public void UpdatePropertyInformation(LotPropertyValueObject listing)
        {
            this.AddressInfo = LotAddressRecord.CreateRecord(listing.AddressInfo);
            this.PropertyInfo = LotPropertyRecord.CreateRecord(listing.PropertyInfo);
            this.FeaturesInfo = LotFeaturesRecord.CreateRecord(listing.FeaturesInfo);
            this.FinancialInfo = LotFinancialRecord.CreateRecord(listing.FinancialInfo);
            this.ShowingInfo = LotShowingRecord.CreateRecord(listing.ShowingInfo);
            this.SchoolsInfo = LotSchoolRecord.CreateRecord(listing.SchoolsInfo);
        }

        public virtual void UpdateRequestInformation(ListingRequestValueObject listingRequestValue, ListingStatusFieldsInfo statusFieldsInfo, LotPropertyValueObject propertyValue)
        {
            this.ExpirationDate = listingRequestValue.ExpirationDate;
            this.ListDate = listingRequestValue.ListDate;
            this.ListPrice = listingRequestValue.ListPrice;
            this.MlsStatus = listingRequestValue.MlsStatus;
            this.StatusFieldsInfo.UpdateInformation(statusFieldsInfo);
            this.UpdatePropertyInformation(propertyValue);
        }

        public virtual void UpdateBaseInformation(ListingRequestValueObject listingRequestValue)
        {
            this.ListDate = listingRequestValue.ListDate;
            this.ExpirationDate = listingRequestValue.ExpirationDate;
            this.ListPrice = listingRequestValue.ListPrice;
            this.MlsStatus = listingRequestValue.MlsStatus;
        }

        public override IEnumerable<ValidationResult> IsValidForSubmit()
        {
            var validationResults = ValidatePropertiesAttribute.GetErrors(this);
            var statusResults = ValidateListingStatus<StatusFieldsRecord>.GetErrors(this.MlsStatus, this.StatusFieldsInfo);
            return statusResults != null ? validationResults.Concat(new[] { statusResults }) : validationResults;
        }

        public override IEnumerable<SummarySection> GetSummary<TListingRequest>(TListingRequest previousRequest)
            => this.GetSummary(previousRequest as LotListingRequest);

        public IEnumerable<SummarySection> GetSummary(LotListingRequest previousRequest)
        {
            var summarySections = this.GetSummarySections(previousRequest).Where(summary => summary != null).ToList();
            var rootFieldChanges = this.GetRootFieldsSummary(previousRequest);

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

        private IEnumerable<SummarySection> GetSummarySections(LotListingRequest prevRecord)
        {
            yield return this.PropertyInfo.GetSummarySection(prevRecord?.PropertyInfo, sectionName: nameof(this.PropertyInfo));
            yield return this.AddressInfo.GetSummarySection(prevRecord?.AddressInfo, sectionName: nameof(this.AddressInfo));
            yield return this.FeaturesInfo.GetSummarySection(prevRecord?.FeaturesInfo, sectionName: nameof(this.FeaturesInfo));
            yield return this.FinancialInfo.GetSummarySection(prevRecord?.FinancialInfo, sectionName: nameof(this.FinancialInfo));
            yield return this.SchoolsInfo.GetSummarySection(prevRecord?.SchoolsInfo, sectionName: nameof(this.SchoolsInfo));
            yield return this.ShowingInfo.GetSummarySection(prevRecord?.ShowingInfo, sectionName: nameof(this.ShowingInfo));
            yield return this.SalesOfficeInfo?.GetSummarySection(prevRecord?.SalesOfficeInfo, sectionName: nameof(this.SalesOfficeInfo));
            yield return this.StatusFieldsInfo?.GetSummarySection(prevRecord?.StatusFieldsInfo, this.MlsStatus, sectionName: nameof(this.StatusFieldsInfo));
        }

        private IEnumerable<SummaryField> GetRootFieldsSummary(LotListingRequest oldObject) => SummaryExtensions.GetFieldSummary(
            this, oldObject, filterFields: new[]
            {
                nameof(this.ListDate),
                nameof(this.ListPrice),
                nameof(this.ExpirationDate),
                nameof(this.OwnerName),
                nameof(this.MlsStatus),
            });
    }
}
