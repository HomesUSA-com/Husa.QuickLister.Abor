namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Husa.Quicklister.Extensions.Application.Services.Listings;
    using Microsoft.Extensions.Logging;

    public class SaleListingDeletionService : ListingDeletionService<SaleListing>
    {
        public SaleListingDeletionService(
            IListingSaleRepository listingRepository,
            ILogger<SaleListingDeletionService> logger,
            IUserContextProvider userContextProvider,
            IEmailService emailService,
            ISaleListingMediaService saleListingMediaService)
            : base(listingRepository, logger, userContextProvider, emailService, saleListingMediaService)
        {
        }

        protected override Func<SaleListing, DraftListingDto> DraftListingDtoProjection => listing => new()
        {
            Address = listing.Address,
            CommunityId = listing.SaleProperty.Community?.Id ?? Guid.Empty,
            CommunityName = listing.SaleProperty.Community?.ProfileInfo.Name,
            CompanyId = listing.CompanyId,
            CompanyName = listing.SaleProperty?.OwnerName,
            MarketCode = Husa.Extensions.Common.Enums.MarketCode.Austin,
        };
    }
}
