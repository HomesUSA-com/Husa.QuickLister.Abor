namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record PropertyRecord : IProvideSummary
    {
        public const string SummarySection = "PropertyInfo";

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "The {0} value is invalid for datetime.")]
        public DateTime? ConstructionCompletionDate { get; set; }

        public ConstructionStage? ConstructionStage { get; set; }

        [Required]
        public int? ConstructionStartYear { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LegalDescription { get; set; }
        public string TaxId { get; set; }
        public MlsArea? MlsArea { get; set; }
        public string MapscoGrid { get; set; }
        public string LotDimension { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LotSize { get; set; }
        public ICollection<LotDescription> LotDescription { get; set; }
        public ICollection<Occupancy> Occupancy { get; set; }
        public bool UpdateGeocodes { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsXmlManaged { get; set; }

        public PropertySubType? PropertyType { get; set; }

        public PropertyRecord CloneRecord() => (PropertyRecord)this.MemberwiseClone();

        public static PropertyRecord CreateRecord(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return new();
            }

            return new()
            {
                ConstructionCompletionDate = propertyInfo.ConstructionCompletionDate,
                ConstructionStage = propertyInfo.ConstructionStage,
                ConstructionStartYear = propertyInfo.ConstructionStartYear,
                LegalDescription = propertyInfo.LegalDescription,
                TaxId = propertyInfo.TaxId,
                MlsArea = propertyInfo.MlsArea,
                MapscoGrid = propertyInfo.MapscoGrid,
                LotDimension = propertyInfo.LotDimension,
                LotSize = propertyInfo.LotSize,
                LotDescription = propertyInfo.LotDescription,
                Occupancy = propertyInfo.Occupancy,
                UpdateGeocodes = propertyInfo.UpdateGeocodes,
                Latitude = propertyInfo.Latitude,
                Longitude = propertyInfo.Longitude,
                IsXmlManaged = propertyInfo.IsXmlManaged,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        {
            var summaryFields = SummaryExtensions.GetFieldSummary(this, entity, isInnerSummary: true);

            if (!summaryFields.Any())
            {
                return null;
            }

            return new()
            {
                Name = SummarySection,
                Fields = summaryFields,
            };
        }
    }
}
