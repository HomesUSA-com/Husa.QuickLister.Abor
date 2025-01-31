namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Common.Validations;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.SaleListing;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record PropertyRecord : IProvideSummary, IProvideSaleProperty
    {
        public const string SummarySection = "PropertyInfo";

        [Required]
        [DataType(DataType.DateTime, ErrorMessage = "The {0} value is invalid for datetime.")]
        public DateTime? ConstructionCompletionDate { get; set; }

        [Required]
        public ConstructionStage? ConstructionStage { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(238, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string LegalDescription { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TaxId { get; set; }
        public MlsArea? MlsArea { get; set; }
        public string LotDimension { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LotSize { get; set; }

        [Required]
        public int? ConstructionStartYear { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string TaxLot { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<LotDescription> LotDescription { get; set; }

        [Required]
        public PropertySubType? PropertyType { get; set; }

        public bool UpdateGeocodes { get; set; }

        [IfRequired(nameof(UpdateGeocodes), true, OperatorType.Equal)]
        public decimal? Latitude { get; set; }

        [IfRequired(nameof(UpdateGeocodes), true, OperatorType.Equal)]
        public decimal? Longitude { get; set; }

        public bool IsXmlManaged { get; set; }

        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }

        public PropertyRecord CloneRecord() => (PropertyRecord)this.MemberwiseClone();

        public static PropertyRecord CreateRecord(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return new();
            }

            return new()
            {
                ConstructionCompletionDate = propertyInfo.ConstructionCompletionDate ?? throw new DomainException(nameof(propertyInfo.ConstructionCompletionDate)),
                ConstructionStage = propertyInfo.ConstructionStage ?? throw new DomainException(nameof(propertyInfo.ConstructionStage)),
                ConstructionStartYear = propertyInfo.ConstructionStartYear ?? throw new DomainException(nameof(propertyInfo.ConstructionStartYear)),
                LegalDescription = propertyInfo.LegalDescription,
                TaxId = propertyInfo.TaxId,
                TaxLot = propertyInfo.TaxLot,
                MlsArea = propertyInfo.MlsArea,
                LotDimension = propertyInfo.LotDimension,
                LotSize = propertyInfo.LotSize,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType ?? throw new DomainException(nameof(propertyInfo.PropertyType)),
                UpdateGeocodes = propertyInfo.UpdateGeocodes,
                Latitude = propertyInfo.Latitude,
                Longitude = propertyInfo.Longitude,
                IsXmlManaged = propertyInfo.IsXmlManaged,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
       => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
