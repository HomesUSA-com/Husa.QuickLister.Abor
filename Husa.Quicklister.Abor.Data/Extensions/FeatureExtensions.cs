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
            builder.Property(r => r.RestrictionsDescription).HasColumnName(nameof(IProvideFeature.RestrictionsDescription))
                .HasEnumCollectionValue<RestrictionsDescription>(255);
            builder.Property(r => r.UtilitiesDescription).HasColumnName(nameof(IProvideFeature.UtilitiesDescription))
                .HasEnumCollectionValue<UtilitiesDescription>(255);
            builder.Property(r => r.WaterSource).HasColumnName(nameof(IProvideFeature.WaterSource))
                .HasEnumCollectionValue<WaterSource>(100);
        }
    }
}
