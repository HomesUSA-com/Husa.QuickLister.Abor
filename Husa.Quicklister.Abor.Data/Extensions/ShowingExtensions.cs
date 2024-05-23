namespace Husa.Quicklister.Abor.Data.Extensions
{
    using System;
    using Husa.Extensions.Linq;
    using Husa.Extensions.Linq.ValueComparer;
    using Husa.Extensions.Linq.ValueConverters;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class ShowingExtensions
    {
        public static void ConfigureShowing<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder)
            where TOwnerEntity : class
            where TDependentEntity : class, IProvideShowingInfo
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Property(r => r.OccupantPhone).HasColumnName(nameof(IProvideShowingInfo.OccupantPhone)).HasMaxLength(14);
            builder.Property(r => r.ContactPhone).HasColumnName(nameof(IProvideShowingInfo.ContactPhone)).HasMaxLength(14).IsRequired(false);
            builder.Property(r => r.ShowingInstructions)
                .HasColumnName(nameof(IProvideShowingInfo.ShowingInstructions))
                .HasMaxLength(2000);
            builder.Property(r => r.ShowingRequirements)
                .HasColumnName(nameof(IProvideShowingInfo.ShowingRequirements))
                .HasEnumCollectionValue<ShowingRequirements>(maxLength: 300);
            builder.Property(r => r.LockBoxType)
                .HasColumnName(nameof(IProvideShowingInfo.LockBoxType))
                .HasEnumFieldValue<LockBoxType>(maxLength: 30);
            builder.Property(r => r.Directions).HasColumnName(nameof(IProvideShowingInfo.Directions)).HasMaxLength(2000).IsRequired(false);
            builder.Property(r => r.OwnerName).HasColumnName("ShowingInfoOwnerName").HasMaxLength(100);
            builder.Property(r => r.RealtorContactEmail).HasColumnName(nameof(IProvideShowingInfo.RealtorContactEmail))
               .HasMaxLength(500)
               .IsRequired(false)
               .HasConversion(new StringCollectionValueConverter(";"), valueComparer: new StringCollectionValueComparer());
        }
    }
}
