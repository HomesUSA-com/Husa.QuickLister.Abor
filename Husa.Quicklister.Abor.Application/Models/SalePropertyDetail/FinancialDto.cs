namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class FinancialDto
    {
        public int? TaxYear { get; set; }
        public decimal? TaxRate { get; set; }
        public string TitleCompany { get; set; }
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }
        public ICollection<TaxExemptions> TaxExemptions { get; set; }
        public ICollection<HoaIncludes> HoaIncludes { get; set; }
        public bool HasHoa { get; set; }
        public string HoaName { get; set; }
        public decimal? HoaFee { get; set; }
        public BillingFrequency? BillingFrequency { get; set; }
        public HoaRequirement? HOARequirement { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public DateTime? BonusExpirationDate { get; set; }
    }
}
