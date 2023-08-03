namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFeature
    {
        ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        ICollection<WaterSource> WaterSource { get; set; }
        ICollection<WaterSewer> WaterSewer { get; set; }
        ICollection<HeatingSystem> HeatSystem { get; set; }
        ICollection<CoolingSystem> CoolingSystem { get; set; }

        ICollection<Appliances> Appliances { get; set; }
        int? GarageSpaces { get; set; }
        ICollection<GarageDescription> GarageDescription { get; set; }
        ICollection<LaundryFeatures> LaundryFeatures { get; set; }
        ICollection<LaundryLocation> LaundryLocation { get; set; }
        ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        ICollection<KitchenFeatures> KitchenFeatures { get; set; }
        ICollection<MasterBedroomFeatures> MasterBedroomFeatures { get; set; }
        ICollection<WaterAccessDescription> WaterAccessDescription { get; set; }
        int? Fireplaces { get; set; }
        ICollection<FireplaceDescription> FireplaceDescription { get; set; }
        ICollection<Flooring> Floors { get; set; }
        ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        ICollection<WindowFeatures> WindowFeatures { get; set; }

        ICollection<Inclusions> Inclusions { get; set; }
        ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        ICollection<RoofDescription> RoofDescription { get; set; }
        ICollection<Foundation> Foundation { get; set; }
        ICollection<GreenCertification> GreenCertification { get; set; }
        ICollection<EnergyFeatures> EnergyFeatures { get; set; }
        ICollection<GreenFeatures> GreenFeatures { get; set; }
        string SupplierElectricity { get; set; }
        string SupplierWater { get; set; }
        string SupplierSewer { get; set; }
        string SupplierGarbage { get; set; }
        string SupplierGas { get; set; }
        string SupplierOther { get; set; }
        ICollection<HeatingFuel> HeatingFuel { get; set; }
        bool HasAccessibility { get; set; }
        ICollection<Accessibility> Accessibility { get; set; }
    }
}
