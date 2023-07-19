namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class CommunityHoa : Hoa
    {
        public CommunityHoa(
            Guid communityId,
            string name,
            decimal transferFee,
            decimal fee,
            BillingFrequency billingFrequency,
            string website,
            string contactPhone)
            : base(name, transferFee, fee, billingFrequency, website, contactPhone)
        {
            this.CommunitySaleId = communityId;
            this.HoaType = EntityType.Community;
        }

        public CommunityHoa(
            string name,
            decimal transferFee,
            decimal fee,
            BillingFrequency billingFrequency,
            string website,
            string contactPhone)
            : base(name, transferFee, fee, billingFrequency, website, contactPhone)
        {
            this.HoaType = EntityType.Community;
        }

        protected CommunityHoa()
            : base()
        {
        }

        public Guid CommunitySaleId { get; set; }

        public virtual CommunitySale Community { get; }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.CommunitySaleId;
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
