namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingDto
    {
        public Guid Id { get; set; }

        public decimal? ListPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? ListDate { get; set; }

        public ListType ListType { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public Guid? LockedBy { get; set; }

        public DateTime? LockedOn { get; set; }

        public LockedStatus LockedStatus { get; set; }

        public bool IsManuallyManaged { get; set; }
    }
}
