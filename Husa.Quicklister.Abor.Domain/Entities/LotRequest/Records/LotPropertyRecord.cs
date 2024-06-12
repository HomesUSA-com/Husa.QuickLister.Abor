namespace Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces.LotListing;

    public record LotPropertyRecord : IProvideLotProperty
    {
        [Required]
        public MlsArea? MlsArea { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<LotDescription> LotDescription { get; set; }

        public PropertySubType? PropertyType { get; set; }

        public ICollection<FemaFloodPlain> FemaFloodPlain { get; set; }
        public ICollection<PropCondition> PropCondition { get; set; }
        public string TaxBlock { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LotSize { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LegalDescription { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string TaxLot { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TaxId { get; set; }

        public string LotDimension { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool LiveStock { get; set; }

        public bool CommercialAllowed { get; set; }
        public int? NumberOfPonds { get; set; }
        public int? NumberOfWells { get; set; }

        public bool SurfaceWater { get; set; }

        [Required]
        public PropertySubTypeLots? PropertySubType { get; set; }

        public ICollection<TypeOfHomeAllowed> TypeOfHomeAllowed { get; set; }
        public ICollection<SoilType> SoilType { get; set; }
        public bool UpdateGeocodes { get; set; }
        public int? AlsoListedAs { get; set; }

        public bool BuilderRestrictions { get; set; }

        public LotPropertyRecord CloneRecord() => (LotPropertyRecord)this.MemberwiseClone();

        public static LotPropertyRecord CreateRecord(LotPropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return new();
            }

            return new()
            {
                MlsArea = propertyInfo.MlsArea,
                LotDescription = propertyInfo.LotDescription,
                PropertyType = propertyInfo.PropertyType,
                FemaFloodPlain = propertyInfo.FemaFloodPlain,
                Latitude = propertyInfo.Latitude,
                LegalDescription = propertyInfo.LegalDescription,
                Longitude = propertyInfo.Longitude,
                LotDimension = propertyInfo.LotDimension,
                LotSize = propertyInfo.LotSize,
                PropCondition = propertyInfo.PropCondition,
                TaxBlock = propertyInfo.TaxBlock,
                TaxId = propertyInfo.TaxId,
                TaxLot = propertyInfo.TaxLot,
                SoilType = propertyInfo.SoilType,
                SurfaceWater = propertyInfo.SurfaceWater,
                TypeOfHomeAllowed = propertyInfo.TypeOfHomeAllowed,
                PropertySubType = propertyInfo.PropertySubType,
                NumberOfPonds = propertyInfo.NumberOfPonds,
                NumberOfWells = propertyInfo.NumberOfWells,
                LiveStock = propertyInfo.LiveStock,
                CommercialAllowed = propertyInfo.CommercialAllowed,
                UpdateGeocodes = propertyInfo.UpdateGeocodes,
                AlsoListedAs = propertyInfo.AlsoListedAs,
                BuilderRestrictions = propertyInfo.BuilderRestrictions,
            };
        }
    }
}
