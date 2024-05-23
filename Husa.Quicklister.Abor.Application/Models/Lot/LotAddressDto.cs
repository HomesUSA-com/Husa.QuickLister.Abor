namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public class LotAddressDto : IProvideLotAddress
    {
        public Counties? County { get; set; }
        public StreetType? StreetType { get; set; }
        public string Subdivision { get; set; }
        public States State { get; set; }
        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public Cities City { get; set; }
        public string ZipCode { get; set; }
        public string UnitNumber { get; set; }
    }
}
