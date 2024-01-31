namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.OwnsOne(v => v.AgentValue, ConfigureValueObject);
        }

        private static void ConfigureValueObject(OwnedNavigationBuilder<Agent, AgentValueObject> builder)
        {
            builder.Property(f => f.MarketUniqueId).HasMaxLength(10).HasColumnName(nameof(AgentValueObject.MarketUniqueId));
            builder.Property(f => f.MemberStateLicense).HasMaxLength(100).HasColumnName(nameof(AgentValueObject.MemberStateLicense));
            builder.Property(f => f.Web).HasMaxLength(50).HasColumnName(nameof(AgentValueObject.Web));
            builder.Property(f => f.OfficeId).HasMaxLength(10).HasColumnName(nameof(AgentValueObject.OfficeId));
            builder.Property(f => f.FirstName).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.FirstName));
            builder.Property(x => x.MiddleName).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.MiddleName));
            builder.Property(f => f.LastName).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.LastName));
            builder.Property(x => x.FullName).HasMaxLength(65).HasColumnName(nameof(AgentValueObject.FullName));
            builder.Property(f => f.Status).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.Status));
            builder.Property(f => f.CellPhone).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.CellPhone));
            builder.Property(f => f.WorkPhone).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.WorkPhone));
            builder.Property(f => f.Email).HasMaxLength(80).HasColumnName(nameof(AgentValueObject.Email));
            builder.Property(f => f.Fax).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.Fax));
            builder.Property(f => f.HomePhone).HasMaxLength(20).HasColumnName(nameof(AgentValueObject.HomePhone));
            builder.Property(f => f.MarketModified).HasColumnType("datetimeoffset").HasColumnName(nameof(AgentValueObject.MarketModified));
        }
    }
}
