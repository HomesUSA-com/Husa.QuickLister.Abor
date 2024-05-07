namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public class LotFinancialInfo : ValueObject, IProvideLotFinancial
    {
        private DateTime? bonusExpirationDate;

        public decimal? TaxRate { get; set; }
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }
        public bool HasHoa { get; set; }
        public HoaRequirement? HOARequirement { get; set; }
        public BillingFrequency? BillingFrequency { get; set; }
        public ICollection<HoaIncludes> HoaIncludes { get; set; }
        public bool HasBuyerIncentive { get; set; }
        public decimal? BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public DateTime? BonusExpirationDate
        {
            get { return this.bonusExpirationDate; }
            set { this.bonusExpirationDate = value?.ToUniversalTime(); }
        }

        public virtual string ReadableBuyersAgentCommission => this.BuyersAgentCommissionType == CommissionType.Percent ?
                $"{(int)this.BuyersAgentCommission}%" :
                $"${this.BuyersAgentCommission}";

        public string HoaName { get; set; }
        public decimal? HoaFee { get; set; }
        public int? EstimatedTax { get; set; }
        public int? TaxYear { get; set; }
        public ICollection<TaxExemptions> TaxExemptions { get; set; }
        public int? TaxAssesedValue { get; set; }
        public string PreferredTitleCompany { get; set; }
        public LandTitleEvidence? LandTitleEvidence { get; set; }

        public LotFinancialInfo Clone()
        {
            return (LotFinancialInfo)this.MemberwiseClone();
        }

        public LotFinancialInfo ImportFinancialFromCommunity(CommunityFinancialInfo financial)
        {
            var clonedFinancial = this.Clone();
            clonedFinancial.TaxRate = financial.TaxRate;
            clonedFinancial.AcceptableFinancing = financial.AcceptableFinancing;
            clonedFinancial.HoaIncludes = financial.HoaIncludes;
            clonedFinancial.HasHoa = financial.HasHoa;
            clonedFinancial.BillingFrequency = financial.BillingFrequency;
            clonedFinancial.HOARequirement = financial.HOARequirement;
            clonedFinancial.BuyersAgentCommission = financial.BuyersAgentCommission;
            clonedFinancial.BuyersAgentCommissionType = financial.BuyersAgentCommissionType;
            clonedFinancial.HasAgentBonus = financial.HasAgentBonus;
            clonedFinancial.HasBonusWithAmount = financial.HasBonusWithAmount;
            clonedFinancial.AgentBonusAmount = financial.AgentBonusAmount;
            clonedFinancial.AgentBonusAmountType = financial.AgentBonusAmountType;
            clonedFinancial.BonusExpirationDate = financial.BonusExpirationDate;
            clonedFinancial.HasBuyerIncentive = financial.HasBuyerIncentive;
            return clonedFinancial;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.TaxRate;
            yield return this.AcceptableFinancing;
            yield return this.HoaIncludes;
            yield return this.HasHoa;
            yield return this.BillingFrequency;
            yield return this.HOARequirement;
            yield return this.BuyersAgentCommission;
            yield return this.BuyersAgentCommissionType;
            yield return this.HasAgentBonus;
            yield return this.HasBonusWithAmount;
            yield return this.AgentBonusAmount;
            yield return this.AgentBonusAmountType;
            yield return this.BonusExpirationDate;
            yield return this.HasBuyerIncentive;
            yield return this.HoaFee;
            yield return this.HoaName;
            yield return this.EstimatedTax;
            yield return this.TaxYear;
            yield return this.TaxExemptions;
            yield return this.TaxAssesedValue;
            yield return this.PreferredTitleCompany;
            yield return this.LandTitleEvidence;
        }
    }
}
