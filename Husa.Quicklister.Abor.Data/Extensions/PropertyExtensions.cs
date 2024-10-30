namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class PropertyExtensions
    {
        public const int LegalDescriptionLength = 40;
        public const int TaxIdLength = 50;

        public static void ConfigureProperty<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideProperty
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(x => x.MlsArea).HasColumnName(nameof(IProvideProperty.MlsArea)).HasEnumFieldValue<MlsArea>(maxLength: 5);
            builder.Property(r => r.LotSize).HasColumnName(nameof(IProvideProperty.LotSize)).HasMaxLength(25).IsRequired(false);
            builder.Property(x => x.ConstructionStage).HasColumnName(nameof(IProvideProperty.ConstructionStage)).HasEnumFieldValue<ConstructionStage>(maxLength: 10);
            builder.Property(x => x.LotDescription).HasColumnName(nameof(IProvideProperty.LotDescription)).HasEnumCollectionValue<LotDescription>(maxLength: 500);
            builder.Property(x => x.PropertyType).HasColumnName(nameof(IProvideProperty.PropertyType)).HasEnumFieldValue<PropertySubType>(maxLength: 32);
        }

        public static void ConfigureCommonProperty<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvidePropertyCommon
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(r => r.LegalDescription).HasColumnName(nameof(IProvidePropertyCommon.LegalDescription)).HasMaxLength(LegalDescriptionLength).IsRequired(false);
            builder.Property(r => r.TaxId).HasColumnName(nameof(IProvidePropertyCommon.TaxId)).HasMaxLength(TaxIdLength);
            builder.Property(r => r.TaxLot).HasColumnName(nameof(IProvidePropertyCommon.TaxLot)).HasMaxLength(25);
            builder.Property(r => r.LotDimension).HasColumnName(nameof(IProvidePropertyCommon.LotDimension)).HasMaxLength(25);
        }
    }
}
