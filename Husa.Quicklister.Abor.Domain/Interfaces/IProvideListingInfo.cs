namespace Husa.Quicklister.Abor.Domain.Interfaces
{
    using Husa.Quicklister.Abor.Domain.Enums;

    public interface IProvideListingInfo
    {
        MarketStatuses MlsStatus { get; set; }
        decimal? ListPrice { get; set; }
        string MlsNumber { get; set; }
    }
}
