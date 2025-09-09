namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Specifications;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Linq.Specifications;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Models;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories.SaleListing;
    using Husa.Quicklister.Extensions.Data.Queries.Specifications;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ListingSaleQueriesRepository : SaleListingQueriesRepository, IListingSaleQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly IPhotoServiceClient photoServiceClient;
        private readonly ApplicationOptions options;

        public ListingSaleQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserContextProvider userContext,
            ILogger<ListingSaleQueriesRepository> logger,
            IUserRepository userQueriesRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            IPhotoServiceClient photoServiceClient,
            IOptions<ApplicationOptions> options)
            : base(serviceSubscriptionClient, userQueriesRepository, userContext, logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.photoServiceClient = photoServiceClient ?? throw new ArgumentNullException(nameof(photoServiceClient));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<DataSet<ListingSaleQueryResult>> GetAsync(ListingQueryFilter queryFilter)
        {
            this.logger.LogInformation("Starting to get the ABOR List Sales in Status : {mlsStatus}", queryFilter.MlsStatus);
            var currentUser = this.userContext.GetCurrentUser();

            var (isSalesEmployee, communityIds) = await currentUser.GetSalesEmployeeCommunitiesAsync(this.context.CommunityEmployee, queryFilter.CommunityId);
            if (isSalesEmployee && !communityIds.Any())
            {
                return new DataSet<ListingSaleQueryResult>(new List<ListingSaleQueryResult>() { }, 0);
            }

            var query = this.context.ListingSale
                .FilterNotDeleted()
                .FilterInactives(queryFilter.Inactive)
                .FilterByCompany(currentUser)
                .FilterByCommunities(communityIds)
                .FilterByPlan(queryFilter.PlanId)
                .FilterByStatus(queryFilter.MlsStatus)
                .FilterByListed(queryFilter.ListedType, ListingSaleSpecifications.FilterByMlsStatusNotEqualTo<SaleListing>(MarketStatuses.Closed))
                .FilterBySearch(queryFilter.SearchBy)
                .FilterByStreetNumber(queryFilter.StreetNumber)
                .FilterByStreetName(queryFilter.StreetName)
                .FilterByIsCompleteHome(queryFilter.IsCompleteHome)
                .FilterByMlsNumber(queryFilter.MlsNumber)
                .FilterByCreationRange(queryFilter.CreationStartDate, queryFilter.CreationEndDate)
                .FilterBySqftRange(queryFilter.SqftMin, queryFilter.SqftMax);
            var total = await query.CountAsync();
            var data = await query.Select(ListingSaleProjection.ProjectToListingSaleQueryResult)
                 .ApplySortByCustomFields(queryFilter.SortBy)
                 .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take, queryFilter.IsForDownloading)
                 .ToListAsync();
            await this.userQueriesRepository.FillUsersNameAsync(data);

            return new DataSet<ListingSaleQueryResult>(data, total);
        }

        public async Task<ListingSaleQueryDetailResult> GetListing(Guid listingId)
        {
            this.logger.LogInformation("Starting to get the Sale listing with id {listingId}", listingId);
            var currentUser = this.userContext.GetCurrentUser();

            var listing = await this.context.ListingSale
                .FilterById(listingId)
                .FilterByCompany(currentUser)
                .Include(x => x.SaleProperty.Rooms)
                .Include(x => x.SaleProperty.OpenHouses)
                .Include(x => x.SaleProperty.Community)
                .Include(x => x.SaleProperty.Plan)
                .Select(ListingSaleProjection.ProjectToListingSaleQueryDetail)
                .SingleOrDefaultAsync();

            if (listing == null)
            {
                return listing;
            }

            var (isSalesEmployee, communityIds) = await currentUser.GetSalesEmployeeCommunitiesAsync(this.context.CommunityEmployee, listing.SaleProperty.SalePropertyInfo.CommunityId);
            if (isSalesEmployee && !communityIds.Any())
            {
                throw new UnauthorizedAccessException("User does not belong to the community.");
            }

            await this.FillSaleUserNameAsync(listing);

            if (listing.LockedStatus == LockedStatus.LockedBySystem)
            {
                listing.LockedByUsername = UserConstants.LockedBySystemLabel;
            }

            if (!this.options.FeatureFlags.FindEmailLeads)
            {
                this.logger.LogInformation("Skipping the call to GetCompanyEmailLeads");
                return listing;
            }

            var companyLeads = await this.GetCompanyEmailLeads(listing.SaleProperty.SalePropertyInfo.CompanyId);
            if (companyLeads != null)
            {
                listing.EmailLead = companyLeads;
            }

            return listing;
        }

        public async Task<ReversePorspectListingQueryResult> GetReverseProspectListing(Guid listingId, Guid userId, Guid companyId)
        {
            this.logger.LogInformation("Starting to get reverse prospect listing with id {listingId}", listingId);

            return await this.context.ListingSale
                .FilterById(listingId)
                .Select(ListingSaleProjection.ReverseProspectListingSaleQueryDetail)
                .SingleOrDefaultAsync();
        }

        public async Task<ListingSaleQueryDetailResult> GetListingByAddress(string streetName, string streetNumber, string zipCode)
        {
            this.logger.LogInformation("Starting to get the Sale listing by Address {streetName} {streetNumber}, zipcode: {zipCode}", streetName, streetNumber, zipCode);
            return await this.context.ListingSale
                .FilterByAddress(streetName, streetNumber, zipCode)
                .Include(x => x.SaleProperty.Rooms)
                .Include(x => x.SaleProperty.OpenHouses)
                .Select(ListingSaleProjection.ProjectToListingSaleQueryDetail)
                .SingleOrDefaultAsync();
        }

        public async Task<Property> GetListingPhotoProperty(Guid listingId, Guid propertyId, CancellationToken cancellationToken)
        {
            _ = await this.context.ListingSale.FilterById(listingId).SingleOrDefaultAsync(cancellationToken) ?? throw new NotFoundException<SaleListing>(listingId);
            return await this.photoServiceClient.Property.GetPropertyByIdAsync(propertyId, cancellationToken);
        }

        public async Task<DataSet<SaleListingOpenHouseQueryResult>> GetListingsWithOpenHouse(BaseQueryFilter queryFilter)
        {
            this.logger.LogInformation("Starting to get listings with Open House");
            var currentUser = this.userContext.GetCurrentUser();

            var query = this.context.ListingSale
                .FilterNotDeleted()
                .FilterByCompany(currentUser)
                .FilterByActiveAndPendingWithShowOpenHousesPendingActive()
                .HasOpenHouse();
            var total = await query.CountAsync();
            var data = await query.Select(ListingSaleProjection.ProjectToSaleListingOpenHouseQueryResult)
                 .ApplySortByNotProjectedFields(queryFilter.SortBy)
                 .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take)
                 .ToListAsync();

            return new DataSet<SaleListingOpenHouseQueryResult>(data, total);
        }

        public async Task<EmailLeadQueryResult> GetEmailLeads(Guid listingId)
        {
            var listing = await this.context.ListingSale
                .Include(x => x.SaleProperty)
                .Include(x => x.SaleProperty.Community)
                .FirstOrDefaultAsync(x => x.Id == listingId && x.SaleProperty.CommunityId.HasValue);
            if (listing == null)
            {
                return null;
            }

            var companyLeads = await this.GetCompanyEmailLeads(listing.SaleProperty.CompanyId);
            if (companyLeads is null)
            {
                return listing.SaleProperty.Community.ToProjectionEmailLead();
            }

            return companyLeads;
        }

        public async Task<IEnumerable<ListingLockedBySystemQueryResult>> GetLockedBySystem()
        {
            var data = await this.context.ListingSale
                .Where(l => l.LockedStatus == LockedStatus.LockedBySystem)
                .Select(ListingSaleProjection.ProjectToListingLockedBySystemQueryResult)
                .ToListAsync();
            return data;
        }

        protected override async Task<IEnumerable<InvalidTaxIdListingsQueryResult>> GetListingsWithInvalidTaxIdQuery()
        {
            return await this.context.ListingSale
                .FilterNotDeleted()
                .FilterByListed()
                .ExcludeAbcHomes()
                .FilterByActiveOrPendingStatus()
                .WithInvalidTaxId()
                .Select(ListingSaleProjection.ProjectToInvalidTaxIdListingsQueryResult)
                .ToListAsync();
        }
    }
}
