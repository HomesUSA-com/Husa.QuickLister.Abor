namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotFeatures
    {
        ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }
        ICollection<View> View { get; set; }
        ICollection<WaterSewer> WaterSewer { get; set; }
        ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        ICollection<WaterSource> WaterSource { get; set; }
        DistanceToWaterAccess? DistanceToWaterAccess { get; set; }
        ICollection<Fencing> Fencing { get; set; }
        ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        ICollection<HorseAmenities> HorseAmenities { get; set; }
        ICollection<Minerals> MineralsFeatures { get; set; }
        ICollection<RoadSurface> RoadSurface { get; set; }
        ICollection<OtherStructures> OtherStructures { get; set; }

        ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        ICollection<Disclosures> Disclosures { get; set; }
        ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        bool GroundWaterConservDistric { get; set; }
        WaterBodyName? WaterBodyName { get; set; }
    }
}
