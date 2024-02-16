namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Linq.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class PlanQueriesRepository : IPlanQueriesRepository
    {
        private readonly ILogger<PlanQueriesRepository> logger;
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserRepository userQueriesRepository;
        private readonly IUserContextProvider userContext;

        public PlanQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserContextProvider userContext,
            IUserRepository userQueriesRepository,
            ILogger<PlanQueriesRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<DataSet<PlanQueryResult>> GetAsync(PlanQueryFilter queryFilter)
        {
            var currentUser = this.userContext.GetCurrentUser();
            this.logger.LogInformation("Starting to get the ABOR List Plan Profile with company {companyId} and filter {@filterOptions}", queryFilter, currentUser.CompanyId);

            var query = this.context.Plan
                .FilterNotDeleted()
                .FilterByImportStatus(queryFilter.XmlStatus)
                .FilterByCompany(currentUser)
                .ApplySearchByFilter(queryFilter.SearchBy);

            var total = await query.CountAsync();
            if (queryFilter.IsOnlyCount)
            {
                return new(data: Array.Empty<PlanQueryResult>(), total);
            }

            var data = await query.Select(PlanProjection.ProjectionToPlanQueryResult)
                .ApplySortByFields(queryFilter.SortBy)
                .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take, queryFilter.IsForDownloading)
                .ToListAsync();

            await this.userQueriesRepository.FillUsersNameAsync(data);
            return new(data, total);
        }

        public async Task<PlanDetailQueryResult> GetPlanByByName(Guid companyId, string planName)
        {
            this.logger.LogInformation("Starting to get the plan by Name {planName} from companyId: {companyId}", planName, companyId);

            return await this.context.Plan
                .FilterByCompany(companyId)
                .FilterByPlanName(planName)
                .Include(c => c.Rooms)
                .Select(PlanProjection.ProjectionToPlanDetailQueryResult)
                .SingleOrDefaultAsync();
        }

        public async Task<PlanDetailQueryResult> GetPlanById(Guid id)
        {
            this.logger.LogInformation("Starting to get the ABOR Plan Profile with Id: {id}", id);
            var currentUser = this.userContext.GetCurrentUser();

            var plan = await this.context.Plan
                .FilterByCompany(currentUser)
                .Include(c => c.Rooms)
                .Select(PlanProjection.ProjectionToPlanDetailQueryResult)
                .SingleOrDefaultAsync(x => x.Id == id);
            await this.userQueriesRepository.FillUserNameAsync(plan);
            return plan;
        }

        public async Task<PlanDetailQueryResult> GetByIdWithListingImportProjection(Guid id, Guid listingId)
        {
            var currentUser = this.userContext.GetCurrentUser();
            var plan = await this.context.Plan
                .Include(c => c.Rooms)
                .FilterByCompany(currentUser)
                .SingleOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException<Plan>(id);

            var listingSale = await this.context.ListingSale
                .Include(x => x.SaleProperty)
                .Include(x => x.SaleProperty.Rooms)
                .FilterByCompany(currentUser)
                .SingleOrDefaultAsync(x => x.Id == listingId) ?? throw new NotFoundException<SaleListing>(listingId);

            plan.ImportFromListing(listingSale);
            var compiledLambda = PlanProjection.ProjectionToPlanDetailQueryResult.Compile();
            return compiledLambda.Invoke(plan);
        }
    }
}
