namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ScrapedListingConfiguration : IEntityTypeConfiguration<ScrapedListing>
    {
        public static void ConfigureValueObject(OwnedNavigationBuilder<ScrapedListing, ScrapedListingValueObject> builder)
        {
            builder.Property(f => f.OfficeName).HasMaxLength(100).HasColumnName(nameof(ScrapedListingValueObject.OfficeName));
            builder.Property(f => f.BuilderName).HasMaxLength(100).HasColumnName(nameof(ScrapedListingValueObject.BuilderName));
            builder.Property(f => f.DOM).HasColumnName(nameof(ScrapedListingValueObject.DOM));
            builder.Property(f => f.UncleanBuilder).HasMaxLength(100).HasColumnName(nameof(ScrapedListingValueObject.UncleanBuilder));
            builder.Property(f => f.MlsNum).HasMaxLength(20).HasColumnName(nameof(ScrapedListingValueObject.MlsNum));
            builder.Property(f => f.ListStatus).HasColumnName(nameof(ScrapedListingValueObject.ListStatus));
            builder.Property(f => f.Community).HasMaxLength(100).HasColumnName(nameof(ScrapedListingValueObject.Community));
            builder.Property(f => f.Address).HasMaxLength(100).HasColumnName(nameof(ScrapedListingValueObject.Address));
            builder.Property(f => f.City).HasMaxLength(20).HasColumnName(nameof(ScrapedListingValueObject.City));
            builder
                .Property(r => r.ListPrice)
                .HasColumnName(nameof(ScrapedListingValueObject.ListPrice))
                .HasPrecision(18, 2);

            builder.Property(f => f.Price).HasColumnName(nameof(ScrapedListingValueObject.Price));
            builder.Property(f => f.Comment).HasMaxLength(30).HasColumnName(nameof(ScrapedListingValueObject.Comment));
            builder.Property(f => f.ListDate).HasColumnType("datetime").HasColumnName(nameof(ScrapedListingValueObject.ListDate));
            builder.Property(f => f.Refreshed).HasColumnType("datetime").HasColumnName(nameof(ScrapedListingValueObject.Refreshed));
            builder.Property(f => f.UnitNum).HasColumnName(nameof(ScrapedListingValueObject.UnitNum));
        }

        public void Configure(EntityTypeBuilder<ScrapedListing> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.OwnsOne(v => v.ListingDetails, ConfigureValueObject);
        }
    }
}
