namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public interface IProvideLotProperty
    {
        MlsArea? MlsArea { get; set; }
        PropertySubType? PropertyType { get; set; }
        ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        ICollection<LotDescription> LotDescription { get; set; }
    }
}
