namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class LotListing : Entities.Listing.Listing
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
            this.PublishInfo = new();
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
        public virtual PublishInfo PublishInfo { get; set; }
        public virtual ListingStatusFieldsInfo StatusFieldsInfo { get; set; }
        public virtual ICollection<LotManagementTrace> ManagementTraces { get; set; }

        public virtual bool IsInMarket => !string.IsNullOrEmpty(this.MlsNumber);
        public override bool HasStatusToBeClosed => this.MlsStatus == MarketStatuses.Closed || this.MlsStatus == MarketStatuses.Canceled;

        public virtual void CloneListing(LotListing saleListingToClone)
        {
            this.CommunityId = saleListingToClone.CommunityId;
            this.CompanyId = saleListingToClone.CompanyId;
            this.OwnerName = saleListingToClone.OwnerName;
            this.AddressInfo.PartialClone(saleListingToClone.AddressInfo);
            this.PropertyInfo.PartialClone(saleListingToClone.PropertyInfo);

            this.FeaturesInfo = saleListingToClone.FeaturesInfo.Clone();
            this.FinancialInfo = saleListingToClone.FinancialInfo.Clone();
            this.SchoolsInfo = saleListingToClone.SchoolsInfo.Clone();
            this.ShowingInfo = saleListingToClone.ShowingInfo.Clone();
        }

        public virtual void ImportDataFromCommunity(CommunitySale communitySale)
        {
            this.CommunityId = communitySale.Id;
            this.SchoolsInfo = this.SchoolsInfo.ImportSchools(communitySale.SchoolsInfo);
            this.FeaturesInfo = this.FeaturesInfo.ImportFeaturesFromCommunity(communitySale.Utilities);
            this.FinancialInfo = this.FinancialInfo.ImportFinancialFromCommunity(communitySale.Financial);
            this.ShowingInfo = this.ShowingInfo.ImportShowingFromCommunity(communitySale.Showing);
            this.AddressInfo = this.AddressInfo.ImportAddressInfoFromCommunity(communitySale.Property);
            this.PropertyInfo = this.PropertyInfo.ImportPropertyFromCommunity(communitySale.Property);
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

        public virtual void UpdateFeatures(LotFeaturesInfo features)
        {
            ArgumentNullException.ThrowIfNull(features);
            if (this.FeaturesInfo != features)
            {
                this.FeaturesInfo = features;
            }
        }

        public virtual void UpdateFinancial(LotFinancialInfo financial)
        {
            ArgumentNullException.ThrowIfNull(financial);

            if (!financial.IsValidBuyersAgentCommissionRange())
            {
                throw new DomainException($"The range for Buyers Agent Commission is invalid for type {financial.BuyersAgentCommissionType}");
            }

            if (financial.HasAgentBonus && !financial.IsValidAgentBonusAmountRange())
            {
                throw new DomainException($"The range for Agent bonus amount is invalid for type {financial.BuyersAgentCommissionType}");
            }

            if (this.FinancialInfo != financial)
            {
                this.FinancialInfo = financial;
            }
        }

        public virtual void UpdatePropertyInfo(LotPropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo);
            if (this.PropertyInfo != propertyInfo)
            {
                this.PropertyInfo = propertyInfo;
            }
        }

        public virtual void UpdateAddressInfo(LotAddressInfo addressInfo)
        {
            ArgumentNullException.ThrowIfNull(addressInfo);

            if (this.AddressInfo != addressInfo)
            {
                this.AddressInfo = addressInfo;
            }
        }

        public virtual void UpdateShowing(LotShowingInfo showing)
        {
            ArgumentNullException.ThrowIfNull(showing);
            if (this.ShowingInfo != showing)
            {
                this.ShowingInfo = showing;
            }
        }

        public virtual void UpdateSchools(LotSchoolsInfo schools)
        {
            ArgumentNullException.ThrowIfNull(schools);
            if (this.SchoolsInfo != schools)
            {
                this.SchoolsInfo = schools;
            }
        }

        public override void UpdateManuallyManagement(bool manuallyManaged)
        {
            if (this.IsManuallyManaged != manuallyManaged)
            {
                this.IsManuallyManaged = manuallyManaged;
                this.ManagementTraces.Add(new LotManagementTrace(this, this.CompanyId, manuallyManaged));
            }
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
