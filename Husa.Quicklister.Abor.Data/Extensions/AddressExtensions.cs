namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class AddressExtensions
    {
        public static void ConfigureAddressInfoMapping<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : AddressInfo
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(x => x.StreetNumber).HasColumnName(nameof(AddressInfo.StreetNumber)).HasMaxLength(12);
            builder.Property(r => r.StreetName).HasColumnName(nameof(AddressInfo.StreetName)).HasMaxLength(50);
            builder.Property(r => r.State).HasColumnName(nameof(AddressInfo.State)).HasEnumFieldValue<States>(maxLength: 2, isRequired: true);
            builder.Property(r => r.StreetType).HasColumnName(nameof(AddressInfo.StreetType)).HasEnumFieldValue<StreetType>(maxLength: 20);

            builder.Property(r => r.City).HasColumnName(nameof(AddressInfo.City)).HasEnumFieldValue<Cities>(maxLength: 50, isRequired: true);
            builder.Property(r => r.County).HasColumnName(nameof(AddressInfo.County)).HasEnumFieldValue<Counties>(maxLength: 20);
            builder.Property(x => x.Subdivision).HasColumnName(nameof(AddressInfo.Subdivision)).HasMaxLength(75);
            builder.Property(x => x.ZipCode).HasColumnName(nameof(AddressInfo.ZipCode)).HasMaxLength(12);
        }
    }
}
