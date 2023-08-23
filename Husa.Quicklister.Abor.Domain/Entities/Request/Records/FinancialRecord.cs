namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Common.Validations;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record FinancialRecord : IProvideSummary
    {
        private CommissionType agentBonusAmountType;

        public const string SummarySection = "Financial";
        public int? TaxYear { get; set; }
        public bool HasMultipleHOA { get; set; }

        [IfRequired(nameof(HasMultipleHOA), true, OperatorType.Equal)]
        public int NumHOA { get; set; }
        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType
        {
            get
            {
                if (!this.HasBonusWithAmount)
                {
                    return CommissionType.Percent;
                }

                return this.agentBonusAmountType;
            }

            set
            {
                this.agentBonusAmountType = value;
            }
        }

        public DateTime? BonusExpirationDate { get; set; }
        public bool HasBuyerIncentive { get; set; }
        public bool IsMultipleTaxed { get; set; }

        [Required]
        public decimal? TaxRate { get; set; }

        [Required]
        public string TitleCompany { get; set; }

        [Required]
        public ICollection<ProposedTerms> ProposedTerms { get; set; }

        [Required]
        public HoaRequirement HOARequirement { get; set; }

        public decimal BuyersAgentCommission { get; set; }

        public CommissionType BuyersAgentCommissionType { get; set; }

        public FinancialRecord CloneRecord() => (FinancialRecord)this.MemberwiseClone();
        public static FinancialRecord CreateRecord(FinancialInfo financialInfo)
        {
            if (financialInfo == null)
            {
                return new();
            }

            if (!financialInfo.IsValidBuyersAgentCommissionRange())
            {
                throw new DomainException($"The range for Buyers Agent Commission is invalid for type {financialInfo.BuyersAgentCommissionType}");
            }

            return new()
            {
                TaxYear = financialInfo.TaxYear,
                HasAgentBonus = financialInfo.HasAgentBonus,
                HasBonusWithAmount = financialInfo.HasBonusWithAmount,
                AgentBonusAmount = financialInfo.AgentBonusAmount,
                AgentBonusAmountType = financialInfo.AgentBonusAmountType,
                BonusExpirationDate = financialInfo.BonusExpirationDate,
                HasBuyerIncentive = financialInfo.HasBuyerIncentive,
                TaxRate = financialInfo.TaxRate,
                TitleCompany = financialInfo.TitleCompany,
                HOARequirement = financialInfo.HOARequirement ?? throw new DomainException(nameof(financialInfo.HOARequirement)),
                BuyersAgentCommission = financialInfo.BuyersAgentCommission ?? throw new DomainException(nameof(financialInfo.BuyersAgentCommission)),
                BuyersAgentCommissionType = financialInfo.BuyersAgentCommissionType,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
