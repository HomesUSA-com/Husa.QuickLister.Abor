namespace Husa.Quicklister.Abor.Domain.Entities.Listing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Attributes;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;

    public class PropertyInfo : ValueObject, IProvideProperty, IProvideGeocodes
    {
        private DateTime? constructionCompletionDate;

        public PropertyInfo(DateTime? constructionCompletionDate)
        {
            this.ConstructionCompletionDate = constructionCompletionDate;
        }

        public PropertyInfo()
        {
        }

        [XmlPropertyUpdate]
        [DataType(DataType.DateTime, ErrorMessage = "The {0} value is invalid for datetime.")]
        public DateTime? ConstructionCompletionDate
        {
            get { return this.constructionCompletionDate?.ToUniversalTime(); }
            set { this.constructionCompletionDate = value?.ToUniversalTime(); }
        }

        public ConstructionStage? ConstructionStage { get; set; }

        public int? ConstructionStartYear { get; set; }

        [XmlPropertyUpdate]
        public string LegalDescription { get; set; }

        public string TaxId { get; set; }

        public string TaxLot { get; set; }

        public MlsArea? MlsArea { get; set; }

        public string LotDimension { get; set; }

        public string LotSize { get; set; }

        public ICollection<LotDescription> LotDescription { get; set; }

        public PropertySubType? PropertyType { get; set; }

        public bool UpdateGeocodes { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsXmlManaged { get; set; }

        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }

        public static PropertyInfo ImportFromXml(XmlListingDetailResponse listing, PropertyInfo propertyInfo)
        {
            var importedPropertyInfo = new PropertyInfo();
            if (propertyInfo != null)
            {
                importedPropertyInfo = propertyInfo.Clone();
            }

            importedPropertyInfo.Latitude = listing.Latitude != null ? listing.Latitude : importedPropertyInfo.Latitude;
            importedPropertyInfo.Longitude = listing.Longitude != null ? listing.Longitude : importedPropertyInfo.Longitude;
            importedPropertyInfo.ConstructionCompletionDate = listing.Day;
            importedPropertyInfo.LotDescription = listing.LegalDescLot.CsvToEnum<LotDescription>().ToArray();
            importedPropertyInfo.IsXmlManaged = true;

            return importedPropertyInfo;
        }

        public void UpdateFromXml(XmlListingDetailResponse listing)
        {
            if (!this.UpdateGeocodes)
            {
                if (listing.Latitude != null)
                {
                    this.Latitude = listing.Latitude;
                }

                if (listing.Longitude != null)
                {
                    this.Longitude = listing.Longitude;
                }
            }

            if (listing.Day.HasValue)
            {
                this.ConstructionCompletionDate = listing.Day;
            }

            if (!string.IsNullOrEmpty(listing.LegalDescLot))
            {
                this.LotDescription = listing.LegalDescLot.CsvToEnum<LotDescription>().ToArray();
            }
        }

        public PropertyInfo Clone()
        {
            return (PropertyInfo)this.MemberwiseClone();
        }

        public void PartialClone(PropertyInfo propertyInfoToClone)
        {
            this.ConstructionStartYear = propertyInfoToClone.ConstructionStartYear;
            this.LegalDescription = propertyInfoToClone.LegalDescription;
            this.MlsArea = propertyInfoToClone.MlsArea;
            this.LotSize = propertyInfoToClone.LotSize;
            this.LotDescription = propertyInfoToClone.LotDescription;
            this.PropertyType = propertyInfoToClone.PropertyType;
        }

        public PropertyInfo ImportAddressInfoFromCommunity(Community.Property property)
        {
            var clonnedProperty = this.Clone();
            clonnedProperty.MlsArea = property.MlsArea;

            return clonnedProperty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.ConstructionCompletionDate;
            yield return this.ConstructionStage;
            yield return this.ConstructionStartYear;
            yield return this.LegalDescription;
            yield return this.TaxId;
            yield return this.TaxLot;
            yield return this.MlsArea;
            yield return this.LotDimension;
            yield return this.LotSize;
            yield return this.LotDescription;
            yield return this.PropertyType;
            yield return this.UpdateGeocodes;
            yield return this.Latitude;
            yield return this.Longitude;
            yield return this.IsXmlManaged;
            yield return this.FemaFloodPlain;
        }
    }
}
