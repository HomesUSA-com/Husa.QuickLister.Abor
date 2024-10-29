namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotAddressQueryResult : AddressQueryResult
    {
        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }
    }
}
