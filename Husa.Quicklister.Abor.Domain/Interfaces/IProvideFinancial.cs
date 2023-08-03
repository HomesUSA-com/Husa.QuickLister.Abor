namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFinancial
    {
        decimal? TaxRate { get; set; }

        string TitleCompany { get; set; }

        ICollection<AcceptableFinancing> AcceptableFinancing { get; set; }

        ICollection<TaxExemptions> TaxExemptions { get; set; }

        ICollection<HoaIncludes> HoaIncludes { get; set; }

        bool HasHoa { get; set; }

        string HoaName { get; set; }

        decimal? HoaFee { get; set; }

        BillingFrequency? BillingFrequency { get; set; }

        HoaRequirement? HOARequirement { get; set; }

        decimal? BuyersAgentCommission { get; set; }

        CommissionType BuyersAgentCommissionType { get; set; }

        bool HasAgentBonus { get; set; }

        bool HasBonusWithAmount { get; set; }

        decimal? AgentBonusAmount { get; set; }

        CommissionType AgentBonusAmountType { get; set; }

        DateTime? BonusExpirationDate { get; set; }

        bool HasBuyerIncentive { get; set; }
    }
}
