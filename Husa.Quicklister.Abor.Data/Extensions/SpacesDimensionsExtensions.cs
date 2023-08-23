namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SpacesDimensionsExtensions
    {
        public static void ConfigureSpacesDimensions<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideSpacesDimensions
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(r => r.StoriesTotal)
                .HasColumnName(nameof(IProvideSpacesDimensions.StoriesTotal))
                .HasEnumFieldValue<Stories>(32);
            builder.Property(r => r.SqFtTotal).HasColumnName(nameof(IProvideSpacesDimensions.SqFtTotal)).HasMaxLength(11);
            builder.Property(r => r.DiningAreasTotal).HasColumnName(nameof(IProvideSpacesDimensions.DiningAreasTotal)).HasMaxLength(1);
            builder.Property(r => r.MainLevelBedroomTotal).HasColumnName(nameof(IProvideSpacesDimensions.MainLevelBedroomTotal)).HasMaxLength(1);
            builder.Property(r => r.OtherLevelsBedroomTotal).HasColumnName(nameof(IProvideSpacesDimensions.OtherLevelsBedroomTotal)).HasMaxLength(1);
            builder.Property(r => r.HalfBathsTotal).HasColumnName(nameof(IProvideSpacesDimensions.HalfBathsTotal)).HasMaxLength(3);
            builder.Property(r => r.FullBathsTotal).HasColumnName(nameof(IProvideSpacesDimensions.FullBathsTotal)).HasMaxLength(50);
            builder.Property(r => r.LivingAreasTotal).HasColumnName(nameof(IProvideSpacesDimensions.LivingAreasTotal)).HasMaxLength(11);
        }
    }
}
