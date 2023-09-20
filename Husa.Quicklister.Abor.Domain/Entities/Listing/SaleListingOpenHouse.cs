namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class SaleListingOpenHouse : OpenHouse
    {
        public SaleListingOpenHouse(
            Guid salePropertyId,
            OpenHouseType type,
            TimeSpan startTime,
            TimeSpan endTime,
            ICollection<Refreshments> refreshments)
            : base(type, startTime, endTime, refreshments)
        {
            this.SalePropertyId = salePropertyId;
            this.OpenHouseType = EntityType.SaleProperty.ToString();
        }

        protected SaleListingOpenHouse()
            : base()
        {
            this.OpenHouseType = EntityType.SaleProperty.ToString();
        }

        public Guid SalePropertyId { get; set; }
        public virtual SaleProperty SaleProperty { get; set; }

        public SaleListingOpenHouse Clone()
        {
            return (SaleListingOpenHouse)this.MemberwiseClone();
        }
    }
}
