namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Linq.Specifications;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Lot;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class LotListingQueriesRepository : ILotListingQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserContextProvider userContext;
        private readonly ILogger<LotListingQueriesRepository> logger;
        private readonly IUserRepository userQueriesRepository;

        public LotListingQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserContextProvider userContext,
            ILogger<LotListingQueriesRepository> logger,
            IUserRepository userQueriesRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<DataSet<LotListingQueryResult>> GetAsync(ListingQueryFilter queryFilter)
        {
            this.logger.LogInformation("Starting to get the ABOR List Sales in Status : {mlsStatus}", queryFilter.MlsStatus);
            var currentUser = this.userContext.GetCurrentUser();

            var communityIds = new List<Guid>();
            if (queryFilter.CommunityId.HasValue)
            {
                communityIds = new List<Guid> { queryFilter.CommunityId.Value };
            }

            if (currentUser.EmployeeRole == RoleEmployee.SalesEmployeeReadonly && !queryFilter.CommunityId.HasValue && !queryFilter.PlanId.HasValue)
            {
                communityIds = await this.context.CommunityEmployee
                   .Where(e => !e.IsDeleted && e.UserId == currentUser.Id)
                   .Select(ce => ce.CommunityId)
                   .ToListAsync();
                if (communityIds.Count < 1)
                {
                    return new DataSet<LotListingQueryResult>(new List<LotListingQueryResult>() { }, 0);
                }
            }

            var query = this.context.LotListing
                .FilterNotDeleted()
                .FilterByCompany(currentUser)
                .FilterByCommunities(communityIds)
                .FilterByStatus(queryFilter.MlsStatus)
                .FilterBySearch(queryFilter.SearchBy)
                .FilterByStreetNumber(queryFilter.StreetNumber)
                .FilterByStreetName(queryFilter.StreetName)
                .FilterByMlsNumber(queryFilter.MlsNumber);
            var total = await query.CountAsync();
            var data = await query.Select(LotListingProjection.ProjectToLotListingQueryResult)
                 .ApplySortByFields(queryFilter.SortBy)
                 .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take, queryFilter.IsForDownloading)
                 .ToListAsync();

            return new DataSet<LotListingQueryResult>(data, total);
        }

        public async Task<LotListingQueryDetailResult> GetListing(Guid listingId)
        {
            this.logger.LogInformation("Starting to get the Sale listing with id {listingId}", listingId);
            var currentUser = this.userContext.GetCurrentUser();

            var listing = await this.context.LotListing
                .FilterById(listingId)
                .FilterByCompany(currentUser)
                .Include(x => x.Community)
                .Select(LotListingProjection.ProjectToLotListingQueryDetail)
                .SingleOrDefaultAsync();

            if (listing == null)
            {
                return listing;
            }

            await this.userQueriesRepository.FillUserNameAsync(listing);

            if (listing.LockedStatus == LockedStatus.LockedBySystem)
            {
                listing.LockedByUsername = UserConstants.LockedBySystemLabel;
            }

            return listing;
        }
    }
}
