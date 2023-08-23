namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System.Collections.Generic;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Xml.Api.Contracts.Response;

    public class AddressInfo : ValueObject
    {
        private string subdivision;

        public AddressInfo(string streetNum, string streetName, string zipCode, Cities city, States state, Counties? county)
        {
            this.City = city;
            this.State = state;
            this.County = county;
            this.ZipCode = zipCode;
            this.StreetName = streetName;
            this.StreetNumber = streetNum;
        }

        public AddressInfo()
        {
        }

        public string FormalAddress => $"{this.StreetNumber} {this.StreetName}, {this.ReadableCity} {this.ZipCode}";

        public string ReadableCity => this.City.GetEnumDescription();

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public Cities City { get; set; }

        public States State { get; set; }

        public string ZipCode { get; set; }

        public Counties? County { get; set; }

        public string LotNum { get; set; }

        public string Block { get; set; }

        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

        public static AddressInfo ImportFromXml(XmlListingDetailResponse listing, AddressInfo addressInfo)
        {
            var importedAddressInfo = new AddressInfo();
            if (addressInfo != null)
            {
                importedAddressInfo = addressInfo.Clone();
            }

            importedAddressInfo.StreetNumber = listing.StreetNum;
            importedAddressInfo.StreetName = listing.StreetName;
            if (!string.IsNullOrEmpty(listing.City))
            {
                importedAddressInfo.City = listing.City.ToCity(isExactValue: false) ?? Cities.NotApplicable;
            }

            importedAddressInfo.State = listing.State.ToState(isExactValue: false) ?? States.Texas;
            importedAddressInfo.ZipCode = listing.Zip;
            importedAddressInfo.County = listing.County.ToCounty(isExactValue: false);
            importedAddressInfo.LotNum = listing.Lot;
            importedAddressInfo.Block = listing.Block;
            importedAddressInfo.Subdivision = listing.Subdivision;

            return importedAddressInfo;
        }

        public AddressInfo Clone()
        {
            return (AddressInfo)this.MemberwiseClone();
        }

        public void PartialClone(AddressInfo addressToClone)
        {
            this.City = addressToClone.City;
            this.State = addressToClone.State;
            this.ZipCode = addressToClone.ZipCode;
            this.County = addressToClone.County;
            this.Subdivision = addressToClone.Subdivision;
        }

        public AddressInfo ImportAddressInfoFromCommunity(Community.Property address)
        {
            var clonedAddress = this.Clone();
            clonedAddress.ZipCode = address.ZipCode ?? clonedAddress.ZipCode;
            clonedAddress.County = address.County;

            if (address.City.HasValue)
            {
                clonedAddress.City = address.City.Value;
            }

            clonedAddress.Subdivision = address.Subdivision;

            return clonedAddress;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StreetNumber;
            yield return this.StreetName;
            yield return this.City;
            yield return this.State;
            yield return this.ZipCode;
            yield return this.County;
            yield return this.LotNum;
            yield return this.Block;
            yield return this.Subdivision;
        }
    }
}
