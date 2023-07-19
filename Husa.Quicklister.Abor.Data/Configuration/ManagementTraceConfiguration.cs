namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ManagementTraceConfiguration : IEntityTypeConfiguration<ManagementTrace>
    {
        public void Configure(EntityTypeBuilder<ManagementTrace> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.SaleListing)
               .WithMany(b => b.ManagementTraces)
               .HasForeignKey(e => e.SaleListingId);

            builder.HasQueryFilter(t => !t.SaleListing.IsDeleted);
        }
    }
}
