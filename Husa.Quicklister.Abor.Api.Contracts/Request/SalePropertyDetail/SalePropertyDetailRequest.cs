namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SalePropertyDetailRequest
    {
        public SalePropertyRequest SalePropertyInfo { get; set; }

        public SpacesDimensionsRequest SpacesDimensionsInfo { get; set; }

        [Required]
        public AddressInfoRequest AddressInfo { get; set; }

        public PropertyInfoRequest PropertyInfo { get; set; }

        public FeaturesRequest FeaturesInfo { get; set; }

        public FinancialRequest FinancialInfo { get; set; }

        public ShowingRequest ShowingInfo { get; set; }

        public SchoolsRequest SchoolsInfo { get; set; }

        public ICollection<RoomRequest> Rooms { get; set; }

        public ICollection<HoaRequest> Hoas { get; set; }

        public ICollection<OpenHouseRequest> OpenHouses { get; set; }
    }
}
