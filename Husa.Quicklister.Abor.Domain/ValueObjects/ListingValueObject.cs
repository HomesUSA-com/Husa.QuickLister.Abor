namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingValueObject : ValueObject
    {
        public virtual int? CDOM { get; set; }

        public virtual int? DOM { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual DateTime? ListDate { get; set; }

        public virtual decimal ListPrice { get; set; }

        public virtual ListType ListType { get; set; }

        public virtual DateTime? MarketModifiedOn { get; set; }

        public virtual string MlsNumber { get; set; }

        public virtual MarketStatuses MlsStatus { get; set; }

        public virtual Guid PropertyId { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.CDOM;
            yield return this.DOM;
            yield return this.ExpirationDate;
            yield return this.ListDate;
            yield return this.ListPrice;
            yield return this.ListType;
            yield return this.MarketModifiedOn;
            yield return this.MlsNumber;
            yield return this.MlsStatus;
            yield return this.PropertyId;
        }
    }
}
