namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CommunityEmployeeConfiguration : IEntityTypeConfiguration<CommunityEmployee>
    {
        public void Configure(EntityTypeBuilder<CommunityEmployee> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasIndex(p => new { p.UserId, p.CommunityId }).IsUnique();

            builder.HasOne(p => p.Community)
               .WithMany(b => b.Employees)
               .HasForeignKey(e => e.CommunityId);

            builder.HasQueryFilter(t => !t.Community.IsDeleted);
        }
    }
}
