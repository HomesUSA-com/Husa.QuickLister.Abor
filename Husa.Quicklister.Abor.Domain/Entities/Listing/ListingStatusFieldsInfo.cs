namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Agent = Husa.Quicklister.Abor.Domain.Entities.Agent.Agent;

    public class ListingStatusFieldsInfo : ValueObject, IProvideStatusFields
    {
        private bool? hasBuyerAgent;

        public DateTime? CancelDate { get; set; }

        public string CancelledReason { get; set; }

        public decimal? ClosePrice { get; set; }

        public DateTime? EstimatedClosedDate { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent
        {
            get => this.hasBuyerAgent ?? true;
            set => this.hasBuyerAgent = value;
        }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? PendingDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public void SetStatusChangeAgent(Agent agent)
        {
            if (agent is null)
            {
                return;
            }

            this.AgentId = agent.Id;
        }

        public virtual void SetSold(decimal listPrice, decimal? closePrice, DateTime? closeDate)
        {
            this.ClosePrice = closePrice ?? listPrice;
            this.ClosedDate = closeDate ?? DateTime.UtcNow;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.CancelDate;
            yield return this.ClosePrice;
            yield return this.EstimatedClosedDate;
            yield return this.HasBuyerAgent;
            yield return this.AgentId;
            yield return this.BackOnMarketDate;
            yield return this.PendingDate;
            yield return this.ClosedDate;
            yield return this.OffMarketDate;
        }
    }
}
