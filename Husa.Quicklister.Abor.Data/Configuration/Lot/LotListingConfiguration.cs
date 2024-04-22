namespace Husa.Quicklister.Abor.Data.Configuration.Lot
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LotListingConfiguration : IEntityTypeConfiguration<LotListing>
    {
        public void Configure(EntityTypeBuilder<LotListing> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.SetBaseListingProperties();
            builder.ToTable("LotListing");

            builder
                .Property(r => r.ListPrice)
                .HasColumnName(nameof(LotListing.ListPrice))
                .HasPrecision(18, 2);
            builder.Property(p => p.MlsStatus).HasConversion<string>().HasMaxLength(30).IsRequired();

            builder.OwnsOne(o => o.PublishInfo, PublishInfoExtensions.ConfigurePublishInfoMapping);
            builder.OwnsOne(o => o.FeaturesInfo, ConfigureFeaturesMapping);
            builder.OwnsOne(o => o.FinancialInfo, ConfigureFinancialMapping);
            builder.OwnsOne(o => o.ShowingInfo, ConfigureShowingMapping);
            builder.OwnsOne(o => o.SchoolsInfo, ConfigureSchoolsMapping);
            builder.OwnsOne(o => o.AddressInfo, AddressExtensions.ConfigureAddressInfoMapping);
            builder.OwnsOne(o => o.PropertyInfo, ConfigurePropertyInfoMapping);

            builder.HasOne(p => p.Community)
               .WithMany(b => b.LotListings)
               .HasForeignKey(e => e.CommunityId);
        }

        private static void ConfigurePropertyInfoMapping(OwnedNavigationBuilder<LotListing, LotPropertyInfo> builder)
        {
            builder.Property(x => x.MlsArea).HasColumnName(nameof(LotPropertyInfo.MlsArea)).HasEnumFieldValue<MlsArea>(maxLength: 5);
            builder.Property(x => x.LotDescription).HasColumnName(nameof(LotPropertyInfo.LotDescription)).HasEnumCollectionValue<LotDescription>(maxLength: 500);
            builder.Property(x => x.PropertyType).HasColumnName(nameof(LotPropertyInfo.PropertyType)).HasEnumFieldValue<PropertySubType>(maxLength: 32);
            builder.Property(r => r.FemaFloodPlain).HasColumnName(nameof(LotPropertyInfo.FemaFloodPlain)).HasEnumCollectionValue<FemaFloodPlain>(25);
        }

        private static void ConfigureFeaturesMapping(OwnedNavigationBuilder<LotListing, LotFeaturesInfo> builder)
        {
            builder.Property(r => r.WaterfrontFeatures).HasColumnName(nameof(LotFeaturesInfo.WaterfrontFeatures)).HasEnumCollectionValue<WaterfrontFeatures>(100);
            builder.Property(r => r.View).HasColumnName(nameof(LotFeaturesInfo.View))
                            .HasEnumCollectionValue<View>(300);
            builder.Property(r => r.RestrictionsDescription).HasColumnName(nameof(LotFeaturesInfo.RestrictionsDescription))
                .HasEnumCollectionValue<RestrictionsDescription>(255);
            builder.Property(r => r.UtilitiesDescription).HasColumnName(nameof(LotFeaturesInfo.UtilitiesDescription))
                .HasEnumCollectionValue<UtilitiesDescription>(255);
            builder.Property(r => r.WaterSource).HasColumnName(nameof(LotFeaturesInfo.WaterSource))
                .HasEnumCollectionValue<WaterSource>(100);
            builder.Property(x => x.WaterSewer).HasColumnName(nameof(LotFeaturesInfo.WaterSewer)).HasEnumCollectionValue<WaterSewer>(255);
            builder.Property(r => r.DistanceToWaterAccess).HasColumnName(nameof(LotFeaturesInfo.DistanceToWaterAccess)).HasEnumFieldValue<DistanceToWaterAccess>(50);
            builder.Property(r => r.Fencing).HasColumnName(nameof(IProvideFeature.Fencing))
                   .HasEnumCollectionValue<Fencing>(300);
            builder.Property(r => r.ExteriorFeatures).HasColumnName(nameof(IProvideFeature.ExteriorFeatures))
                   .HasEnumCollectionValue<ExteriorFeatures>(300);
        }

        private static void ConfigureFinancialMapping(OwnedNavigationBuilder<LotListing, LotFinancialInfo> builder)
        {
            builder.ConfigureLotFinancial();
            builder.Ignore(p => p.ReadableBuyersAgentCommission);
        }

        private static void ConfigureShowingMapping(OwnedNavigationBuilder<LotListing, LotShowingInfo> builder)
        {
            builder.Property(r => r.ShowingRequirements)
                .HasColumnName(nameof(IProvideShowingInfo.ShowingRequirements))
                .HasEnumCollectionValue<ShowingRequirements>(maxLength: 300);
        }

        private static void ConfigureSchoolsMapping(OwnedNavigationBuilder<LotListing, LotSchoolsInfo> builder)
        {
            builder.ConfigureSchools();
        }
    }
}
