namespace Husa.Quicklister.Abor.Data.Queries.Models.Lot
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotPropertyQueryResult
    {
        public MlsArea? MlsArea { get; set; }
        public PropertySubType? PropertyType { get; set; }
        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        public ICollection<LotDescription> LotDescription { get; set; }
        public ICollection<PropCondition> PropCondition { get; set; }
        public PropertySubTypeLots? PropertySubType { get; set; }
        public ICollection<TypeOfHomeAllowed> TypeOfHomeAllowed { get; set; }
        public ICollection<SoilType> SoilType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LegalDescription { get; set; }
        public string TaxLot { get; set; }
        public string TaxId { get; set; }
        public string TaxBlock { get; set; }
        public string LotDimension { get; set; }
        public string LotSize { get; set; }
        public bool SurfaceWater { get; set; }
        public int? NumberOfPonds { get; set; }
        public int? NumberOfWells { get; set; }
        public bool LiveStock { get; set; }
        public bool CommercialAllowed { get; set; }
        public bool UpdateGeocodes { get; set; }
        public int? AlsoListedAs { get; set; }
        public bool BuilderRestrictions { get; set; }
    }
}
