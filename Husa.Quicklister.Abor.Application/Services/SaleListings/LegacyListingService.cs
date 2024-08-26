namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.SaleListings;
    public class LegacyListingService : ExtensionsServices.LegacyListingService<SaleListing>
    {
        public LegacyListingService(ILegacySaleListingRepository legacySaleListingRepository, ILogger<LegacyListingService> logger)
            : base(legacySaleListingRepository, logger)
        {
        }
    }
}
