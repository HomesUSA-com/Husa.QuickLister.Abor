namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SaleListingTraceConfiguration : IEntityTypeConfiguration<SaleListingTrace>
    {
        public void Configure(EntityTypeBuilder<SaleListingTrace> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.ListingSale)
               .WithMany(b => b.ListingSaleTraces)
               .HasForeignKey(e => e.ListingSaleId);

            builder.Property(r => r.ActionType).HasConversion<string>().HasMaxLength(20).IsRequired();
            builder.Property(p => p.RequestMlsStatus).HasConversion<string>().HasMaxLength(30).IsRequired();

            builder.ToTable("ListingSaleTrace");

            builder.HasQueryFilter(t => !t.ListingSale.IsDeleted);
        }
    }
}
