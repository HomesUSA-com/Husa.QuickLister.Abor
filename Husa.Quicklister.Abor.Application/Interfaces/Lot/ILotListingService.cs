namespace Husa.Quicklister.Abor.Application.Interfaces.Lot
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface ILotListingService : IListingService
    {
        Task<CommandSingleResult<Guid, string>> CreateAsync(QuickCreateListingDto lotListing);
        Task<CommandResult<LotListing>> QuickCreateAsync(QuickCreateListingDto lotListing, bool importFromListing);
        Task UpdateListing(Guid listingId, LotListingDto listingDto);
        Task AssignMlsNumberAsync(Guid listingId, string mlsNumber, MarketStatuses requestStatus, ActionType actionType);
    }
}
