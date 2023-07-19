namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class SaleListingHoa : Hoa
    {
        public SaleListingHoa(
            Guid salePropertyId,
            string name,
            decimal transferFee,
            decimal fee,
            BillingFrequency billingFrequency,
            string website,
            string contactPhone)
            : base(name, transferFee, fee, billingFrequency, website, contactPhone)
        {
            this.SalePropertyId = salePropertyId;
            this.HoaType = EntityType.SaleProperty;
        }

        protected SaleListingHoa()
            : base()
        {
        }

        public Guid SalePropertyId { get; set; }

        public virtual SaleProperty SaleProperty { get; }

        public SaleListingHoa Clone()
        {
            return (SaleListingHoa)this.MemberwiseClone();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.SalePropertyId;
            yield return this.Name;
            yield return this.TransferFee;
            yield return this.Fee;
            yield return this.BillingFrequency;
            yield return this.Website;
            yield return this.ContactPhone;
            yield return this.HoaType;
        }
    }
}
