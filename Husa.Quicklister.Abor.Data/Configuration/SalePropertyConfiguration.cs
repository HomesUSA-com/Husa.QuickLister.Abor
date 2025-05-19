namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SalePropertyConfiguration : IEntityTypeConfiguration<SaleProperty>
    {
        public const int PropertyDescriptionLength = 4000;
        public const int AgentPrivateRemarksLength = 4000;

        public void Configure(EntityTypeBuilder<SaleProperty> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(r => r.OwnerName).HasMaxLength(100);
            builder.OwnsOne(o => o.SpacesDimensionsInfo, ConfigureSpacesDimensionsMapping);
            builder.OwnsOne(o => o.FeaturesInfo, ConfigureFeaturesMapping);
            builder.OwnsOne(o => o.FinancialInfo, ConfigureFinancialMapping);
            builder.OwnsOne(o => o.ShowingInfo, ConfigureShowingMapping);
            builder.OwnsOne(o => o.SchoolsInfo, ConfigureSchoolsMapping).Navigation(e => e.SchoolsInfo).IsRequired();
            builder.OwnsOne(o => o.AddressInfo, ConfigureAddressMapping);
            builder.OwnsOne(o => o.PropertyInfo, ConfigurePropertyInfoMapping);
            builder.OwnsOne(o => o.SalesOfficeInfo, ConfigureSalesOfficeMapping).Navigation(e => e.SalesOfficeInfo).IsRequired();

            builder.HasOne(p => p.Community)
               .WithMany(b => b.SaleProperties)
               .HasForeignKey(e => e.CommunityId);

            builder.HasOne(p => p.Plan)
               .WithMany(b => b.SaleProperties)
               .HasForeignKey(e => e.PlanId);
        }

        private static void ConfigurePropertyInfoMapping(OwnedNavigationBuilder<SaleProperty, PropertyInfo> builder)
        {
            builder.Property(x => x.ConstructionCompletionDate).HasColumnName(nameof(PropertyInfo.ConstructionCompletionDate)).IsRequired(false);
            builder.Property(r => r.ConstructionStartYear).HasColumnName(nameof(PropertyInfo.ConstructionStartYear)).IsRequired(false);
            builder.Property(r => r.IsXmlManaged).HasColumnName(nameof(PropertyInfo.IsXmlManaged));
            builder.Property(r => r.UpdateGeocodes).HasColumnName(nameof(PropertyInfo.UpdateGeocodes));
            builder.Property(r => r.FemaFloodPlain).HasColumnName(nameof(PropertyInfo.FemaFloodPlain)).HasEnumCollectionValue<FemaFloodPlain>(25);

            builder.ConfigureCommonProperty();
            builder.ConfigureProperty();
            builder.ConfigureGeocodes();
        }

        private static void ConfigureSalesOfficeMapping(OwnedNavigationBuilder<SaleProperty, SalesOffice> builder)
        {
            builder.Property(x => x.StreetNumber).HasColumnName("SalesOfficeStreetNumber").HasMaxLength(12);
            builder.Property(x => x.StreetName).HasColumnName("SalesOfficeStreetName").HasMaxLength(50);
            builder.Property(x => x.StreetSuffix).HasColumnName("SalesOfficeStreetSuffix").HasMaxLength(50);

            builder.Property(x => x.SalesOfficeCity)
                .HasColumnName(nameof(SalesOffice.SalesOfficeCity))
                .HasConversion<EnumFieldValueConverter<Cities>>()
                .HasMaxLength(50);

            builder.Property(x => x.SalesOfficeZip).HasColumnName(nameof(SalesOffice.SalesOfficeZip)).HasMaxLength(50);
        }

        private static void ConfigureAddressMapping(OwnedNavigationBuilder<SaleProperty, SaleAddressInfo> builder)
        {
            builder.ConfigureAddressInfoMapping();
            builder.Property(r => r.UnitNumber).HasColumnName(nameof(SaleAddressInfo.UnitNumber)).HasMaxLength(20).IsRequired(false);
        }

        private static void ConfigureSpacesDimensionsMapping(OwnedNavigationBuilder<SaleProperty, SpacesDimensionsInfo> builder)
        {
            builder.ConfigureSpacesDimensions();
        }

        private static void ConfigureFeaturesMapping(OwnedNavigationBuilder<SaleProperty, FeaturesInfo> builder)
        {
            builder.ConfigureFeature();
            builder.Property(r => r.HomeFaces).HasColumnName(nameof(FeaturesInfo.HomeFaces)).HasEnumFieldValue<HomeFaces>(20);
            builder.Property(r => r.WaterBodyName).HasColumnName(nameof(FeaturesInfo.WaterBodyName)).HasEnumFieldValue<WaterBodyName>(50);
            builder.Property(r => r.DistanceToWaterAccess).HasColumnName(nameof(FeaturesInfo.DistanceToWaterAccess)).HasEnumFieldValue<DistanceToWaterAccess>(50);
            builder.Property(r => r.WaterfrontFeatures).HasColumnName(nameof(FeaturesInfo.WaterfrontFeatures)).HasEnumCollectionValue<WaterfrontFeatures>(100);
            builder.Property(r => r.GuestAccommodationsDescription).HasColumnName(nameof(FeaturesInfo.GuestAccommodationsDescription)).HasEnumCollectionValue<GuestAccommodationsDescription>(100);
            builder.Property(r => r.UnitStyle).HasColumnName(nameof(FeaturesInfo.UnitStyle)).HasEnumCollectionValue<UnitStyle>(100);
            builder.Property(r => r.IsNewConstruction).HasColumnName(nameof(FeaturesInfo.IsNewConstruction));
            builder.Property(r => r.PropertyDescription).HasColumnName(nameof(FeaturesInfo.PropertyDescription)).HasMaxLength(PropertyDescriptionLength).IsRequired(false);
            builder.Property(r => r.IsAIGeneratedPropertyDescription).HasColumnName(nameof(FeaturesInfo.IsAIGeneratedPropertyDescription)).HasColumnType("bit");
            builder.Property(r => r.GuestBedroomsTotal).HasColumnName(nameof(FeaturesInfo.GuestBedroomsTotal)).HasMaxLength(3);
            builder.Property(r => r.GuestHalfBathsTotal).HasColumnName(nameof(FeaturesInfo.GuestHalfBathsTotal)).HasMaxLength(3);
            builder.Property(r => r.GuestFullBathsTotal).HasColumnName(nameof(FeaturesInfo.GuestFullBathsTotal)).HasMaxLength(3);
        }

        private static void ConfigureFinancialMapping(OwnedNavigationBuilder<SaleProperty, FinancialInfo> builder)
        {
            builder.ConfigureFinancial();
            builder.Property(r => r.TaxYear).HasColumnName(nameof(FinancialInfo.TaxYear)).HasMaxLength(4);
            builder.Ignore(p => p.ReadableBuyersAgentCommission);
        }

        private static void ConfigureShowingMapping(OwnedNavigationBuilder<SaleProperty, ShowingInfo> builder)
        {
            builder.ConfigureShowing();
            builder.Property(r => r.RealtorContactEmail).HasColumnName(nameof(ShowingInfo.RealtorContactEmail)).HasMaxLength(255);
            builder.Property(r => r.AgentPrivateRemarks).HasColumnName(nameof(ShowingInfo.AgentPrivateRemarks)).HasMaxLength(AgentPrivateRemarksLength);
            builder.Property(r => r.AgentPrivateRemarksAdditional).HasColumnName(nameof(ShowingInfo.AgentPrivateRemarksAdditional)).HasMaxLength(1000);
            builder.Property(r => r.EnableOpenHouses).HasColumnName(nameof(ShowingInfo.EnableOpenHouses));
            builder.Property(r => r.ShowOpenHousesPending).HasColumnName(nameof(ShowingInfo.ShowOpenHousesPending));
            builder.Property(r => r.LockBoxSerialNumber).HasColumnName(nameof(ShowingInfo.LockBoxSerialNumber)).HasMaxLength(50);
        }

        private static void ConfigureSchoolsMapping(OwnedNavigationBuilder<SaleProperty, SchoolsInfo> builder)
        {
            builder.ConfigureSchools();
        }
    }
}
