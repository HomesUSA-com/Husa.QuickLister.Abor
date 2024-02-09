namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class CommunityQueriesRepository : ICommunityQueriesRepository
    {
        private readonly ILogger<CommunityQueriesRepository> logger;
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserRepository userQueriesRepository;
        private readonly IUserContextProvider userContext;

        public CommunityQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserContextProvider userContext,
            ILogger<CommunityQueriesRepository> logger,
            IUserRepository userQueriesRepository)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<DataSet<CommunityQueryResult>> GetAsync(CommunityQueryFilter queryFilter)
        {
            var currentUser = this.userContext.GetCurrentUser();
            this.logger.LogInformation("Getting the communities for the user {@user}", currentUser);
            var query = this.context.Community
                .FilterNotDeleted()
                .FilterByImportStatus(queryFilter.XmlStatus)
                .FilterByCompany(currentUser)
                .FilterByCommunityName(queryFilter.Name)
                .ApplySearchByFilter(queryFilter.SearchBy);

            var total = await query.CountAsync();
            if (queryFilter.IsOnlyCount)
            {
                return new DataSet<CommunityQueryResult>(Array.Empty<CommunityQueryResult>(), total);
            }

            var data = await query.Select(CommunityProjection.ProjectionToCommunityQueryResult)
                .ApplySortByFields(queryFilter.SortBy)
                .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take, queryFilter.IsForDownloading)
                .ToListAsync();

            await this.userQueriesRepository.FillUsersNameAsync(data);

            return new DataSet<CommunityQueryResult>(data, total);
        }

        public async Task<CommunityDetailQueryResult> GetCommunityById(Guid id)
        {
            this.logger.LogInformation("Getting the Community with Id: {id}", id);
            var currentUser = this.userContext.GetCurrentUser();
            var community = await this.context.Community
                .FilterByCompany(currentUser)
                .Select(CommunityProjection.ProjectionToCommunityDetailQueryResult)
                .SingleOrDefaultAsync(x => x.Id == id);
            await this.userQueriesRepository.FillUserNameAsync(community);
            return community;
        }

        public async Task<DataSet<CommunityEmployeeQueryResult>> GetCommunityEmployees(Guid communityId, string sortBy)
        {
            this.logger.LogInformation("Getting the Community employees for the communityId: '{communityId}'", communityId);
            var employees = await this.context.CommunityEmployee
                .Where(e => !e.IsDeleted && e.CommunityId == communityId)
                .Select(CommunityProjection.ProjectionToCommunityEmployeeQueryResult)
                .ToListAsync();

            if (!employees.Any())
            {
                return new DataSet<CommunityEmployeeQueryResult>(employees, employees.Count);
            }

            var users = await this.userQueriesRepository.GetUsersById(employees.Select(u => u.UserId));

            foreach (var employee in employees)
            {
                var user = users.FirstOrDefault(u => u.Id == employee.UserId);
                if (user != null)
                {
                    employee.UserName = user.UserName;
                    employee.FirstName = user.FirstName;
                    employee.LastName = user.LastName;
                    employee.Email = user.Email;
                    employee.Title = user.CompanyEmployees.Where(x => x.CompanyId == employee.CompanyId).Select(x => x.Title).FirstOrDefault();
                }
            }

            var employeesQuery = employees.AsQueryable().ApplySortByFields(sortBy).ToList();

            return new DataSet<CommunityEmployeeQueryResult>(employeesQuery, employees.Count);
        }

        public async Task<CommunityDetailQueryResult> GetCommunityByName(Guid companyId, string communityName)
        {
            this.logger.LogInformation("Getting the Community by Name {communityName} from companyId: {companyId}", companyId, communityName);
            return await this.context.Community
                .FilterByCompany(companyId)
                .FilterByCommunityName(communityName)
                .Include(c => c.OpenHouses)
                .Select(CommunityProjection.ProjectionToCommunityDetailQueryResult)
                .SingleOrDefaultAsync();
        }

        public async Task<CommunityDetailQueryResult> GetByIdWithListingImportProjection(Guid id, Guid listingId)
        {
            var currentUser = this.userContext.GetCurrentUser();
            var community = await this.context.Community
                .Include(c => c.OpenHouses)
                .FilterByCompany(currentUser)
                .SingleOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException<CommunitySale>(id);
            var listingSale = await this.context.ListingSale
                .Include(x => x.SaleProperty)
                .Include(x => x.SaleProperty.OpenHouses).FilterByCompany(currentUser)
                .SingleOrDefaultAsync(x => x.Id == listingId) ?? throw new NotFoundException<SaleListing>(listingId);
            community.ImportFromListing(listingSale);

            var compiledLambda = CommunityProjection.ProjectionToCommunityDetailQueryResult.Compile();
            return compiledLambda.Invoke(community);
        }
    }
}
