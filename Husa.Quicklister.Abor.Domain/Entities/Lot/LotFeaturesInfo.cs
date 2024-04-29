namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotFeaturesInfo : ValueObject
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
        }
    }
}
