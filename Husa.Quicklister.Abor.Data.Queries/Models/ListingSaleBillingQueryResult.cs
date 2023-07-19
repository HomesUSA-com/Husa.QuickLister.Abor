namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSaleBillingQueryResult : BaseQueryResult
    {
        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public DateTime? ListDate { get; set; }

        public string StreetNum { get; set; }

        public string StreetName { get; set; }

        public string Subdivision { get; set; }

        public string ZipCode { get; set; }

        public ActionType? PublishType { get; set; }

        public Guid? PublishUser { get; set; }

        public string PublishUserName { get; set; }

        public MarketStatuses? PublishStatus { get; set; }

        public DateTime? PublishDate { get; set; }

        public decimal? ListFee { get; set; }
    }
}
