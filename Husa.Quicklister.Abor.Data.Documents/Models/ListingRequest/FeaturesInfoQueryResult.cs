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

        public ICollection<WindowFeatures> WindowCoverings { get; set; }

        public bool HasAccessibility { get; set; }

        public ICollection<Accessibility> Accessibility { get; set; }

        public ICollection<HousingStyle> HousingStyle { get; set; }

        public ICollection<Exterior> Exterior { get; set; }

        public bool HasPrivatePool { get; set; }

        public ICollection<PrivatePool> PrivatePool { get; set; }

        public ICollection<HomeFaces> HomeFaces { get; set; }

        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }

        public ICollection<LotImprovements> LotImprovements { get; set; }

        public ICollection<Inclusions> Inclusions { get; set; }

        public ICollection<Flooring> Floors { get; set; }

        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        public ICollection<RoofDescription> RoofDescription { get; set; }

        public ICollection<Foundation> Foundation { get; set; }

        public ICollection<HeatingSystem> HeatSystem { get; set; }

        public ICollection<CoolingSystem> CoolingSystem { get; set; }
        public ICollection<Appliances> Appliances { get; set; }
        public int? GarageSpaces { get; set; }
        public ICollection<GarageDescription> GarageDescription { get; set; }
        public ICollection<LaundryFeatures> LaundryFeatures { get; set; }
        public ICollection<LaundryLocation> LaundryLocation { get; set; }
        public ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        public ICollection<KitchenFeatures> KitchenFeatures { get; set; }
        public ICollection<MasterBedroomFeatures> MasterBedroomFeatures { get; set; }
        public ICollection<WaterAccessDescription> WaterAccessDescription { get; set; }
        public ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        public ICollection<WindowFeatures> WindowFeatures { get; set; }
        public ICollection<Fencing> Fencing { get; set; }
        public ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        public ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        public ICollection<View> View { get; set; }

        public ICollection<GreenCertification> GreenCertification { get; set; }

        public ICollection<EnergyFeatures> EnergyFeatures { get; set; }

        public ICollection<GreenFeatures> GreenFeatures { get; set; }

        public ICollection<WaterSewer> WaterSewer { get; set; }

        public string SupplierElectricity { get; set; }

        public string SupplierWater { get; set; }

        public string SupplierSewer { get; set; }

        public string SupplierGarbage { get; set; }

        public string SupplierGas { get; set; }

        public string SupplierOther { get; set; }

        public ICollection<HeatingFuel> HeatingFuel { get; set; }

        public bool IsNewConstruction { get; set; }
    }
}
