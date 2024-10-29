namespace Husa.Quicklister.Abor.Api.Contracts.Response.LotListing
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotAddressResponse : AddressInfoResponse
    {
        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }
    }
}
