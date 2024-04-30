namespace Husa.Quicklister.Abor.Data.Extensions
{
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class PublishInfoExtensions
    {
        public static void ConfigurePublishInfoMapping<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, PublishInfo> builder)
            where TOwnerEntity : class
        {
            builder.Property(r => r.PublishType).HasColumnName(nameof(PublishInfo.PublishType)).HasConversion<string>().HasMaxLength(20).IsRequired(false);
            builder.Property(r => r.PublishUser).HasColumnName(nameof(PublishInfo.PublishUser)).IsRequired(false);
            builder.Property(r => r.PublishStatus).HasColumnName(nameof(PublishInfo.PublishStatus)).HasConversion<string>().HasMaxLength(20).IsRequired(false);
            builder.Property(r => r.PublishDate).HasColumnName(nameof(PublishInfo.PublishDate)).IsRequired(false);
        }
    }
}
