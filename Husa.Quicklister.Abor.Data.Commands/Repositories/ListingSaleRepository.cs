namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Attributes;
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
            MarketStatuses.ActiveUnderContract,
            MarketStatuses.Pending,
            MarketStatuses.Hold,
        };

        public ListingSaleRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<ListingSaleRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<SaleListing> GetListingByLocationAsync(string mlsNumber, string streetNumber, string streetName, string zip)
        {
            this.logger.LogInformation("Starting to get ABOR list sale by locations - mls number: {mlsNumber}, address: {streetNumber} {streetName}, zip: {zip}", mlsNumber, streetNumber, streetName, zip);
            var query = this.context.ListingSale.Include(x => x.SaleProperty).Where(x => !x.IsDeleted);
            var saleListing = !string.IsNullOrEmpty(mlsNumber) ? await query.Where(x => x.MlsNumber == mlsNumber).FirstOrDefaultAsync() : null;

            return saleListing ?? await query.Where(x => x.SaleProperty.AddressInfo.StreetNumber == streetNumber && x.SaleProperty.AddressInfo.StreetName == streetName && x.SaleProperty.AddressInfo.ZipCode == zip)
                 .FirstOrDefaultAsync();
        }

        public async Task<SaleListing> GetListing(string streetNumber, string streetName, Cities city, string zipcode)
        {
            this.logger.LogInformation("Starting to get the Sale listing with streetNumber: {streetNumber} streetName: {streetName} and zipcode: {zipcode}", streetNumber, streetName, zipcode);
            return await this.context.ListingSale
                .Include(x => x.SaleProperty.Rooms)
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

        public bool HasXmlChanges(SaleListing entity)
        {
            if (entity == null)
            {
                this.logger.LogError("{parameterName} entity must not be nulll {entityType}", nameof(entity), typeof(SaleListing).Name);
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} entity must not be nulll {typeof(SaleListing).Name}");
            }

            this.context.ChangeTracker.DetectChanges();

            var entry = this.context.Entry(entity);

            if (entry.State != EntityState.Modified)
            {
                return false;
            }

            var decoratedProperties = this.GetDecoratedProperties(typeof(SaleListing));

            var modifiedProperties = entry.Properties
                .Where(p => p.IsModified)
                .Select(p => p.Metadata.Name);

            return decoratedProperties.Intersect(modifiedProperties).Any();
        }

        public async Task<IEnumerable<SaleListing>> GetListingsByCompanyId(Guid companyId)
        {
            var query = this.context.ListingSale.FilterNotDeleted().FilterByCompany(companyId);
            return await query.ToListAsync();
        }

        private List<string> GetDecoratedProperties(Type type, string prefix = "")
        {
            var decoratedProperties = new List<string>();

            foreach (var property in type.GetProperties())
            {
                var fullName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                if (Attribute.IsDefined(property, typeof(XmlPropertyUpdateAttribute)))
                {
                    decoratedProperties.Add(fullName);
                }

                if (property.PropertyType.Namespace == "System" || property.PropertyType.IsGenericType)
                {
                    continue;
                }

                decoratedProperties.AddRange(this.GetDecoratedProperties(property.PropertyType, fullName));
            }

            return decoratedProperties;
        }
    }
}
