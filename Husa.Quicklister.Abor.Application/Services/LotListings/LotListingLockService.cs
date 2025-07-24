namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using System;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Extensions;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Husa.Quicklister.Extensions.Application.Services.Listings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class LotListingLockService : ListingLockService<LotListing, LotListingRequest>, ILotListingLockService
    {
        public LotListingLockService(
            ILotListingRepository listingRepository,
            ILogger<LotListingLockService> logger,
            IUserContextProvider userContextProvider,
            IEmailService emailService,
            ILotListingNotesService notesService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILotListingRequestRepository listingRequestRepository,
            IOptions<ApplicationOptions> applicationOptions)
            : base(listingRepository, logger, userContextProvider, emailService, notesService, serviceSubscriptionClient, listingRequestRepository, applicationOptions)
        {
        }

        protected override Func<LotListing, UnlockedListingDto> UnlockedListingDtoProjection => ListingDtoExtensions.ToUnlockedLotDto;
    }
}
