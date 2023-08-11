namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using System.Collections.Generic;

    public class SalePropertyDetailResponse
    {
        public SalePropertyResponse SalePropertyInfo { get; set; }

        public SpacesDimensionsResponse SpacesDimensionsInfo { get; set; }

        public AddressInfoResponse AddressInfo { get; set; }

        public PropertyInfoResponse PropertyInfo { get; set; }

        public FeaturesResponse FeaturesInfo { get; set; }

        public FinancialResponse FinancialInfo { get; set; }

        public ShowingResponse ShowingInfo { get; set; }

        public SchoolsResponse SchoolsInfo { get; set; }

        public IEnumerable<RoomResponse> Rooms { get; set; }

        public IEnumerable<OpenHouseResponse> OpenHouses { get; set; }
    }
}
