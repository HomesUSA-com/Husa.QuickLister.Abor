namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using System;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Husa.Quicklister.Extensions.Application.Services.Listings;
    using Microsoft.Extensions.Logging;

    public class LotListingDeletionService : ListingDeletionService<LotListing>, ILotListingDeletionService
    {
        public LotListingDeletionService(
            ILotListingRepository listingRepository,
            ILogger<LotListingDeletionService> logger,
            IUserContextProvider userContextProvider,
            IEmailService emailService)
            : base(listingRepository, logger, userContextProvider, emailService)
        {
        }

        protected override Func<LotListing, DraftListingDto> DraftListingDtoProjection => listing => new()
        {
            Address = listing.Address,
            CommunityId = listing.Community?.Id ?? Guid.Empty,
            CommunityName = listing.Community?.ProfileInfo.Name,
            CompanyId = listing.CompanyId,
            CompanyName = listing.OwnerName,
            MarketCode = Husa.Extensions.Common.Enums.MarketCode.Austin,
        };
    }
}
