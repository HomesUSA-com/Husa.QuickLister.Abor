namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DinkToPdf.Contracts;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Quickbooks.Interfaces;
    using Husa.Extensions.Quickbooks.Models.Invoice;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Services.SaleListings;
    public class SaleListingBillService : ExtensionsServices.SaleListingBillService<
        SaleListing,
        IListingSaleRepository>, ISaleListingBillService
    {
        private readonly IListingSaleRepository listingSaleRepository;
        private readonly ILogger<SaleListingBillService> logger;
        public SaleListingBillService(
            IOptions<ApplicationOptions> options,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IListingSaleRepository listingSaleRepository,
            IQuickbooksApi quickbooksApi,
            IConverter converter,
            IUserContextProvider userContextProvider,
            ILogger<SaleListingBillService> logger)
            : base(options.Value.InvoiceSettings, converter, quickbooksApi, serviceSubscriptionClient, listingSaleRepository, userContextProvider, logger)
        {
            this.listingSaleRepository = listingSaleRepository ?? throw new ArgumentNullException(nameof(listingSaleRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<IEnumerable<SaleListing>> GetListings(IEnumerable<Guid> listingIds, Guid companyId)
        {
            this.logger.LogInformation("Getting listing list for companyId: {company}", companyId);
            var billingListings = new List<SaleListing>();
            foreach (var listingId in listingIds)
            {
                var listing = await this.listingSaleRepository.GetById(listingId);
                if (listing is null)
                {
                    continue;
                }

                if (listing.CompanyId == companyId)
                {
                    billingListings.Add(listing);
                }
            }

            return billingListings;
        }

        public override IEnumerable<BillingListingDto> GetBillingListings(IEnumerable<SaleListing> listings)
        {
            if (listings == null || !listings.Any())
            {
                return new List<BillingListingDto>();
            }

            var billingListings = new List<BillingListingDto>();
            foreach (var listing in listings)
            {
                var billDetail = new BillingListingDto()
                {
                    CompanyId = listing.CompanyId,
                    ListingId = listing.Id,
                    MlsNumber = listing.MlsNumber,
                    Subdivision = listing.SaleProperty.AddressInfo.Subdivision,
                    ListDate = listing.ListDate,
                    MarketStatus = listing.MlsStatus.ToString(),
                    PublishType = listing.PublishInfo.PublishType.Value.ToString(),
                    Market = MarketCode.SanAntonio,
                    StreetName = listing.SaleProperty.AddressInfo.StreetName,
                    StreetNum = listing.SaleProperty.AddressInfo.StreetNumber,
                };
                billingListings.Add(billDetail);
            }

            return billingListings;
        }

        public override async Task UpdateBilleableListings(IEnumerable<SaleListing> listings, string invoiceId, DateTime? createdDate, string docNumber = null)
        {
            var currentUserId = this.UserContextProvider.GetCurrentUserId();
            var iterableListings = listings.Select(x => x.InvoiceInfo).ToList();
            foreach (var listing in iterableListings)
            {
                listing.InvoiceRequestedOn = (DateTime)createdDate;
                listing.InvoiceRequestedBy = currentUserId;
                listing.InvoiceId = invoiceId;
                listing.DocNumber = docNumber;
            }

            this.logger.LogInformation("Updating billeable listings with invoice information");
            await this.listingSaleRepository.SaveChangesAsync();
        }
    }
}
