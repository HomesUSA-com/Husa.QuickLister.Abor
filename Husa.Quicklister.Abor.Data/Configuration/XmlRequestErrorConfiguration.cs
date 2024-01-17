namespace Husa.Quicklister.Abor.Data.Configuration
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using ExtensionConfiguration = Husa.Quicklister.Extensions.Data.Configuration;

    public class XmlRequestErrorConfiguration : ExtensionConfiguration.XmlRequestErrorConfiguration<XmlRequestError>
    {
        public override void Configure(EntityTypeBuilder<XmlRequestError> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.SaleListing)
                .WithOne(p => p.XmlRequestError)
                .HasForeignKey<XmlRequestError>(e => e.ListingId);
        }
    }
}
