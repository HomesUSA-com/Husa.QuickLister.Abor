namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class HoaQueryResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal TransferFee { get; set; }

        public decimal Fee { get; set; }

        public BillingFrequency BillingFrequency { get; set; }

        public string Website { get; set; }

        public string ContactPhone { get; set; }

        public bool IsDeleted { get; set; }

        public EntityType HoaType { get; set; }
    }
}
