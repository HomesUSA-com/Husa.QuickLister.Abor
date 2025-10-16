namespace Husa.Quicklister.Abor.Data.Documents.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces.QueryResults;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public class ListingRequestQueryResult : IProvideQuicklisterUserInfo, IListingRequestQueryResult
    {
        public Guid Id { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public string ModifiedBy { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedByEmail { get; set; }

        public string LockedByUsername { get; set; }

        public Guid? LockedBy { get; set; }

        public string OwnerName { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Market { get; set; }

        public Cities City { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public string Address { get; set; }

        public decimal? ListPrice { get; set; }

        public string UnitNumber { get; set; }

        public StreetType? StreetType { get; set; }

        public bool UpdateGeocodes { get; set; }

        public bool UseShowingTime { get; set; }

        public MediaVerificationStatus? MediaVerificationStatus { get; set; }
        public RequestUploadStatus UploadStatus { get; set; }
    }
}
