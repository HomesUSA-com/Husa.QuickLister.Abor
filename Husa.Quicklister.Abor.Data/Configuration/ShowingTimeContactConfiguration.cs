namespace Husa.Quicklister.Abor.Data.Configuration
{
    using System;
    using Husa.Extensions.ShowingTime.Data.DbConfiguration;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShowingTimeContactConfiguration : IEntityTypeConfiguration<ShowingTimeContact>
    {
        public void Configure(EntityTypeBuilder<ShowingTimeContact> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable(nameof(ShowingTimeContact));
            builder.SetSysProperties();
            builder.ConfigureShowingTimeContact();
            builder.HasMany(o => o.Communities)
                .WithMany(o => o.ShowingTimeContacts)
                .UsingEntity<CommunityShowingTimeContact>();
            builder.HasMany(o => o.Listings)
                .WithMany(o => o.ShowingTimeContacts)
                .UsingEntity<ListingShowingTimeContact>();
        }
    }
}
