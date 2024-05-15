namespace Husa.Quicklister.Abor.Application.Models
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class ListingStatusFieldsDto
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
        public bool HasContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
    }
}
