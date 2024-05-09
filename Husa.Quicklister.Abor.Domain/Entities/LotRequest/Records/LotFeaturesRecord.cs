namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public record LotFeaturesRecord : IProvideLotFeatures
    {
        [Required]
        [MinLength(1)]
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<View> View { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterSewer> WaterSewer { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterSource> WaterSource { get; set; }

        public DistanceToWaterAccess? DistanceToWaterAccess { get; set; }

        public ICollection<Fencing> Fencing { get; set; }

        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<HorseAmenities> HorseAmenities { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Minerals> MineralsFeatures { get; set; }

        public ICollection<RoadSurface> RoadSurface { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<OtherStructures> OtherStructures { get; set; }

        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        [Required]
        [MinLength(1)]
        public ICollection<Disclosures> Disclosures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public bool GroundWaterConservDistric { get; set; }

        public LotFeaturesRecord CloneRecord() => (LotFeaturesRecord)this.MemberwiseClone();

        public static LotFeaturesRecord CreateRecord(LotFeaturesInfo featuresInfo)
        {
            if (featuresInfo == null)
            {
                return new();
            }

            return new()
            {
                RestrictionsDescription = featuresInfo.RestrictionsDescription,
                UtilitiesDescription = featuresInfo.UtilitiesDescription,
                WaterSource = featuresInfo.WaterSource,
                WaterSewer = featuresInfo.WaterSewer,
                Fencing = featuresInfo.Fencing,
                View = featuresInfo.View,
                ExteriorFeatures = featuresInfo.ExteriorFeatures,
                WaterfrontFeatures = featuresInfo.WaterfrontFeatures,
                DistanceToWaterAccess = featuresInfo.DistanceToWaterAccess,
                NeighborhoodAmenities = featuresInfo.NeighborhoodAmenities,
                Disclosures = featuresInfo.Disclosures,
                DocumentsAvailable = featuresInfo.DocumentsAvailable,
                GroundWaterConservDistric = featuresInfo.GroundWaterConservDistric,
                HorseAmenities = featuresInfo.HorseAmenities,
                MineralsFeatures = featuresInfo.MineralsFeatures,
                OtherStructures = featuresInfo.OtherStructures,
                RoadSurface = featuresInfo.RoadSurface,
            };
        }
    }
}
