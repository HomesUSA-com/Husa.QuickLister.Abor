namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class SpacesDimensionsInfo : ValueObject, IProvideSpacesDimensions
    {
        public SpacesDimensionsInfo()
        {
        }

        public virtual Stories? StoriesTotal { get; set; }
        public virtual int? SqFtTotal { get; set; }
        public virtual int? DiningAreasTotal { get; set; }
        public virtual int? MainLevelBedroomTotal { get; set; }
        public virtual int? OtherLevelsBedroomTotal { get; set; }
        public virtual int? HalfBathsTotal { get; set; }
        public virtual int? FullBathsTotal { get; set; }
        public virtual int? LivingAreasTotal { get; set; }

        public static SpacesDimensionsInfo ImportFromXml(XmlListingDetailResponse listing, SpacesDimensionsInfo spacesDimensions)
        {
            var importedSpacesDimensions = new SpacesDimensionsInfo();
            if (spacesDimensions != null)
            {
                importedSpacesDimensions = spacesDimensions.Clone();
            }

            importedSpacesDimensions.SqFtTotal = listing.Sqft;
            importedSpacesDimensions.StoriesTotal = listing.Stories.ToStories();
            importedSpacesDimensions.HalfBathsTotal = listing.HalfBaths;
            importedSpacesDimensions.FullBathsTotal = listing.Baths;
            importedSpacesDimensions.MainLevelBedroomTotal = null;
            importedSpacesDimensions.OtherLevelsBedroomTotal = null;

            return importedSpacesDimensions;
        }

        public SpacesDimensionsInfo Clone()
        {
            return (SpacesDimensionsInfo)this.MemberwiseClone();
        }

        public SpacesDimensionsInfo ImportSpacesDimensionsFromPlan(BasePlan basePlan)
        {
            var clonnedSpacesDimensions = this.Clone();
            clonnedSpacesDimensions.StoriesTotal = basePlan.StoriesTotal;
            clonnedSpacesDimensions.SqFtTotal = basePlan.SqFtTotal ?? clonnedSpacesDimensions.SqFtTotal;
            clonnedSpacesDimensions.DiningAreasTotal = basePlan.DiningAreasTotal;
            clonnedSpacesDimensions.MainLevelBedroomTotal = basePlan.MainLevelBedroomTotal;
            clonnedSpacesDimensions.OtherLevelsBedroomTotal = basePlan.OtherLevelsBedroomTotal;
            clonnedSpacesDimensions.HalfBathsTotal = basePlan.HalfBathsTotal;
            clonnedSpacesDimensions.FullBathsTotal = basePlan.FullBathsTotal;
            clonnedSpacesDimensions.LivingAreasTotal = basePlan.LivingAreasTotal;

            return clonnedSpacesDimensions;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StoriesTotal;
            yield return this.SqFtTotal;
            yield return this.DiningAreasTotal;
            yield return this.MainLevelBedroomTotal;
            yield return this.OtherLevelsBedroomTotal;
            yield return this.HalfBathsTotal;
            yield return this.FullBathsTotal;
            yield return this.LivingAreasTotal;
        }
    }
}
