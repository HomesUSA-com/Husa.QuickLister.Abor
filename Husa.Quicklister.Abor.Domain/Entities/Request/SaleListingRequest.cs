namespace Husa.Quicklister.Abor.Domain.Entities.Request
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Common;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public class SaleListingRequest : ListingRequest
    {
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
            this.StatusFieldsInfo = StatusFieldsRecord.CreateRecord(saleListing.StatusFieldsInfo);
            this.SaleProperty = SalePropertyRecord.CreateRecord(saleListing.SaleProperty);
            this.PublishInfo = PublishFieldsRecord.CreateRecord(saleListing.PublishInfo);
            this.UpdateTrackValues(userId, isNewRecord: true);
        }

        protected SaleListingRequest()
            : base()
        {
            this.StatusFieldsInfo = new();
            this.SaleProperty = new();
            this.PublishInfo = new();
        }

        public virtual Guid ListingSaleId { get; set; }

        [Required]
        [ValidateProperties]
        public virtual SalePropertyRecord SaleProperty { get; set; }

        public virtual StatusFieldsRecord StatusFieldsInfo { get; set; }

        public virtual PublishFieldsRecord PublishInfo { get; set; }

        public override Guid ListingId => this.ListingSaleId;

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

        public virtual void UpdateRequestInformation(ListingRequestValueObject listingRequestValue, ListingSaleStatusFieldsInfo statusFieldsInfo, SalePropertyValueObject salePropertyValue)
        {
            this.ExpirationDate = listingRequestValue.ExpirationDate;
            this.ListDate = listingRequestValue.ListDate;
            this.ListPrice = listingRequestValue.ListPrice;
            this.MlsStatus = listingRequestValue.MlsStatus;
            this.StatusFieldsInfo.UpdateInformation(statusFieldsInfo);
            this.SaleProperty.UpdateInformation(salePropertyValue);
        }

        public override IEnumerable<SummarySection> GetSummary<TListingRequest>(TListingRequest previousRequest)
            => this.GetSummary(previousRequest as SaleListingRequest);

        protected SummarySection GenerateSummary(SaleListingRequest previousRequest)
        {
            var summaryFields = this.GetRequestSummary(previousRequest);
            if (!summaryFields.Any())
            {
                return new()
                {
                    Name = SummarySection,
                    Fields = Array.Empty<SummaryField>(),
                };
            }

            if (!summaryFields.Any(x => x.FieldName == nameof(this.MlsStatus)))
            {
                summaryFields = summaryFields.Append(new()
                {
                    FieldName = nameof(this.MlsStatus),
                    NewValue = this.MlsStatus,
                    OldValue = this.MlsStatus,
                });
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }

        private IEnumerable<SummarySection> GetSummary(SaleListingRequest previousRequest)
        {
            var summarySections = new List<SummarySection>
            {
                this.GenerateSummary(previousRequest),
            };

            if (previousRequest is null || !this.SaleProperty.Equals(previousRequest.SaleProperty))
            {
                summarySections.AddRange(this.SaleProperty.GetSummarySections(previousRequest?.SaleProperty));
            }

            summarySections.Add(this.StatusFieldsInfo.GetSummary(previousRequest?.StatusFieldsInfo, this.MlsStatus));

            return summarySections.Where(summary => summary != null);
        }
    }
}
