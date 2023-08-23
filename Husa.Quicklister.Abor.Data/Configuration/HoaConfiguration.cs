namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HoaConfiguration : IEntityTypeConfiguration<Hoa>
    {
        public void Configure(EntityTypeBuilder<Hoa> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(70);

            builder
                .Property(p => p.TransferFee)
                .HasPrecision(18, 2)
                .IsRequired();
            builder
                .Property(p => p.Fee)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(r => r.BillingFrequency)
                .HasConversion<EnumFieldValueConverter<BillingFrequency>>()
                .HasMaxLength(1);

            builder.Property(r => r.Website).HasMaxLength(100);
            builder.Property(r => r.ContactPhone).HasMaxLength(14);
            builder.Property(r => r.HoaType)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder
                .HasDiscriminator(s => s.HoaType)
                .HasValue<SaleListingHoa>(EntityType.SaleProperty)
                .IsComplete(false);
        }
    }
}
