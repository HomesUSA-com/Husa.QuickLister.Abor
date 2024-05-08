namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideFinancial : IProvideLotFinancial
    {
        string TitleCompany { get; set; }

        ICollection<TaxExemptions> TaxExemptions { get; set; }

        string HoaName { get; set; }

        decimal? HoaFee { get; set; }
    }
}
