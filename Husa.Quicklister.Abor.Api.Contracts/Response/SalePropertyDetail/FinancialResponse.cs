namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class FinancialResponse
    {
        public decimal? TaxRate { get; set; }
        public int? TaxYear { get; set; }
        public bool IsMultipleTaxed { get; set; }
        public string TitleCompany { get; set; }
        public ICollection<ProposedTerms> ProposedTerms { get; set; }
        public HoaRequirement? HOARequirement { get; set; }
        public bool HasMultipleHOA { get; set; }
        public int NumHOA { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public DateTime? BonusExpirationDate { get; set; }
        public bool HasBuyerIncentive { get; set; }
    }
}
