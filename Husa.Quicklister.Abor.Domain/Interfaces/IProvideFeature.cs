namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFeature
    {
        ICollection<Inclusions> Inclusions { get; set; }
        ICollection<Floors> Floors { get; set; }
        ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        ICollection<RoofDescription> RoofDescription { get; set; }
        ICollection<Foundation> Foundation { get; set; }
        ICollection<HeatingSystem> HeatSystem { get; set; }
        ICollection<CoolingSystem> CoolingSystem { get; set; }
        ICollection<GreenCertification> GreenCertification { get; set; }
        ICollection<EnergyFeatures> EnergyFeatures { get; set; }
        ICollection<GreenFeatures> GreenFeatures { get; set; }
        ICollection<WaterSewer> WaterSewer { get; set; }
        string SupplierElectricity { get; set; }
        string SupplierWater { get; set; }
        string SupplierSewer { get; set; }
        string SupplierGarbage { get; set; }
        string SupplierGas { get; set; }
        string SupplierOther { get; set; }
        ICollection<HeatingFuel> HeatingFuel { get; set; }
        int? Fireplaces { get; set; }
        ICollection<FireplaceDescription> FireplaceDescription { get; set; }
        ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        bool HasAccessibility { get; set; }
        ICollection<Accessibility> Accessibility { get; set; }
    }
}
