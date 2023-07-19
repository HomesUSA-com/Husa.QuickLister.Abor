namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using Husa.Extensions.Domain.Entities;
    using Husa.Extensions.Domain.Extensions;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public abstract class Hoa : Entity, IProvideType, IProvideHoaInfo
    {
        protected Hoa(
            string name,
            decimal transferFee,
            decimal fee,
            BillingFrequency billingFrequency,
            string website,
            string contactPhone)
        {
            this.Name = name;
            this.TransferFee = transferFee;
            this.Fee = fee;
            this.BillingFrequency = billingFrequency;
            this.Website = website;
            this.ContactPhone = contactPhone.CleanPhoneValue();
        }

        protected Hoa()
        {
        }

        public string Name { get; set; }

        public decimal TransferFee { get; set; }

        public decimal Fee { get; set; }

        public BillingFrequency BillingFrequency { get; set; }

        public string Website { get; set; }

        public string ContactPhone { get; set; }

        public string FieldType => this.HoaType.ToString();

        public EntityType HoaType { get; protected set; }

        public string CustomString()
        {
            return $"Name: {this.Name}, TransferFee: {this.TransferFee}, Fee: {this.Fee}, BillingType: {this.BillingFrequency}, ContactPhone: {this.ContactPhone}, Website; {this.Website}";
        }

        protected override void DeleteChildren(Guid userId) => throw new NotImplementedException();
    }
}
