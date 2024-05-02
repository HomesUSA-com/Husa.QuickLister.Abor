namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using ExtensionsRepositories = Husa.Quicklister.Extensions.Domain.Repositories;

    public interface ISaleListingRequestRepository : ExtensionsRepositories.IListingRequestRepository<SaleListingRequest>
    {
    }
}
