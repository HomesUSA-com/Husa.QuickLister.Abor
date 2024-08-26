namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using ExtensionsRepositories = Husa.Quicklister.Extensions.Domain.Repositories;
    public interface ILegacySaleListingRepository : ExtensionsRepositories.ILegacySaleListingRepository<SaleListing>
    {
    }
}
