namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingSaleHoaConfiguration
    {
        public void Configure(EntityTypeBuilder<SaleListingHoa> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.SaleProperty)
               .WithMany(b => b.ListingSaleHoas)
               .HasForeignKey(e => e.SalePropertyId);
        }
    }
}
