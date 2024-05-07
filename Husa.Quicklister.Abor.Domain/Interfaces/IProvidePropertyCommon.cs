namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    public interface IProvidePropertyCommon
    {
        string LegalDescription { get; set; }
        string TaxLot { get; set; }
        string TaxId { get; set; }
        string LotDimension { get; set; }
    }
}
