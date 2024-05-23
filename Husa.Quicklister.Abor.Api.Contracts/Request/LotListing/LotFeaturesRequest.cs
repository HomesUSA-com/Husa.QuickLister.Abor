namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotFeaturesRequest
    {
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }
        public ICollection<View> View { get; set; }
        public ICollection<WaterSewer> WaterSewer { get; set; }
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public ICollection<WaterSource> WaterSource { get; set; }
        public DistanceToWaterAccess? DistanceToWaterAccess { get; set; }
        public ICollection<Fencing> Fencing { get; set; }
        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public bool GroundWaterConservDistric { get; set; }
        public ICollection<HorseAmenities> HorseAmenities { get; set; }
        public ICollection<Minerals> MineralsFeatures { get; set; }
        public ICollection<RoadSurface> RoadSurface { get; set; }
        public ICollection<OtherStructures> OtherStructures { get; set; }
        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public ICollection<Disclosures> Disclosures { get; set; }
        public ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public WaterBodyName? WaterBodyName { get; set; }
    }
}
