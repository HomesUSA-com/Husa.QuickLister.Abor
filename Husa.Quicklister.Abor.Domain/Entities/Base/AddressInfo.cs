namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System.Collections.Generic;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class AddressInfo : ValueObject, IProvideAddress
    {
        private string subdivision;

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

        public StreetType? StreetType { get; set; }

        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

        public static TAddress ImportFromXml<TAddress>(XmlListingDetailResponse listing, TAddress addressInfo)
            where TAddress : AddressInfo, new()
        {
            var importedAddressInfo = new TAddress();
            if (addressInfo != null)
            {
                importedAddressInfo = addressInfo.Clone<TAddress>();
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
            importedAddressInfo.Subdivision = listing.Subdivision;

            return importedAddressInfo;
        }

        public virtual TAddress Clone<TAddress>()
            where TAddress : AddressInfo
        {
            return (TAddress)this.MemberwiseClone();
        }

        public void PartialClone<TAddress>(TAddress addressToClone)
            where TAddress : AddressInfo
        {
            this.City = addressToClone.City;
            this.State = addressToClone.State;
            this.ZipCode = addressToClone.ZipCode;
            this.County = addressToClone.County;
            this.Subdivision = addressToClone.Subdivision;
        }

        public TAddress ImportAddressInfoFromCommunity<TAddress>(Community.Property address)
            where TAddress : AddressInfo
        {
            var clonedAddress = this.Clone<TAddress>();
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
            yield return this.Subdivision;
            yield return this.StreetType;
        }
    }
}
