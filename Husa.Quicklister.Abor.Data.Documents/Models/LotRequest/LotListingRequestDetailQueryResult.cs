namespace Husa.Quicklister.Abor.Data.Documents.Models.LotRequest
{
    using System;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class LotListingRequestDetailQueryResult
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public string OwnerName { get; set; }
        public Guid? CommunityId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ListDate { get; set; }
        public decimal? ListPrice { get; set; }
        public ListingRequestState RequestState { get; set; }
        public string MlsNumber { get; set; }
        public MarketStatuses MlsStatus { get; set; }
        public DateTime SysCreatedOn { get; set; }
        public Guid? SysCreatedBy { get; set; }
        public DateTime? SysModifiedOn { get; set; }
        public Guid? SysModifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? LockedOn { get; set; }
        public Guid? LockedBy { get; set; }
        public LockedStatus LockedStatus { get; set; }
        public string LockedByUsername { get; set; }
        public bool IsFirstRequest { get; set; }

        public AddressQueryResult AddressInfo { get; set; }
        public SchoolsQueryResult SchoolsInfo { get; set; }
        public LotPropertyQueryResult PropertyInfo { get; set; }
        public LotFeaturesQueryResult FeaturesInfo { get; set; }
        public LotFinancialQueryResult FinancialInfo { get; set; }
        public LotShowingQueryResult ShowingInfo { get; set; }
        public PublishInfoQueryResult PublishInfo { get; set; }
        public ListingStatusFieldsQueryResult StatusFieldsInfo { get; set; }
    }
}
