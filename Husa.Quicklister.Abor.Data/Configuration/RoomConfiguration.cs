namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(r => r.Level).HasEnumFieldValue<RoomLevel>(10, isRequired: true);
            builder.Property(r => r.RoomType).IsRequired().HasConversion<string>();
            builder.Property(r => r.EntityOwnerType).HasConversion<string>().HasMaxLength(20);
            builder.HasDiscriminator(ppr => ppr.EntityOwnerType)
                .HasValue<ListingSaleRoom>(EntityType.SaleProperty.ToString())
                .HasValue<PlanRoom>(EntityType.Plan.ToString());
        }
    }
}
