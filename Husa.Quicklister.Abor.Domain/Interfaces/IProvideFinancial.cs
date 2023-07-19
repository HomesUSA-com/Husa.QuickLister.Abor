namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFinancial
    {
        bool IsMultipleTaxed { get; set; }

        decimal? TaxRate { get; set; }

        string TitleCompany { get; set; }

        ICollection<ProposedTerms> ProposedTerms { get; set; }

        HoaRequirement? HOARequirement { get; set; }

        decimal? BuyersAgentCommission { get; set; }

        CommissionType BuyersAgentCommissionType { get; set; }
    }
}
