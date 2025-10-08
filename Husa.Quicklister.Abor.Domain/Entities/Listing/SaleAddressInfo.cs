namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class SaleAddressInfo : Base.AddressInfo, IProvideSaleAddress
    {
        public SaleAddressInfo(string streetNum, string streetName, string unitNumber, string zipCode, Cities city, States state, Counties? county, StreetType? streetType)
        {
            this.City = city;
            this.State = state;
            this.County = county;
            this.ZipCode = zipCode;
            this.StreetName = streetName;
            this.StreetNumber = streetNum;
            this.UnitNumber = unitNumber;
            this.StreetType = streetType;
        }

        public SaleAddressInfo()
        {
        }

        public string UnitNumber { get; set; }

        public SaleAddressInfo Clone()
        {
            return (SaleAddressInfo)this.MemberwiseClone();
        }

        public SaleAddressInfo ImportAddressInfoFromCommunity(Community.Property address)
        {
            return this.ImportAddressInfoFromCommunity<SaleAddressInfo>(address);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StreetNumber;
            yield return this.StreetName;
            yield return this.City;
            yield return this.State;
            yield return this.ZipCode;
            yield return this.County;
            yield return this.Subdivision;
            yield return this.UnitNumber;
            yield return this.StreetType;
        }
    }
}
