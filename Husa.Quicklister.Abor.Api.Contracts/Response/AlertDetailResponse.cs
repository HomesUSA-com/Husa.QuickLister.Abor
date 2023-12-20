namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using AlertExtension = Husa.Quicklister.Extensions.Api.Contracts.Response.Alert.AlertDetailResponse;

    public class AlertDetailResponse : AlertExtension
    {
        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? BonusExpirationDate { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public int? DOM { get; set; }

        public string PublicRemarks { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public string CreatedBy { get; set; }

        public Guid? LockedBy { get; set; }

        public string Obtained { get; set; }

        public string MissingField { get; set; }

        public string OldPrice { get; set; }

        public string NewPrice { get; set; }

        public string OldStatus { get; set; }

        public string NewStatus { get; set; }

        public DateTime? MarketModifiedOn { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CommunityId { get; set; }

        public string CommunityName { get; set; }

        public Guid ListingId { get; set; }

        public Guid PhotoRequestId { get; set; }

        public DateTime? AssignedOn { get; set; }

        public string AssignedTo { get; set; }

        public DateTime? ContactDate { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}
