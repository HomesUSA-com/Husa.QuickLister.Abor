namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;

    public class LotPropertyValueObject : ValueObject
    {
        public virtual string OwnerName { get; set; }

        public virtual Guid? CommunityId { get; set; }

        public virtual LotPropertyInfo PropertyInfo { get; set; }

        public virtual LotAddressInfo AddressInfo { get; set; }

        public virtual LotFeaturesInfo FeaturesInfo { get; set; }

        public virtual LotSchoolsInfo SchoolsInfo { get; set; }

        public virtual LotShowingInfo ShowingInfo { get; set; }

        public virtual LotFinancialInfo FinancialInfo { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.OwnerName;
            yield return this.CommunityId;
            yield return this.PropertyInfo;
            yield return this.AddressInfo;
            yield return this.FeaturesInfo;
            yield return this.SchoolsInfo;
            yield return this.ShowingInfo;
            yield return this.FinancialInfo;
        }
    }
}
