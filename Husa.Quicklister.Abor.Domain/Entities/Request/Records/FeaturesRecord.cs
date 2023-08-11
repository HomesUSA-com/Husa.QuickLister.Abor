namespace Husa.Quicklister.Abor.Domain.Entities.Request.Records
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
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
        public int Fireplaces { get; set; }

        [IfRequired(nameof(Fireplaces), "0", OperatorType.GreaterThan)]
        public ICollection<FireplaceDescription> FireplaceDescription { get; set; }

        public ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public ICollection<WaterSource> WaterSource { get; set; }
        public ICollection<WaterSewer> WaterSewer { get; set; }
        public ICollection<HeatingSystem> HeatSystem { get; set; }
        public ICollection<CoolingSystem> CoolingSystem { get; set; }
        public ICollection<Appliances> Appliances { get; set; }
        public int GarageSpaces { get; set; }
        public ICollection<GarageDescription> GarageDescription { get; set; }
        public ICollection<LaundryFeatures> LaundryFeatures { get; set; }
        public ICollection<LaundryLocation> LaundryLocation { get; set; }
        public ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        public ICollection<KitchenFeatures> KitchenFeatures { get; set; }
        public ICollection<MasterBedroomFeatures> MasterBedroomFeatures { get; set; }
        public ICollection<WaterAccessDescription> WaterAccessDescription { get; set; }
        public ICollection<Flooring> Floors { get; set; }
        public ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        public ICollection<WindowFeatures> WindowFeatures { get; set; }
        public ICollection<Foundation> Foundation { get; set; }
        public ICollection<RoofDescription> RoofDescription { get; set; }
        public ICollection<Fencing> Fencing { get; set; }
        public ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        public ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        public ICollection<View> View { get; set; }
        public ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public ICollection<HomeFaces> HomeFaces { get; set; }
        public ICollection<WaterBodyName> WaterBodyName { get; set; }
        public ICollection<DistanceToWaterAccess> DistanceToWaterAccess { get; set; }
        public ICollection<WaterfrontFeatures> WaterfrontFeatures { get; set; }
        public UnitStyle? UnitStyle { get; set; }
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
                UtilitiesDescription = featuresInfo.UtilitiesDescription,
                WaterSource = featuresInfo.WaterSource,
                WaterSewer = featuresInfo.WaterSewer,
                HeatSystem = featuresInfo.HeatSystem,
                CoolingSystem = featuresInfo.CoolingSystem,
                Appliances = featuresInfo.Appliances,
                GarageSpaces = featuresInfo.GarageSpaces ?? throw new DomainException(nameof(featuresInfo.GarageSpaces)),
                GarageDescription = featuresInfo.GarageDescription,
                LaundryFeatures = featuresInfo.LaundryFeatures,
                LaundryLocation = featuresInfo.LaundryLocation,
                InteriorFeatures = featuresInfo.InteriorFeatures,
                KitchenFeatures = featuresInfo.KitchenFeatures,
                MasterBedroomFeatures = featuresInfo.MasterBedroomFeatures,
                WaterAccessDescription = featuresInfo.WaterAccessDescription,
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
                HomeFaces = featuresInfo.HomeFaces,
                WaterBodyName = featuresInfo.WaterBodyName,
                DistanceToWaterAccess = featuresInfo.DistanceToWaterAccess,
                WaterfrontFeatures = featuresInfo.WaterfrontFeatures,
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
