namespace Husa.Quicklister.Abor.Application.Interfaces.Listing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Models.SalePropertyDetail;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;

    public interface ISaleListingService
    {
        Task<CommandSingleResult<Guid, string>> CreateAsync(ListingSaleDto listingSale);

        Task<CommandResult<SaleListing>> QuickCreateAsync(ListingSaleDto listingSale, bool importFromListing);

        Task UpdateListing(Guid listingId, SaleListingDto listingDto);

        Task DeleteListing(Guid listingId);

        Task UpdateBaseListingInfo(SaleListingDto saleListingDto, Guid listingId = default, SaleListing entity = null);

        Task UpdatePropertyInfo(PropertyDto propertyDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateAddressInfo(AddressDto addressDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateFeaturesInfo(FeaturesDto featuresDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateFinancialInfo(FinancialDto financialDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateSchoolsInfo(Models.SchoolsDto schoolsDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateSpacesDimensionsInfo(SpacesDimensionsDto spacesDimensionsDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateShowingInfo(ShowingDto showingDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateRooms(IEnumerable<RoomDto> roomDto, Guid listingId = default, SaleListing entity = null);

        Task UpdateOpenHouse(IEnumerable<OpenHouseDto> openHouseDto, SaleListing entity = null);

        Task<SaleListing> GetEntity(SaleListing entity = null, Guid listingId = default);

        Task ChangeCommunity(Guid listingId, Guid communityId);

        Task ChangePlan(Guid listingId, Guid planId, bool updateRooms = false);

        Task AssignMlsNumberAsync(Guid listingId, string mlsNumber, MarketStatuses requestStatus, ActionType actionType);

        Task<SaleListing> SaveListingChanges(Guid listingId, ListingSaleRequestDto listingSaleDto);

        Task<CommandResult<string>> UnlockListing(Guid listingId, CancellationToken cancellationToken = default);

        Task<CommandResult<string>> CloseListing(Guid listingId);
        Task LockListingByUser(Guid listingId);

        Task DeclinePhotos(Guid listingId, CancellationToken cancellationToken = default);

        Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken cancellationToken = default);
    }
}
