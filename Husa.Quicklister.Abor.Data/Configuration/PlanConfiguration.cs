namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.ConfigurePlan();
            builder.OwnsOne(o => o.BasePlan, ConfigurePlanInformation);
        }

        private static void ConfigurePlanInformation(OwnedNavigationBuilder<Plan, BasePlan> builder)
        {
            builder.Property(r => r.Name).HasColumnName(nameof(BasePlan.Name)).HasMaxLength(255);
            builder.Property(r => r.OwnerName).HasColumnName(nameof(BasePlan.OwnerName)).HasMaxLength(100);
            builder.Property(r => r.IsNewConstruction).HasColumnName(nameof(BasePlan.IsNewConstruction));
            builder.ConfigureSpacesDimensions();
        }
    }
}
