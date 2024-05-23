namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;

    public class SalePropertyDetailDto
    {
        public virtual Guid Id { get; set; }

        public virtual SalePropertyDto SalePropertyInfo { get; set; }

        public virtual SaleAddressDto AddressInfo { get; set; }

        public virtual PropertyDto PropertyInfo { get; set; }

        public virtual SpacesDimensionsDto SpacesDimensionsInfo { get; set; }

        public virtual FeaturesDto FeaturesInfo { get; set; }

        public virtual FinancialDto FinancialInfo { get; set; }

        public virtual ShowingDto ShowingInfo { get; set; }

        public virtual SchoolsDto SchoolsInfo { get; set; }

        public virtual IEnumerable<RoomDto> Rooms { get; set; }

        public virtual IEnumerable<OpenHouseDto> OpenHouses { get; set; }
    }
}
