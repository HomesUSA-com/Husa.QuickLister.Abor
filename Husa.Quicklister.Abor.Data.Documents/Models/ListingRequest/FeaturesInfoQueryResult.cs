namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class FeaturesInfoQueryResult
    {
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public ICollection<WaterSource> WaterSource { get; set; }
        public string PropertyDescription { get; set; }

        public int? Fireplaces { get; set; }

        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }

        public ICollection<WindowCoverings> WindowCoverings { get; set; }

        public bool HasAccessibility { get; set; }

        public ICollection<Accessibility> Accessibility { get; set; }

        public ICollection<HousingStyle> HousingStyle { get; set; }

        public ICollection<Exterior> Exterior { get; set; }

        public bool HasPrivatePool { get; set; }

        public ICollection<PrivatePool> PrivatePool { get; set; }

        public ICollection<HomeFaces> HomeFaces { get; set; }

        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }

        public ICollection<LotImprovements> LotImprovements { get; set; }

        public virtual ICollection<Inclusions> Inclusions { get; set; }

        public virtual ICollection<Floors> Floors { get; set; }

        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        public virtual ICollection<RoofDescription> RoofDescription { get; set; }

        public virtual ICollection<Foundation> Foundation { get; set; }

        public virtual ICollection<HeatingSystem> HeatSystem { get; set; }

        public virtual ICollection<CoolingSystem> CoolingSystem { get; set; }

        public virtual ICollection<GreenCertification> GreenCertification { get; set; }

        public virtual ICollection<EnergyFeatures> EnergyFeatures { get; set; }

        public virtual ICollection<GreenFeatures> GreenFeatures { get; set; }

        public virtual ICollection<WaterSewer> WaterSewer { get; set; }

        public virtual string SupplierElectricity { get; set; }

        public virtual string SupplierWater { get; set; }

        public virtual string SupplierSewer { get; set; }

        public virtual string SupplierGarbage { get; set; }

        public virtual string SupplierGas { get; set; }

        public virtual string SupplierOther { get; set; }

        public virtual ICollection<HeatingFuel> HeatingFuel { get; set; }

        public bool IsNewConstruction { get; set; }
    }
}
