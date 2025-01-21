namespace Husa.Quicklister.Abor.Data.Configuration.Lot
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class LotListingConfiguration : IEntityTypeConfiguration<LotListing>
    {
        public void Configure(EntityTypeBuilder<LotListing> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ListingProperties();
            builder.Ignore(p => p.AppointmentType);
            builder.Ignore(p => p.AccessInformation);
            builder.Ignore(p => p.AppointmentRestrictions);
            builder.Ignore(p => p.AdditionalInstructions);

            builder.OwnsOne(o => o.StatusFieldsInfo, StatusExtensions.ConfigureStatusInfoMapping).Navigation(e => e.StatusFieldsInfo);
            builder.OwnsOne(o => o.PublishInfo, PublishInfoExtensions.ConfigurePublishInfoMapping);
            builder.OwnsOne(o => o.FeaturesInfo, ConfigureFeaturesMapping);
            builder.OwnsOne(o => o.FinancialInfo, ConfigureFinancialMapping);
            builder.OwnsOne(o => o.ShowingInfo, ConfigureShowingMapping);
            builder.OwnsOne(o => o.SchoolsInfo, ConfigureSchoolsMapping);
            builder.OwnsOne(o => o.AddressInfo, ConfigureAddressMapping);
            builder.OwnsOne(o => o.PropertyInfo, ConfigurePropertyInfoMapping);

            builder.Property(p => p.OwnerName).HasColumnName(nameof(LotListing.OwnerName)).HasMaxLength(100);
            builder.HasOne(p => p.Community)
               .WithMany(b => b.LotListings)
               .HasForeignKey(e => e.CommunityId);

            builder.ToTable("LotListing");
        }

        private static void ConfigureAddressMapping(OwnedNavigationBuilder<LotListing, LotAddressInfo> builder)
        {
            builder.ConfigureAddressInfoMapping();
            builder.Property(r => r.StreetDirPrefix).HasColumnName(nameof(LotAddressInfo.StreetDirPrefix)).HasEnumFieldValue<StreetDirPrefix>(maxLength: 3);
            builder.Property(r => r.StreetDirSuffix).HasColumnName(nameof(LotAddressInfo.StreetDirSuffix)).HasEnumFieldValue<StreetDirPrefix>(maxLength: 3);
            builder.Property(r => r.UnitNumber).HasColumnName(nameof(LotAddressInfo.UnitNumber)).HasMaxLength(10);
        }

        private static void ConfigurePropertyInfoMapping(OwnedNavigationBuilder<LotListing, LotPropertyInfo> builder)
        {
            builder.Property(x => x.MlsArea).HasColumnName(nameof(LotPropertyInfo.MlsArea)).HasEnumFieldValue<MlsArea>(maxLength: 5);
            builder.Property(x => x.LotDescription).HasColumnName(nameof(LotPropertyInfo.LotDescription)).HasEnumCollectionValue<LotDescription>(maxLength: 500);
            builder.Property(x => x.PropertyType).HasColumnName(nameof(LotPropertyInfo.PropertyType)).HasEnumFieldValue<PropertySubType>(maxLength: 32);
            builder.Property(r => r.FemaFloodPlain).HasColumnName(nameof(LotPropertyInfo.FemaFloodPlain)).HasEnumCollectionValue<FemaFloodPlain>(25);
            builder.Property(r => r.TaxBlock).HasColumnName(nameof(LotPropertyInfo.TaxBlock)).HasMaxLength(25).IsRequired(false);
            builder.Property(r => r.LotSize).HasColumnName(nameof(LotPropertyInfo.LotSize)).HasMaxLength(25).IsRequired(false);
            builder.Property(r => r.PropCondition).HasColumnName(nameof(LotPropertyInfo.PropCondition)).HasEnumCollectionValue<PropCondition>(30);
            builder.Property(r => r.NumberOfPonds).HasColumnName(nameof(LotPropertyInfo.NumberOfPonds));
            builder.Property(r => r.NumberOfWells).HasColumnName(nameof(LotPropertyInfo.NumberOfWells));
            builder.Property(r => r.PropertySubType).HasColumnName(nameof(LotPropertyInfo.PropertySubType)).HasEnumFieldValue<PropertySubTypeLots>(9);
            builder.Property(r => r.SurfaceWater).HasColumnName(nameof(LotPropertyInfo.SurfaceWater)).HasDefaultValue(false);
            builder.Property(r => r.TypeOfHomeAllowed).HasColumnName(nameof(LotPropertyInfo.TypeOfHomeAllowed)).HasEnumCollectionValue<TypeOfHomeAllowed>(50);
            builder.Property(r => r.LiveStock).HasColumnName(nameof(LotPropertyInfo.LiveStock)).HasDefaultValue(false);
            builder.Property(r => r.CommercialAllowed).HasColumnName(nameof(LotPropertyInfo.CommercialAllowed)).HasDefaultValue(false);
            builder.Property(r => r.SoilType).HasColumnName(nameof(LotPropertyInfo.SoilType)).HasEnumCollectionValue<SoilType>(60);
            builder.Property(r => r.UpdateGeocodes).HasColumnName(nameof(LotPropertyInfo.UpdateGeocodes));
            builder.Property(p => p.AlsoListedAs).HasColumnName(nameof(LotPropertyInfo.AlsoListedAs));
            builder.Property(p => p.BuilderRestrictions).HasColumnName(nameof(LotPropertyInfo.BuilderRestrictions));
            builder.ConfigureGeocodes();
            builder.ConfigureCommonProperty();
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
            builder.Property(r => r.GroundWaterConservDistric).HasColumnName(nameof(LotFeaturesInfo.GroundWaterConservDistric)).HasDefaultValue(false);
            builder.Property(r => r.HorseAmenities).HasColumnName(nameof(LotFeaturesInfo.HorseAmenities)).HasEnumCollectionValue<HorseAmenities>(120);
            builder.Property(r => r.MineralsFeatures).HasColumnName(nameof(LotFeaturesInfo.MineralsFeatures)).HasEnumCollectionValue<Minerals>(30);
            builder.Property(r => r.RoadSurface).HasColumnName(nameof(LotFeaturesInfo.RoadSurface)).HasEnumCollectionValue<RoadSurface>(100);
            builder.Property(r => r.OtherStructures).HasColumnName(nameof(LotFeaturesInfo.OtherStructures)).HasEnumCollectionValue<OtherStructures>(315);
            builder.Property(r => r.NeighborhoodAmenities).HasColumnName(nameof(LotFeaturesInfo.NeighborhoodAmenities)).HasEnumCollectionValue<NeighborhoodAmenities>(1000);
            builder.Property(r => r.DocumentsAvailable).HasColumnName(nameof(LotFeaturesInfo.DocumentsAvailable)).HasEnumCollectionValue<DocumentsAvailable>(1805);
            builder.Property(r => r.Disclosures).HasColumnName(nameof(LotFeaturesInfo.Disclosures)).HasEnumCollectionValue<Disclosures>(240);
            builder.Property(r => r.WaterBodyName).HasColumnName(nameof(LotFeaturesInfo.WaterBodyName)).HasEnumFieldValue<WaterBodyName>(50);
        }

        private static void ConfigureFinancialMapping(OwnedNavigationBuilder<LotListing, LotFinancialInfo> builder)
        {
            builder.ConfigureLotFinancial();
            builder.Ignore(p => p.ReadableBuyersAgentCommission);
        }

        private static void ConfigureShowingMapping(OwnedNavigationBuilder<LotListing, LotShowingInfo> builder)
        {
            builder.Property(r => r.ShowingRequirements)
                .HasColumnName(nameof(LotShowingInfo.ShowingRequirements))
                .HasEnumCollectionValue<ShowingRequirements>(maxLength: 300);
            builder.Property(r => r.OwnerName).HasMaxLength(100);
            builder.Property(r => r.ShowingInstructions).HasColumnName(nameof(LotShowingInfo.ShowingInstructions)).HasMaxLength(2000);
            builder.Property(r => r.Directions).HasColumnName(nameof(LotShowingInfo.Directions)).HasMaxLength(1000);
            builder.Property(r => r.ApptPhone).HasColumnName(nameof(LotShowingInfo.ApptPhone)).HasMaxLength(12);
            builder.Property(r => r.ShowingServicePhone).HasColumnName(nameof(LotShowingInfo.ShowingServicePhone)).HasMaxLength(12);
            builder.Property(r => r.PublicRemarks).HasColumnName(nameof(LotShowingInfo.PublicRemarks)).HasMaxLength(1000);
            builder.Property(r => r.ShowingContactType).HasColumnName(nameof(LotShowingInfo.ShowingContactType)).HasEnumCollectionValue<ShowingContactType>(maxLength: 20);
            builder.Property(r => r.ShowingContactName).HasColumnName(nameof(LotShowingInfo.ShowingContactName)).HasMaxLength(100);
        }

        private static void ConfigureSchoolsMapping(OwnedNavigationBuilder<LotListing, LotSchoolsInfo> builder)
        {
            builder.ConfigureSchools();
        }
    }
}
