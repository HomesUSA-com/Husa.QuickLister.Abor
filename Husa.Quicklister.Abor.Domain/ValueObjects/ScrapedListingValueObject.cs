namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ScrapedListingValueObject : ValueObject
    {
        public virtual string OfficeName { get; set; }

        public virtual string BuilderName { get; set; }
        public virtual int? DOM { get; set; }
        public virtual string UncleanBuilder { get; set; }
        public virtual string MlsNum { get; set; }
        public virtual MarketStatuses ListStatus { get; set; }
        public virtual string Community { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual decimal? ListPrice { get; set; }
        public virtual int Price { get; set; }
        public virtual string Comment { get; set; }
        public virtual DateTime? ListDate { get; set; }
        public virtual DateTime? Refreshed { get; set; }
        public virtual int? UnitNum { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.OfficeName;
            yield return this.BuilderName;
            yield return this.DOM;
            yield return this.UncleanBuilder;
            yield return this.MlsNum;
            yield return this.ListStatus;
            yield return this.Community;
            yield return this.Address;
            yield return this.City;
            yield return this.ListPrice;
            yield return this.Price;
            yield return this.Comment;
            yield return this.ListDate;
            yield return this.Refreshed;
            yield return this.UnitNum;
        }
    }
}
