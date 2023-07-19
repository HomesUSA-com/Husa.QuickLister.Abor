namespace Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingRequestPublishInfoQueryResult
    {
        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}
