namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Listing;
    using Microsoft.Extensions.Logging;
    public class CallForwardServiceTestClass : CallForwardService
    {
        public CallForwardServiceTestClass(
            IListingSaleRepository listingRepository,
            IUserContextProvider userContextProvider,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CallForwardServiceTestClass> logger)
            : base(listingRepository, userContextProvider, serviceSubscriptionClient, logger)
        {
        }

        public string PublicGetCommunityPhone(SaleListing listing)
        {
            return this.GetCommunityPhone(listing);
        }

        public (bool Centralized, string Phone) PublicGetCentralizedPhone(SaleListing listing, CompanyDetail company)
        {
            return this.GetCentralizedPhone(listing, company);
        }

        public ListingDetails PublicGetListingDetails(SaleListing listing)
        {
            return this.GetListingDetails(listing);
        }
    }
}
