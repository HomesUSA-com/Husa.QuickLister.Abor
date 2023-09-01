namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;

    public interface IProvideStatusFields
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
    }
}
