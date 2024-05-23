namespace Husa.Quicklister.Abor.Application.Models.Lot
{
    using System.Collections.Generic;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotPropertyDto
    {
        public MlsArea? MlsArea { get; set; }
        public PropertySubType? PropertyType { get; set; }
        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        public ICollection<LotDescription> LotDescription { get; set; }
        public ICollection<PropCondition> PropCondition { get; set; }
        public virtual PropertySubTypeLots? PropertySubType { get; set; }
        public virtual ICollection<TypeOfHomeAllowed> TypeOfHomeAllowed { get; set; }
        public virtual ICollection<SoilType> SoilType { get; set; }
        public virtual bool SurfaceWater { get; set; }
        public virtual int? NumberOfPonds { get; set; }
        public virtual int? NumberOfWells { get; set; }
        public virtual bool LiveStock { get; set; }
        public virtual bool CommercialAllowed { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LegalDescription { get; set; }
        public string TaxLot { get; set; }
        public string TaxId { get; set; }
        public string TaxBlock { get; set; }
        public string LotDimension { get; set; }
        public string LotSize { get; set; }
        public bool UpdateGeocodes { get; set; }
        public int? AlsoListedAs { get; set; }
        public bool BuilderRestrictions { get; set; }
    }
}
