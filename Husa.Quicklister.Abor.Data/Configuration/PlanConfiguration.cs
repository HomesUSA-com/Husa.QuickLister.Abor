namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();

            builder.OwnsOne(o => o.BasePlan, ConfigurePlanInformation);
            builder.Property(f => f.LastPhotoRequestCreationDate).HasColumnType("datetime");
            builder.Property(r => r.XmlStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(XmlStatus.NotFromXml)
                .IsRequired();
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
