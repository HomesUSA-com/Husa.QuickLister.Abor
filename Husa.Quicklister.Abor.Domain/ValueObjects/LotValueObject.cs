namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class LotValueObject : ValueObject
    {
        public string OwnerName { get; set; }
        public decimal? ListPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ListDate { get; set; }
        public ListType ListType { get; set; }
        public string MlsNumber { get; set; }
        public MarketStatuses MlsStatus { get; set; }
        public DateTime? MarketModifiedOn { get; set; }
        public bool IsManuallyManaged { get; set; }
        public LotAddressInfo AddressInfo { get; set; }
        public LotPropertyInfo PropertyInfo { get; set; }
        public LotFeaturesInfo FeaturesInfo { get; set; }
        public LotFinancialInfo FinancialInfo { get; set; }
        public LotShowingInfo ShowingInfo { get; set; }
        public LotSchoolsInfo SchoolsInfo { get; set; }
        public ListingStatusFieldsInfo StatusFieldsInfo { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.OwnerName;
            yield return this.ListPrice;
            yield return this.ExpirationDate;
            yield return this.ListDate;
            yield return this.ListType;
            yield return this.MlsNumber;
            yield return this.MlsStatus;
            yield return this.MarketModifiedOn;
            yield return this.IsManuallyManaged;
            yield return this.AddressInfo;
            yield return this.PropertyInfo;
            yield return this.FeaturesInfo;
            yield return this.FinancialInfo;
            yield return this.ShowingInfo;
            yield return this.SchoolsInfo;
            yield return this.StatusFieldsInfo;
        }
    }
}
