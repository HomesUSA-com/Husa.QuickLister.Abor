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
    using Husa.Xml.Domain.Enums.Xml;
    using DomainEnums = Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class Utilities : ValueObject, IProvideFeature
    {
        private ICollection<DomainEnums.FireplaceDescription> fireplaceDescription;
        public ICollection<DomainEnums.NeighborhoodAmenities> NeighborhoodAmenities { get; set; }
        public virtual ICollection<Inclusions> Inclusions { get; set; }
        public virtual ICollection<Floors> Floors { get; set; }
        public virtual ICollection<ExteriorFeatures> ExteriorFeatures { get; set; }
        public virtual ICollection<RoofDescription> RoofDescription { get; set; }
        public virtual ICollection<Foundation> Foundation { get; set; }
        public virtual ICollection<HeatingSystem> HeatSystem { get; set; }
        public virtual ICollection<CoolingSystem> CoolingSystem { get; set; }
        public virtual ICollection<GreenCertification> GreenCertification { get; set; }
        public virtual ICollection<EnergyFeatures> EnergyFeatures { get; set; }
        public virtual ICollection<GreenFeatures> GreenFeatures { get; set; }
        public virtual ICollection<WaterSewer> WaterSewer { get; set; }
        public virtual string SupplierElectricity { get; set; }
        public virtual string SupplierWater { get; set; }
        public virtual string SupplierSewer { get; set; }
        public virtual string SupplierGarbage { get; set; }
        public virtual string SupplierGas { get; set; }
        public virtual string SupplierOther { get; set; }
        public virtual ICollection<HeatingFuel> HeatingFuel { get; set; }
        public virtual ICollection<SpecialtyRooms> SpecialtyRooms { get; set; }
        public virtual bool HasAccessibility { get; set; }
        public virtual ICollection<Accessibility> Accessibility { get; set; }
        public virtual int? Fireplaces { get; set; }
        public virtual ICollection<DomainEnums.FireplaceDescription> FireplaceDescription
        {
            get { return this.fireplaceDescription; }
            set { this.fireplaceDescription = this.Fireplaces.HasValue && this.Fireplaces.Value > 0 ? value : null; }
        }

        public virtual ICollection<Exterior> Exterior { get; set; }

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

            if (subdivision.Utility != null && subdivision.Utility.Any())
            {
                importedUtilities.SupplierWater = subdivision.Utility.Where(x => x.Type == UtilitiesType.Water).Select(x => x.Name).FirstOrDefault();
                importedUtilities.SupplierGas = subdivision.Utility.Where(x => x.Type == UtilitiesType.Gas).Select(x => x.Name).FirstOrDefault();
                importedUtilities.SupplierSewer = subdivision.Utility.Where(x => x.Type == UtilitiesType.Sewer).Select(x => x.Name).FirstOrDefault();
                importedUtilities.SupplierElectricity = subdivision.Utility.Where(x => x.Type == UtilitiesType.Electric).Select(x => x.Name).FirstOrDefault();
            }

            return importedUtilities;
        }

        public Utilities Clone()
        {
            return (Utilities)this.MemberwiseClone();
        }

        public virtual Utilities ImportUtilities(FeaturesInfo info, SpacesDimensionsInfo spacesDimensionsInfo)
        {
            var clonedUtilities = this.Clone();
            clonedUtilities.NeighborhoodAmenities = info.NeighborhoodAmenities;
            clonedUtilities.Inclusions = info.Inclusions;
            clonedUtilities.Floors = info.Floors;
            clonedUtilities.ExteriorFeatures = info.ExteriorFeatures;
            clonedUtilities.RoofDescription = info.RoofDescription;
            clonedUtilities.Foundation = info.Foundation;
            clonedUtilities.HeatSystem = info.HeatSystem;
            clonedUtilities.CoolingSystem = info.CoolingSystem;
            clonedUtilities.GreenCertification = info.GreenCertification;
            clonedUtilities.EnergyFeatures = info.EnergyFeatures;
            clonedUtilities.GreenFeatures = info.GreenFeatures;
            clonedUtilities.WaterSewer = info.WaterSewer;
            clonedUtilities.SupplierElectricity = info.SupplierElectricity;
            clonedUtilities.SupplierWater = info.SupplierWater;
            clonedUtilities.SupplierSewer = info.SupplierSewer;
            clonedUtilities.SupplierGarbage = info.SupplierGarbage;
            clonedUtilities.SupplierGas = info.SupplierGas;
            clonedUtilities.SupplierOther = info.SupplierOther;
            clonedUtilities.HeatingFuel = info.HeatingFuel;
            clonedUtilities.HasAccessibility = info.HasAccessibility;
            clonedUtilities.Accessibility = info.Accessibility;
            clonedUtilities.Fireplaces = info.Fireplaces;
            clonedUtilities.FireplaceDescription = info.FireplaceDescription;
            clonedUtilities.Exterior = info.Exterior;
            clonedUtilities.SpecialtyRooms = spacesDimensionsInfo.SpecialtyRooms;

            return clonedUtilities;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.NeighborhoodAmenities;
            yield return this.Inclusions;
            yield return this.Floors;
            yield return this.ExteriorFeatures;
            yield return this.RoofDescription;
            yield return this.Foundation;
            yield return this.HeatSystem;
            yield return this.CoolingSystem;
            yield return this.GreenCertification;
            yield return this.EnergyFeatures;
            yield return this.GreenFeatures;
            yield return this.WaterSewer;
            yield return this.SupplierElectricity;
            yield return this.SupplierWater;
            yield return this.SupplierSewer;
            yield return this.SupplierGarbage;
            yield return this.SupplierGas;
            yield return this.SupplierOther;
            yield return this.HeatingFuel;
            yield return this.SpecialtyRooms;
            yield return this.HasAccessibility;
            yield return this.Accessibility;
            yield return this.Fireplaces;
            yield return this.FireplaceDescription;
            yield return this.Exterior;
        }

        private static DomainEnums.NeighborhoodAmenities GetNeighborhoodAmenity(AmenityType amenityType) => amenityType switch
        {
            AmenityType.Pool => DomainEnums.NeighborhoodAmenities.Pool,
            AmenityType.Playground => DomainEnums.NeighborhoodAmenities.ParkPlayground,
            AmenityType.GolfCourse => DomainEnums.NeighborhoodAmenities.GolfCourse,
            AmenityType.Tennis => DomainEnums.NeighborhoodAmenities.Tennis,
            AmenityType.Volleyball => DomainEnums.NeighborhoodAmenities.VolleyballCourt,
            AmenityType.Basketball => DomainEnums.NeighborhoodAmenities.BasketballCourt,
            AmenityType.Baseball => DomainEnums.NeighborhoodAmenities.BasketballCourt,
            AmenityType.Park => DomainEnums.NeighborhoodAmenities.ParkPlayground,
            AmenityType.Trails => DomainEnums.NeighborhoodAmenities.JoggingTrails,
            AmenityType.Clubhouse => DomainEnums.NeighborhoodAmenities.Clubhouse,
            _ => DomainEnums.NeighborhoodAmenities.None,
        };
    }
}
