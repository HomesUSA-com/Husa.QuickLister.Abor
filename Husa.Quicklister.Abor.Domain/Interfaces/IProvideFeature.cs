namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFeature
    {
        ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        ICollection<Disclosures> Disclosures { get; set; }
        ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        ICollection<WaterSource> WaterSource { get; set; }
        ICollection<WaterSewer> WaterSewer { get; set; }
        ICollection<HeatingSystem> HeatSystem { get; set; }
        ICollection<CoolingSystem> CoolingSystem { get; set; }
        ICollection<Appliances> Appliances { get; set; }
        int? GarageSpaces { get; set; }
        int? ParkingTotal { get; set; }
        ICollection<GarageDescription> GarageDescription { get; set; }
        ICollection<LaundryLocation> LaundryLocation { get; set; }
        ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        int? Fireplaces { get; set; }
        ICollection<FireplaceDescription> FireplaceDescription { get; set; }
        ICollection<Flooring> Floors { get; set; }
        ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        ICollection<WindowFeatures> WindowFeatures { get; set; }

        ICollection<Foundation> Foundation { get; set; }
        ICollection<RoofDescription> RoofDescription { get; set; }
        ICollection<Fencing> Fencing { get; set; }
        ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        ICollection<View> View { get; set; }
        ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
    }
}
