namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;
    using Husa.Extensions.Common.Enums;

    public class CommunityMissingInfoQueryResult
    {
        public Guid Id { get; set; }

        public MarketCode MarketCode { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public string Obtained { get; set; }

        public string MissingField { get; set; }
    }
}
