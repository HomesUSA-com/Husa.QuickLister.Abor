namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlanRoomConfiguration
    {
        public void Configure(EntityTypeBuilder<PlanRoom> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.Plan)
               .WithMany(b => b.Rooms)
               .HasForeignKey(e => e.PlanId);
        }
    }
}
