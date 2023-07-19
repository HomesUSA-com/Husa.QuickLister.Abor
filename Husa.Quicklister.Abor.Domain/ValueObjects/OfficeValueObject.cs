namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class OfficeValueObject : ValueObject
    {
        public string MarketUniqueId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public Cities City { get; set; }

        public States? State { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Status { get; set; }

        public string LicenseNumber { get; set; }

        public DateTime MarketModified { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MarketUniqueId;
            yield return this.Name;
            yield return this.Email;
            yield return this.Address;
            yield return this.City;
            yield return this.State;
            yield return this.Zip;
            yield return this.Phone;
            yield return this.Fax;
            yield return this.Status;
            yield return this.LicenseNumber;
            yield return this.MarketModified;
        }
    }
}
