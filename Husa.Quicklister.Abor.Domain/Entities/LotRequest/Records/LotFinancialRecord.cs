namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Common.Validations;
    using Husa.Extensions.Domain.Validations;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Extensions;

    public record LotFinancialRecord : IProvideLotFinancial
    {
        private string readableAgentBonusAmount;
        private string readableBuyersAgentCommission;

        [Required]
        public decimal? TaxRate { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }

        public bool HasHoa { get; set; }
        public HoaRequirement? HOARequirement { get; set; }

        [IfRequired(nameof(HOARequirement), HoaRequirement.Mandatory, OperatorType.Equal)]
        public ICollection<HoaIncludes> HoaIncludes { get; set; }

        [IfRequired(nameof(HOARequirement), HoaRequirement.Mandatory, OperatorType.Equal)]
        public string HoaName { get; set; }

        [IfRequired(nameof(HOARequirement), HoaRequirement.Mandatory, OperatorType.Equal)]
        public BillingFrequency? BillingFrequency { get; set; }

        [Required]
        public decimal? BuyersAgentCommission { get; set; }
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

        [TodayOrAfter]
        public DateTime? BonusExpirationDate { get; set; }
        public bool HasBuyerIncentive { get; set; }

        [Required]
        public decimal? EstimatedTax { get; set; }

        [Required]
        public int? TaxYear { get; set; }

        [Required]
        public ICollection<TaxExemptions> TaxExemptions { get; set; }

        [Required]
        public int? TaxAssesedValue { get; set; }

        public LandTitleEvidence? LandTitleEvidence { get; set; }

        public string PreferredTitleCompany { get; set; }

        public decimal? HoaFee { get; set; }

        public LotFinancialRecord CloneRecord() => (LotFinancialRecord)this.MemberwiseClone();
        public static LotFinancialRecord CreateRecord(LotFinancialInfo financialInfo)
        {
            if (financialInfo == null)
            {
                return new();
            }

            if (!financialInfo.IsValidBuyersAgentCommissionRange())
            {
                throw new DomainException($"The range for Buyers Agent Commission is invalid for type {financialInfo.BuyersAgentCommissionType}");
            }

            if (!financialInfo.IsValidHoa())
            {
                throw new DomainException(nameof(financialInfo.HOARequirement));
            }

            return new()
            {
                HOARequirement = financialInfo.HOARequirement,
                BuyersAgentCommission = financialInfo.BuyersAgentCommission,
                BuyersAgentCommissionType = financialInfo.BuyersAgentCommissionType,
                TaxRate = financialInfo.TaxRate,
                AcceptableFinancing = financialInfo.AcceptableFinancing,
                HoaIncludes = financialInfo.HoaIncludes,
                HasHoa = financialInfo.HasHoa,
                BillingFrequency = financialInfo.BillingFrequency,
                HasAgentBonus = financialInfo.HasAgentBonus,
                HasBonusWithAmount = financialInfo.HasBonusWithAmount,
                AgentBonusAmount = financialInfo.AgentBonusAmount,
                AgentBonusAmountType = financialInfo.AgentBonusAmountType,
                BonusExpirationDate = financialInfo.BonusExpirationDate,
                HasBuyerIncentive = financialInfo.HasBuyerIncentive,
                ReadableAgentBonusAmount = financialInfo.AgentBonusAmount.GetCommissionAmount(financialInfo.AgentBonusAmountType),
                ReadableBuyersAgentCommission = financialInfo.BuyersAgentCommission.GetCommissionAmount(financialInfo.BuyersAgentCommissionType),
                EstimatedTax = financialInfo.EstimatedTax,
                HoaFee = financialInfo.HoaFee,
                HoaName = financialInfo.HoaName,
                LandTitleEvidence = financialInfo.LandTitleEvidence,
                PreferredTitleCompany = financialInfo.PreferredTitleCompany,
                TaxAssesedValue = financialInfo.TaxAssesedValue,
                TaxExemptions = financialInfo.TaxExemptions,
                TaxYear = financialInfo.TaxYear,
            };
        }
    }
}
