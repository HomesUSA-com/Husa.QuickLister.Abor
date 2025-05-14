namespace Husa.Quicklister.Abor.Api.Contracts.Response.Community.CommunityDetail
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class FeaturesAndUtilitiesResponse
    {
        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public ICollection<Disclosures> Disclosures { get; set; }
        public ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public ICollection<WaterSource> WaterSource { get; set; }
        public ICollection<WaterSewer> WaterSewer { get; set; }
        public ICollection<HeatingSystem> HeatSystem { get; set; }
        public ICollection<CoolingSystem> CoolingSystem { get; set; }
        public ICollection<Appliances> Appliances { get; set; }
        public int? GarageSpaces { get; set; }
        public int? ParkingTotal { get; set; }
        public ICollection<GarageDescription> GarageDescription { get; set; }
        public ICollection<LaundryLocation> LaundryLocation { get; set; }
        public ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        public int? Fireplaces { get; set; }
        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }
        public ICollection<Flooring> Floors { get; set; }
        public ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        public ICollection<WindowFeatures> WindowFeatures { get; set; }

        public ICollection<Foundation> Foundation { get; set; }
        public ICollection<RoofDescription> RoofDescription { get; set; }
        public ICollection<Fencing> Fencing { get; set; }
        public ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        public ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        public ICollection<View> View { get; set; }
        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
    }
}
