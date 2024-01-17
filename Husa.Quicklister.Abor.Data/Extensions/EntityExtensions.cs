namespace Husa.Quicklister.Abor.Data.Extensions
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class EntityExtensions
    {
        public static void SetListingProperties<T>(this EntityTypeBuilder<T> entity)
            where T : Listing
        {
            entity.SetBaseListingProperties();
            entity.Property(p => p.ListPrice).HasPrecision(18, 2);
            entity.Property(p => p.ListType).HasConversion<string>().HasMaxLength(30);
            entity.Property(p => p.MarketUniqueId).HasMaxLength(50);
            entity.Property(p => p.MlsStatus).HasConversion<string>().HasMaxLength(30).IsRequired();
            entity.Property(p => p.MarketModifiedOn).HasColumnType("datetime");
        }
    }
}
