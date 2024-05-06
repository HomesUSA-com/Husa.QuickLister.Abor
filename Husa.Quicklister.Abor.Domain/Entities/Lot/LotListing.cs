namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions.Lot;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;

    public class LotListing : Entities.Listing.Listing, IGenerateListingRequest<LotListingRequest>
    {
        public LotListing(
                MarketStatuses mlsStatus,
                string streetName,
                string streetNum,
                Cities city,
                States state,
                string zipCode,
                Guid companyId,
                string ownerName,
                Counties? county = null,
                Guid? communityId = null,
                bool manuallyManaged = false)
                : this()
        {
            this.CompanyId = companyId;
            this.MlsStatus = mlsStatus;
            this.CompanyId = companyId;
            this.OwnerName = ownerName;
            this.CommunityId = communityId;
            this.AddressInfo = new(streetNum, streetName, zipCode, city, state, county);
            this.IsManuallyManaged = manuallyManaged;
        }

        public LotListing()
            : base()
        {
            this.FeaturesInfo = new();
            this.SchoolsInfo = new();
            this.FinancialInfo = new();
            this.ShowingInfo = new();
            this.PropertyInfo = new();
            this.AddressInfo = new();
            this.StatusFieldsInfo = new();
            this.ManagementTraces = new List<LotManagementTrace>();
        }

        public override ListType ListType { get; protected set; } = ListType.Lots;
        public virtual string OwnerName { get; set; }
        public virtual Guid? CommunityId { get; set; }
        public virtual LotAddressInfo AddressInfo { get; set; }
        public virtual LotSchoolsInfo SchoolsInfo { get; set; }
        public virtual LotPropertyInfo PropertyInfo { get; set; }
        public virtual LotFeaturesInfo FeaturesInfo { get; set; }
        public virtual LotFinancialInfo FinancialInfo { get; set; }
        public virtual LotShowingInfo ShowingInfo { get; set; }
        public virtual CommunitySale Community { get; set; }
        public virtual ListingStatusFieldsInfo StatusFieldsInfo { get; set; }
        public virtual ICollection<LotManagementTrace> ManagementTraces { get; set; }

        public virtual bool IsInMarket => !string.IsNullOrEmpty(this.MlsNumber);
        public override bool HasStatusToBeClosed => this.MlsStatus == MarketStatuses.Closed || this.MlsStatus == MarketStatuses.Canceled;

        public LotListing Clone()
        {
            var clonedProperty = (LotListing)this.MemberwiseClone();
            clonedProperty.AddressInfo = this.AddressInfo.Clone();
            clonedProperty.PropertyInfo = this.PropertyInfo.Clone();
            clonedProperty.FinancialInfo = this.FinancialInfo.Clone();
            clonedProperty.SchoolsInfo = this.SchoolsInfo.Clone();
            clonedProperty.FeaturesInfo = this.FeaturesInfo.Clone();
            clonedProperty.ShowingInfo = this.ShowingInfo.Clone();
            return clonedProperty;
        }

        public virtual void CompleteListingRequest(string mlsNumber, Guid userId, MarketStatuses requestStatus, ActionType actionType, bool isDownloaderEnabled)
        {
            if (string.IsNullOrEmpty(this.MlsNumber))
            {
                if (string.IsNullOrEmpty(mlsNumber))
                {
                    throw new DomainException($"Cannot assign an empty mls number to the listing id {this.Id}");
                }

                this.MlsNumber = mlsNumber;

                if (!isDownloaderEnabled && this.ListDate == null)
                {
                    this.ListDate = DateTime.UtcNow;
                }
            }

            if (this.PublishInfo?.PublishType == null)
            {
                this.PublishInfo = new PublishInfo(actionType, userId, requestStatus);
            }

            this.Lock(userId, LockedStatus.LockedBySystem);
        }

        public override void UpdateManuallyManagement(bool manuallyManaged)
        {
            if (this.IsManuallyManaged != manuallyManaged)
            {
                this.IsManuallyManaged = manuallyManaged;
                this.ManagementTraces.Add(new LotManagementTrace(this, this.CompanyId, manuallyManaged));
            }
        }

        public virtual CommandSingleResult<LotListingRequest, ValidationResult> GenerateRequest(Guid userId)
        {
            try
            {
                var request = new LotListingRequest(listing: this, userId);
                return this.AddRequest(request, userId);
            }
            catch (Exception ex)
            {
                this.LockUnsubmitted(userId);
                return CommandSingleResult<LotListingRequest, ValidationResult>.Error(new ValidationResult(ex.Message));
            }
        }

        public virtual CommandSingleResult<LotListingRequest, ValidationResult> GenerateRequestFromCommunity(LotListingRequest lastCompletedRequest, Guid userId)
        {
            this.UpdateListingFromCommunitySubmit();

            var newRequest = lastCompletedRequest.Clone();
            newRequest.ImportDataFromCommunitySubmit(this.Community);
            newRequest.UpdateTrackValues(userId, isNewRecord: true);
            newRequest.MlsNumber = this.MlsNumber;
            newRequest.ListDate = this.ListDate;

            return this.AddRequest(newRequest, userId);
        }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.OwnerName;
            yield return this.ListPrice;
            yield return this.MlsNumber;
            yield return this.AddressInfo;
            yield return this.SchoolsInfo;
            yield return this.PropertyInfo;
            yield return this.FeaturesInfo;
            yield return this.FinancialInfo;
            yield return this.ShowingInfo;
        }
    }
}
