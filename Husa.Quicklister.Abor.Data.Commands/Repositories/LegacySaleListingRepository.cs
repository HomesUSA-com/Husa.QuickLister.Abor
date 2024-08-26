namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Repositories;
    public class LegacySaleListingRepository :
        ExtensionRepositories.LegacySaleListingRepository<ApplicationDbContext, SaleListing>,
        ILegacySaleListingRepository
    {
        public LegacySaleListingRepository(
            ApplicationDbContext context,
            IUserContextProvider userContextProvider,
            ILogger<LegacySaleListingRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
