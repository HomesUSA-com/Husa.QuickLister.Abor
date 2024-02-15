namespace Husa.Quicklister.Abor.Domain.Entities.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;
    using Husa.Xml.Api.Contracts.Response;

    public class SaleProperty : Entity, IProvideBasicPropertyInfo, ISaleProperty, IEntityOpenHouse<SaleListingOpenHouse>
    {
        protected bool isMarketUpdate = false;
        protected bool processFullListing = true;
        public SaleProperty(
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
            Guid? planId)
            : this()
        {
            this.PlanId = planId;
            this.CompanyId = companyId;
            this.OwnerName = ownerName;
            this.CommunityId = communityId;
            this.PropertyInfo = new(constructionCompletionDate);
            this.AddressInfo = new(streetNum, streetName, unitNumber, zipCode, city, state, county);
            this.ShowingInfo = new(ownerName);
        }

        public SaleProperty(SalePropertyValueObject saleProperty, Guid companyId)
            : this()
        {
            if (saleProperty is null)
            {
                throw new ArgumentNullException(nameof(saleProperty));
            }

            this.CompanyId = companyId;
            this.InitializeEntity(saleProperty);
        }

        protected SaleProperty()
        {
            this.FeaturesInfo = new();
            this.SchoolsInfo = new();
            this.SpacesDimensionsInfo = new();
            this.FinancialInfo = new();
            this.ShowingInfo = new();
            this.PropertyInfo = new();
            this.AddressInfo = new();
            this.SalesOfficeInfo = new();
            this.Rooms = new HashSet<ListingSaleRoom>();
            this.OpenHouses = new HashSet<SaleListingOpenHouse>();
        }

        public virtual string OwnerName { get; set; }

        public virtual Guid? PlanId { get; set; }

        public virtual Guid? CommunityId { get; set; }

        public virtual AddressInfo AddressInfo { get; set; }

        public virtual PropertyInfo PropertyInfo { get; set; }

        public virtual SpacesDimensionsInfo SpacesDimensionsInfo { get; set; }

        public virtual FeaturesInfo FeaturesInfo { get; set; }

        public virtual FinancialInfo FinancialInfo { get; set; }

        public virtual ShowingInfo ShowingInfo { get; set; }

        public virtual SchoolsInfo SchoolsInfo { get; set; }

        public virtual SalesOffice SalesOfficeInfo { get; set; }

        public virtual ICollection<ListingSaleRoom> Rooms { get; set; }

        public virtual ICollection<SaleListingOpenHouse> OpenHouses { get; set; }

        public virtual ICollection<SaleListing> SaleListings { get; set; }

        public virtual CommunitySale Community { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual string Address => $"{this.AddressInfo.StreetNumber} {this.AddressInfo.StreetName}";

        public virtual bool CanBeDeleted => !this.IsDeleted && this.SaleListings != null && this.SaleListings.Any(sl => sl.IsInMarket && sl.IsExisting);

        public virtual void UpdatePropertyInfo(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (this.PropertyInfo != propertyInfo)
            {
                this.PropertyInfo = propertyInfo;
            }
        }

        public virtual void UpdateAddressInfo(AddressInfo addressInfo)
        {
            if (addressInfo is null)
            {
                throw new ArgumentNullException(nameof(addressInfo));
            }

            if (this.AddressInfo != addressInfo)
            {
                this.AddressInfo = addressInfo;
            }
        }

        public virtual void UpdateFeatures(FeaturesInfo features)
        {
            if (features is null)
            {
                throw new ArgumentNullException(nameof(features));
            }

            if (this.FeaturesInfo != features)
            {
                this.FeaturesInfo = features;
            }
        }

        public virtual void UpdateFinancial(FinancialInfo financial)
        {
            if (financial is null)
            {
                throw new ArgumentNullException(nameof(financial));
            }

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

        public virtual void UpdateShowing(ShowingInfo showing)
        {
            if (showing is null)
            {
                throw new ArgumentNullException(nameof(showing));
            }

            if (this.ShowingInfo != showing)
            {
                this.ShowingInfo = showing;
            }
        }

        public virtual void UpdateSchools(SchoolsInfo schools)
        {
            if (schools is null)
            {
                throw new ArgumentNullException(nameof(schools));
            }

            if (this.SchoolsInfo != schools)
            {
                this.SchoolsInfo = schools;
            }
        }

        public virtual void UpdateSpacesDimensions(SpacesDimensionsInfo spacesDimensions)
        {
            if (spacesDimensions is null)
            {
                throw new ArgumentNullException(nameof(spacesDimensions));
            }

            if (this.SpacesDimensionsInfo != spacesDimensions)
            {
                this.SpacesDimensionsInfo = spacesDimensions;
            }
        }

        public virtual void UpdateRooms(IEnumerable<ListingSaleRoom> rooms)
        {
            if (rooms is null)
            {
                throw new ArgumentNullException(nameof(rooms));
            }

            this.Rooms.Clear();

            foreach (var roomDetail in rooms)
            {
                var room = new ListingSaleRoom(
                    this.Id,
                    roomDetail.RoomType,
                    roomDetail.Level,
                    roomDetail.Features);

                this.Rooms.Add(room);
            }
        }

        public virtual bool ImportOpenHouseInfoFromMarket(SaleListingOpenHouse openHouse)
        {
            if (openHouse is null)
            {
                throw new ArgumentNullException(nameof(openHouse));
            }

            if (!this.ShowingInfo.EnableOpenHouses)
            {
                this.ShowingInfo.EnableOpenHouse();
            }

            this.OpenHouses.Add(openHouse);
            return true;
        }

        public virtual void ImportDataFromCommunity(CommunitySale communitySale)
        {
            this.CommunityId = communitySale.Id;
            this.SchoolsInfo = this.SchoolsInfo.ImportSchools(communitySale.SchoolsInfo);
            this.FeaturesInfo = this.FeaturesInfo.ImportFeaturesFromCommunity(communitySale.Utilities);
            this.FinancialInfo = this.FinancialInfo.ImportFinancialFromCommunity(communitySale.Financial);
            this.ShowingInfo = this.ShowingInfo.ImportShowingFromCommunity(communitySale.Showing);
            this.AddressInfo = this.AddressInfo.ImportAddressInfoFromCommunity(communitySale.Property);

            this.ImportSaleOfficeFromCommunity(communitySale.SaleOffice);
            this.ImportPropertyFromCommunity(communitySale.Property);
            this.ImportOpenHouseFromCommunity(communitySale);
        }

        public virtual void ImportDataFromCommunitySubmit(CommunitySale communitySale)
        {
            communitySale.SchoolsInfo.CopyProperties(this.SchoolsInfo, communitySale.GetChangedProperties(nameof(communitySale.SchoolsInfo)));
            communitySale.Financial.CopyProperties(this.FinancialInfo, communitySale.GetChangedProperties(nameof(communitySale.Financial)));
            communitySale.Showing.CopyProperties(this.ShowingInfo, communitySale.GetChangedProperties(nameof(communitySale.Showing)));
            communitySale.SaleOffice.CopyProperties(this.SalesOfficeInfo, communitySale.GetChangedProperties(nameof(communitySale.SaleOffice)));

            var utilitiesChanges = communitySale.GetChangedProperties(nameof(communitySale.Utilities));
            communitySale.Utilities.CopyProperties(this.FeaturesInfo, utilitiesChanges);
            communitySale.Utilities.CopyProperties(this.SpacesDimensionsInfo, utilitiesChanges);

            var propertyChanges = communitySale.GetChangedProperties(nameof(communitySale.Property));
            communitySale.Property.CopyProperties(this.AddressInfo, propertyChanges);
            communitySale.Property.CopyProperties(this.PropertyInfo, propertyChanges);
        }

        public virtual void ImportOpenHouseFromCommunity(CommunitySale communitySale)
        {
            this.ImportOpenHouse<CommunityOpenHouse, SaleListingOpenHouse, SaleProperty>(communitySale.OpenHouses);
            this.ShowingInfo.EnableOpenHouse();
        }

        public virtual void ImportDataFromPlan(Plan plan)
        {
            if (plan is null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            this.PlanId = plan.Id;
            this.SpacesDimensionsInfo = this.SpacesDimensionsInfo.ImportSpacesDimensionsFromPlan(plan.BasePlan);
            this.FeaturesInfo = this.FeaturesInfo.ImportFeaturesFromPlan(plan.BasePlan);
            this.ImportRoomsFromEntity(plan.Rooms);
        }

        public virtual void AddOpenHouses<T>(IEnumerable<T> openHouses)
            where T : OpenHouse
        {
            var filteredOpenHouses = openHouses
            .GroupBy(o => o.Type)
            .Select(group => group.Last())
            .ToList();
            foreach (var openHouse in filteredOpenHouses)
            {
                var listingOpenHouse = new SaleListingOpenHouse(
                    salePropertyId: this.Id,
                    type: openHouse.Type,
                    startTime: openHouse.StartTime,
                    endTime: openHouse.EndTime,
                    refreshments: openHouse.Refreshments);

                this.OpenHouses.Add(listingOpenHouse);
            }
        }

        public virtual void AddRooms(IEnumerable<ListingSaleRoom> rooms)
        {
            foreach (var roomDetail in rooms)
            {
                var room = new ListingSaleRoom(
                    this.Id,
                    roomDetail.RoomType,
                    roomDetail.Level,
                    roomDetail.Features);

                this.Rooms.Add(room);
            }
        }

        public virtual void FillSalesPropertyInformation(SalePropertyValueObject salePropertyInfo)
        {
            if (salePropertyInfo is null)
            {
                throw new ArgumentNullException(nameof(salePropertyInfo));
            }

            this.InitializeEntity(salePropertyInfo);
        }

        public virtual bool UpdateListingFromCommunitySubmit()
        {
            var oldProperty = this.Clone();

            this.ImportDataFromCommunitySubmit(this.Community);

            var hasChanges = !this.IsEqualTo(oldProperty);

            return hasChanges;
        }

        public virtual void UpdateRoomsInfoFromPlan(Plan plan, Guid listingId, bool updateRooms)
        {
            if (!updateRooms)
            {
                return;
            }

            this.ImportRoomsFromEntity(plan.Rooms);
            this.ImportSpacesAndDimensionsFromPlan(plan.BasePlan);
        }

        public virtual void ImportRoomsFromEntity<T>(IEnumerable<T> rooms)
            where T : Room
        {
            this.Rooms.Clear();

            foreach (var roomDetail in rooms)
            {
                var room = new ListingSaleRoom(
                    this.Id,
                    roomDetail.RoomType,
                    roomDetail.Level,
                    roomDetail.Features);

                this.Rooms.Add(room);
            }
        }

        public virtual void ImportSpacesAndDimensionsFromPlan(BasePlan basePlan)
        {
            this.SpacesDimensionsInfo = this.SpacesDimensionsInfo.ImportSpacesDimensionsFromPlan(basePlan);
        }

        public virtual SaleListing AddListing(
            ListingValueObject listingInfo,
            ListingSaleStatusFieldsInfo listingStatusInfo,
            SalePropertyValueObject salePropertyInfo,
            IEnumerable<ListingSaleRoom> rooms,
            Guid companyId)
        {
            if (listingInfo is null)
            {
                throw new ArgumentNullException(nameof(listingInfo));
            }

            if (listingStatusInfo is null)
            {
                throw new ArgumentNullException(nameof(listingStatusInfo));
            }

            if (salePropertyInfo is null)
            {
                throw new ArgumentNullException(nameof(salePropertyInfo));
            }

            var saleListing = new SaleListing(listingInfo, listingStatusInfo, saleProperty: this, companyId);
            if (SaleListing.ActiveAndPendingListingStatuses.Contains(listingInfo.MlsStatus))
            {
                this.ApplyMarketUpdate(salePropertyInfo, rooms);
            }

            saleListing.Unlock(allowUnlock: true);
            this.SaleListings.Add(saleListing);
            return saleListing;
        }

        public virtual void ApplyMarketUpdate(SalePropertyValueObject salePropertyInfo, IEnumerable<ListingSaleRoom> roomsInfo)
        {
            if (salePropertyInfo is null)
            {
                throw new ArgumentNullException(nameof(salePropertyInfo));
            }

            if (roomsInfo is null)
            {
                throw new ArgumentNullException(nameof(roomsInfo));
            }

            this.FillSalesPropertyInformation(salePropertyInfo);

            if (!this.isMarketUpdate)
            {
                this.UpdateRooms(roomsInfo);
            }
        }

        public SaleProperty Clone()
        {
            var clonedProperty = (SaleProperty)this.MemberwiseClone();
            clonedProperty.SalesOfficeInfo = this.SalesOfficeInfo.Clone();
            clonedProperty.AddressInfo = this.AddressInfo.Clone();
            clonedProperty.PropertyInfo = this.PropertyInfo.Clone();
            clonedProperty.FinancialInfo = this.FinancialInfo.Clone();
            clonedProperty.SchoolsInfo = this.SchoolsInfo.Clone();
            clonedProperty.FeaturesInfo = this.FeaturesInfo.Clone();
            clonedProperty.ShowingInfo = this.ShowingInfo.Clone();
            clonedProperty.SpacesDimensionsInfo = this.SpacesDimensionsInfo.Clone();
            clonedProperty.OpenHouses = this.OpenHouses.Select(openHouseToClone => openHouseToClone.Clone()).ToList();
            clonedProperty.Rooms = this.Rooms.Select(roomToClone => roomToClone.Clone()).ToList();

            return clonedProperty;
        }

        public void UpdateFromMarket(bool processFullListing)
        {
            this.isMarketUpdate = true;
            this.processFullListing = processFullListing;
        }

        public void ImportFromXml(XmlListingDetailResponse listing, string companyName)
        {
            this.OwnerName = companyName;
            this.PlanId = listing.PlanId;
            this.CommunityId = listing.CommunityId;
            this.CompanyId = (Guid)listing.CompanyId;

            var profile = AddressInfo.ImportFromXml(listing, this.AddressInfo);
            this.UpdateAddressInfo(profile);

            var property = PropertyInfo.ImportFromXml(listing, this.PropertyInfo);
            this.UpdatePropertyInfo(property);

            var spacesDimensions = SpacesDimensionsInfo.ImportFromXml(listing, this.SpacesDimensionsInfo);
            this.UpdateSpacesDimensions(spacesDimensions);

            var features = FeaturesInfo.ImportFromXml(listing, this.FeaturesInfo);
            this.UpdateFeatures(features);
        }

        public void UpdateFromXml(XmlListingDetailResponse listing)
        {
            this.PropertyInfo.UpdateFromXml(listing);
            this.SpacesDimensionsInfo.UpdateFromXml(listing);
            this.FeaturesInfo.UpdateFromXml(listing);
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();

        protected bool AreRoomsEqual(ICollection<ListingSaleRoom> other)
        {
            return this.Rooms
                .OrderBy(x => x.RoomType)
                .SequenceEqual(other.OrderBy(x => x.RoomType), new ListingRoomComparer());
        }

        protected bool AreOpenHousesEqual(ICollection<SaleListingOpenHouse> other)
        {
            return this.OpenHouses
                .OrderBy(x => x.Type)
                .SequenceEqual(other.OrderBy(x => x.Type), new OpenHouseComparer());
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.OwnerName;
            yield return this.PlanId;
            yield return this.CommunityId;
            yield return this.AddressInfo;
            yield return this.PropertyInfo;
            yield return this.SpacesDimensionsInfo;
            yield return this.FeaturesInfo;
            yield return this.FinancialInfo;
            yield return this.ShowingInfo;
            yield return this.SchoolsInfo;
            yield return this.SalesOfficeInfo;
        }

        private void ImportPropertyFromCommunity(Property property)
        {
            this.PropertyInfo.MlsArea = property.MlsArea;
            this.PropertyInfo.LotDimension = property.LotDimension;
            this.PropertyInfo.LotDescription = property.LotDescription;
            this.PropertyInfo.LotSize = property.LotSize;
            this.PropertyInfo.PropertyType = property.PropertyType;
        }

        private void CopyFireplaces(int? fireplaces)
        {
            if (this.FeaturesInfo.Fireplaces == fireplaces)
            {
                return;
            }

            var maxFireplaces = 3;
            if (this.FeaturesInfo.Fireplaces > maxFireplaces && fireplaces == maxFireplaces)
            {
                return;
            }

            this.FeaturesInfo.Fireplaces = fireplaces;
        }

        private void ImportSaleOfficeFromCommunity(CommunitySaleOffice saleOffice)
        {
            if (saleOffice is null)
            {
                throw new ArgumentNullException(nameof(saleOffice));
            }

            this.SalesOfficeInfo = new(saleOffice.StreetNumber, saleOffice.StreetName, saleOffice.StreetSuffix, saleOffice.SalesOfficeCity, saleOffice.SalesOfficeZip);
        }

        private void CopyPropertyInfoData(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            this.PropertyInfo ??= new();
            if (!this.isMarketUpdate)
            {
                this.PropertyInfo.ConstructionStage = propertyInfo.ConstructionStage;
                this.PropertyInfo.ConstructionCompletionDate = propertyInfo.ConstructionCompletionDate;
                this.PropertyInfo.UpdateGeocodes = propertyInfo.UpdateGeocodes;
                this.PropertyInfo.IsXmlManaged = propertyInfo.IsXmlManaged;
            }

            if (this.processFullListing)
            {
                this.PropertyInfo.ConstructionStartYear = propertyInfo.ConstructionStartYear;
                this.PropertyInfo.MlsArea = propertyInfo.MlsArea;
                this.PropertyInfo.LotDimension = propertyInfo.LotDimension;
                this.PropertyInfo.LotSize = propertyInfo.LotSize;
                this.PropertyInfo.LotDescription = propertyInfo.LotDescription;
                this.PropertyInfo.PropertyType = propertyInfo.PropertyType;
                this.PropertyInfo.TaxLot = propertyInfo.TaxLot;
                this.PropertyInfo.LegalDescription = propertyInfo.LegalDescription;
                this.PropertyInfo.TaxId = propertyInfo.TaxId;
            }

            this.PropertyInfo.Latitude = propertyInfo.Latitude;
            this.PropertyInfo.Longitude = propertyInfo.Longitude;
        }

        private void CopyAddressData(AddressInfo addressInfo)
        {
            if (addressInfo is null)
            {
                throw new ArgumentNullException(nameof(addressInfo));
            }

            this.AddressInfo = new(addressInfo.StreetNumber, addressInfo.StreetName, addressInfo.UnitNumber, addressInfo.ZipCode, addressInfo.City, addressInfo.State, addressInfo.County)
            {
                Subdivision = addressInfo.Subdivision,
                StreetType = addressInfo.StreetType,
                UnitNumber = addressInfo.UnitNumber,
            };
        }

        private void CopyFeaturesData(FeaturesInfo featuresInfo)
        {
            if (featuresInfo is null)
            {
                throw new ArgumentNullException(nameof(featuresInfo));
            }

            this.FeaturesInfo ??= new();
            if (!this.isMarketUpdate)
            {
                this.FeaturesInfo.IsNewConstruction = featuresInfo.IsNewConstruction;
                this.FeaturesInfo.RestrictionsDescription = featuresInfo.RestrictionsDescription;
                this.FeaturesInfo.DistanceToWaterAccess = featuresInfo.DistanceToWaterAccess;
                this.FeaturesInfo.UnitStyle = featuresInfo.UnitStyle;
                this.FeaturesInfo.GuestAccommodationsDescription = featuresInfo.GuestAccommodationsDescription;
                this.FeaturesInfo.GuestBedroomsTotal = featuresInfo.GuestBedroomsTotal;
                this.FeaturesInfo.GuestFullBathsTotal = featuresInfo.GuestFullBathsTotal;
                this.FeaturesInfo.GuestHalfBathsTotal = featuresInfo.GuestHalfBathsTotal;
            }

            if (this.processFullListing)
            {
                this.FeaturesInfo.PatioAndPorchFeatures = featuresInfo.PatioAndPorchFeatures;
                this.FeaturesInfo.NeighborhoodAmenities = featuresInfo.NeighborhoodAmenities;
                this.FeaturesInfo.UtilitiesDescription = featuresInfo.UtilitiesDescription;
                this.FeaturesInfo.WaterSource = featuresInfo.WaterSource;
                this.FeaturesInfo.WaterSewer = featuresInfo.WaterSewer;
                this.FeaturesInfo.HeatSystem = featuresInfo.HeatSystem;
                this.FeaturesInfo.CoolingSystem = featuresInfo.CoolingSystem;
                this.FeaturesInfo.Appliances = featuresInfo.Appliances;
                this.FeaturesInfo.GarageSpaces = featuresInfo.GarageSpaces;
                this.FeaturesInfo.GarageDescription = featuresInfo.GarageDescription;
                this.FeaturesInfo.LaundryLocation = featuresInfo.LaundryLocation;
                this.FeaturesInfo.InteriorFeatures = featuresInfo.InteriorFeatures;
                this.FeaturesInfo.Floors = featuresInfo.Floors;
                this.FeaturesInfo.SecurityFeatures = featuresInfo.SecurityFeatures;
                this.FeaturesInfo.WindowFeatures = featuresInfo.WindowFeatures;
                this.FeaturesInfo.Foundation = featuresInfo.Foundation;
                this.FeaturesInfo.RoofDescription = featuresInfo.RoofDescription;
                this.FeaturesInfo.Fencing = featuresInfo.Fencing;
                this.FeaturesInfo.ConstructionMaterials = featuresInfo.ConstructionMaterials;
                this.FeaturesInfo.View = featuresInfo.View;
                this.FeaturesInfo.ExteriorFeatures = featuresInfo.ExteriorFeatures;
                this.FeaturesInfo.HomeFaces = featuresInfo.HomeFaces;
                this.FeaturesInfo.WaterfrontFeatures = featuresInfo.WaterfrontFeatures;
                this.FeaturesInfo.WaterBodyName = featuresInfo.WaterBodyName;
                this.CopyFireplaces(featuresInfo.Fireplaces);
                this.FeaturesInfo.FireplaceDescription = featuresInfo.FireplaceDescription;
            }

            this.FeaturesInfo.PropertyDescription = featuresInfo.PropertyDescription;
        }

        private void CopyFinancialData(FinancialInfo financialInfo)
        {
            if (financialInfo is null)
            {
                throw new ArgumentNullException(nameof(financialInfo));
            }

            this.FinancialInfo ??= new();
            if (!this.isMarketUpdate)
            {
                this.FinancialInfo.TaxExemptions = financialInfo.TaxExemptions;
                this.FinancialInfo.TitleCompany = financialInfo.TitleCompany;
                this.FinancialInfo.AgentBonusAmount = financialInfo.AgentBonusAmount;
                this.FinancialInfo.AgentBonusAmountType = financialInfo.AgentBonusAmountType;
                this.FinancialInfo.HasBonusWithAmount = financialInfo.HasBonusWithAmount;
                this.FinancialInfo.BonusExpirationDate = financialInfo.BonusExpirationDate;
                this.FinancialInfo.HasAgentBonus = financialInfo.AgentBonusAmount != null;
                this.FinancialInfo.HasBuyerIncentive = financialInfo.HasBuyerIncentive;
                this.FinancialInfo.HOARequirement = financialInfo.HOARequirement;
                this.FinancialInfo.TaxRate = financialInfo.TaxRate;
            }

            if (this.processFullListing)
            {
                this.FinancialInfo.TaxYear = financialInfo.TaxYear;
                this.FinancialInfo.AcceptableFinancing = financialInfo.AcceptableFinancing;
                this.FinancialInfo.HoaName = financialInfo.HoaName;
                this.FinancialInfo.HoaIncludes = financialInfo.HoaIncludes;
                this.FinancialInfo.HasHoa = financialInfo.HasHoa;
                this.FinancialInfo.BillingFrequency = financialInfo.BillingFrequency;
            }

            this.FinancialInfo.BuyersAgentCommission = financialInfo.BuyersAgentCommission;
            this.FinancialInfo.BuyersAgentCommissionType = financialInfo.BuyersAgentCommissionType;
            this.FinancialInfo.HoaFee = financialInfo.HoaFee;
        }

        private void CopyShowingData(ShowingInfo showingInfo)
        {
            if (showingInfo is null)
            {
                throw new ArgumentNullException(nameof(showingInfo));
            }

            this.ShowingInfo ??= new();
            if (!this.isMarketUpdate)
            {
                this.ShowingInfo.OccupantPhone = showingInfo.OccupantPhone;
                this.ShowingInfo.AgentPrivateRemarksAdditional = showingInfo.AgentPrivateRemarksAdditional;
                this.ShowingInfo.RealtorContactEmail = showingInfo.RealtorContactEmail;
            }

            if (this.processFullListing)
            {
                this.ShowingInfo.Directions = showingInfo.Directions;
                this.ShowingInfo.ShowingRequirements = showingInfo.ShowingRequirements;
                this.ShowingInfo.LockBoxType = showingInfo.LockBoxType;
                this.ShowingInfo.LockBoxSerialNumber = showingInfo.LockBoxSerialNumber;
                this.ShowingInfo.OwnerName = showingInfo.OwnerName;
            }

            this.ShowingInfo.AgentPrivateRemarks = showingInfo.AgentPrivateRemarks;
            this.ShowingInfo.ContactPhone = showingInfo.ContactPhone;
            this.ShowingInfo.ShowingInstructions = showingInfo.ShowingInstructions;
        }

        private void CopySpacesDimensionsData(SpacesDimensionsInfo spacesDimensions)
        {
            if (spacesDimensions is null)
            {
                throw new ArgumentNullException(nameof(spacesDimensions));
            }

            this.SpacesDimensionsInfo ??= new();

            if (!this.isMarketUpdate)
            {
                this.SpacesDimensionsInfo.DiningAreasTotal = spacesDimensions.DiningAreasTotal;
                this.SpacesDimensionsInfo.LivingAreasTotal = spacesDimensions.LivingAreasTotal;
            }

            if (this.processFullListing)
            {
                this.SpacesDimensionsInfo.StoriesTotal = spacesDimensions.StoriesTotal;
                this.SpacesDimensionsInfo.MainLevelBedroomTotal = spacesDimensions.MainLevelBedroomTotal;
                this.SpacesDimensionsInfo.OtherLevelsBedroomTotal = spacesDimensions.OtherLevelsBedroomTotal;
                this.SpacesDimensionsInfo.HalfBathsTotal = spacesDimensions.HalfBathsTotal;
                this.SpacesDimensionsInfo.FullBathsTotal = spacesDimensions.FullBathsTotal;
            }

            this.SpacesDimensionsInfo.SqFtTotal = spacesDimensions.SqFtTotal;
        }

        private void CopySchoolsData(SchoolsInfo schoolsInfo)
        {
            if (schoolsInfo is null)
            {
                throw new ArgumentNullException(nameof(schoolsInfo));
            }

            this.SchoolsInfo = new()
            {
                ElementarySchool = schoolsInfo.ElementarySchool,
                MiddleSchool = schoolsInfo.MiddleSchool,
                HighSchool = schoolsInfo.HighSchool,
                SchoolDistrict = schoolsInfo.SchoolDistrict,
            };
        }

        private void InitializeEntity(SalePropertyValueObject salePropertyInfo)
        {
            if (this.processFullListing)
            {
                this.OwnerName = salePropertyInfo.OwnerName;
                this.CopyAddressData(salePropertyInfo.AddressInfo);
            }

            this.CopyPropertyInfoData(salePropertyInfo.PropertyInfo);
            this.CopyFinancialData(salePropertyInfo.FinancialInfo);
            this.CopyFeaturesData(salePropertyInfo.FeaturesInfo);
            this.CopyShowingData(salePropertyInfo.ShowingInfo);
            this.CopySchoolsData(salePropertyInfo.SchoolsInfo);
            this.CopySpacesDimensionsData(salePropertyInfo.SpacesDimensionsInfo);
        }
    }
}
