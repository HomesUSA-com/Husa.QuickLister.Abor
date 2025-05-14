namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;
    using Husa.Quicklister.Extensions.Crosscutting.Extensions;
    using Husa.Xml.Api.Contracts.Response;

    public class FeaturesInfo : ValueObject, IProvideSaleFeature
    {
        private static readonly string RemoveKeyword = "MLS Num";
        private static readonly int PropertyDescriptionLength = 4000;
        private ICollection<FireplaceDescription> fireplaceDescription;
        private WaterBodyName? waterBodyName;

        public FeaturesInfo()
        {
            this.IsNewConstruction = true;
        }

        public virtual ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public virtual ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public virtual ICollection<Disclosures> Disclosures { get; set; }
        public virtual ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public virtual ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public virtual ICollection<WaterSource> WaterSource { get; set; }
        public virtual ICollection<WaterSewer> WaterSewer { get; set; }
        public virtual ICollection<HeatingSystem> HeatSystem { get; set; }
        public virtual ICollection<CoolingSystem> CoolingSystem { get; set; }
        public virtual ICollection<Appliances> Appliances { get; set; }
        public virtual int? GarageSpaces { get; set; }
        public virtual int? ParkingTotal { get; set; }
        public virtual ICollection<GarageDescription> GarageDescription { get; set; }
        public virtual ICollection<LaundryLocation> LaundryLocation { get; set; }
        public virtual ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        public virtual int? Fireplaces { get; set; }
        public virtual ICollection<FireplaceDescription> FireplaceDescription
        {
            get { return this.fireplaceDescription; }
            set { this.fireplaceDescription = this.Fireplaces.HasValue && this.Fireplaces.Value > 0 ? value : null; }
        }

        public virtual ICollection<Flooring> Floors { get; set; }
        public virtual ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        public virtual ICollection<WindowFeatures> WindowFeatures { get; set; }
        public virtual ICollection<Foundation> Foundation { get; set; }
        public virtual ICollection<RoofDescription> RoofDescription { get; set; }
        public virtual ICollection<Fencing> Fencing { get; set; }
        public virtual ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        public virtual ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        public virtual ICollection<View> View { get; set; }
        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public virtual HomeFaces? HomeFaces { get; set; }
        public virtual ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }

        public virtual WaterBodyName? WaterBodyName
        {
            get { return this.waterBodyName; }
            set { this.waterBodyName = this.WaterfrontFeatures.Contains(Enums.Domain.WaterfrontFeatures.None) ? null : value; }
        }

        public virtual DistanceToWaterAccess? DistanceToWaterAccess { get; set; }
        public virtual ICollection<UnitStyle> UnitStyle { get; set; }
        public virtual ICollection<GuestAccommodationsDescription> GuestAccommodationsDescription { get; set; }
        public virtual int? GuestBedroomsTotal { get; set; }
        public virtual int? GuestFullBathsTotal { get; set; }
        public virtual int? GuestHalfBathsTotal { get; set; }
        public virtual string PropertyDescription { get; set; }
        public virtual bool IsAIGeneratedPropertyDescription { get; set; }
        public virtual bool IsNewConstruction { get; set; }

        public static FeaturesInfo ImportFromXml(XmlListingDetailResponse listing, FeaturesInfo features)
        {
            var importedFeatures = new FeaturesInfo();
            if (features != null)
            {
                importedFeatures = features.Clone();
            }

            importedFeatures.PropertyDescription = listing.Description
                .CleanAfterKeyword(RemoveKeyword)
                .GetSubstring(PropertyDescriptionLength);
            return importedFeatures;
        }

        public FeaturesInfo Clone()
        {
            return (FeaturesInfo)this.MemberwiseClone();
        }

        public FeaturesInfo ImportFeaturesFromCommunity(Utilities utilities)
        {
            var clonnedFeatures = this.Clone();
            clonnedFeatures.NeighborhoodAmenities = utilities.NeighborhoodAmenities;
            clonnedFeatures.RestrictionsDescription = utilities.RestrictionsDescription;
            clonnedFeatures.Disclosures = utilities.Disclosures;
            clonnedFeatures.DocumentsAvailable = utilities.DocumentsAvailable;
            clonnedFeatures.UtilitiesDescription = utilities.UtilitiesDescription;
            clonnedFeatures.WaterSource = utilities.WaterSource;
            clonnedFeatures.WaterSewer = utilities.WaterSewer;
            clonnedFeatures.HeatSystem = utilities.HeatSystem;
            clonnedFeatures.CoolingSystem = utilities.CoolingSystem;
            clonnedFeatures.Appliances = utilities.Appliances;
            clonnedFeatures.GarageSpaces = utilities.GarageSpaces;
            clonnedFeatures.GarageDescription = utilities.GarageDescription;
            clonnedFeatures.LaundryLocation = utilities.LaundryLocation;
            clonnedFeatures.InteriorFeatures = utilities.InteriorFeatures;
            clonnedFeatures.Fireplaces = utilities.Fireplaces;
            clonnedFeatures.FireplaceDescription = utilities.FireplaceDescription;
            clonnedFeatures.Floors = utilities.Floors;
            clonnedFeatures.SecurityFeatures = utilities.SecurityFeatures;
            clonnedFeatures.WindowFeatures = utilities.WindowFeatures;
            clonnedFeatures.Foundation = utilities.Foundation;
            clonnedFeatures.RoofDescription = utilities.RoofDescription;
            clonnedFeatures.Fencing = utilities.Fencing;
            clonnedFeatures.ConstructionMaterials = utilities.ConstructionMaterials;
            clonnedFeatures.PatioAndPorchFeatures = utilities.PatioAndPorchFeatures;
            clonnedFeatures.View = utilities.View;
            clonnedFeatures.ExteriorFeatures = utilities.ExteriorFeatures;
            clonnedFeatures.ParkingTotal = utilities.ParkingTotal;

            return clonnedFeatures;
        }

        public FeaturesInfo ImportFeaturesFromPlan(BasePlan plan)
        {
            var clonnedFeatures = this.Clone();
            clonnedFeatures.IsNewConstruction = plan.IsNewConstruction;
            return clonnedFeatures;
        }

        internal void CopyExteriorFeatures(ICollection<ExteriorFeatures> exteriorFeatures)
        {
            if (this.ExteriorFeatures != null && this.ExteriorFeatures.Contains(Enums.Domain.ExteriorFeatures.GuttersFull))
            {
                exteriorFeatures.Add(Enums.Domain.ExteriorFeatures.GuttersFull);
            }

            this.ExteriorFeatures = exteriorFeatures;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.NeighborhoodAmenities;
            yield return this.RestrictionsDescription;
            yield return this.Disclosures;
            yield return this.DocumentsAvailable;
            yield return this.UtilitiesDescription;
            yield return this.WaterSource;
            yield return this.WaterSewer;
            yield return this.HeatSystem;
            yield return this.CoolingSystem;
            yield return this.Appliances;
            yield return this.GarageSpaces;
            yield return this.GarageDescription;
            yield return this.LaundryLocation;
            yield return this.InteriorFeatures;
            yield return this.Fireplaces;
            yield return this.FireplaceDescription;
            yield return this.Floors;
            yield return this.SecurityFeatures;
            yield return this.WindowFeatures;
            yield return this.Foundation;
            yield return this.RoofDescription;
            yield return this.Fencing;
            yield return this.ConstructionMaterials;
            yield return this.PatioAndPorchFeatures;
            yield return this.View;
            yield return this.ExteriorFeatures;
            yield return this.HomeFaces;
            yield return this.WaterBodyName;
            yield return this.DistanceToWaterAccess;
            yield return this.WaterfrontFeatures;
            yield return this.UnitStyle;
            yield return this.GuestAccommodationsDescription;
            yield return this.GuestBedroomsTotal;
            yield return this.GuestFullBathsTotal;
            yield return this.GuestHalfBathsTotal;
            yield return this.PropertyDescription;
            yield return this.IsNewConstruction;
            yield return this.ParkingTotal;
        }
    }
}
