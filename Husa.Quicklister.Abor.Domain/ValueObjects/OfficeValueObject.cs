namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Domain.ValueObjects;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;

    public class OfficeValueObject : ValueObject
    {
        public string MarketUniqueId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Cities? City { get; set; }

        public StateOrProvince StateOrProvince { get; set; }

        public string Zip { get; set; }

        public string ZipExt { get; set; }

        public string Phone { get; set; }

        public OfficeStatus Status { get; set; }

        public DateTimeOffset MarketModified { get; set; }

        public OfficeType Type { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MarketUniqueId;
            yield return this.Name;
            yield return this.Address;
            yield return this.City;
            yield return this.Zip;
            yield return this.Phone;
            yield return this.Status;
            yield return this.MarketModified;
        }
    }
}
