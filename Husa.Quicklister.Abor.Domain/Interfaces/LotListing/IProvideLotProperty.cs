namespace Husa.Quicklister.Abor.Domain.Interfaces.LotListing
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public interface IProvideLotProperty : IProvidePropertyCommon, IProvideGeocodes
    {
        MlsArea? MlsArea { get; set; }
        PropertySubType? PropertyType { get; set; }
        ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        ICollection<LotDescription> LotDescription { get; set; }
        ICollection<PropCondition> PropCondition { get; set; }
        PropertySubTypeLots? PropertySubType { get; set; }
        ICollection<TypeOfHomeAllowed> TypeOfHomeAllowed { get; set; }
        ICollection<SoilType> SoilType { get; set; }
        bool SurfaceWater { get; set; }
        int? NumberOfPonds { get; set; }
        int? NumberOfWells { get; set; }
        bool LiveStock { get; set; }
        bool CommercialAllowed { get; set; }
        string TaxBlock { get; set; }
        string LotSize { get; set; }
        bool UpdateGeocodes { get; set; }
        int? AlsoListedAs { get; set; }
        bool BuilderRestrictions { get; set; }
    }
}
