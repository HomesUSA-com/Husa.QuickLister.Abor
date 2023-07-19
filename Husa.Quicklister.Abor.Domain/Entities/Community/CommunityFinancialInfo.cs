namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class CommunityFinancialInfo : ValueObject, IProvideFinancial, IProvideAgentCommission
    {
        public CommunityFinancialInfo()
        {
            this.BuyersAgentCommissionType = CommissionType.Percent;
        }

        public bool IsMultipleTaxed { get; set; }

        public decimal? TaxRate { get; set; }

        public string TitleCompany { get; set; }

        public ICollection<ProposedTerms> ProposedTerms { get; set; }

        public HoaRequirement? HOARequirement { get; set; }

        public decimal? BuyersAgentCommission { get; set; }

        public virtual CommissionType BuyersAgentCommissionType { get; set; }

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
            clonedFinancial.IsMultipleTaxed = info.IsMultipleTaxed;
            clonedFinancial.TaxRate = info.TaxRate;
            clonedFinancial.TitleCompany = info.TitleCompany;
            clonedFinancial.ProposedTerms = info.ProposedTerms;
            clonedFinancial.HOARequirement = info.HOARequirement;
            clonedFinancial.BuyersAgentCommission = info.BuyersAgentCommission;
            clonedFinancial.BuyersAgentCommissionType = info.BuyersAgentCommissionType;

            return clonedFinancial;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.IsMultipleTaxed;
            yield return this.TaxRate;
            yield return this.TitleCompany;
            yield return this.ProposedTerms;
            yield return this.HOARequirement;
            yield return this.BuyersAgentCommission;
            yield return this.BuyersAgentCommissionType;
        }
    }
}
