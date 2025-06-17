namespace Husa.Quicklister.Abor.Application.Extensions
{
    using System;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Extensions.Application.Models.Listing;

    public static class ListingDtoExtensions
    {
        public static Func<SaleListing, UnlockedListingDto> ToUnlockedListingDto => listing => new()
        {
            MlsNumber = listing.MlsNumber,
            Address = listing.Address,
            CommunityId = listing.SaleProperty.Community?.Id ?? Guid.Empty,
            CommunityName = listing.SaleProperty.Community?.ProfileInfo.Name,
            CompanyId = listing.CompanyId,
            CompanyName = listing.SaleProperty.OwnerName,
            MarketCode = Husa.Extensions.Common.Enums.MarketCode.Austin,
        };

        public static Func<LotListing, UnlockedListingDto> ToUnlockedLotDto => listing => new()
        {
            MlsNumber = listing.MlsNumber,
            Address = listing.Address,
            CommunityId = listing.Community?.Id ?? Guid.Empty,
            CommunityName = listing.Community?.ProfileInfo.Name,
            CompanyId = listing.CompanyId,
            CompanyName = listing.OwnerName,
            MarketCode = Husa.Extensions.Common.Enums.MarketCode.Austin,
        };
    }
}
