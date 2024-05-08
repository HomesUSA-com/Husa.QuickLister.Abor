namespace Husa.Quicklister.Abor.Domain.Entities.Lot
{
    using System.Collections.Generic;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class LotPropertyInfo : ValueObject
    {
        public MlsArea? MlsArea { get; set; }
        public PropertySubType? PropertyType { get; set; }
        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        public ICollection<LotDescription> LotDescription { get; set; }

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
            return clonnedProperty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.MlsArea;
            yield return this.PropertyType;
            yield return this.FemaFloodPlain;
            yield return this.LotDescription;
        }
    }
}
