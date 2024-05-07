namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotAddress : IProvideAddress
    {
        Counties? County { get; set; }

        StreetType? StreetType { get; set; }

        string Subdivision { get; set; }

        States State { get; set; }

        StreetDirPrefix? StreetDirPrefix { get; set; }

        StreetDirPrefix? StreetDirSuffix { get; set; }

        string UnitNumber { get; set; }
    }
}
