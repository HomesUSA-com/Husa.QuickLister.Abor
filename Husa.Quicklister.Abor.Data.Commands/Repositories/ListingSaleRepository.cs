namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Specifications;
    using Husa.Extensions.Document.Models;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Data.Commands.Extensions;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Repositories;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.Azure.Cosmos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ListingSaleRepository : SavedChangesRepository<SaleListing, ApplicationDbContext>, IListingSaleRepository
    {
        private readonly List<MarketStatuses> openStatuses = new()
        {
            MarketStatuses.Active,
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Pending,
            MarketStatuses.Hold,
        };

        public ListingSaleRepository(
           ApplicationDbContext context,
           CosmosClient cosmosClient,
           IUserContextProvider userContextProvider,
           ILogger<ListingSaleRepository> logger,
           IOptions<DocumentDbSettings> options,
           IOptions<Crosscutting.ApplicationOptions> applicationOptions)
           : base(context, cosmosClient, userContextProvider, logger, options.Value.DatabaseName, options.Value.ListingChangesCollectionName, applicationOptions)
        {
        }

        public async Task<SaleListing> GetListingByLocationAsync(string mlsNumber, string streetNumber, string streetName, string zip, string unitNumber = null)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by locations - mls number: {mlsNumber}, address: {streetNumber} {streetName}, zip: {zip}", mlsNumber, streetNumber, streetName, zip);
            var query = this.context.ListingSale.Include(x => x.SaleProperty).Where(x => !x.IsDeleted);
            var saleListing = !string.IsNullOrEmpty(mlsNumber) ? await query.Where(x => x.MlsNumber == mlsNumber).FirstOrDefaultAsync() : null;
            query = query.Where(x => x.SaleProperty.AddressInfo.StreetNumber == streetNumber && x.SaleProperty.AddressInfo.StreetName == streetName && x.SaleProperty.AddressInfo.ZipCode == zip);

            if (!string.IsNullOrEmpty(unitNumber))
            {
                query = query.Where(l => l.SaleProperty.AddressInfo.UnitNumber == unitNumber);
            }

            return saleListing ?? await query.FirstOrDefaultAsync();
        }

        public async Task<SaleListing> GetListing(string streetNumber, string streetName, Cities city, string zipcode, string unitNumber = null)
        {
            this.logger.LogInformation("Starting to get the Sale listing with streetNumber: {streetNumber} streetName: {streetName} and zipcode: {zipcode}", streetNumber, streetName, zipcode);
            var query = this.context.ListingSale
                .Include(x => x.SaleProperty.Rooms)
                .Include(x => x.SaleProperty.OpenHouses)
                .Where(l => l.SaleProperty.AddressInfo.StreetName == streetName &&
                            l.SaleProperty.AddressInfo.StreetNumber == streetNumber &&
                            l.SaleProperty.AddressInfo.ZipCode == zipcode &&
                            l.SaleProperty.AddressInfo.City == city &&
                            this.openStatuses.Contains(l.MlsStatus) &&
                            !l.IsDeleted);
            if (!string.IsNullOrEmpty(unitNumber))
            {
                query = query.Where(l => l.SaleProperty.AddressInfo.UnitNumber == unitNumber);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SaleListing> GetListingByXmlListingId(Guid xmlListingId)
        {
            this.logger.LogInformation("Starting to get the Sale listing by xmlListingId: {xmlListingId}", xmlListingId);
            return await this.context.ListingSale
                .Include(x => x.SaleProperty.Rooms)
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
            this.logger.LogInformation("Starting to get ABOR list sale by mls number: {mlsNumber}", mlsNumber);
            return await this.context.ListingSale
                 .Include(x => x.SaleProperty)
                 .Include(x => x.SaleProperty.AddressInfo)
                 .FirstOrDefaultAsync(x => !x.IsDeleted && x.MlsNumber == mlsNumber);
        }

        public async Task<SaleListing> GetListingByLegacyId(int legacyId)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by legacy id: {legacyId}", legacyId);
            return await this.context.ListingSale
                 .Include(x => x.SaleProperty)
                 .FirstOrDefaultAsync(x => !x.IsDeleted && x.LegacyId == legacyId);
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

        public async Task<IEnumerable<SaleListing>> GetListingsByCompanyId(Guid companyId)
        {
            var query = this.context.ListingSale.FilterNotDeleted().FilterByCompany(companyId);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<SaleListing>> GetListingsForDiscrepancyAsync(bool listingsInBoth)
        {
            var currentUser = this.userContextProvider.GetCurrentUser();

            var query = this.context.ListingSale
            .FilterNotDeleted()
            .FilterByCompany(currentUser)
            .FilterByActiveOrPendingStatus()
            .WithPriceAbove();
            if (listingsInBoth)
            {
                query.Where(l => (l.XmlListingId != null || l.XmlDiscrepancyListingId != null) && !string.IsNullOrEmpty(l.MlsNumber));
            }
            else
            {
                query.Where(l => (l.XmlListingId != null || l.XmlDiscrepancyListingId != null || !string.IsNullOrEmpty(l.MlsNumber)));
            }

            return await query.ToListAsync();
        }

        protected override SavedChangesLog CreateChangesLog(SaleListing originalEntity, SaleListing updatedEntity)
        {
            var currentUser = this.userContextProvider.GetCurrentUser();
            var fieldChanges = new List<SummaryField>();
            ListingChangesLogExtensions.ProcessEntityChanges(fieldChanges, originalEntity, updatedEntity);
            ListingChangesLogExtensions.ProcessSalePropertyChanges(fieldChanges, originalEntity.SaleProperty, updatedEntity.SaleProperty);
            return new()
            {
                EntityId = updatedEntity.Id,
                UserId = currentUser.Id,
                UserName = currentUser.Name,
                Fields = fieldChanges,
            };
        }
    }
}
