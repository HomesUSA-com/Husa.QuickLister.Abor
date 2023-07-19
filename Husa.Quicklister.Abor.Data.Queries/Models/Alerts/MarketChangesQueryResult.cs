namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;
    using Husa.Extensions.Common.Enums;

    public class MarketChangesQueryResult
    {
        public Guid Id { get; set; }

        public MarketCode MarketCode { get; set; }

        public string MlsNumber { get; set; }

        public string Address { get; set; }

        public string Subdivision { get; set; }

        public string OwnerName { get; set; }

        public string OldPrice { get; set; }

        public string NewPrice { get; set; }

        public string OldStatus { get; set; }

        public string NewStatus { get; set; }

        public DateTime? MarketModifiedOn { get; set; }
    }
}
