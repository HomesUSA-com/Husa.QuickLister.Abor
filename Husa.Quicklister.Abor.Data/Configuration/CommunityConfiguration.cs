namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueComparer;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Data.Extensions;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CommunityConfiguration : IEntityTypeConfiguration<CommunitySale>
    {
        public void Configure(EntityTypeBuilder<CommunitySale> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.SetSysProperties();
            builder.Property(r => r.CommunityType).HasConversion<string>().HasMaxLength(20);
            builder.Property(r => r.XmlStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(XmlStatus.NotFromXml)
                .IsRequired();
            builder.Property(f => f.LastPhotoRequestCreationDate).HasColumnType("datetime");

            builder.OwnsOne(o => o.SaleOffice, ConfigureSaleOffice);
            builder.OwnsOne(o => o.ProfileInfo, ConfigureProFile);
            builder.OwnsOne(o => o.Property, ConfigureProperty).Navigation(e => e.Property).IsRequired();
            builder.OwnsOne(o => o.Utilities, ConfigureUtilities);
            builder.OwnsOne(o => o.SchoolsInfo, ConfigureSchoolsMapping).Navigation(e => e.SchoolsInfo).IsRequired();
            builder.OwnsOne(o => o.Financial, ConfigureFinancial);
            builder.OwnsOne(o => o.Showing, ConfigureShowing).Navigation(e => e.Showing).IsRequired();
            builder.OwnsOne(o => o.EmailLead, ConfigureEmailLead).Navigation(e => e.EmailLead).IsRequired();
            builder.Property(x => x.Changes)
             .HasMaxLength(2000)
             .IsRequired(false)
             .HasConversion(new StringCollectionValueConverter(), valueComparer: new StringCollectionValueComparer());
        }

        private static void ConfigureShowing(OwnedNavigationBuilder<CommunitySale, CommunityShowingInfo> builder)
        {
            builder.ConfigureShowing();
        }

        private static void ConfigureFinancial(OwnedNavigationBuilder<CommunitySale, CommunityFinancialInfo> builder)
        {
            builder.ConfigureFinancial();
            builder.Ignore(p => p.ReadableBuyersAgentCommission);
        }

        private static void ConfigureProFile(OwnedNavigationBuilder<CommunitySale, ProfileInfo> builder)
        {
            builder.Property(r => r.Name).HasColumnName(nameof(ProfileInfo.Name)).IsRequired().HasMaxLength(50);
            builder.Property(r => r.OwnerName).HasColumnName(nameof(ProfileInfo.OwnerName)).HasMaxLength(100);
            builder.Property(r => r.OfficePhone).HasColumnName(nameof(ProfileInfo.OfficePhone)).HasMaxLength(14);
            builder.Property(r => r.BackupPhone).HasColumnName(nameof(ProfileInfo.BackupPhone)).HasMaxLength(14);
            builder.Property(r => r.Fax).HasColumnName(nameof(ProfileInfo.Fax)).HasMaxLength(20);
            builder.Property(r => r.UseLatLong).HasColumnName(nameof(ProfileInfo.UseLatLong));
            builder.Property(r => r.EmailMailViolationsWarnings).HasColumnName(nameof(ProfileInfo.EmailMailViolationsWarnings)).HasMaxLength(500);
            builder.ConfigureGeocodes();
        }

        private static void ConfigureSaleOffice(OwnedNavigationBuilder<CommunitySale, CommunitySaleOffice> builder)
        {
            builder.Property(x => x.StreetNumber).HasColumnName(nameof(CommunitySaleOffice.StreetNumber)).HasMaxLength(12);
            builder.Property(x => x.StreetName).HasColumnName(nameof(CommunitySaleOffice.StreetName)).HasMaxLength(50);
            builder.Property(x => x.StreetSuffix).HasColumnName(nameof(CommunitySaleOffice.StreetSuffix)).HasMaxLength(50);
            builder.Property(x => x.SalesOfficeCity)
                .HasColumnName(nameof(SalesOffice.SalesOfficeCity))
                .HasConversion<EnumFieldValueConverter<Cities>>()
                .HasMaxLength(50);

            builder.Property(x => x.SalesOfficeZip).HasColumnName(nameof(CommunitySaleOffice.SalesOfficeZip)).HasMaxLength(50);
            builder.Property(x => x.IsSalesOffice).HasColumnName(nameof(CommunitySaleOffice.IsSalesOffice));
        }

        private static void ConfigureEmailLead(OwnedNavigationBuilder<CommunitySale, EmailLead> builder)
        {
            builder.Property(x => x.EmailLeadPrincipal).HasColumnName(nameof(EmailLead.EmailLeadPrincipal)).HasMaxLength(60);
            builder.Property(x => x.EmailLeadSecondary).HasColumnName(nameof(EmailLead.EmailLeadSecondary)).HasMaxLength(60);
            builder.Property(x => x.EmailLeadOther).HasColumnName(nameof(EmailLead.EmailLeadOther)).HasMaxLength(60);
        }

        private static void ConfigureProperty(OwnedNavigationBuilder<CommunitySale, Property> builder)
        {
            builder.Property(r => r.City).HasColumnName(nameof(Property.City)).HasEnumFieldValue<Cities>(maxLength: 50);
            builder.Property(r => r.County).HasColumnName(nameof(Property.County)).HasEnumFieldValue<Counties>(maxLength: 20);
            builder.Property(x => x.Subdivision).HasColumnName(nameof(Property.Subdivision)).HasMaxLength(75);
            builder.Property(x => x.ZipCode).HasColumnName(nameof(Property.ZipCode)).HasMaxLength(12);
            builder.ConfigureProperty();
        }

        private static void ConfigureUtilities(OwnedNavigationBuilder<CommunitySale, Utilities> builder)
        {
            builder.ConfigureFeature();
        }

        private static void ConfigureSchoolsMapping(OwnedNavigationBuilder<CommunitySale, SchoolsInfo> builder)
        {
            builder.ConfigureSchools();
        }
    }
}
