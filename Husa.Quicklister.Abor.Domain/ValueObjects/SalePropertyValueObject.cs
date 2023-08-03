namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;

    public class SalePropertyValueObject : ValueObject
    {
        public virtual string OwnerName { get; set; }

        public virtual Guid? PlanId { get; set; }

        public virtual Guid? CommunityId { get; set; }

        public virtual PropertyInfo PropertyInfo { get; set; }

        public virtual AddressInfo AddressInfo { get; set; }

        public virtual FeaturesInfo FeaturesInfo { get; set; }

        public virtual SchoolsInfo SchoolsInfo { get; set; }

        public virtual ShowingInfo ShowingInfo { get; set; }

        public virtual SpacesDimensionsInfo SpacesDimensionsInfo { get; set; }

        public virtual FinancialInfo FinancialInfo { get; set; }

        public virtual ICollection<ListingSaleRoom> Rooms { get; set; }

        public virtual ICollection<SaleListingHoa> Hoas { get; set; }

        public virtual ICollection<SaleListingOpenHouse> OpenHouses { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.OwnerName;
            yield return this.CommunityId;
            yield return this.PlanId;
            yield return this.PropertyInfo;
            yield return this.AddressInfo;
            yield return this.FeaturesInfo;
            yield return this.SchoolsInfo;
            yield return this.ShowingInfo;
            yield return this.SpacesDimensionsInfo;
            yield return this.FinancialInfo;
            yield return this.Rooms;
            yield return this.Hoas;
            yield return this.OpenHouses;
        }
    }
}
