namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums;

    public class TempOffMarketQueryResult
    {
        public Guid Id { get; set; }

        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public MarketStatuses MlsStatus { get; set; }

        public string Address { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public DateTime? BackOnMarketDate { get; set; }
    }
}
