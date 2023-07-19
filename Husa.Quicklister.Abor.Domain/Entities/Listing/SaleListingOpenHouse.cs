namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using ExtensionsOpenHouse = Husa.Quicklister.Extensions.Domain.Entities.Listing.SaleListingOpenHouse;

    public class SaleListingOpenHouse : ExtensionsOpenHouse
    {
        public SaleListingOpenHouse(
            Guid salePropertyId,
            OpenHouseType type,
            TimeSpan startTime,
            TimeSpan endTime,
            bool refreshments,
            bool lunch)
            : base(salePropertyId, type, startTime, endTime, refreshments, lunch)
        {
        }

        protected SaleListingOpenHouse()
            : base()
        {
        }

        public virtual SaleProperty SaleProperty { get; set; }

        public SaleListingOpenHouse Clone()
        {
            return (SaleListingOpenHouse)this.MemberwiseClone();
        }
    }
}
