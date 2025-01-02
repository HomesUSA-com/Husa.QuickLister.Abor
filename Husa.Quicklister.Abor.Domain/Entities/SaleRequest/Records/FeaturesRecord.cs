namespace Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Common.Validations;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Extensions.Domain.Validations;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Interfaces;

    public record FeaturesRecord : IProvideSummary
    {
        public const string SummarySection = "Features";

        [Required(AllowEmptyStrings = false)]
        [DataChecker]
        public string PropertyDescription { get; set; }

        [Required]
        public int Fireplaces { get; set; }

        [IfRequired(nameof(Fireplaces), "0", OperatorType.GreaterThan)]
        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Disclosures> Disclosures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterSource> WaterSource { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterSewer> WaterSewer { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<HeatingSystem> HeatSystem { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<CoolingSystem> CoolingSystem { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Appliances> Appliances { get; set; }
        public int GarageSpaces { get; set; }

        [IfRequired(nameof(GarageSpaces), 0, OperatorType.GreaterThan)]
        public ICollection<GarageDescription> GarageDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<LaundryLocation> LaundryLocation { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<InteriorFeatures> InteriorFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Flooring> Floors { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<SecurityFeatures> SecurityFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WindowFeatures> WindowFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Foundation> Foundation { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<RoofDescription> RoofDescription { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<Fencing> Fencing { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<View> View { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        public HomeFaces HomeFaces { get; set; }

        [RequiredIfCollection(nameof(WaterfrontFeatures), Enums.Domain.WaterfrontFeatures.None, isIn: false)]
        public WaterBodyName? WaterBodyName { get; set; }

        public DistanceToWaterAccess? DistanceToWaterAccess { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }
        public ICollection<UnitStyle> UnitStyle { get; set; }

        [Required]
        [MinLength(1)]
        public ICollection<GuestAccommodationsDescription> GuestAccommodationsDescription { get; set; }
        public int? GuestBedroomsTotal { get; set; }
        public int? GuestFullBathsTotal { get; set; }
        public int? GuestHalfBathsTotal { get; set; }
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
                NeighborhoodAmenities = featuresInfo.NeighborhoodAmenities,
                RestrictionsDescription = featuresInfo.RestrictionsDescription,
                Disclosures = featuresInfo.Disclosures,
                DocumentsAvailable = featuresInfo.DocumentsAvailable,
                UtilitiesDescription = featuresInfo.UtilitiesDescription,
                WaterSource = featuresInfo.WaterSource,
                WaterSewer = featuresInfo.WaterSewer,
                HeatSystem = featuresInfo.HeatSystem,
                CoolingSystem = featuresInfo.CoolingSystem,
                Appliances = featuresInfo.Appliances,
                GarageSpaces = featuresInfo.GarageSpaces ?? throw new DomainException(nameof(featuresInfo.GarageSpaces)),
                GarageDescription = featuresInfo.GarageDescription,
                LaundryLocation = featuresInfo.LaundryLocation,
                InteriorFeatures = featuresInfo.InteriorFeatures,
                Fireplaces = featuresInfo.Fireplaces ?? throw new DomainException(nameof(featuresInfo.Fireplaces)),
                FireplaceDescription = featuresInfo.FireplaceDescription,
                Floors = featuresInfo.Floors,
                SecurityFeatures = featuresInfo.SecurityFeatures,
                WindowFeatures = featuresInfo.WindowFeatures,
                Foundation = featuresInfo.Foundation,
                RoofDescription = featuresInfo.RoofDescription,
                Fencing = featuresInfo.Fencing,
                ConstructionMaterials = featuresInfo.ConstructionMaterials,
                PatioAndPorchFeatures = featuresInfo.PatioAndPorchFeatures,
                View = featuresInfo.View,
                ExteriorFeatures = featuresInfo.ExteriorFeatures,
                HomeFaces = featuresInfo.HomeFaces ?? throw new DomainException(nameof(featuresInfo.HomeFaces)),
                WaterfrontFeatures = featuresInfo.WaterfrontFeatures,
                WaterBodyName = featuresInfo.WaterBodyName,
                DistanceToWaterAccess = featuresInfo.DistanceToWaterAccess,
                UnitStyle = featuresInfo.UnitStyle,
                GuestAccommodationsDescription = featuresInfo.GuestAccommodationsDescription,
                GuestBedroomsTotal = featuresInfo.GuestBedroomsTotal,
                GuestFullBathsTotal = featuresInfo.GuestFullBathsTotal,
                GuestHalfBathsTotal = featuresInfo.GuestHalfBathsTotal,
                PropertyDescription = featuresInfo.PropertyDescription,
                IsNewConstruction = featuresInfo.IsNewConstruction,
            };
        }

        public virtual SummarySection GetSummary<T>(T entity)
            where T : class
        => this.GetSummarySection(entity, sectionName: SummarySection);
    }
}
