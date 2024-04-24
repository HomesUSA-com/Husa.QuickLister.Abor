namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Repositories;

    public interface ILotListingRepository : IListingRepository<LotListing>
    {
        Task<IEnumerable<LotListing>> GetListingsByAddress<TAddress>(TAddress address, IEnumerable<MarketStatuses> excludeStatuses = null)
            where TAddress : IProvideAddress;
    }
}
