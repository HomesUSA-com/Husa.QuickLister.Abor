namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Validations;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;

    public record FeaturesRecord : IProvideSummary
    {
        public const string SummarySection = "Features";

        [Required(AllowEmptyStrings = false)]
        public string PropertyDescription { get; set; }

        [Required]
        public int? Fireplaces { get; set; }

        [IfRequired(nameof(Fireplaces), "0", OperatorType.GreaterThan)]
        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<WindowCoverings> WindowCoverings { get; set; }
        public bool HasAccessibility { get; set; }

        [IfRequired(nameof(HasAccessibility), true, OperatorType.Equal)]
        public ICollection<Accessibility> Accessibility { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<HousingStyle> HousingStyle { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<Exterior> Exterior { get; set; }
        public bool HasPrivatePool { get; set; }

        [IfRequired(nameof(HasPrivatePool), true, OperatorType.Equal)]
        public ICollection<PrivatePool> PrivatePool { get; set; }
        public ICollection<HomeFaces> HomeFaces { get; set; }

        [Required]
        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public ICollection<LotImprovements> LotImprovements { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<Inclusions> Inclusions { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<Floors> Floors { get; set; }

        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<RoofDescription> RoofDescription { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<Foundation> Foundation { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<HeatingSystem> HeatSystem { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<CoolingSystem> CoolingSystem { get; set; }
        public ICollection<GreenCertification> GreenCertification { get; set; }
        public ICollection<EnergyFeatures> EnergyFeatures { get; set; }
        public ICollection<GreenFeatures> GreenFeatures { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<WaterSewer> WaterSewer { get; set; }
        public string SupplierElectricity { get; set; }
        public string SupplierWater { get; set; }
        public string SupplierSewer { get; set; }
        public string SupplierGarbage { get; set; }
        public string SupplierGas { get; set; }
        public string SupplierOther { get; set; }

        [Required(AllowEmptyStrings = false)]
        public ICollection<HeatingFuel> HeatingFuel { get; set; }
        public bool IsNewConstruction { get; set; }

        public FeaturesRecord CloneRecord() => (FeaturesRecord)this.MemberwiseClone();

        public static FeaturesRecord CreateRecord(FeaturesInfo featuresInfo)
        {
            if (featuresInfo == null)
            {
                return new();
            }

            return new()
            {
                PropertyDescription = featuresInfo.PropertyDescription,
                Fireplaces = featuresInfo.Fireplaces,
                FireplaceDescription = featuresInfo.FireplaceDescription,
                WindowCoverings = featuresInfo.WindowCoverings,
                HasAccessibility = featuresInfo.HasAccessibility,
                Accessibility = featuresInfo.Accessibility,
                HousingStyle = featuresInfo.HousingStyle,
                Exterior = featuresInfo.Exterior,
                HasPrivatePool = featuresInfo.HasPrivatePool,
                PrivatePool = featuresInfo.PrivatePool,
                HomeFaces = featuresInfo.HomeFaces,
                NeighborhoodAmenities = featuresInfo.NeighborhoodAmenities,
                LotImprovements = featuresInfo.LotImprovements,
                Inclusions = featuresInfo.Inclusions,
                Floors = featuresInfo.Floors,
                ExteriorFeatures = featuresInfo.ExteriorFeatures,
                RoofDescription = featuresInfo.RoofDescription,
                Foundation = featuresInfo.Foundation,
                HeatSystem = featuresInfo.HeatSystem,
                CoolingSystem = featuresInfo.CoolingSystem,
                GreenCertification = featuresInfo.GreenCertification,
                EnergyFeatures = featuresInfo.EnergyFeatures,
                GreenFeatures = featuresInfo.GreenFeatures,
                WaterSewer = featuresInfo.WaterSewer,
                SupplierElectricity = featuresInfo.SupplierElectricity,
                SupplierWater = featuresInfo.SupplierWater,
                SupplierSewer = featuresInfo.SupplierSewer,
                SupplierGarbage = featuresInfo.SupplierGarbage,
                SupplierGas = featuresInfo.SupplierGas,
                SupplierOther = featuresInfo.SupplierOther,
                HeatingFuel = featuresInfo.HeatingFuel,
                IsNewConstruction = featuresInfo.IsNewConstruction,
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
