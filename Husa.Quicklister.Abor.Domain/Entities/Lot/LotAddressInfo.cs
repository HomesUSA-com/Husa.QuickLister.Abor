namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public class LotAddressInfo : Base.AddressInfo, IProvideLotAddress
    {
        public LotAddressInfo(string streetNum, string streetName, string zipCode, Cities city, States state, Counties? county)
        {
            this.City = city;
            this.State = state;
            this.County = county;
            this.ZipCode = zipCode;
            this.StreetName = streetName;
            this.StreetNumber = streetNum;
        }

        public LotAddressInfo()
        {
        }

        public StreetDirPrefix? StreetDirPrefix { get; set; }
        public StreetDirPrefix? StreetDirSuffix { get; set; }
        public string UnitNumber { get; set; }

        public LotAddressInfo Clone()
        {
            return (LotAddressInfo)this.MemberwiseClone();
        }

        public LotAddressInfo ImportAddressInfoFromCommunity(Community.Property address)
        {
            return this.ImportAddressInfoFromCommunity<LotAddressInfo>(address);
        }
    }
}
