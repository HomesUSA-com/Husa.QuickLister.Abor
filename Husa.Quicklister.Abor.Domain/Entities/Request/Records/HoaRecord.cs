namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record HoaRecord : IProvideType, IProvideHoaInfo
    {
        public const string SummarySection = "HOA";

        public Guid Id { get; set; }

        public Guid SalePropertyId { get; set; }

        public string Name { get; set; }

        public decimal TransferFee { get; set; }

        public decimal Fee { get; set; }

        public BillingFrequency BillingFrequency { get; set; }

        public string Website { get; set; }

        public string ContactPhone { get; set; }

        public EntityType HoaType { get; set; }

        public string FieldType { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysTimestamp { get; set; }

        public Guid CompanyId { get; set; }

        public HoaRecord CloneRecord() => (HoaRecord)this.MemberwiseClone();

        public static HoaRecord CreateHoa(SaleListingHoa hoa)
        {
            if (hoa == null)
            {
                return new();
            }

            return new()
            {
                SalePropertyId = hoa.SalePropertyId,
                Name = hoa.Name,
                TransferFee = hoa.TransferFee,
                Fee = hoa.Fee,
                BillingFrequency = hoa.BillingFrequency,
                Website = hoa.Website,
                ContactPhone = hoa.ContactPhone,
                HoaType = hoa.HoaType,
                FieldType = hoa.FieldType,
                Id = hoa.Id,
                SysCreatedOn = hoa.SysCreatedOn,
                SysModifiedOn = hoa.SysModifiedOn,
                IsDeleted = hoa.IsDeleted,
                SysModifiedBy = hoa.SysModifiedBy,
                SysCreatedBy = hoa.SysCreatedBy,
                SysTimestamp = hoa.SysTimestamp,
                CompanyId = hoa.CompanyId,
            };
        }

        public string CustomString()
        {
            throw new NotImplementedException();
        }
    }
}
