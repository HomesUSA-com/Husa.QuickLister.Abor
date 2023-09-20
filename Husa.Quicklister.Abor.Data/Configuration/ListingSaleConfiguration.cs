namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ListingSaleConfiguration : IEntityTypeConfiguration<SaleListing>
    {
        public void Configure(EntityTypeBuilder<SaleListing> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(p => p.SaleProperty)
               .WithMany(b => b.SaleListings)
               .HasForeignKey(e => e.SalePropertyId);

            builder.SetListingProperties();
            builder.OwnsOne(sf => sf.StatusFieldsInfo, ConfigureStatusFieldsMapping);
            builder.OwnsOne(sf => sf.PublishInfo, ConfigurePublishInfoMapping).Navigation(e => e.PublishInfo).IsRequired();
            builder.Property(f => f.LastPhotoRequestCreationDate).HasColumnType("datetime");
            builder.Ignore(p => p.IsInMls);
            builder
                .Property(r => r.ListPrice)
                .HasColumnName(nameof(SaleListing.ListPrice))
                .HasPrecision(18, 2);

            builder.ToTable("ListingSale");

            builder.HasQueryFilter(t => !t.SaleProperty.IsDeleted);
        }

        private static void ConfigureStatusFieldsMapping(OwnedNavigationBuilder<SaleListing, ListingSaleStatusFieldsInfo> builder)
        {
            builder.Property(f => f.AgentId).HasColumnName(nameof(ListingSaleStatusFieldsInfo.AgentId));
            builder.Property(f => f.HasBuyerAgent).HasColumnName(nameof(ListingSaleStatusFieldsInfo.HasBuyerAgent)).HasDefaultValue(false);
            builder.Property(f => f.BackOnMarketDate).HasColumnType("datetime").HasColumnName(nameof(ListingSaleStatusFieldsInfo.BackOnMarketDate));
            builder.Property(f => f.CancelledReason).HasMaxLength(300).HasColumnName(nameof(ListingSaleStatusFieldsInfo.CancelledReason));
            builder.Property(f => f.ClosedDate).HasColumnType("datetime").HasColumnName(nameof(ListingSaleStatusFieldsInfo.ClosedDate));
            builder.Property(f => f.ClosePrice).HasPrecision(18, 2).HasColumnName(nameof(ListingSaleStatusFieldsInfo.ClosePrice));
            builder.Property(f => f.EstimatedClosedDate).HasColumnType("datetime").HasColumnName(nameof(ListingSaleStatusFieldsInfo.EstimatedClosedDate));
            builder.Property(f => f.OffMarketDate).HasColumnType("datetime").HasColumnName(nameof(ListingSaleStatusFieldsInfo.OffMarketDate));
            builder.Property(f => f.SellConcess).HasMaxLength(50).HasColumnName(nameof(ListingSaleStatusFieldsInfo.SellConcess));
            builder.Property(f => f.SaleTerms).HasColumnName(nameof(ListingSaleStatusFieldsInfo.SaleTerms)).HasEnumCollectionValue<SaleTerms>(300);
            builder.Property(f => f.PendingDate).HasColumnType("datetime").HasColumnName(nameof(ListingSaleStatusFieldsInfo.PendingDate));
            builder.Property(f => f.ContingencyInfo).HasColumnName(nameof(ListingSaleStatusFieldsInfo.ContingencyInfo))
                .HasEnumCollectionValue<ContingencyInfo>(maxLength: 100);
            builder.Property(f => f.HasContingencyInfo).HasColumnName(nameof(ListingSaleStatusFieldsInfo.HasContingencyInfo));
            builder.Property(f => f.AgentIdSecond).HasColumnName(nameof(ListingSaleStatusFieldsInfo.AgentIdSecond));
            builder.Property(f => f.HasSecondBuyerAgent).HasColumnName(nameof(ListingSaleStatusFieldsInfo.HasSecondBuyerAgent));
        }

        private static void ConfigurePublishInfoMapping(OwnedNavigationBuilder<SaleListing, PublishInfo> builder)
        {
            builder.Property(r => r.PublishType).HasColumnName(nameof(PublishInfo.PublishType)).HasConversion<string>().HasMaxLength(20).IsRequired(false);
            builder.Property(r => r.PublishUser).HasColumnName(nameof(PublishInfo.PublishUser)).IsRequired(false);
            builder.Property(r => r.PublishStatus).HasColumnName(nameof(PublishInfo.PublishStatus)).HasConversion<string>().HasMaxLength(20).IsRequired(false);
            builder.Property(r => r.PublishDate).HasColumnName(nameof(PublishInfo.PublishDate)).IsRequired(false);
        }
    }
}
