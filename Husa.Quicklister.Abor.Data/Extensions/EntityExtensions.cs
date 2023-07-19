namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class EntityExtensions
    {
        public static void SetSysProperties<T>(this EntityTypeBuilder<T> entity, bool filterOutDeleted = true)
            where T : Entity
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Id)
                  .HasDatabaseName($"IX_{typeof(T).Name}")
                  .IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.SysCreatedOn)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SysCreatedBy)
                .HasColumnType("uniqueidentifier");

            entity
                .Property(e => e.SysModifiedOn)
                .HasColumnType("datetime");

            entity.Property(e => e.SysModifiedBy)
                .HasColumnType("uniqueidentifier");

            entity
                .Property(e => e.SysTimestamp)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity
                .Property(e => e.IsDeleted)
                .IsRequired()
                .HasColumnType("bit")
                .HasDefaultValue(false);

            if (filterOutDeleted)
            {
                entity.HasQueryFilter(c => !c.IsDeleted);
            }
        }

        public static void SetIdentitySysProperties<T>(this EntityTypeBuilder<T> entity, string keyColumn, bool isGuid = true)
            where T : class
        {
            entity.HasKey(keyColumn);

            entity.HasIndex(keyColumn)
                .HasDatabaseName($"IX_{typeof(T).Name}")
                .IsUnique();

            if (isGuid)
            {
                entity.Property(keyColumn)
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("(newid())");
            }

            SetCommonProperties(entity);
        }

        public static void SetIdentitySysProperties<T>(this EntityTypeBuilder<T> entity, IEnumerable<KeyValuePair<string, bool>> keyColumns)
            where T : class
        {
            var allProperties = (from kvp in keyColumns select kvp.Key).ToArray();

            entity.HasKey(allProperties);

            entity.HasIndex(allProperties, $"IX_{typeof(T).Name}")
                  .IsUnique();

            foreach (KeyValuePair<string, bool> element in keyColumns)
            {
                if (element.Value)
                {
                    entity.Property(element.Key)
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");
                }
            }
        }

        public static void SetCommonProperties<T>(this EntityTypeBuilder<T> entity)
            where T : class
        {
            entity.Property("SysCreatedOn")
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property("SysCreatedBy")
                .HasColumnType("uniqueidentifier");

            entity
                .Property("SysModifiedOn")
                .HasColumnType("datetime");

            entity.Property("SysModifiedBy")
                .HasColumnType("uniqueidentifier");

            entity
                .Property("SysTimestamp")
                .HasColumnType("datetime");

            entity
                .Property("IsDeleted")
                .IsRequired()
                .HasColumnType("bit")
                .HasDefaultValue(false);

            entity.HasQueryFilter(m => !EF.Property<bool>(m, "IsDeleted"));
        }

        public static void SetListingProperties<T>(this EntityTypeBuilder<T> entity)
            where T : Listing
        {
            entity.Property(p => p.ExpirationDate).IsRequired(false).HasColumnType("datetime");
            entity.Property(p => p.ListPrice).HasPrecision(18, 2);
            entity.Property(p => p.ListType).HasConversion<string>().HasMaxLength(30);
            entity.Property(p => p.LockedStatus).HasConversion<string>().HasColumnName(nameof(Listing.LockedStatus)).HasMaxLength(20);
            entity.Property(p => p.LockedBy).HasColumnName(nameof(Listing.LockedBy)).IsRequired(false);
            entity.Property(p => p.LockedOn).HasColumnName(nameof(Listing.LockedOn)).IsRequired(false);
            entity.Property(p => p.ListDate).IsRequired(false).HasColumnType("datetime");
            entity.Property(p => p.MarketUniqueId).HasMaxLength(50);
            entity.Property(p => p.MlsNumber).HasMaxLength(20);
            entity.Property(p => p.MlsStatus).HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(p => p.MarketModifiedOn).HasColumnType("datetime");
        }
    }
}
