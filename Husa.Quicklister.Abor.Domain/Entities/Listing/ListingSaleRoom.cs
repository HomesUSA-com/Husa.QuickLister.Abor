namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingSaleRoom : Room
    {
        public ListingSaleRoom(Guid salePropertyId, RoomType roomType, RoomLevel level, ICollection<RoomFeatures> features, string description)
            : base(roomType, level, features, description)
        {
            this.SalePropertyId = salePropertyId;
            this.EntityOwnerType = EntityType.SaleProperty.ToString();
        }

        protected ListingSaleRoom()
            : base()
        {
        }

        public Guid SalePropertyId { get; set; }

        public virtual SaleProperty SaleProperty { get; }

        public ListingSaleRoom Clone()
        {
            return (ListingSaleRoom)this.MemberwiseClone();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.SalePropertyId;
            yield return this.Level;
            yield return this.RoomType;
            yield return this.EntityOwnerType;
            yield return this.Features;
            yield return this.Description;
        }
    }
}
