namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SchoolsExtensions
    {
        public static void ConfigureSchools<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, SchoolsInfo> builder)
            where TOwnerEntity : class
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(r => r.SchoolDistrict)
                .HasColumnName(nameof(SchoolsInfo.SchoolDistrict))
                .HasEnumFieldValue<SchoolDistrict>(maxLength: 50);

            builder.Property(r => r.ElementarySchool)
                .HasColumnName(nameof(SchoolsInfo.ElementarySchool))
                .HasEnumFieldValue<ElementarySchool>(maxLength: 50);

            builder.Property(r => r.MiddleSchool)
                .HasColumnName(nameof(SchoolsInfo.MiddleSchool))
                .HasEnumFieldValue<MiddleSchool>(maxLength: 50);

            builder.Property(r => r.HighSchool)
                .HasColumnName(nameof(SchoolsInfo.HighSchool))
                .HasEnumFieldValue<HighSchool>(maxLength: 50);

            builder.Property(r => r.OtherElementarySchool)
                .HasColumnName(nameof(SchoolsInfo.OtherElementarySchool))
                .HasEnumFieldValue<OtherElementarySchool>(maxLength: 50);

            builder.Property(r => r.OtherMiddleSchool)
                .HasColumnName(nameof(SchoolsInfo.OtherMiddleSchool))
                .HasEnumFieldValue<OtherMiddleSchool>(maxLength: 50);

            builder.Property(r => r.OtherHighSchool)
                .HasColumnName(nameof(SchoolsInfo.OtherHighSchool))
                .HasEnumFieldValue<OtherHighSchool>(maxLength: 50);
        }
    }
}
