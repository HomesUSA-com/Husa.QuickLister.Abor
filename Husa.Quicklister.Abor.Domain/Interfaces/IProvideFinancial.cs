namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFinancial : IProvideCommonFinancial
    {
        string TitleCompany { get; set; }
        ICollection<TaxExemptions> TaxExemptions { get; set; }
    }
}
