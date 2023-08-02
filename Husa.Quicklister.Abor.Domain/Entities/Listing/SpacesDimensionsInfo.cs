namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class SpacesDimensionsInfo : ValueObject, IProvideSpacesDimensions, IProvideSpecialtyRooms
    {
        public SpacesDimensionsInfo()
        {
        }

        public CategoryType TypeCategory { get; set; }
        public virtual SqFtSource? SqFtSource { get; set; }
        public virtual ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
        public virtual ICollection<OtherParking> OtherParking { get; set; }

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

            importedSpacesDimensions.TypeCategory = listing.Type.ToCategoryType();
            importedSpacesDimensions.SqFtTotal = listing.Sqft;
            importedSpacesDimensions.StoriesTotal = listing.Stories.ToStories();
            importedSpacesDimensions.HalfBathsTotal = listing.HalfBaths;
            importedSpacesDimensions.FullBathsTotal = listing.Baths;
            importedSpacesDimensions.MainLevelBedroomTotal = listing.Bedrooms;

            return importedSpacesDimensions;
        }

        public virtual void UpdateFromXml(XmlListingDetailResponse listing)
        {
            if (listing.Sqft.HasValue)
            {
                this.SqFtTotal = listing.Sqft;
            }

            if (listing.Stories.HasValue)
            {
                this.StoriesTotal = listing.Stories.ToStories();
            }

            if (listing.HalfBaths.HasValue)
            {
                this.HalfBathsTotal = listing.HalfBaths;
            }

            if (listing.Baths.HasValue)
            {
                this.FullBathsTotal = listing.Baths;
            }

            if (listing.Bedrooms.HasValue)
            {
                this.MainLevelBedroomTotal = listing.Bedrooms;
            }
        }

        public SpacesDimensionsInfo Clone()
        {
            return (SpacesDimensionsInfo)this.MemberwiseClone();
        }

        public SpacesDimensionsInfo ImportSpacesDimensionsFromPlan(BasePlan basePlan)
        {
            var clonnedSpacesDimensions = this.Clone();
            clonnedSpacesDimensions.StoriesTotal = basePlan.StoriesTotal;
            clonnedSpacesDimensions.SqFtTotal = basePlan.SqFtTotal;
            clonnedSpacesDimensions.DiningAreasTotal = basePlan.DiningAreasTotal;
            clonnedSpacesDimensions.MainLevelBedroomTotal = basePlan.MainLevelBedroomTotal;
            clonnedSpacesDimensions.OtherLevelsBedroomTotal = basePlan.OtherLevelsBedroomTotal;
            clonnedSpacesDimensions.HalfBathsTotal = basePlan.HalfBathsTotal;
            clonnedSpacesDimensions.FullBathsTotal = basePlan.FullBathsTotal;
            clonnedSpacesDimensions.LivingAreasTotal = basePlan.LivingAreasTotal;

            return clonnedSpacesDimensions;
        }

        public SpacesDimensionsInfo ImportSpacesDimensionsFromCommunity(Utilities utilities)
        {
            var clonned = this.Clone();
            clonned.SpecialtyRooms = utilities.SpecialtyRooms;
            return clonned;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.TypeCategory;
            yield return this.SqFtSource;
            yield return this.SpecialtyRooms;
            yield return this.OtherParking;

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
