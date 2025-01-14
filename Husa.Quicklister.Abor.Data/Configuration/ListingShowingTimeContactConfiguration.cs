namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingShowingTimeContactConfiguration : IEntityTypeConfiguration<ListingShowingTimeContact>
    {
        public void Configure(EntityTypeBuilder<ListingShowingTimeContact> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable(nameof(ListingShowingTimeContact));
            builder.Property(p => p.ScopeId).IsRequired();
            builder.Property(p => p.ContactId).IsRequired();
            builder.Property(p => p.Order).HasDefaultValue(1);
            builder.HasOne(p => p.Scope);
            builder.HasOne(p => p.Contact);
            builder.HasIndex("ScopeId", "ContactId");
        }
    }
}
