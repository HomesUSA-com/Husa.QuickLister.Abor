namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotFeaturesResponse
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
    }
}
