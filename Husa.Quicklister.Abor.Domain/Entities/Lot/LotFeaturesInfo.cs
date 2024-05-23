namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public class LotFeaturesInfo : ValueObject, IProvideLotFeatures
    {
        public virtual ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public virtual ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }
        public virtual ICollection<View> View { get; set; }
        public virtual ICollection<WaterSewer> WaterSewer { get; set; }
        public virtual ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public virtual ICollection<WaterSource> WaterSource { get; set; }
        public virtual DistanceToWaterAccess? DistanceToWaterAccess { get; set; }
        public virtual ICollection<Fencing> Fencing { get; set; }
        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public virtual bool GroundWaterConservDistric { get; set; }
        public virtual ICollection<HorseAmenities> HorseAmenities { get; set; }
        public virtual ICollection<Minerals> MineralsFeatures { get; set; }
        public virtual ICollection<RoadSurface> RoadSurface { get; set; }
        public virtual ICollection<OtherStructures> OtherStructures { get; set; }
        public virtual ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public virtual ICollection<Disclosures> Disclosures { get; set; }
        public virtual ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public WaterBodyName? WaterBodyName { get; set; }

        public LotFeaturesInfo Clone()
        {
            return (LotFeaturesInfo)this.MemberwiseClone();
        }

        public LotFeaturesInfo ImportFeaturesFromCommunity(Utilities utilities)
        {
            var clonnedFeatures = this.Clone();
            clonnedFeatures.RestrictionsDescription = utilities.RestrictionsDescription;
            clonnedFeatures.WaterSewer = utilities.WaterSewer;
            clonnedFeatures.WaterSource = utilities.WaterSource;
            clonnedFeatures.UtilitiesDescription = utilities.UtilitiesDescription;
            clonnedFeatures.View = utilities.View;
            clonnedFeatures.Fencing = utilities.Fencing;
            clonnedFeatures.ExteriorFeatures = utilities.ExteriorFeatures;
            clonnedFeatures.NeighborhoodAmenities = utilities.NeighborhoodAmenities;
            return clonnedFeatures;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.RestrictionsDescription;
            yield return this.WaterfrontFeatures;
            yield return this.View;
            yield return this.WaterSewer;
            yield return this.UtilitiesDescription;
            yield return this.WaterSource;
            yield return this.DistanceToWaterAccess;
            yield return this.Fencing;
            yield return this.ExteriorFeatures;
            yield return this.GroundWaterConservDistric;
            yield return this.HorseAmenities;
            yield return this.MineralsFeatures;
            yield return this.RoadSurface;
            yield return this.OtherStructures;
            yield return this.NeighborhoodAmenities;
            yield return this.DocumentsAvailable;
            yield return this.Disclosures;
            yield return this.WaterBodyName;
        }
    }
}
