namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Xml.Api.Contracts.Response;

    public class CommunityFinancialInfo : ValueObject, IProvideFinancial, IProvideAgentCommission, IProvideAgentBonusCommission
    {
        public CommunityFinancialInfo()
        {
            this.BuyersAgentCommissionType = CommissionType.Percent;
        }

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
        public bool HasBuyerIncentive { get; set; }

        public virtual string ReadableBuyersAgentCommission => this.BuyersAgentCommissionType == CommissionType.Percent ?
                $"{this.BuyersAgentCommission}%" :
                $"${(int)this.BuyersAgentCommission}";

        public static CommunityFinancialInfo ImportFromXml(SubdivisionResponse subdivision, CommunityFinancialInfo financialInfo)
        {
            var importedFinancialInfo = new CommunityFinancialInfo();
            if (financialInfo != null)
            {
                importedFinancialInfo = financialInfo.Clone();
            }

            importedFinancialInfo.TaxRate = subdivision.TotalTaxRate;

            return importedFinancialInfo;
        }

        public virtual CommunityFinancialInfo UpdateFromXml(SubdivisionResponse subdivision)
        {
            var clonnedFinancial = this.Clone();
            if (subdivision.TotalTaxRate.HasValue)
            {
                clonnedFinancial.TaxRate = subdivision.TotalTaxRate;
            }

            return clonnedFinancial;
        }

        public CommunityFinancialInfo Clone()
        {
            return (CommunityFinancialInfo)this.MemberwiseClone();
        }

        public virtual CommunityFinancialInfo ImportFinancial(FinancialInfo info)
        {
            var clonedFinancial = this.Clone();
            clonedFinancial.TaxRate = info.TaxRate;
            clonedFinancial.TitleCompany = info.TitleCompany;
            clonedFinancial.AcceptableFinancing = info.AcceptableFinancing;
            clonedFinancial.TaxExemptions = info.TaxExemptions;
            clonedFinancial.HoaIncludes = info.HoaIncludes;
            clonedFinancial.HasHoa = info.HasHoa;
            clonedFinancial.HoaName = info.HoaName;
            clonedFinancial.HoaFee = info.HoaFee;
            clonedFinancial.BillingFrequency = info.BillingFrequency;
            clonedFinancial.HOARequirement = info.HOARequirement;
            clonedFinancial.BuyersAgentCommission = info.BuyersAgentCommission;
            clonedFinancial.BuyersAgentCommissionType = info.BuyersAgentCommissionType;
            clonedFinancial.HasAgentBonus = info.HasAgentBonus;
            clonedFinancial.HasBonusWithAmount = info.HasBonusWithAmount;
            clonedFinancial.AgentBonusAmount = info.AgentBonusAmount;
            clonedFinancial.AgentBonusAmountType = info.AgentBonusAmountType;
            clonedFinancial.BonusExpirationDate = info.BonusExpirationDate;
            clonedFinancial.HasBuyerIncentive = info.HasBuyerIncentive;

            return clonedFinancial;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.TaxRate;
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
