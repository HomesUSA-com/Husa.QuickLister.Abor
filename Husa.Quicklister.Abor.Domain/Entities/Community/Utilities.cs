namespace Husa.Quicklister.Abor.Domain.Entities.Community
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Xml.Api.Contracts.Response;
    using Husa.Xml.Domain.Enums;
    using DomainEnums = Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class Utilities : ValueObject, IProvideFeature
    {
        private ICollection<DomainEnums.FireplaceDescription> fireplaceDescription;
        public virtual ICollection<NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public virtual ICollection<RestrictionsDescription> RestrictionsDescription { get; set; }
        public virtual ICollection<Disclosures> Disclosures { get; set; }
        public virtual ICollection<DocumentsAvailable> DocumentsAvailable { get; set; }
        public virtual ICollection<UtilitiesDescription> UtilitiesDescription { get; set; }
        public virtual ICollection<WaterSource> WaterSource { get; set; }
        public virtual ICollection<WaterSewer> WaterSewer { get; set; }
        public virtual ICollection<HeatingSystem> HeatSystem { get; set; }
        public virtual ICollection<CoolingSystem> CoolingSystem { get; set; }
        public virtual ICollection<Appliances> Appliances { get; set; }
        public virtual int? GarageSpaces { get; set; }
        public virtual ICollection<GarageDescription> GarageDescription { get; set; }
        public virtual ICollection<LaundryLocation> LaundryLocation { get; set; }
        public virtual ICollection<InteriorFeatures> InteriorFeatures { get; set; }
        public virtual int? Fireplaces { get; set; }
        public virtual ICollection<DomainEnums.FireplaceDescription> FireplaceDescription
        {
            get { return this.fireplaceDescription; }
            set { this.fireplaceDescription = this.Fireplaces.HasValue && this.Fireplaces.Value > 0 ? value : null; }
        }

        public virtual ICollection<Flooring> Floors { get; set; }
        public virtual ICollection<SecurityFeatures> SecurityFeatures { get; set; }
        public virtual ICollection<WindowFeatures> WindowFeatures { get; set; }

        public virtual ICollection<Foundation> Foundation { get; set; }
        public virtual ICollection<RoofDescription> RoofDescription { get; set; }
        public virtual ICollection<Fencing> Fencing { get; set; }
        public virtual ICollection<ConstructionMaterials> ConstructionMaterials { get; set; }
        public virtual ICollection<PatioAndPorchFeatures> PatioAndPorchFeatures { get; set; }
        public virtual ICollection<View> View { get; set; }
        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }

        public static Utilities ImportFromXml(SubdivisionResponse subdivision, Utilities utilities)
        {
            var importedUtilities = new Utilities();
            if (utilities != null)
            {
                importedUtilities = utilities.Clone();
            }

            if (subdivision.Amenities != null && subdivision.Amenities.Any())
            {
                importedUtilities.NeighborhoodAmenities = subdivision.Amenities
                    .Select(x => GetNeighborhoodAmenity(x.Type))
                    .Where(x => x != DomainEnums.NeighborhoodAmenities.None)
                    .ToArray();
            }

            return importedUtilities;
        }

        public Utilities Clone()
        {
            return (Utilities)this.MemberwiseClone();
        }

        public virtual Utilities ImportUtilities(FeaturesInfo info)
        {
            var clonedUtilities = this.Clone();
            clonedUtilities.NeighborhoodAmenities = info.NeighborhoodAmenities;
            clonedUtilities.RestrictionsDescription = info.RestrictionsDescription;
            clonedUtilities.UtilitiesDescription = info.UtilitiesDescription;
            clonedUtilities.WaterSource = info.WaterSource;
            clonedUtilities.WaterSewer = info.WaterSewer;
            clonedUtilities.HeatSystem = info.HeatSystem;
            clonedUtilities.CoolingSystem = info.CoolingSystem;
            clonedUtilities.Appliances = info.Appliances;
            clonedUtilities.GarageSpaces = info.GarageSpaces;
            clonedUtilities.GarageDescription = info.GarageDescription;
            clonedUtilities.LaundryLocation = info.LaundryLocation;
            clonedUtilities.InteriorFeatures = info.InteriorFeatures;
            clonedUtilities.Fireplaces = info.Fireplaces;
            clonedUtilities.FireplaceDescription = info.FireplaceDescription;
            clonedUtilities.Floors = info.Floors;
            clonedUtilities.SecurityFeatures = info.SecurityFeatures;
            clonedUtilities.WindowFeatures = info.WindowFeatures;
            clonedUtilities.Foundation = info.Foundation;
            clonedUtilities.RoofDescription = info.RoofDescription;
            clonedUtilities.Fencing = info.Fencing;
            clonedUtilities.ConstructionMaterials = info.ConstructionMaterials;
            clonedUtilities.PatioAndPorchFeatures = info.PatioAndPorchFeatures;
            clonedUtilities.View = info.View;
            clonedUtilities.ExteriorFeatures = info.ExteriorFeatures;

            return clonedUtilities;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.NeighborhoodAmenities;
            yield return this.RestrictionsDescription;
            yield return this.UtilitiesDescription;
            yield return this.WaterSource;
            yield return this.WaterSewer;
            yield return this.HeatSystem;
            yield return this.CoolingSystem;
            yield return this.Appliances;
            yield return this.GarageSpaces;
            yield return this.GarageDescription;
            yield return this.LaundryLocation;
            yield return this.InteriorFeatures;
            yield return this.Fireplaces;
            yield return this.FireplaceDescription;
            yield return this.Floors;
            yield return this.SecurityFeatures;
            yield return this.WindowFeatures;
            yield return this.Foundation;
            yield return this.RoofDescription;
            yield return this.Fencing;
            yield return this.ConstructionMaterials;
            yield return this.PatioAndPorchFeatures;
            yield return this.View;
            yield return this.ExteriorFeatures;
        }

        private static DomainEnums.NeighborhoodAmenities GetNeighborhoodAmenity(AmenityType amenityType) => amenityType switch
        {
            AmenityType.Pool => DomainEnums.NeighborhoodAmenities.Pool,
            AmenityType.Playground => DomainEnums.NeighborhoodAmenities.Playground,
            AmenityType.GolfCourse => DomainEnums.NeighborhoodAmenities.GolfCourse,
            AmenityType.Tennis => DomainEnums.NeighborhoodAmenities.TennisCourt,
            AmenityType.Park => DomainEnums.NeighborhoodAmenities.Park,
            AmenityType.Trails => DomainEnums.NeighborhoodAmenities.WalkBikeHikeJogTrails,
            AmenityType.Clubhouse => DomainEnums.NeighborhoodAmenities.Clubhouse,
            _ => DomainEnums.NeighborhoodAmenities.None,
        };
    }
}
