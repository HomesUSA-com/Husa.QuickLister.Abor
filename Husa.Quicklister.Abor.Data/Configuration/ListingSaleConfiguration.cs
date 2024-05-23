namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Data.Configuration;
    using Husa.Quicklister.Extensions.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingSaleConfiguration : IEntityTypeConfiguration<SaleListing>
    {
        public void Configure(EntityTypeBuilder<SaleListing> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.HasOne(p => p.SaleProperty)
               .WithMany(b => b.SaleListings)
               .HasForeignKey(e => e.SalePropertyId);

            builder.ListingProperties();
            builder.OwnsOne(sf => sf.StatusFieldsInfo, StatusExtensions.ConfigureStatusInfoMapping).Navigation(e => e.StatusFieldsInfo);
            builder.OwnsOne(sf => sf.PublishInfo, PublishInfoExtensions.ConfigurePublishInfoMapping).Navigation(e => e.PublishInfo).IsRequired();
            builder.OwnsOne(sf => sf.InvoiceInfo, ConfigureInvoiceInfoMapping).Navigation(e => e.InvoiceInfo).IsRequired();
            builder.Property(f => f.LastPhotoRequestCreationDate).HasColumnType("datetime");
            builder.Ignore(p => p.IsInMls);
            builder
                .Property(r => r.ListPrice)
                .HasColumnName(nameof(SaleListing.ListPrice))
                .HasPrecision(18, 2);

            builder.ToTable("ListingSale");

            builder.HasQueryFilter(t => !t.SaleProperty.IsDeleted);
            builder.Property(r => r.LegacyId).HasMaxLength(100);
        }

        private static void ConfigureInvoiceInfoMapping(OwnedNavigationBuilder<SaleListing, InvoiceInfo> builder)
        {
            builder.ConfigureInvoiceInfo();
        }
    }
}
