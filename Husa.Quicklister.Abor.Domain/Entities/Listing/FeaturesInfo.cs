namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class FeaturesInfo : ValueObject, IProvideFeature
    {
        private ICollection<FireplaceDescription> fireplaceDescription;
        private ICollection<PrivatePool> privatePool;

        public FeaturesInfo()
        {
            this.IsNewConstruction = true;
            this.PrivatePool = new List<Enums.Domain.PrivatePool> { Enums.Domain.PrivatePool.None };
        }

        public virtual ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public virtual ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public virtual ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public virtual ICollection<WaterSource> WaterSource { get; set; }
        public virtual ICollection<WaterSewer> WaterSewer { get; set; }
        public virtual ICollection<HeatingSystem> HeatSystem { get; set; }
        public virtual ICollection<CoolingSystem> CoolingSystem { get; set; }

        public virtual string PropertyDescription { get; set; }

        public virtual int? Fireplaces { get; set; }

        public virtual ICollection<FireplaceDescription> FireplaceDescription
        {
            get { return this.fireplaceDescription; }
            set { this.fireplaceDescription = this.Fireplaces.HasValue && this.Fireplaces.Value > 0 ? value : null; }
        }

        public virtual ICollection<WindowCoverings> WindowCoverings { get; set; }

        public virtual bool HasAccessibility { get; set; }

        public virtual ICollection<Accessibility> Accessibility { get; set; }

        public virtual ICollection<HousingStyle> HousingStyle { get; set; }

        public virtual ICollection<Exterior> Exterior { get; set; }

        public virtual bool HasPrivatePool { get; set; }

        public virtual ICollection<PrivatePool> PrivatePool
        {
            get { return this.privatePool; }
            set { this.privatePool = !this.HasPrivatePool ? new PrivatePool[] { Enums.Domain.PrivatePool.None } : value;  }
        }

        public virtual ICollection<HomeFaces> HomeFaces { get; set; }

        public virtual ICollection<LotImprovements> LotImprovements { get; set; }

        public virtual ICollection<Inclusions> Inclusions { get; set; }

        public virtual ICollection<Floors> Floors { get; set; }

        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        public virtual ICollection<RoofDescription> RoofDescription { get; set; }

        public virtual ICollection<Foundation> Foundation { get; set; }

        public virtual ICollection<GreenCertification> GreenCertification { get; set; }

        public virtual ICollection<EnergyFeatures> EnergyFeatures { get; set; }

        public virtual ICollection<GreenFeatures> GreenFeatures { get; set; }

        public virtual string SupplierElectricity { get; set; }

        public virtual string SupplierWater { get; set; }

        public virtual string SupplierSewer { get; set; }

        public virtual string SupplierGarbage { get; set; }

        public virtual string SupplierGas { get; set; }

        public virtual string SupplierOther { get; set; }

        public virtual ICollection<HeatingFuel> HeatingFuel { get; set; }

        public bool IsNewConstruction { get; set; }

        public static FeaturesInfo ImportFromXml(XmlListingDetailResponse listing, FeaturesInfo features)
        {
            var importedFeatures = new FeaturesInfo();
            if (features != null)
            {
                importedFeatures = features.Clone();
            }

            importedFeatures.PropertyDescription = listing.Description;
            return importedFeatures;
        }

        public virtual void UpdateFromXml(XmlListingDetailResponse listing)
        {
            if (!string.IsNullOrEmpty(listing.Description))
            {
                this.PropertyDescription = listing.Description;
            }
        }

        public FeaturesInfo Clone()
        {
            return (FeaturesInfo)this.MemberwiseClone();
        }

        public FeaturesInfo ImportFeaturesFromCommunity(Utilities utilities)
        {
            var clonnedFeatures = this.Clone();
            clonnedFeatures.NeighborhoodAmenities = utilities.NeighborhoodAmenities;
            clonnedFeatures.Inclusions = utilities.Inclusions;
            clonnedFeatures.Floors = utilities.Floors;
            clonnedFeatures.ExteriorFeatures = utilities.ExteriorFeatures;
            clonnedFeatures.RoofDescription = utilities.RoofDescription;
            clonnedFeatures.Foundation = utilities.Foundation;
            clonnedFeatures.HeatSystem = utilities.HeatSystem;
            clonnedFeatures.CoolingSystem = utilities.CoolingSystem;
            clonnedFeatures.GreenCertification = utilities.GreenCertification;
            clonnedFeatures.EnergyFeatures = utilities.EnergyFeatures;
            clonnedFeatures.GreenFeatures = utilities.GreenFeatures;
            clonnedFeatures.WaterSewer = utilities.WaterSewer;
            clonnedFeatures.SupplierElectricity = utilities.SupplierElectricity;
            clonnedFeatures.SupplierWater = utilities.SupplierWater;
            clonnedFeatures.SupplierSewer = utilities.SupplierSewer;
            clonnedFeatures.SupplierGarbage = utilities.SupplierGarbage;
            clonnedFeatures.SupplierGas = utilities.SupplierGas;
            clonnedFeatures.SupplierOther = utilities.SupplierOther;
            clonnedFeatures.HeatingFuel = utilities.HeatingFuel;
            clonnedFeatures.Accessibility = utilities.Accessibility;
            clonnedFeatures.HasAccessibility = utilities.HasAccessibility;
            clonnedFeatures.Fireplaces = utilities.Fireplaces;
            clonnedFeatures.FireplaceDescription = utilities.FireplaceDescription;
            clonnedFeatures.Exterior = utilities.Exterior;

            return clonnedFeatures;
        }

        public FeaturesInfo ImportFeaturesFromPlan(BasePlan plan)
        {
            var clonnedFeatures = this.Clone();
            clonnedFeatures.IsNewConstruction = plan.IsNewConstruction;
            return clonnedFeatures;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.PropertyDescription;
            yield return this.Fireplaces;
            yield return this.FireplaceDescription;
            yield return this.Inclusions;
            yield return this.Floors;
            yield return this.WindowCoverings;
            yield return this.HasAccessibility;
            yield return this.Accessibility;
            yield return this.HousingStyle;
            yield return this.ExteriorFeatures;
            yield return this.RoofDescription;
            yield return this.Foundation;
            yield return this.Exterior;
            yield return this.HasPrivatePool;
            yield return this.PrivatePool;
            yield return this.HomeFaces;
            yield return this.NeighborhoodAmenities;
            yield return this.SupplierElectricity;
            yield return this.HeatSystem;
            yield return this.CoolingSystem;
            yield return this.GreenCertification;
            yield return this.EnergyFeatures;
            yield return this.GreenFeatures;
            yield return this.WaterSewer;
            yield return this.SupplierWater;
            yield return this.SupplierSewer;
            yield return this.SupplierGarbage;
            yield return this.SupplierGas;
            yield return this.SupplierOther;
            yield return this.HeatingFuel;
            yield return this.LotImprovements;
        }
    }
}
