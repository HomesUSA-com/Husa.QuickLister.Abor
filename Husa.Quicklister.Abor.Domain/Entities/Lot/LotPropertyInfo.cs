namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public class LotPropertyInfo : ValueObject, IProvideLotProperty
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

        public LotPropertyInfo Clone()
        {
            return (LotPropertyInfo)this.MemberwiseClone();
        }

        public void PartialClone(LotPropertyInfo propertyInfoToClone)
        {
            this.MlsArea = propertyInfoToClone.MlsArea;
            this.LotDescription = propertyInfoToClone.LotDescription;
            this.PropertyType = propertyInfoToClone.PropertyType;
        }

        public LotPropertyInfo ImportPropertyFromCommunity(Entities.Community.Property property)
        {
            var clonnedProperty = this.Clone();
            clonnedProperty.MlsArea = property.MlsArea;
            clonnedProperty.LotDescription = property.LotDescription;
            clonnedProperty.PropertyType = property.PropertyType;
            clonnedProperty.LotDimension = property.LotDimension;
            clonnedProperty.LotSize = property.LotSize;
            return clonnedProperty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MlsArea;
            yield return this.PropertyType;
            yield return this.FemaFloodPlain;
            yield return this.LotDescription;
            yield return this.Latitude;
            yield return this.Longitude;
            yield return this.LegalDescription;
            yield return this.TaxId;
            yield return this.TaxLot;
            yield return this.TaxBlock;
            yield return this.LotDimension;
            yield return this.LotSize;
            yield return this.NumberOfPonds;
            yield return this.NumberOfWells;
            yield return this.PropertySubType;
            yield return this.TypeOfHomeAllowed;
            yield return this.SoilType;
            yield return this.SurfaceWater;
            yield return this.LiveStock;
            yield return this.CommercialAllowed;
            yield return this.UpdateGeocodes;
            yield return this.AlsoListedAs;
            yield return this.BuilderRestrictions;
        }
    }
}
