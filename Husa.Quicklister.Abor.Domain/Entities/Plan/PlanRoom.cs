namespace Husa.Quicklister.Abor.Domain.Entities.Plan
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class PlanRoom : Room
    {
        public PlanRoom(Guid planId, RoomType roomType, RoomLevel level, ICollection<RoomFeatures> features, string description)
            : base(roomType, level, features, description)
        {
            this.PlanId = planId;
            this.EntityOwnerType = EntityType.Plan.ToString();
        }

        protected PlanRoom()
            : base()
        {
        }

        public Guid PlanId { get; set; }

        public virtual Plan Plan { get; }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.PlanId;
            yield return base.GetEntityEqualityComponents();
        }
    }
}
