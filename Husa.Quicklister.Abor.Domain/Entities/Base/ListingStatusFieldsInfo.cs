namespace Husa.Quicklister.Abor.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Attributes;
    using Agent = Husa.Quicklister.Abor.Domain.Entities.Agent.Agent;

    public class ListingStatusFieldsInfo : ValueObject, IProvideStatusFields
    {
        private bool? hasBuyerAgent;

        public string CancelledReason { get; set; }

        [XmlPropertyUpdate]
        public decimal? ClosePrice { get; set; }

        [XmlPropertyUpdate]
        public DateTime? EstimatedClosedDate { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent
        {
            get => this.hasBuyerAgent ?? true;
            set => this.hasBuyerAgent = value;
        }

        public DateTime? PendingDate { get; set; }

        [XmlPropertyUpdate]
        public DateTime? ClosedDate { get; set; }

        public bool HasSecondBuyerAgent { get; set; }

        public Guid? AgentIdSecond { get; set; }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public bool HasContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }

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
            yield return this.CancelledReason;
            yield return this.ClosePrice;
            yield return this.EstimatedClosedDate;
            yield return this.AgentId;
            yield return this.HasBuyerAgent;
            yield return this.PendingDate;
            yield return this.ClosedDate;
            yield return this.HasSecondBuyerAgent;
            yield return this.AgentIdSecond;
            yield return this.BackOnMarketDate;
            yield return this.OffMarketDate;
            yield return this.HasContingencyInfo;
            yield return this.SaleTerms;
            yield return this.PendingDate;
            yield return this.SellConcess;
        }
    }
}
