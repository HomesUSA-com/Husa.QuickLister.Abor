namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ListingSaleRepository : Repository<SaleListing>, IListingSaleRepository
    {
        private readonly List<MarketStatuses> openStatuses = new()
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveOption,
            MarketStatuses.ActiveRFR,
            MarketStatuses.BackOnMarket,
            MarketStatuses.Pending,
            MarketStatuses.PendingSB,
            MarketStatuses.PriceChange,
            MarketStatuses.Withdrawn,
        };

        public ListingSaleRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<ListingSaleRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<SaleListing> GetListingByLocationAsync(string mlsNumber, string streetNumber, string streetName, string zip)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by locations - mls number: {mlsNumber}, address: {streetNumber} {streetName}, zip: {zip}", mlsNumber, streetNumber, streetName, zip);
            return await this.context.ListingSale
                 .Where(x => !x.IsDeleted
                 && ((!string.IsNullOrEmpty(x.MlsNumber) && x.MlsNumber == mlsNumber) || (x.SaleProperty.AddressInfo.StreetNumber == streetNumber && x.SaleProperty.AddressInfo.StreetName == streetName && x.SaleProperty.AddressInfo.ZipCode == zip)))
                 .Include(x => x.SaleProperty)
                 .FirstOrDefaultAsync();
        }

        public async Task<SaleListing> GetListing(string streetNumber, string streetName, Cities city, string zipcode)
        {
            this.logger.LogInformation("Starting to get the Sale listing with streetNumber: {streetNumber} streetName: {streetName} and zipcode: {zipcode}", streetNumber, streetName, zipcode);
            return await this.context.ListingSale
                .Include(x => x.SaleProperty.Rooms)
                .Include(x => x.SaleProperty.ListingSaleHoas)
                .Include(x => x.SaleProperty.OpenHouses)
                .Where(l => l.SaleProperty.AddressInfo.StreetName == streetName &&
                            l.SaleProperty.AddressInfo.StreetNumber == streetNumber &&
                            l.SaleProperty.AddressInfo.ZipCode == zipcode &&
                            l.SaleProperty.AddressInfo.City == city &&
                            this.openStatuses.Contains(l.MlsStatus) &&
                            !l.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<SaleListing> GetListingByXmlListingId(Guid xmlListingId)
        {
            this.logger.LogInformation("Starting to get the Sale listing by xmlListingId: {xmlListingId}", xmlListingId);
            return await this.context.ListingSale
                .Include(x => x.SaleProperty.Rooms)
                .Include(x => x.SaleProperty.ListingSaleHoas)
                .Include(x => x.SaleProperty.OpenHouses)
                .FirstOrDefaultAsync(l => l.XmlListingId == xmlListingId || l.XmlDiscrepancyListingId == xmlListingId);
        }

        public Task<SaleListing> GetListingByMlsNumber(Guid listingId, string mlsNumber)
        {
            this.logger.LogInformation("Starting to get ABOR Sale listing with MLS Number: {mlsNumber}", mlsNumber);
            return this.context
                .ListingSale
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.MlsNumber == mlsNumber && x.Id != listingId);
        }

        public async Task<SaleListing> GetListingByMlsNumber(string mlsNumber)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by locations - mls number: {mlsNumber}", mlsNumber);
            return await this.context.ListingSale
                 .Include(x => x.SaleProperty)
                 .FirstOrDefaultAsync(x => !x.IsDeleted && x.MlsNumber == mlsNumber);
        }

        public async Task<IEnumerable<SaleListing>> GetAutmaticMatchingListingsAsync(
            string streetName,
            string streetNumber,
            string zipCode,
            Guid companyId,
            bool partialMatch = false)
        {
            this.logger.LogInformation(
                "Starting get listing by streetNumber: {}, streetName: {}, zipCode: {} and CompanyId: {} for {type} xml match",
                streetNumber,
                streetName,
                zipCode,
                companyId,
                !partialMatch ? "exact" : "partial");

            var query = this.context.ListingSale
                .FilterNotDeleted()
                .FilterMatchingAddress(streetName, streetNumber, zipCode, companyId, partialMatch: partialMatch);

            return await query.ToListAsync();
        }
    }
}
