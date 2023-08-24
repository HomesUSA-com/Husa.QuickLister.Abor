namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IProvideStatusFields
    {
        public DateTime? PendingDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? EstimatedClosedDate { get; set; }

        public DateTime? CancelDate { get; set; }

        public CancelledOptions? CancelledOption { get; set; }

        public string CancelledReason { get; set; }

        public decimal? ClosePrice { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent { get; set; }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? OffMarketDate { get; set; }
    }
}
