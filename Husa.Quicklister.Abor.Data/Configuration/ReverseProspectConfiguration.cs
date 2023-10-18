namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ReverseProspectConfiguration : IEntityTypeConfiguration<ReverseProspect>
    {
        public void Configure(EntityTypeBuilder<ReverseProspect> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(f => f.ReportData).HasColumnName(nameof(ReverseProspect.ReportData));
            builder.Property(f => f.Status).HasConversion<string>().HasColumnName(nameof(ReverseProspect.Status));
        }
    }
}
