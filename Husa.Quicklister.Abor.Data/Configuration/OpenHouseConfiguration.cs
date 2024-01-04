namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.OpenHouse;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OpenHouseConfiguration : IEntityTypeConfiguration<OpenHouse>
    {
        public void Configure(EntityTypeBuilder<OpenHouse> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(r => r.StartTime).IsRequired().HasMaxLength(8);
            builder.Property(r => r.EndTime).IsRequired().HasMaxLength(8);
            builder.Property(r => r.Refreshments).IsRequired().HasEnumCollectionValue<Refreshments>(maxLength: 200);
            builder.Property(r => r.Type)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();
            builder.Property(r => r.OpenHouseType).HasMaxLength(30);

            builder.HasDiscriminator<string>(s => s.OpenHouseType)
                .HasValue<SaleListingOpenHouse>(EntityType.SaleProperty.ToString())
                .HasValue<CommunityOpenHouse>(EntityType.Community.ToString())
                .IsComplete(false);
        }
    }
}
