namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using Husa.Extensions.Common;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class Property : ValueObject, IProvideProperty
    {
        private string subdivision;

        public Cities? City { get; set; }

        public Counties? County { get; set; }

        public string ZipCode { get; set; }

        public string Subdivision { get => this.subdivision; set => this.subdivision = value.ToTitleCase(); }

        public string LotSize { get; set; }

        public MlsArea? MlsArea { get; set; }

        public ConstructionStage? ConstructionStage { get; set; }

        public string LotDimension { get; set; }

        public ICollection<LotDescription> LotDescription { get; set; }

        public PropertySubType? PropertyType { get; set; }

        public static Property ImportFromXml(SubdivisionResponse subdivision, Property property)
        {
            var importedProperty = new Property();
            if (property != null)
            {
                importedProperty = property.Clone();
            }

            importedProperty.City = subdivision.City.ToCity(isExactValue: false);
            importedProperty.County = subdivision.County.ToCounty(isExactValue: false);
            importedProperty.Subdivision = subdivision.Name;
            importedProperty.ZipCode = subdivision.Zip;

            return importedProperty;
        }

        public virtual Property UpdateFromXml(SubdivisionResponse subdivision)
        {
            var clonnedProperty = this.Clone();
            var city = subdivision.City.ToCity(isExactValue: false);
            if (city.HasValue)
            {
                clonnedProperty.City = city;
            }

            var county = subdivision.County.ToCounty(isExactValue: false);
            if (county.HasValue)
            {
                clonnedProperty.County = county;
            }

            if (!string.IsNullOrEmpty(subdivision.Zip))
            {
                clonnedProperty.ZipCode = subdivision.Zip;
            }

            return clonnedProperty;
        }

        public Property Clone()
        {
            return (Property)this.MemberwiseClone();
        }

        public virtual Property ImportProperty(SaleProperty saleProperty)
        {
            var clonedProperty = this.Clone();
            clonedProperty.City = saleProperty.AddressInfo.City;
            if (saleProperty.AddressInfo.County.HasValue)
            {
                clonedProperty.County = saleProperty.AddressInfo.County;
            }

            clonedProperty.Subdivision = saleProperty.AddressInfo.Subdivision;
            clonedProperty.ZipCode = saleProperty.AddressInfo.ZipCode;
            clonedProperty.MlsArea = saleProperty.PropertyInfo.MlsArea;

            return clonedProperty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.City;
            yield return this.County;
            yield return this.MlsArea;
            yield return this.Subdivision;
            yield return this.ZipCode;
            yield return this.PropertyType;
            yield return this.LotDescription;
            yield return this.LotDimension;
            yield return this.LotSize;
        }
    }
}
