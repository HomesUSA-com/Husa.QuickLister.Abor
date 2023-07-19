namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSalePublishInfoQueryResult
    {
        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}
