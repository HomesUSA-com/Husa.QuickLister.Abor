namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class GeocodesExtensions
    {
        public static void ConfigureGeocodes<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideGeocodes
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(r => r.Latitude).HasColumnName(nameof(IProvideGeocodes.Latitude)).HasPrecision(32, 12);
            builder.Property(r => r.Longitude).HasColumnName(nameof(IProvideGeocodes.Longitude)).HasPrecision(32, 12);
        }
    }
}
