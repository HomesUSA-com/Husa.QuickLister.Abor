namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.OwnsOne(v => v.OfficeValue, ConfigureValueObject);
        }

        private static void ConfigureValueObject(OwnedNavigationBuilder<Office, OfficeValueObject> builder)
        {
            builder.Property(f => f.MarketUniqueId).HasMaxLength(10).HasColumnName(nameof(OfficeValueObject.MarketUniqueId));
            builder.Property(f => f.Name).HasMaxLength(50).HasColumnName(nameof(OfficeValueObject.Name));
            builder.Property(f => f.Email).HasMaxLength(50).HasColumnName(nameof(OfficeValueObject.Email));
            builder.Property(f => f.Address).HasMaxLength(65).HasColumnName(nameof(OfficeValueObject.Address));
            builder.Property(f => f.City).HasColumnName(nameof(OfficeValueObject.City)).HasEnumFieldValue<Cities>(50, isRequired: true);
            builder.Property(f => f.State).HasColumnName(nameof(OfficeValueObject.State)).HasEnumFieldValue<States>(2);
            builder.Property(f => f.Zip).HasMaxLength(5).HasColumnName(nameof(OfficeValueObject.Zip));
            builder.Property(f => f.Phone).HasMaxLength(20).HasColumnName(nameof(OfficeValueObject.Phone));
            builder.Property(f => f.Fax).HasMaxLength(20).HasColumnName(nameof(OfficeValueObject.Fax));
            builder.Property(f => f.Status).HasMaxLength(10).HasColumnName(nameof(OfficeValueObject.Status));
            builder.Property(f => f.LicenseNumber).HasMaxLength(20).HasColumnName(nameof(OfficeValueObject.LicenseNumber));
            builder.Property(f => f.MarketModified).HasColumnType("datetime").HasColumnName(nameof(OfficeValueObject.MarketModified));
        }
    }
}
