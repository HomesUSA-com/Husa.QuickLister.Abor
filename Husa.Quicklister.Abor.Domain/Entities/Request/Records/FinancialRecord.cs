namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record FinancialRecord : IProvideSummary
    {
        private string readableAgentBonusAmount;
        private string readableBuyersAgentCommission;

        public const string SummarySection = "Financial";
        public int TaxYear { get; set; }
        public decimal TaxRate { get; set; }
        public string TitleCompany { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<TaxExemptions> TaxExemptions { get; set; }
        public ICollection<HoaIncludes> HoaIncludes { get; set; }
        public bool HasHoa { get; set; }
        public string HoaName { get; set; }
        public decimal? HoaFee { get; set; }
        public BillingFrequency? BillingFrequency { get; set; }
        public HoaRequirement HOARequirement { get; set; }
        public decimal BuyersAgentCommission { get; set; }
        public CommissionType BuyersAgentCommissionType { get; set; }
        public string ReadableBuyersAgentCommission
        {
            get { return this.readableBuyersAgentCommission ?? this.BuyersAgentCommission.GetCommissionAmount(this.BuyersAgentCommissionType); }
            set { this.readableBuyersAgentCommission = value; }
        }

        public bool HasAgentBonus { get; set; }
        public bool HasBonusWithAmount { get; set; }
        public decimal? AgentBonusAmount { get; set; }
        public CommissionType AgentBonusAmountType { get; set; }
        public string ReadableAgentBonusAmount
        {
            get { return this.readableAgentBonusAmount ?? this.AgentBonusAmount.GetCommissionAmount(this.AgentBonusAmountType); }
            set { this.readableAgentBonusAmount = value; }
        }

        public DateTime? BonusExpirationDate { get; set; }
        public bool HasBuyerIncentive { get; set; }

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
                HOARequirement = financialInfo.HOARequirement ?? throw new DomainException(nameof(financialInfo.HOARequirement)),
                BuyersAgentCommission = financialInfo.BuyersAgentCommission ?? throw new DomainException(nameof(financialInfo.BuyersAgentCommission)),
                BuyersAgentCommissionType = financialInfo.BuyersAgentCommissionType,
                TaxYear = financialInfo.TaxYear ?? throw new DomainException(nameof(financialInfo.TaxYear)),
                TaxRate = financialInfo.TaxRate ?? throw new DomainException(nameof(financialInfo.TaxRate)),
                TitleCompany = financialInfo.TitleCompany,
                AcceptableFinancing = financialInfo.AcceptableFinancing,
                TaxExemptions = financialInfo.TaxExemptions,
                HoaIncludes = financialInfo.HoaIncludes,
                HasHoa = financialInfo.HasHoa,
                HoaName = financialInfo.HoaName,
                HoaFee = financialInfo.HoaFee,
                BillingFrequency = financialInfo.BillingFrequency,
                HasAgentBonus = financialInfo.HasAgentBonus,
                HasBonusWithAmount = financialInfo.HasBonusWithAmount,
                AgentBonusAmount = financialInfo.AgentBonusAmount,
                AgentBonusAmountType = financialInfo.AgentBonusAmountType,
                BonusExpirationDate = financialInfo.BonusExpirationDate,
                HasBuyerIncentive = financialInfo.HasBuyerIncentive,
                ReadableAgentBonusAmount = financialInfo.AgentBonusAmount.GetCommissionAmount(financialInfo.AgentBonusAmountType),
                ReadableBuyersAgentCommission = financialInfo.BuyersAgentCommission.GetCommissionAmount(financialInfo.BuyersAgentCommissionType),
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
