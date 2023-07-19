namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingSaleRoomConfiguration
    {
        public void Configure(EntityTypeBuilder<ListingSaleRoom> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.SaleProperty)
               .WithMany(b => b.Rooms)
               .HasForeignKey(e => e.SalePropertyId);
        }
    }
}
