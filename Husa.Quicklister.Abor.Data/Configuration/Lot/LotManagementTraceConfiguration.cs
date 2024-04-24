namespace Husa.Quicklister.Abor.Data.Configuration.Lot
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LotManagementTraceConfiguration : IEntityTypeConfiguration<LotManagementTrace>
    {
        public void Configure(EntityTypeBuilder<LotManagementTrace> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(p => p.Listing)
               .WithMany(b => b.ManagementTraces)
               .HasForeignKey(e => e.ListingId);

            builder.HasQueryFilter(t => !t.Listing.IsDeleted);
        }
    }
}
