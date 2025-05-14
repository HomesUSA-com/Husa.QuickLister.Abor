namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class FeatureExtensions
    {
        public static void ConfigureFeature<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideFeature
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(x => x.NeighborhoodAmenities)
               .HasColumnName(nameof(IProvideFeature.NeighborhoodAmenities))
               .HasEnumCollectionValue<NeighborhoodAmenities>(maxLength: 286);
            builder.Property(x => x.WaterSewer).HasColumnName(nameof(IProvideFeature.WaterSewer)).HasEnumCollectionValue<WaterSewer>(255);
            builder.Property(r => r.HeatSystem).HasColumnName(nameof(IProvideFeature.HeatSystem)).HasEnumCollectionValue<HeatingSystem>(255);
            builder.Property(r => r.CoolingSystem).HasColumnName(nameof(IProvideFeature.CoolingSystem)).HasEnumCollectionValue<CoolingSystem>(100);
            builder.Property(r => r.Disclosures).HasColumnName(nameof(IProvideFeature.Disclosures)).HasEnumCollectionValue<Disclosures>(255);
            builder.Property(r => r.DocumentsAvailable).HasColumnName(nameof(IProvideFeature.DocumentsAvailable)).HasEnumCollectionValue<DocumentsAvailable>(255);
            builder.Property(r => r.RestrictionsDescription).HasColumnName(nameof(IProvideFeature.RestrictionsDescription))
                .HasEnumCollectionValue<RestrictionsDescription>(255);
            builder.Property(r => r.UtilitiesDescription).HasColumnName(nameof(IProvideFeature.UtilitiesDescription))
                .HasEnumCollectionValue<UtilitiesDescription>(255);
            builder.Property(r => r.WaterSource).HasColumnName(nameof(IProvideFeature.WaterSource))
                .HasEnumCollectionValue<WaterSource>(100);

            builder.Property(r => r.GarageSpaces).HasColumnName(nameof(IProvideFeature.GarageSpaces)).HasMaxLength(20);
            builder.Property(r => r.Floors).HasColumnName(nameof(IProvideFeature.Floors)).HasEnumCollectionValue<Flooring>(300);
            builder.Property(r => r.Fireplaces).HasColumnName(nameof(IProvideFeature.Fireplaces)).HasMaxLength(20);
            builder.Property(r => r.FireplaceDescription).HasColumnName(nameof(IProvideFeature.FireplaceDescription))
                .HasEnumCollectionValue<FireplaceDescription>(255);
            builder.Property(r => r.Appliances).HasColumnName(nameof(IProvideFeature.Appliances))
                .HasEnumCollectionValue<Appliances>(300);
            builder.Property(r => r.GarageDescription).HasColumnName(nameof(IProvideFeature.GarageDescription))
                .HasEnumCollectionValue<GarageDescription>(300);
            builder.Property(r => r.LaundryLocation).HasColumnName(nameof(IProvideFeature.LaundryLocation))
                .HasEnumCollectionValue<LaundryLocation>(300);
            builder.Property(r => r.InteriorFeatures).HasColumnName(nameof(IProvideFeature.InteriorFeatures))
                .HasEnumCollectionValue<InteriorFeatures>(300);
            builder.Property(r => r.SecurityFeatures).HasColumnName(nameof(IProvideFeature.SecurityFeatures))
                .HasEnumCollectionValue<SecurityFeatures>(300);
            builder.Property(r => r.WindowFeatures).HasColumnName(nameof(IProvideFeature.WindowFeatures))
                .HasEnumCollectionValue<WindowFeatures>(300);
            builder.Property(r => r.Foundation).HasColumnName(nameof(IProvideFeature.Foundation))
                .HasEnumCollectionValue<Foundation>(300);
            builder.Property(r => r.RoofDescription).HasColumnName(nameof(IProvideFeature.RoofDescription))
                   .HasEnumCollectionValue<RoofDescription>(300);
            builder.Property(r => r.Fencing).HasColumnName(nameof(IProvideFeature.Fencing))
                   .HasEnumCollectionValue<Fencing>(300);
            builder.Property(r => r.ConstructionMaterials).HasColumnName(nameof(IProvideFeature.ConstructionMaterials))
                   .HasEnumCollectionValue<ConstructionMaterials>(300);
            builder.Property(r => r.PatioAndPorchFeatures).HasColumnName(nameof(IProvideFeature.PatioAndPorchFeatures))
                   .HasEnumCollectionValue<PatioAndPorchFeatures>(300);
            builder.Property(r => r.View).HasColumnName(nameof(IProvideFeature.View))
                   .HasEnumCollectionValue<View>(300);
            builder.Property(r => r.ExteriorFeatures).HasColumnName(nameof(IProvideFeature.ExteriorFeatures))
                   .HasEnumCollectionValue<ExteriorFeatures>(300);
            builder.Property(r => r.ParkingTotal).HasColumnName(nameof(IProvideFeature.ParkingTotal)).HasMaxLength(20);
        }
    }
}
