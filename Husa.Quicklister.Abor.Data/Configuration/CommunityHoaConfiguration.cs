namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CommunityHoaConfiguration
    {
        public void Configure(EntityTypeBuilder<CommunityHoa> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.Community)
               .WithMany(b => b.CommunityHoas)
               .HasForeignKey(e => e.CommunitySaleId);
        }
    }
}
