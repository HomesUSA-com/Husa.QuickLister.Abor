namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using ExtensionsRepositories = Husa.Quicklister.Extensions.Domain.Repositories;

    public interface ILotListingRequestRepository : ExtensionsRepositories.IListingRequestRepository<LotListingRequest>
    {
    }
}
