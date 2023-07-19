namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts.Listing
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class ListingSaleQueryResult : BaseQueryResult
    {
        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Address { get; set; }

        public string Subdivision { get; set; }

        public DateTime? ConstructionCompletionDate { get; set; }

        public DateTime? BonusExpirationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string PublicRemarks { get; set; }

        public int? DOM { get; set; }

        public ListingSaleStatusFieldQueryResult StatusFieldsInfo { get; set; }
    }
}
