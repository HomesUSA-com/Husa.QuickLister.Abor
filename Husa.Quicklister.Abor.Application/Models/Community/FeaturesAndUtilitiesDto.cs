namespace Husa.Quicklister.Abor.Application.Models.Community
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class FeaturesAndUtilitiesDto : IProvideFeature
    {
        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public ICollection<WaterSource> WaterSource { get; set; }
        public ICollection<WaterSewer> WaterSewer { get; set; }
        public ICollection<HeatingSystem> HeatSystem { get; set; }
        public ICollection<CoolingSystem> CoolingSystem { get; set; }
        public ICollection<Inclusions> Inclusions { get; set; }
        public ICollection<Floors> Floors { get; set; }
        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public ICollection<RoofDescription> RoofDescription { get; set; }
        public ICollection<Foundation> Foundation { get; set; }
        public ICollection<GreenCertification> GreenCertification { get; set; }
        public ICollection<EnergyFeatures> EnergyFeatures { get; set; }
        public ICollection<GreenFeatures> GreenFeatures { get; set; }
        public string SupplierElectricity { get; set; }
        public string SupplierWater { get; set; }
        public string SupplierSewer { get; set; }
        public string SupplierGarbage { get; set; }
        public string SupplierGas { get; set; }
        public string SupplierOther { get; set; }
        public ICollection<HeatingFuel> HeatingFuel { get; set; }
        public ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
        public bool HasAccessibility { get; set; }
        public ICollection<Accessibility> Accessibility { get; set; }
        public int? Fireplaces { get; set; }
        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }
        public ICollection<Exterior> Exterior { get; set; }
    }
}
