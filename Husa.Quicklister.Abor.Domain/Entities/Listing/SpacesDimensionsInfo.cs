namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Common;
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
            this.GarageDescription = new HashSet<GarageDescription>();
        }

        public CategoryType TypeCategory { get; set; }

        public virtual int? SqFtTotal { get; set; }

        public virtual SqFtSource? SqFtSource { get; set; }

        public virtual ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }

        public virtual ICollection<OtherParking> OtherParking { get; set; }

        public virtual Stories? Stories { get; set; }

        public virtual int? BathsFull { get; set; }

        public virtual int? BathsHalf { get; set; }

        public virtual int? NumBedrooms { get; set; }

        public virtual ICollection<GarageDescription> GarageDescription { get; set; }

        public static SpacesDimensionsInfo ImportFromXml(XmlListingDetailResponse listing, SpacesDimensionsInfo spacesDimensions)
        {
            var importedSpacesDimensions = new SpacesDimensionsInfo();
            if (spacesDimensions != null)
            {
                importedSpacesDimensions = spacesDimensions.Clone();
            }

            importedSpacesDimensions.TypeCategory = listing.Type.ToCategoryType();
            var garageSpaces = listing.Stories != null ? decimal.ToInt32((decimal)listing.Stories) : 0;
            importedSpacesDimensions.GarageDescription = listing.Entry.ToEntry(garageSpaces);
            importedSpacesDimensions.SqFtTotal = listing.Sqft;
            importedSpacesDimensions.Stories = listing.Stories.ToStories();
            importedSpacesDimensions.BathsHalf = listing.HalfBaths;
            importedSpacesDimensions.BathsFull = listing.Baths;
            importedSpacesDimensions.NumBedrooms = listing.Bedrooms;

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
                this.Stories = listing.Stories.ToStories();
            }

            if (listing.HalfBaths.HasValue)
            {
                this.BathsHalf = listing.HalfBaths;
            }

            if (listing.Baths.HasValue)
            {
                this.BathsFull = listing.Baths;
            }

            if (listing.Bedrooms.HasValue)
            {
                this.NumBedrooms = listing.Bedrooms;
            }
        }

        public SpacesDimensionsInfo Clone()
        {
            return (SpacesDimensionsInfo)this.MemberwiseClone();
        }

        public SpacesDimensionsInfo ImportSpacesDimensionsFromPlan(BasePlan basePlan)
        {
            var clonnedSpacesDimensions = this.Clone();
            clonnedSpacesDimensions.Stories = basePlan.Stories;
            clonnedSpacesDimensions.BathsFull = basePlan.BathsFull;
            clonnedSpacesDimensions.BathsHalf = basePlan.BathsHalf;
            clonnedSpacesDimensions.NumBedrooms = basePlan.NumBedrooms;
            clonnedSpacesDimensions.GarageDescription = basePlan.GarageDescription;

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
            yield return this.Stories;
            yield return this.SqFtTotal;
            yield return this.SqFtSource;
            yield return this.SpecialtyRooms;
            yield return this.NumBedrooms;
            yield return this.BathsFull;
            yield return this.BathsHalf;
            yield return this.GarageDescription.ToStringFromEnumMembers();
            yield return this.OtherParking;
        }
    }
}
