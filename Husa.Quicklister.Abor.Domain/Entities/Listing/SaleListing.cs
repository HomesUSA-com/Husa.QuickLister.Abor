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
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;
    using Husa.Xml.Api.Contracts.Response;

    public class SaleListing : Listing, ISaleListing<SaleProperty>, ISaleListingRequest<SaleListingRequest>
    {
        public const int YearsInThePast = -2;
        public const int MaxExpirationDaysInTheFuture = 10;
        public const int MaxExpirationDaysLeftInTheFuture = 7;
        public const int MaxDaysInMarket = 179;
        public const int MinPropertyDescriptionLength = 100;

        public static readonly IEnumerable<MarketStatuses> ActiveListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
        };
        public static readonly IEnumerable<MarketStatuses> OrphanListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
            MarketStatuses.Closed,
        };
        public static readonly IEnumerable<MarketStatuses> PendingListingStatuses = new[] { MarketStatuses.Pending };
        public static readonly IEnumerable<MarketStatuses> PendingAndCanceledStatuses = new[]
        {
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
        };
        public static readonly IEnumerable<MarketStatuses> ActivePhotoRequestListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.Hold,
        };
        public static readonly IEnumerable<MarketStatuses> ActiveAndPendingListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
        };
        public static readonly IEnumerable<MarketStatuses> ExistingListingStatuses = new[]
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Hold,
            MarketStatuses.Pending,
            MarketStatuses.Canceled,
        };

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

        public SaleListing(ListingValueObject listingInfo, ListingSaleStatusFieldsInfo statusFieldsInfo, SalePropertyValueObject salePropertyInfo, Guid companyId)
            : this()
        {
            this.CompanyId = companyId;
            this.StatusFieldsInfo = statusFieldsInfo;
            this.SaleProperty = new(salePropertyInfo, companyId);
            this.SalePropertyId = listingInfo.PropertyId;
            this.CopyInformationFromValueObject(listingInfo);
        }

        public SaleListing(ListingValueObject listingInfo, ListingSaleStatusFieldsInfo statusFieldsInfo, SaleProperty saleProperty, Guid companyId)
            : this()
        {
            if (saleProperty is null)
            {
                throw new ArgumentNullException(nameof(saleProperty));
            }

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
            this.PublishInfo = new();
        }

        public virtual Guid SalePropertyId { get; set; }

        public virtual SaleProperty SaleProperty { get; set; }

        public virtual InvoiceInfo InvoiceInfo { get; set; }

        public virtual ListingSaleStatusFieldsInfo StatusFieldsInfo { get; set; }

        public virtual PublishInfo PublishInfo { get; set; }

        public virtual ICollection<SaleListingTrace> ListingSaleTraces { get; set; }

        public virtual ICollection<ManagementTrace> ManagementTraces { get; set; }

        public virtual bool IsInMarket => !string.IsNullOrEmpty(this.MlsNumber);

        public virtual bool IsExisting => ExistingListingStatuses.Contains(this.MlsStatus);

        public virtual void UpdateStatusFieldsInfo(ListingSaleStatusFieldsInfo listingSaleStatusFields)
        {
            if (listingSaleStatusFields is null)
            {
                throw new ArgumentNullException(nameof(listingSaleStatusFields));
            }

            this.CopyInformationFromValueObject(listingSaleStatusFields);
        }

        public virtual CommandSingleResult<SaleListingRequest, ValidationResult> GenerateRequest(Guid userId)
        {
            var request = new SaleListingRequest(saleListing: this, userId);
            return this.AddRequest(request, userId);
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

        public virtual void UpdateActionType(ActionType actionType)
        {
            this.PublishInfo.PublishType = actionType;
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
        }

        public virtual void ApplyMarketUpdate(
            ListingValueObject listingInfo,
            ListingSaleStatusFieldsInfo listingStatusInfo,
            SalePropertyValueObject salePropertyInfo,
            Guid companyId)
        {
            if (this.MlsNumber?.Trim() != listingInfo.MlsNumber.Trim())
            {
                this.SaleProperty.AddListing(listingInfo, listingStatusInfo, salePropertyInfo, new List<ListingSaleRoom>(), companyId);
                return;
            }

            this.isMarketUpdate = true;
            this.UpdateBaseListingInformation(listingInfo);
            this.UpdateStatusFieldsInfo(listingStatusInfo);
            this.SaleProperty.UpdateFromMarket();
            this.SaleProperty.ApplyMarketUpdate(salePropertyInfo, new List<ListingSaleRoom>());

            if (listingInfo.MlsStatus == MarketStatuses.Closed)
            {
                this.LockAndClose();
            }
            else
            {
                this.Unlock(allowUnlock: true);
            }
        }

        public virtual void ImportFromXml(XmlListingDetailResponse listing, string companyName, ListActionType listAction, Guid userId)
        {
            this.MlsStatus = listing.Status.ToStatus();
            this.ListPrice = listing.Price;
            this.XmlListingId = listing.Id;

            if (listAction == ListActionType.ListCompare)
            {
                this.MlsStatus = MarketStatuses.Closed;
                this.StatusFieldsInfo.SetSold(
                    listPrice: this.ListPrice.Value,
                    closePrice: listing.SalesPrice,
                    closeDate: listing.ClosedDate);
            }

            this.SaleProperty.ImportFromXml(listing, companyName);
            this.Lock(userId, LockedStatus.LockedNotSubmitted);
        }

        public virtual void UpdateInvoiceData(Guid userId, string invoiceId, string docNumber, DateTime createdDate)
        {
            this.InvoiceInfo = new InvoiceInfo(userId, invoiceId, docNumber, createdDate);
        }

        public virtual void UpdateFromXml(XmlListingDetailResponse listing, Guid userId)
        {
            if (listing.Price.HasValue && !PendingAndCanceledStatuses.Contains(this.MlsStatus))
            {
                this.ListPrice = listing.Price;
            }

            this.XmlListingId = listing.Id;
            this.SaleProperty.UpdateFromXml(listing);
            this.LockByUser(userId);
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();

        protected void CopyInformationFromValueObject(ListingSaleStatusFieldsInfo listingSaleStatusFields)
        {
            if (!this.isMarketUpdate)
            {
                this.StatusFieldsInfo.AgentId = listingSaleStatusFields.AgentId;
                this.StatusFieldsInfo.AgentIdSecond = listingSaleStatusFields.AgentIdSecond;
                this.StatusFieldsInfo.HasSecondBuyerAgent = listingSaleStatusFields.HasSecondBuyerAgent;
                this.StatusFieldsInfo.HasContingencyInfo = listingSaleStatusFields.HasContingencyInfo;
                this.StatusFieldsInfo.ContingencyInfo = listingSaleStatusFields.ContingencyInfo;
                this.StatusFieldsInfo.SaleTerms = listingSaleStatusFields.SaleTerms;
                this.StatusFieldsInfo.CancelledReason = listingSaleStatusFields.CancelledReason;
            }

            this.StatusFieldsInfo.EstimatedClosedDate = listingSaleStatusFields.EstimatedClosedDate;
            this.StatusFieldsInfo.SellConcess = listingSaleStatusFields.SellConcess;
            this.StatusFieldsInfo.ClosePrice = listingSaleStatusFields.ClosePrice;
            this.StatusFieldsInfo.HasBuyerAgent = listingSaleStatusFields.HasBuyerAgent;
            this.StatusFieldsInfo.PendingDate = listingSaleStatusFields.PendingDate;
            this.StatusFieldsInfo.ClosedDate = listingSaleStatusFields.ClosedDate;
            this.StatusFieldsInfo.BackOnMarketDate = listingSaleStatusFields.BackOnMarketDate;
            this.StatusFieldsInfo.OffMarketDate = listingSaleStatusFields.OffMarketDate;
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.SalePropertyId;
            yield return this.PublishInfo;
            yield return this.StatusFieldsInfo;
        }
    }
}
