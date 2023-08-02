namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

    public class SalePropertyConfiguration : IEntityTypeConfiguration<SaleProperty>
    {
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
            builder.OwnsOne(o => o.AddressInfo, ConfigureAddressInfoMapping);
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
            var converter = new ValueConverter<ConstructionStage, string>(constructionStage => constructionStage.GetEnumDescription(), constructionStageDescription => constructionStageDescription.GetEnumValueFromDescription<ConstructionStage>());

            builder.Property(x => x.ConstructionCompletionDate).HasColumnName(nameof(PropertyInfo.ConstructionCompletionDate)).IsRequired(false);
            builder.Property(x => x.ConstructionStage).HasConversion<string>().HasColumnName(nameof(PropertyInfo.ConstructionStage)).HasConversion(converter).HasMaxLength(1);
            builder.Property(r => r.ConstructionStartYear).HasColumnName(nameof(PropertyInfo.ConstructionStartYear)).IsRequired(false);
            builder.Property(r => r.IsXmlManaged).HasColumnName(nameof(PropertyInfo.IsXmlManaged));
            builder.Property(r => r.Latitude).HasColumnName(nameof(PropertyInfo.Latitude)).HasPrecision(32, 12);
            builder.Property(r => r.LegalDescription).HasColumnName(nameof(PropertyInfo.LegalDescription)).HasMaxLength(60).IsRequired(false);
            builder.Property(r => r.Longitude).HasColumnName(nameof(PropertyInfo.Longitude)).HasPrecision(32, 12);
            builder.Property(r => r.LotDescription).HasColumnName(nameof(PropertyInfo.LotDescription)).HasEnumCollectionValue<LotDescription>(296);
            builder.Property(r => r.LotDimension).HasColumnName(nameof(PropertyInfo.LotDimension)).HasMaxLength(25);
            builder.Property(r => r.LotSize).HasColumnName(nameof(PropertyInfo.LotSize)).HasMaxLength(25).IsRequired(false);
            builder.Property(r => r.MapscoGrid).HasColumnName(nameof(PropertyInfo.MapscoGrid)).HasMaxLength(5);

            builder.Property(r => r.MlsArea)
               .HasColumnName(nameof(PropertyInfo.MlsArea))
               .HasEnumFieldValue<MlsArea>(maxLength: 5);

            builder.Property(r => r.Occupancy).HasColumnName(nameof(PropertyInfo.Occupancy)).HasEnumCollectionValue<Occupancy>(255);
            builder.Property(r => r.TaxId).HasColumnName(nameof(PropertyInfo.TaxId)).HasMaxLength(25);
            builder.Property(r => r.UpdateGeocodes).HasColumnName(nameof(PropertyInfo.UpdateGeocodes));
        }

        private static void ConfigureAddressInfoMapping(OwnedNavigationBuilder<SaleProperty, AddressInfo> builder)
        {
            builder.Property(x => x.StreetNumber).HasColumnName(nameof(AddressInfo.StreetNumber)).HasMaxLength(12);
            builder.Property(r => r.StreetName).HasColumnName(nameof(AddressInfo.StreetName)).HasMaxLength(50);

            builder.Property(r => r.City)
                .HasColumnName(nameof(AddressInfo.City))
                .HasEnumFieldValue<Cities>(maxLength: 50, isRequired: true);

            builder.Property(r => r.State).HasConversion<string>().HasColumnName(nameof(AddressInfo.State)).HasMaxLength(2).HasConversion<EnumFieldValueConverter<States>>();
            builder.Property(r => r.ZipCode).HasColumnName(nameof(AddressInfo.ZipCode)).HasMaxLength(12);
            builder.Property(r => r.County)
               .HasColumnName(nameof(AddressInfo.County))
                .HasEnumFieldValue<Counties>(maxLength: 20, isRequired: false);

            builder.Property(r => r.Block).HasColumnName(nameof(AddressInfo.Block)).HasMaxLength(5).IsRequired(false);
            builder.Property(r => r.Subdivision).HasColumnName(nameof(AddressInfo.Subdivision)).HasMaxLength(75).IsRequired(false);
            builder.Property(r => r.LotNum).HasColumnName(nameof(AddressInfo.LotNum)).HasMaxLength(6).IsRequired(false);
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

        private static void ConfigureSpacesDimensionsMapping(OwnedNavigationBuilder<SaleProperty, SpacesDimensionsInfo> builder)
        {
            var converter = new ValueConverter<CategoryType, string>(categoryType => categoryType.GetEnumDescription(), categoryTypeDescription => categoryTypeDescription.GetEnumValueFromDescription<CategoryType>());

            builder.Property(x => x.TypeCategory).HasConversion<string>().HasColumnName(nameof(SpacesDimensionsInfo.TypeCategory)).HasConversion(converter).HasMaxLength(5);
            builder.Property(r => r.SqFtSource)
                .HasColumnName(nameof(SpacesDimensionsInfo.SqFtSource))
                .HasConversion<EnumFieldValueConverter<SqFtSource>>()
                .HasMaxLength(10);
            builder.Property(r => r.SpecialtyRooms).HasColumnName(nameof(SpacesDimensionsInfo.SpecialtyRooms)).HasEnumCollectionValue<SpecialtyRooms>(400);
            builder.Property(r => r.OtherParking).HasColumnName(nameof(SpacesDimensionsInfo.OtherParking)).HasEnumCollectionValue<OtherParking>(40);

            builder.ConfigureSpacesDimensions();
        }

        private static void ConfigureFeaturesMapping(OwnedNavigationBuilder<SaleProperty, FeaturesInfo> builder)
        {
            builder.Property(r => r.PropertyDescription).HasColumnName(nameof(FeaturesInfo.PropertyDescription)).HasMaxLength(4000).IsRequired(false);
            builder.Property(r => r.Inclusions).HasColumnName(nameof(FeaturesInfo.Inclusions)).HasEnumCollectionValue<Inclusions>(500);
            builder.Property(r => r.Fireplaces).HasColumnName(nameof(FeaturesInfo.Fireplaces)).HasMaxLength(20);
            builder.Property(r => r.FireplaceDescription).HasColumnName(nameof(FeaturesInfo.FireplaceDescription)).HasEnumCollectionValue<FireplaceDescription>(256);
            builder.Property(r => r.Floors).HasColumnName(nameof(FeaturesInfo.Floors)).HasEnumCollectionValue<Floors>(300);
            builder.Property(r => r.WindowCoverings).HasColumnName(nameof(FeaturesInfo.WindowCoverings)).HasEnumCollectionValue<WindowCoverings>(100);
            builder.Property(r => r.Accessibility).HasColumnName(nameof(FeaturesInfo.Accessibility)).HasEnumCollectionValue<Accessibility>(200);
            builder.Property(r => r.HousingStyle).HasColumnName(nameof(FeaturesInfo.HousingStyle)).HasEnumCollectionValue<HousingStyle>(255);
            builder.Property(r => r.ExteriorFeatures).HasColumnName(nameof(FeaturesInfo.ExteriorFeatures)).HasEnumCollectionValue<ExteriorFeatures>(500);
            builder.Property(r => r.RoofDescription).HasColumnName(nameof(FeaturesInfo.RoofDescription)).HasEnumCollectionValue<RoofDescription>(145);
            builder.Property(r => r.Foundation).HasColumnName(nameof(FeaturesInfo.Foundation)).HasEnumCollectionValue<Foundation>(94);
            builder.Property(r => r.Exterior).HasColumnName(nameof(FeaturesInfo.Exterior)).HasEnumCollectionValue<Exterior>(157);
            builder.Property(r => r.PrivatePool).HasColumnName(nameof(FeaturesInfo.PrivatePool)).HasEnumCollectionValue<PrivatePool>(256);
            builder.Property(r => r.HomeFaces).HasColumnName(nameof(FeaturesInfo.HomeFaces)).HasEnumCollectionValue<HomeFaces>(14);
            builder.Property(r => r.SupplierElectricity).HasColumnName(nameof(FeaturesInfo.SupplierElectricity)).HasMaxLength(60);
            builder.Property(r => r.SupplierWater).HasColumnName(nameof(FeaturesInfo.SupplierWater)).HasMaxLength(25);
            builder.Property(r => r.SupplierGarbage).HasColumnName(nameof(FeaturesInfo.SupplierGarbage)).HasMaxLength(25);
            builder.Property(r => r.SupplierGas).HasColumnName(nameof(FeaturesInfo.SupplierGas)).HasMaxLength(25);
            builder.Property(r => r.SupplierSewer).HasColumnName(nameof(FeaturesInfo.SupplierSewer)).HasMaxLength(25);
            builder.Property(r => r.SupplierOther).HasColumnName(nameof(FeaturesInfo.SupplierOther)).HasMaxLength(25);
            builder.Property(r => r.HeatSystem).HasColumnName(nameof(FeaturesInfo.HeatSystem)).HasEnumCollectionValue<HeatingSystem>(255);
            builder.Property(r => r.CoolingSystem).HasColumnName(nameof(FeaturesInfo.CoolingSystem)).HasEnumCollectionValue<CoolingSystem>(100);
            builder.Property(r => r.HeatingFuel).HasColumnName(nameof(FeaturesInfo.HeatingFuel)).HasEnumCollectionValue<HeatingFuel>(100);
            builder.Property(r => r.WaterSewer).HasColumnName(nameof(FeaturesInfo.WaterSewer)).HasEnumCollectionValue<WaterSewer>(255);
            builder.Property(r => r.EnergyFeatures).HasColumnName(nameof(FeaturesInfo.EnergyFeatures)).HasEnumCollectionValue<EnergyFeatures>(176);
            builder.Property(r => r.GreenCertification).HasColumnName(nameof(FeaturesInfo.GreenCertification)).HasEnumCollectionValue<GreenCertification>(80);
            builder.Property(r => r.GreenFeatures).HasColumnName(nameof(FeaturesInfo.GreenFeatures)).HasEnumCollectionValue<GreenFeatures>(90);
            builder.Property(x => x.NeighborhoodAmenities).HasColumnName(nameof(Utilities.NeighborhoodAmenities)).HasEnumCollectionValue<NeighborhoodAmenities>(286);

            builder.Property(r => r.LotImprovements).HasColumnName(nameof(FeaturesInfo.LotImprovements)).HasEnumCollectionValue<LotImprovements>(359);

            builder.Property(r => r.HasAccessibility).HasColumnName(nameof(FeaturesInfo.HasAccessibility));
            builder.Property(r => r.HasPrivatePool).HasColumnName(nameof(FeaturesInfo.HasPrivatePool));
            builder.Property(r => r.IsNewConstruction).HasColumnName(nameof(FeaturesInfo.IsNewConstruction));
        }

        private static void ConfigureFinancialMapping(OwnedNavigationBuilder<SaleProperty, FinancialInfo> builder)
        {
            var converter = new ValueConverter<CommissionType, string>(bonusType => bonusType.GetEnumDescription(), bonusTypeDescription => bonusTypeDescription.GetEnumValueFromDescription<CommissionType>());

            builder
                .Property(r => r.TaxRate)
                .HasColumnName(nameof(FinancialInfo.TaxRate))
                .HasPrecision(18, 2)
                .IsRequired(false);

            builder.Property(r => r.TaxYear).HasColumnName(nameof(FinancialInfo.TaxYear)).HasMaxLength(4);
            builder.Property(r => r.TitleCompany).HasColumnName(nameof(FinancialInfo.TitleCompany)).HasMaxLength(45).IsRequired(false);
            builder.Property(r => r.ProposedTerms).HasColumnName(nameof(FinancialInfo.ProposedTerms)).HasEnumCollectionValue<ProposedTerms>(100);
            builder.Property(r => r.HOARequirement)
                .HasColumnName(nameof(FinancialInfo.HOARequirement))
                .HasMaxLength(10)
                .HasConversion<EnumFieldValueConverter<HoaRequirement>>();

            builder.Property(r => r.NumHOA).HasColumnName(nameof(FinancialInfo.NumHOA)).HasMaxLength(2);
            builder.Property(r => r.BuyersAgentCommission).HasPrecision(18, 2).HasColumnName(nameof(FinancialInfo.BuyersAgentCommission)).HasMaxLength(6);
            builder.Property(r => r.BuyersAgentCommissionType)
                .HasColumnName(nameof(FinancialInfo.BuyersAgentCommissionType))
                .HasConversion(converter)
                .HasMaxLength(1);

            builder.Property(r => r.IsMultipleTaxed).HasColumnName(nameof(FinancialInfo.IsMultipleTaxed));
            builder.Property(r => r.HasAgentBonus).HasColumnName(nameof(FinancialInfo.HasAgentBonus));
            builder.Property(r => r.HasBonusWithAmount).HasColumnName(nameof(FinancialInfo.HasBonusWithAmount));
            builder
                .Property(r => r.AgentBonusAmount)
                .HasColumnName(nameof(FinancialInfo.AgentBonusAmount))
                .HasPrecision(18, 2);

            builder.Property(r => r.AgentBonusAmountType).HasColumnName(nameof(FinancialInfo.AgentBonusAmountType)).HasConversion(converter).HasMaxLength(1);
            builder.Property(r => r.BonusExpirationDate).HasColumnName(nameof(FinancialInfo.BonusExpirationDate));
            builder.Property(r => r.HasBuyerIncentive).HasColumnName(nameof(FinancialInfo.HasBuyerIncentive));

            builder.Ignore(p => p.ReadableBuyersAgentCommission);
        }

        private static void ConfigureShowingMapping(OwnedNavigationBuilder<SaleProperty, ShowingInfo> builder)
        {
            builder.Property(r => r.AltPhoneCommunity).HasColumnName(nameof(ShowingInfo.AltPhoneCommunity)).HasMaxLength(14);
            builder.Property(r => r.AgentListApptPhone).HasColumnName(nameof(ShowingInfo.AgentListApptPhone)).HasMaxLength(14).IsRequired(false);
            builder.Property(r => r.Showing)
                .HasColumnName(nameof(IProvideShowingInfo.Showing))
                .HasMaxLength(10)
                .HasConversion<EnumFieldValueConverter<Showing>>();

            builder.Property(r => r.RealtorContactEmail).HasColumnName(nameof(ShowingInfo.RealtorContactEmail)).HasMaxLength(255);
            builder.Property(r => r.Directions).HasColumnName(nameof(ShowingInfo.Directions)).HasMaxLength(255).IsRequired(false);
            builder.Property(r => r.AgentPrivateRemarks).HasColumnName(nameof(ShowingInfo.AgentPrivateRemarks)).HasMaxLength(1024);
            builder.Property(r => r.EnableOpenHouses).HasColumnName(nameof(ShowingInfo.EnableOpenHouses));
            builder.Property(r => r.OpenHousesAgree).HasColumnName(nameof(ShowingInfo.OpenHousesAgree));
            builder.Property(r => r.ShowOpenHousesPending).HasColumnName(nameof(ShowingInfo.ShowOpenHousesPending));
        }

        private static void ConfigureSchoolsMapping(OwnedNavigationBuilder<SaleProperty, SchoolsInfo> builder)
        {
            builder.Property(r => r.SchoolDistrict)
                .HasColumnName(nameof(SchoolsInfo.SchoolDistrict))
                .HasMaxLength(50)
                .HasConversion<EnumFieldValueConverter<SchoolDistrict>>();

            builder.Property(r => r.ElementarySchool)
                .HasColumnName(nameof(SchoolsInfo.ElementarySchool))
                .HasMaxLength(50)
                .HasConversion<EnumFieldValueConverter<ElementarySchool>>();

            builder.Property(r => r.MiddleSchool)
                .HasColumnName(nameof(SchoolsInfo.MiddleSchool))
                .HasMaxLength(50)
                .HasConversion<EnumFieldValueConverter<MiddleSchool>>();

            builder.Property(r => r.HighSchool)
                .HasColumnName(nameof(SchoolsInfo.HighSchool))
                .HasMaxLength(50)
                .HasConversion<EnumFieldValueConverter<HighSchool>>();
        }
    }
}
