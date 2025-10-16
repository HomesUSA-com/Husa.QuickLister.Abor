namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces.Listings;

    public interface IProvideStatusFields : IProvideClosedFields, IProvidePendingDate
    {
        public string CancelledReason { get; set; }

        public Guid? AgentId { get; set; }

        public bool HasBuyerAgent { get; set; }

        public bool HasSecondBuyerAgent { get; set; }

        public Guid? AgentIdSecond { get; set; }

        public DateTime? BackOnMarketDate { get; set; }

        public DateTime? OffMarketDate { get; set; }

        public bool HasContingencyInfo { get; set; }

        public ICollection<SaleTerms> SaleTerms { get; set; }

        public string SellConcess { get; set; }
        public ICollection<ContingencyInfo> ContingencyInfo { get; set; }
    }
}
