namespace Husa.Quicklister.Abor.Api.Contracts.Response
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSalePublishInfoResponse
    {
        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }
    }
}
