namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.JsonImport.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Extensions.JsonImport;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.JsonImport;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using JsonExtensions = Husa.Quicklister.Extensions.Application.Services.JsonImport;

    public class ListingJsonImportService : JsonExtensions.ListingJsonImportService<
        SaleListing,
        CommunitySale,
        IListingSaleRepository,
        ICommunitySaleRepository>
    {
        private readonly ISaleListingService listingService;
        public ListingJsonImportService(
            IListingSaleRepository listingRepository,
            ICommunitySaleRepository communitySaleRepository,
            IUserContextProvider userContextProvider,
            IJsonImportClient jsonClient,
            ISaleListingService listingService,
            IListingRequestJsonImportService requestJsonImportService,
            IServiceSubscriptionClient companyClient,
            IOptions<ApplicationOptions> applicationOptions,
            ILogger<ListingJsonImportService> logger)
            : base(listingRepository, communitySaleRepository, userContextProvider, jsonClient, applicationOptions, requestJsonImportService, companyClient, logger)
        {
            this.listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
        }

        protected override async Task<SaleListing> CreateListing(SpecDetailResponse spec)
        {
            var listingDto = this.GetQuickCreateListingDto<QuickCreateListingDto, MarketStatuses, Cities, Counties>(spec.QlCompanyId.Value, spec);
            var quickCreateResult = await this.listingService.QuickCreateAsync(listingDto, importFromListing: false);
            if (quickCreateResult.Code == ResponseCode.Error)
            {
                throw new DomainException(quickCreateResult.Message);
            }

            return quickCreateResult.Results.Single();
        }

        protected override string GetMarketStatus(ListStatus specStatus) => specStatus.ToMarket().ToString();
        protected override void UpdateListing(SaleListing listing, SpecDetailResponse spec)
        {
            listing.Import(spec);
        }
    }
}
