namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingQueryResult : BaseQueryResult
    {
        public decimal? ListPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string UnitNumber { get; set; }
        public DateTime? ListDate { get; set; }

        public ListType ListType { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public DateTime? LockedOn { get; set; }

        public LockedStatus LockedStatus { get; set; } = LockedStatus.NoLocked;

        public DateTime? LastPhotoRequestCreationDate { get; set; }

        public Guid? LastPhotoRequestId { get; set; }

        public bool IsPhotosDeclined { get; set; }

        public bool IsManuallyManaged { get; set; }

        public Guid? XmlListingId { get; set; }

        public Guid? XmlDiscrepancyListingId { get; set; }

        public Guid? CompanyId { get; set; }
    }
}
