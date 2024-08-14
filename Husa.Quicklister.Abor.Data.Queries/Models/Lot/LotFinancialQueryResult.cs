namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class LotFinancialQueryResult
    {
        public decimal? TaxRate { get; set; }
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }
        public bool HasHoa { get; set; }
        public HoaRequirement? HOARequirement { get; set; }
        public BillingFrequency? BillingFrequency { get; set; }
        public ICollection<HoaIncludes> HoaIncludes { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public DateTime? BonusExpirationDate { get; set; }
        public string HoaName { get; set; }
        public decimal? HoaFee { get; set; }
        public decimal? EstimatedTax { get; set; }
        public int? TaxYear { get; set; }
        public ICollection<TaxExemptions> TaxExemptions { get; set; }
        public int? TaxAssesedValue { get; set; }
        public string PreferredTitleCompany { get; set; }
        public LandTitleEvidence? LandTitleEvidence { get; set; }
    }
}
