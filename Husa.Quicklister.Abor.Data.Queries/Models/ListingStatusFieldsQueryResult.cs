namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;
    using Husa.Quicklister.Abor.Domain.Interfaces;

    public class ListingStatusFieldsQueryResult : IProvideStatusFields
    {
        public DateTime? PendingDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? EstimatedClosedDate { get; set; }

        public string CancelledReason { get; set; }

        public decimal? ClosePrice { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent { get; set; }

        public bool HasSecondBuyerAgent { get; set; }

        public Guid? AgentIdSecond { get; set; }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public string AgentMarketUniqueId { get; set; }

        public string SecondAgentMarketUniqueId { get; set; }

        public bool HasContingencyInfo { get; set; }
    }
}
