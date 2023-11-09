namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public class FinancialInfo : ValueObject, IProvideFinancial, IProvideAgentCommission, IProvideAgentBonusCommission
    {
        private DateTime? bonusExpirationDate;

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

        public DateTime? BonusExpirationDate
        {
            get { return this.bonusExpirationDate; }
            set { this.bonusExpirationDate = value?.ToUniversalTime(); }
        }

        public bool HasBuyerIncentive { get; set; }

        public virtual string ReadableBuyersAgentCommission => this.BuyersAgentCommissionType == CommissionType.Percent ?
                $"{(int)this.BuyersAgentCommission}%" :
                $"${this.BuyersAgentCommission}";

        public FinancialInfo Clone()
        {
            return (FinancialInfo)this.MemberwiseClone();
        }

        public FinancialInfo ImportFinancialFromCommunity(CommunityFinancialInfo financial)
        {
            var clonedFinancial = this.Clone();
            clonedFinancial.TaxRate = financial.TaxRate;
            clonedFinancial.TitleCompany = financial.TitleCompany;
            clonedFinancial.AcceptableFinancing = financial.AcceptableFinancing;
            clonedFinancial.TaxExemptions = financial.TaxExemptions;
            clonedFinancial.HoaIncludes = financial.HoaIncludes;
            clonedFinancial.HasHoa = financial.HasHoa;
            clonedFinancial.HoaName = financial.HoaName;
            clonedFinancial.HoaFee = financial.HoaFee;
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
            yield return this.TaxYear;
            yield return this.TitleCompany;
            yield return this.AcceptableFinancing;
            yield return this.TaxExemptions;
            yield return this.HoaIncludes;
            yield return this.HasHoa;
            yield return this.HoaName;
            yield return this.HoaFee;
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
        }
    }
}
