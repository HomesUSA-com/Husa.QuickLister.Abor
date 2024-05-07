namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotFinancial : IProvideCommonFinancial
    {
        int? EstimatedTax { get; set; }
        int? TaxYear { get; set; }
        ICollection<TaxExemptions> TaxExemptions { get; set; }
        int? TaxAssesedValue { get; set; }
        LandTitleEvidence? LandTitleEvidence { get; set; }
        string PreferredTitleCompany { get; set; }
    }
}
