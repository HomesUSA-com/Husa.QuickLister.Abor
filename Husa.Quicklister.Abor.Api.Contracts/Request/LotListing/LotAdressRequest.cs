namespace Husa.Quicklister.Abor.Api.Contracts.Request.LotListing
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotAdressRequest : AddressInfoRequest
    {
        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }
        public string UnitNumber { get; set; }
    }
}
