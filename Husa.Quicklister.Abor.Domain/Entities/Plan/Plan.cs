namespace Husa.Quicklister.Abor.Domain.Entities.Plan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using ExtensionPlan = Husa.Quicklister.Extensions.Domain.Entities.Plan.Plan;

    public class Plan : ExtensionPlan, IProvideActiveListings<SaleListing>
    {
        public Plan(Guid companyId, string name, string ownerName, JsonImportStatus jsonStatus)
            : this(companyId, name, ownerName)
        {
            this.JsonImportStatus = jsonStatus;
        }

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

        public virtual Expression<Func<SaleListing, bool>> ActiveListingsInMarketExpression => listing
           => !listing.IsDeleted && listing.SaleProperty.PlanId == this.Id && SaleListing.ActiveListingStatuses.Contains(listing.MlsStatus) && !string.IsNullOrWhiteSpace(listing.MlsNumber);

        public virtual IEnumerable<SaleListing> GetListingsToUpdate() => this.SaleProperties.GetListingsToUpdate();

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
            => this.ImportRooms(rooms);

        public virtual void ImportRooms<TRoom>(IEnumerable<TRoom> rooms)
            where TRoom : Room
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
                    roomDetail.Features,
                    roomDetail.Description);

                this.Rooms.Add(room);
            }
        }

        public virtual void Migrate(int legacyId, BasePlan basePlan, IEnumerable<PlanRoom> rooms)
        {
            this.LegacyProfileId = legacyId;
            this.UpdateBasePlanInformation(basePlan);
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
