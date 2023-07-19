namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingSaleOpenHouseConfiguration
    {
        public void Configure(EntityTypeBuilder<SaleListingOpenHouse> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.SaleProperty)
               .WithMany(b => b.OpenHouses)
               .HasForeignKey(e => e.SalePropertyId);
        }
    }
}
