namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class ListingResponse
    {
        public Guid Id { get; set; }

        public decimal? ListPrice { get; set; }

        public DateTime? ListDate { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string UnitNumber { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public string StreetNum { get; set; }

        public string StreetName { get; set; }

        public string StreetType { get; set; }

        public Cities City { get; set; }

        public Counties? County { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public string OwnerName { get; set; }

        public MarketCode MarketCode { get; set; }

        public bool IsCompleteHome { get; set; }

        public States State { get; set; }

        public string PlanName { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public ListType ListType { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? LockedBy { get; set; }

        public DateTime? LockedOn { get; set; }

        public LockedStatus LockedStatus { get; set; } = LockedStatus.NoLocked;
        public string LockedByUsername { get; set; }

        public DateTime? LastPhotoRequestCreationDate { get; set; }

        public Guid? LastPhotoRequestId { get; set; }

        public bool IsPhotosDeclined { get; set; }

        public bool IsManuallyManaged { get; set; }

        public Guid? XmlListingId { get; set; }

        public Guid? CommunityId { get; set; }

        public string Directions { get; set; }
    }
}
