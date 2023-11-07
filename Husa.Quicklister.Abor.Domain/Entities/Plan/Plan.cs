namespace Husa.Quicklister.Abor.Domain.Entities.Plan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using ExtensionPlan = Husa.Quicklister.Extensions.Domain.Entities.Plan.Plan;

    public class Plan : ExtensionPlan
    {
        public Plan(Guid companyId, string name, string ownerName, XmlStatus xmlStatus)
            : this(companyId, name, ownerName)
        {
            this.XmlStatus = xmlStatus;
        }

        public Plan(Guid companyId, string name, string ownerName)
            : this()
        {
            this.CompanyId = companyId;
            this.BasePlan = new(name, ownerName);
        }

        public Plan()
            : base()
        {
            this.Rooms = new HashSet<PlanRoom>();
            this.SaleProperties = new HashSet<SaleProperty>();
        }

        protected Plan(Guid companyId)
            : base(companyId)
        {
            this.Rooms = new HashSet<PlanRoom>();
            this.SaleProperties = new HashSet<SaleProperty>();
        }

        public virtual BasePlan BasePlan { get; set; }

        public virtual ICollection<PlanRoom> Rooms { get; set; }

        public virtual ICollection<SaleProperty> SaleProperties { get; set; }

        public virtual void UpdateBasePlanInformation(BasePlan plan)
        {
            if (plan is null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (this.BasePlan != plan)
            {
                this.BasePlan = plan;
            }
        }

        public virtual void UpdateRooms(IEnumerable<PlanRoom> rooms)
        {
            if (rooms is null)
            {
                throw new ArgumentNullException(nameof(rooms));
            }

            this.Rooms.Clear();

            foreach (var roomDetail in rooms)
            {
                var room = new PlanRoom(
                    this.Id,
                    roomDetail.RoomType,
                    roomDetail.Level,
                    roomDetail.Features);

                this.Rooms.Add(room);
            }
        }

        public virtual void Migrate(Guid legacyId, BasePlan basePlan, IEnumerable<PlanRoom> rooms)
        {
            this.LegacyId = legacyId;
            this.UpdateBasePlanInformation(basePlan);
            this.UpdateRooms(rooms);
        }

        public virtual void ImportFromListing(SaleListing listing)
        {
            this.BasePlan.StoriesTotal = listing.SaleProperty.SpacesDimensionsInfo.StoriesTotal;
            this.BasePlan.SqFtTotal = listing.SaleProperty.SpacesDimensionsInfo.SqFtTotal;
            this.BasePlan.DiningAreasTotal = listing.SaleProperty.SpacesDimensionsInfo.DiningAreasTotal;
            this.BasePlan.MainLevelBedroomTotal = listing.SaleProperty.SpacesDimensionsInfo.MainLevelBedroomTotal;
            this.BasePlan.OtherLevelsBedroomTotal = listing.SaleProperty.SpacesDimensionsInfo.OtherLevelsBedroomTotal;
            this.BasePlan.HalfBathsTotal = listing.SaleProperty.SpacesDimensionsInfo.HalfBathsTotal;
            this.BasePlan.FullBathsTotal = listing.SaleProperty.SpacesDimensionsInfo.FullBathsTotal;
            this.BasePlan.LivingAreasTotal = listing.SaleProperty.SpacesDimensionsInfo.LivingAreasTotal;

            var rooms = listing.SaleProperty.Rooms.Select(room => new PlanRoom(
                    room.Id, room.RoomType, room.Level, room.Features));

            this.UpdateRooms(rooms);
        }

        protected bool AreRoomsEqual(ICollection<PlanRoom> other)
        {
            return this.Rooms
                .OrderBy(x => x.RoomType)
                .SequenceEqual(other.OrderBy(x => x.RoomType), new ListingRoomComparer());
        }

        protected override void DeleteChildren(Guid userId)
        {
            foreach (var listing in this.SaleProperties)
            {
                listing.Delete(userId);

                listing.PlanId = null;

                listing.UpdateTrackValues(userId);
            }
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.BasePlan;
        }
    }
}
