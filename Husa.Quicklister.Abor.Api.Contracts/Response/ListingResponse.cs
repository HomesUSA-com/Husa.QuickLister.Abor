namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using ExtensionsContract = Husa.Quicklister.Extensions.Api.Contracts.Response.Listing;

    public class ListingResponse : ExtensionsContract.ListingResponse
    {
        public decimal? ListPrice { get; set; }

        public DateTime? ListDate { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public Cities City { get; set; }

        public Counties? County { get; set; }

        public bool IsCompleteHome { get; set; }

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

        public string LockedByUsername { get; set; }

        public DateTime? LastPhotoRequestCreationDate { get; set; }

        public Guid? LastPhotoRequestId { get; set; }

        public bool IsPhotosDeclined { get; set; }

        public bool IsManuallyManaged { get; set; }

        public Guid? XmlListingId { get; set; }

        public string Directions { get; set; }
    }
}
