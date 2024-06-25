namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Domain.Entities;
    using Husa.Extensions.Domain.ValueObjects;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Logging;
    using HusaXmlExtensions = Husa.Quicklister.Extensions.Data.Extensions.XmlExtensions;

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

            var decoratedProperties = HusaXmlExtensions.GetDecoratedProperties(typeof(SaleListing));

            var modifiedProperties = this.GetModifiedProperties(entry);

            return decoratedProperties.Intersect(modifiedProperties).Any();
        }

        public async Task<IEnumerable<SaleListing>> GetListingsByCompanyId(Guid companyId)
        {
            var query = this.context.ListingSale.FilterNotDeleted().FilterByCompany(companyId);
            return await query.ToListAsync();
        }

        public async Task AddXmlRequestError(Guid listingId, string errorMessage)
        {
            var error = this.context.XmlRequestError.FirstOrDefault(error => error.ListingId == listingId);

            if (error == null)
            {
                error = new XmlRequestError(listingId, errorMessage);
                this.context.XmlRequestError.Add(error);
            }
            else
            {
                error.UpdateErrorMessage(errorMessage);
            }

            await this.SaveChangesAsync();
        }

        public async Task DeleteXmlRequestError(Guid listingId)
        {
            var error = this.context.XmlRequestError.FirstOrDefault(error => error.ListingId == listingId);
            if (error != null)
            {
                this.context.XmlRequestError.Remove(error);
                await this.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SaleListing>> GetListingsForDiscrepancyAsync(bool listingsInBoth)
        {
            var currentUser = this.userContextProvider.GetCurrentUser();

            var query = this.context.ListingSale
            .FilterNotDeleted()
            .FilterByCompany(currentUser.CompanyId, currentUser.UserRole == UserRole.User || currentUser.CompanyId.HasValue)
            .Where(l => (l.MlsStatus == MarketStatuses.Active || l.MlsStatus == MarketStatuses.Pending) &&
                l.ListPrice > 0);
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

        private IList<string> GetModifiedProperties<T>(EntityEntry<T> entry)
            where T : class
        {
            var modifiedProperties = new List<string>();
            var decoratedProperties = HusaXmlExtensions.GetDecoratedProperties(typeof(T));
            modifiedProperties.AddRange(entry.Properties
                .Where(p => p.IsModified && decoratedProperties.Contains(p.Metadata.Name))
                .Select(p => p.Metadata.Name));

            this.AddModifiedPropertiesFromOwnedEntities(entry, modifiedProperties, parentPath: string.Empty);
            return modifiedProperties;
        }

        private void AddModifiedPropertiesFromOwnedEntities(EntityEntry entry, IList<string> modifiedProperties, string parentPath)
        {
            var navigations = entry.Navigations.Where(navigationEntry => navigationEntry.IsLoaded).ToList();
            foreach (var navigationEntry in navigations)
            {
                var baseType = navigationEntry.Metadata.ClrType.BaseType;
                if (baseType != typeof(ValueObject) && baseType != typeof(Entity))
                {
                    continue;
                }

                var ownedEntityEntry = navigationEntry.CurrentValue != null ? this.context.Entry(navigationEntry.CurrentValue) : null;
                if (ownedEntityEntry != null)
                {
                    string currentPath = string.IsNullOrEmpty(parentPath) ? navigationEntry.Metadata.Name : $"{parentPath}.{navigationEntry.Metadata.Name}";
                    foreach (var property in ownedEntityEntry.Properties)
                    {
                        if (property.IsModified)
                        {
                            modifiedProperties.Add($"{currentPath}.{property.Metadata.Name}");
                        }
                    }

                    this.AddModifiedPropertiesFromOwnedEntities(ownedEntityEntry, modifiedProperties, currentPath);
                }
            }
        }
    }
}
