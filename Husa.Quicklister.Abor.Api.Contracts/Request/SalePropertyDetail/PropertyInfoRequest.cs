namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class PropertyInfoRequest
    {
        [DataType(DataType.DateTime, ErrorMessage = "The {0} value is not valid for datetime")]
        public DateTime? ConstructionCompletionDate { get; set; }

        public ConstructionStage? ConstructionStage { get; set; }

        public int? ConstructionStartYear { get; set; }

        [MaxLength(255, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
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
    }
}
