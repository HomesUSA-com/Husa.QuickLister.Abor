namespace Husa.Quicklister.Abor.Data.Queries.Models.ResidentialIdx
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class FinancialIdxQueryResult
    {
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public DateTime? BonusExpirationDate { get; set; }
    }
}
