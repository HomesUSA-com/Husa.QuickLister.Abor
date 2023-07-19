namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingRequestValueObject : ValueObject
    {
        public DateTime? ExpirationDate { get; set; }

        public DateTime? ListDate { get; set; }

        public decimal ListPrice { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.ExpirationDate;
            yield return this.ListDate;
            yield return this.ListPrice;
            yield return this.MlsNumber;
            yield return this.MlsStatus;
        }
    }
}
