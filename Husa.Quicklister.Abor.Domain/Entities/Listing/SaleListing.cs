namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Extensions.Listing;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;
    using Husa.Xml.Api.Contracts.Response;

    public class SaleListing :
        Listing,
        ISaleListing<SaleProperty>,
        IGenerateListingRequest<SaleListingRequest>,
        IListingInvoiceInfo,
        IListingPlan<Plan>,
        IListingCommunity<CommunitySale>,
        IProvideLegacy
    {
        public const int YearsInThePast = -2;
        public const int MaxExpirationDaysInTheFuture = 10;
        public const int MaxExpirationDaysLeftInTheFuture = 7;
        public const int MaxDaysInMarket = 179;
        public const int MinPropertyDescriptionLength = 100;

        public static readonly IEnumerable<MarketStatuses> ActiveListingStatuses = MarketStatusesExtensions.ActiveListingStatuses;
        public static readonly IEnumerable<MarketStatuses> OrphanListingStatuses = MarketStatusesExtensions.OrphanListingStatuses;
        public static readonly IEnumerable<MarketStatuses> PendingListingStatuses = MarketStatusesExtensions.PendingListingStatuses;
        public static readonly IEnumerable<MarketStatuses> PendingAndCanceledStatuses = MarketStatusesExtensions.PendingAndCanceledStatuses;
        public static readonly IEnumerable<MarketStatuses> ActivePhotoRequestListingStatuses = MarketStatusesExtensions.ActivePhotoRequestListingStatuses;
        public static readonly IEnumerable<MarketStatuses> ActiveAndPendingListingStatuses = MarketStatusesExtensions.ActiveAndPendingListingStatuses;
        public static readonly IEnumerable<MarketStatuses> ExistingListingStatuses = MarketStatusesExtensions.ExistingListingStatuses;
        public static readonly IEnumerable<MarketStatuses> PendingAndActiveUnderContractStatuses = MarketStatusesExtensions.PendingAndActiveUnderContractStatuses;

        public SaleListing(
                MarketStatuses mlsStatus,
                string streetName,
                string streetNum,
                string unitNumber,
                Cities city,
                States state,
                string zipCode,
                Counties? county,
                DateTime? constructionCompletionDate,
                Guid companyId,
                string ownerName,
                Guid? communityId,
                Guid? planId,
                bool manuallyManaged)
                : this()
        {
            this.CompanyId = companyId;
            this.MlsStatus = mlsStatus;
            this.SaleProperty = new(streetName, streetNum, unitNumber, city, state, zipCode, county, constructionCompletionDate, companyId, ownerName, communityId, planId);
            this.IsManuallyManaged = manuallyManaged;
        }

        public SaleListing(ListingValueObject listingInfo, ListingStatusFieldsInfo statusFieldsInfo, SalePropertyValueObject salePropertyInfo, Guid companyId, bool isManuallyManaged = false)
            : this()
        {
            this.CompanyId = companyId;
            this.StatusFieldsInfo = statusFieldsInfo;
            this.SaleProperty = new(salePropertyInfo, companyId);
            this.SalePropertyId = listingInfo.PropertyId;
            this.IsManuallyManaged = isManuallyManaged;
            this.CopyInformationFromValueObject(listingInfo);
        }

        public SaleListing(ListingValueObject listingInfo, ListingStatusFieldsInfo statusFieldsInfo, SaleProperty saleProperty, Guid companyId)
            : this()
        {
            ArgumentNullException.ThrowIfNull(saleProperty);

            this.SaleProperty = saleProperty;
            this.SalePropertyId = saleProperty.Id;
            this.CompanyId = companyId;
            this.StatusFieldsInfo = statusFieldsInfo;
            this.CopyInformationFromValueObject(listingInfo);
        }

        protected SaleListing()
            : base()
        {
            this.StatusFieldsInfo = new();
            this.InvoiceInfo = new();
        }

        public bool LockedByLegacy { get; set; }
        public Guid? UnlockedFromLegacyBy { get; set; }
        public DateTime? UnlockedFromLegacyOn { get; set; }

        public virtual Guid SalePropertyId { get; set; }

        public virtual SaleProperty SaleProperty { get; set; }

        public virtual InvoiceInfo InvoiceInfo { get; set; }

        public virtual ICollection<SaleListingTrace> ListingSaleTraces { get; set; }

        public virtual ICollection<ManagementTrace> ManagementTraces { get; set; }

        public virtual ICollection<ShowingTimeContact> ShowingTimeContacts { get; set; }

        public virtual bool IsExisting => ExistingListingStatuses.Contains(this.MlsStatus);

        public virtual CommandSingleResult<SaleListingRequest, ValidationResult> GenerateRequest(Guid userId)
        {
            try
            {
                var request = new SaleListingRequest(saleListing: this, userId);
                return this.AddRequest(request, userId);
            }
            catch (Exception ex)
            {
                this.LockUnsubmitted(userId);
                return CommandSingleResult<SaleListingRequest, ValidationResult>.Error(new ValidationResult(ex.Message));
            }
        }

        public virtual CommandSingleResult<SaleListingRequest, ValidationResult> GenerateRequestFromCommunity(SaleListingRequest lastCompletedRequest, Guid userId)
        {
            this.SaleProperty.UpdateListingFromCommunitySubmit();

            var newRequest = lastCompletedRequest.Clone();
            newRequest.SaleProperty.ImportDataFromCommunitySubmit(this.SaleProperty.Community);
            newRequest.UpdateTrackValues(userId, isNewRecord: true);
            newRequest.MlsNumber = this.MlsNumber;
            newRequest.ListDate = this.ListDate;

            return this.AddRequest(newRequest, userId);
        }

        public override void UpdateManuallyManagement(bool manuallyManaged)
        {
            if (this.IsManuallyManaged != manuallyManaged)
            {
                this.IsManuallyManaged = manuallyManaged;
                this.ManagementTraces.Add(new ManagementTrace(this, this.CompanyId, manuallyManaged));
            }
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

        public virtual void CloneListing(SaleListing saleListingToClone)
        {
            this.SaleProperty.PlanId = saleListingToClone.SaleProperty.PlanId;
            this.SaleProperty.CommunityId = saleListingToClone.SaleProperty.CommunityId;
            this.SaleProperty.OwnerName = saleListingToClone.SaleProperty.OwnerName;
            this.SaleProperty.AddressInfo.PartialClone(saleListingToClone.SaleProperty.AddressInfo);
            this.SaleProperty.PropertyInfo.PartialClone(saleListingToClone.SaleProperty.PropertyInfo);

            this.SaleProperty.SpacesDimensionsInfo = saleListingToClone.SaleProperty.SpacesDimensionsInfo.Clone();
            this.SaleProperty.FeaturesInfo = saleListingToClone.SaleProperty.FeaturesInfo.Clone();
            this.SaleProperty.FinancialInfo = saleListingToClone.SaleProperty.FinancialInfo.Clone();
            this.SaleProperty.SchoolsInfo = saleListingToClone.SaleProperty.SchoolsInfo.Clone();
            this.SaleProperty.ShowingInfo = saleListingToClone.SaleProperty.ShowingInfo.Clone();
            this.SaleProperty.SalesOfficeInfo = saleListingToClone.SaleProperty.SalesOfficeInfo.Clone();

            this.SaleProperty.ImportRoomsFromEntity(saleListingToClone.SaleProperty.Rooms);
            this.SaleProperty.UpdateOpenHouse(saleListingToClone.SaleProperty.OpenHouses);

            this.AppointmentType = saleListingToClone.AppointmentType;
            this.AccessInformation = saleListingToClone.AccessInformation.Clone();
            this.AppointmentRestrictions = saleListingToClone.AppointmentRestrictions.Clone();
            this.AdditionalInstructions = saleListingToClone.AdditionalInstructions.Clone();
        }

        public virtual void ApplyMarketUpdate(
            ListingValueObject listingInfo,
            ListingStatusFieldsInfo listingStatusInfo,
            SalePropertyValueObject salePropertyInfo,
            Guid companyId,
            bool processFullListing)
        {
            if (this.MlsNumber?.Trim() != listingInfo.MlsNumber.Trim())
            {
                this.SaleProperty.AddListing(listingInfo, listingStatusInfo, salePropertyInfo, new List<ListingSaleRoom>(), companyId);
                return;
            }

            this.isMarketUpdate = true;
            this.processFullListing = processFullListing;
            this.UpdateBaseListingInformation(listingInfo);
            this.UpdateStatusFieldsInfo(listingStatusInfo);
            this.SaleProperty.UpdateFromMarket(processFullListing);
            this.SaleProperty.ApplyMarketUpdate(salePropertyInfo, new List<ListingSaleRoom>());

            if (listingInfo.MlsStatus == MarketStatuses.Closed || listingInfo.MlsStatus == MarketStatuses.Canceled || listingInfo.MlsStatus == MarketStatuses.Expired)
            {
                this.LockAndClose();
            }
            else
            {
                this.Unlock(allowUnlock: true);
            }
        }

        public virtual void ImportFromXml(XmlListingDetailResponse listing, string companyName, ImportActionType listAction, Guid userId, CommunitySale community = null)
        {
            this.MlsStatus = listing.Status.ToStatus();
            this.ListPrice = listing.Price;
            this.XmlListingId = listing.Id;

            if (listAction == ImportActionType.ListCompare)
            {
                this.MlsStatus = MarketStatuses.Pending;
            }

            var county = community?.Property?.County;
            this.SaleProperty.ImportFromXml(listing, companyName, county);
            this.Lock(userId, LockedStatus.LockedNotSubmitted);
        }

        public virtual void UpdateInvoiceData(Guid userId, string invoiceId, string docNumber, DateTime createdDate)
        {
            this.InvoiceInfo = new InvoiceInfo(userId, invoiceId, docNumber, createdDate);
        }

        public void ImportDataFromPlan(Plan plan)
        {
            this.SaleProperty.ImportDataFromPlan(plan);
        }

        public void ImportDataFromCommunity(CommunitySale community)
        {
            this.SaleProperty.ImportDataFromCommunity(community);
        }

        public override void ChangeCompany(Guid companyId, string companyName)
        {
            this.CompanyId = companyId;
            this.SaleProperty.OwnerName = companyName;
            this.SaleProperty.CompanyId = companyId;
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.SalePropertyId;
            yield return base.GetEntityEqualityComponents();
        }
    }
}
