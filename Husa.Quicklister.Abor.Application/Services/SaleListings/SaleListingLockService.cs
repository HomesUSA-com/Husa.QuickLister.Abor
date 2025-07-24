namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Extensions;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Husa.Quicklister.Extensions.Application.Services.Listings;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SaleListingLockService : ListingLockService<SaleListing, SaleListingRequest>, ISaleListingLockService
    {
        public SaleListingLockService(
            IListingSaleRepository listingRepository,
            ILogger<SaleListingLockService> logger,
            IUserContextProvider userContextProvider,
            IEmailService emailService,
            ISaleListingNotesService notesService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ISaleListingRequestRepository listingRequestRepository,
            IOptions<ApplicationOptions> applicationOptions)
            : base(listingRepository, logger, userContextProvider, emailService, notesService, serviceSubscriptionClient, listingRequestRepository, applicationOptions)
        {
        }

        protected override Func<SaleListing, UnlockedListingDto> UnlockedListingDtoProjection => ListingDtoExtensions.ToUnlockedListingDto;
    }
}
