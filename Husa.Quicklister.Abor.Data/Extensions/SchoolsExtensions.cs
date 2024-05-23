namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SchoolsExtensions
    {
        public static void ConfigureSchools<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideSchool
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Property(r => r.SchoolDistrict)
                .HasColumnName(nameof(IProvideSchool.SchoolDistrict))
                .HasEnumFieldValue<SchoolDistrict>(maxLength: 50);

            builder.Property(r => r.ElementarySchool)
                .HasColumnName(nameof(IProvideSchool.ElementarySchool))
                .HasEnumFieldValue<ElementarySchool>(maxLength: 50);

            builder.Property(r => r.MiddleSchool)
                .HasColumnName(nameof(IProvideSchool.MiddleSchool))
                .HasEnumFieldValue<MiddleSchool>(maxLength: 50);

            builder.Property(r => r.HighSchool)
                .HasColumnName(nameof(IProvideSchool.HighSchool))
                .HasEnumFieldValue<HighSchool>(maxLength: 50);

            builder.Property(r => r.OtherElementarySchool)
                .HasColumnName(nameof(IProvideSchool.OtherElementarySchool))
                .HasMaxLength(50);

            builder.Property(r => r.OtherMiddleSchool)
                .HasColumnName(nameof(IProvideSchool.OtherMiddleSchool))
                .HasMaxLength(50);

            builder.Property(r => r.OtherHighSchool)
                .HasColumnName(nameof(IProvideSchool.OtherHighSchool))
                .HasMaxLength(50);
        }
    }
}
