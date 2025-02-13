namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using AlertExtension = Husa.Quicklister.Extensions.Data.Queries.Repositories;

    public class AlertQueriesRepository : AlertExtension.AlertQueriesRepository<ApplicationQueriesDbContext, SaleListing, DetailAlertQueryResult, MarketStatuses>, IAlertQueriesRepository
    {
        public AlertQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserRepository userQueriesRepository,
            IPhotoServiceClient photoService,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<AlertQueriesRepository> logger,
            IUserContextProvider userContext)
            : base(userQueriesRepository, logger, photoService, serviceSubscriptionClient, userContext, context)
        {
        }

        protected override IReadOnlyDictionary<AlertType, Expression<Func<SaleListing, bool>>> AlertDictionary => AlertsQueryExtensions.AlertsDictionary;

        protected override Expression<Func<SaleListing, DetailAlertQueryResult>> DetailAlertQueryResultProjection => ListingSaleAlertsProjection.ProjectListingSaleQueryResult;

        protected override MarketCode MarketCode => MarketCode.Austin;

        protected override async Task<DataSet<DetailAlertQueryResult>> GetActiveEmployeesAsync(BaseAlertQueryFilter filter)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            var employeesQueriable = this.Context.Community
                .FilterByCompany(currentUser)
                .Join(this.Context.CommunityEmployee, comm => comm.Id, emp => emp.CommunityId, (community, employee) => new { community, employee })
                .Where(communityEmployee => !communityEmployee.community.IsDeleted && !communityEmployee.employee.IsDeleted);

            var employees = await employeesQueriable
                .Select((communityEmployee) => new DetailAlertQueryResult
                {
                    Id = communityEmployee.employee.Id,
                    UserId = communityEmployee.employee.UserId,
                    CommunityName = communityEmployee.community.ProfileInfo.Name,
                    CommunityId = communityEmployee.community.Id,
                    CompanyId = communityEmployee.community.CompanyId,
                })
                .ToListAsync();

            return await this.FillCommunityEmployeeName(employees, filter);
        }

        protected override IQueryable<SaleListing> FilterByCommunityEmployee(IQueryable<SaleListing> query)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            if (currentUser.UserRole == UserRole.User && currentUser.EmployeeRole == RoleEmployee.SalesEmployee)
            {
                var communityIds = this.Context.CommunityEmployee.Where(x => x.UserId == currentUser.Id && !x.IsDeleted).Select(x => x.CommunityId).ToList();
                query = query.Where(x => x.SaleProperty.CommunityId.HasValue && communityIds.Contains(x.SaleProperty.CommunityId.Value));
            }

            return query;
        }

        protected override IQueryable<SaleListing> GetSaleListingAlerts(string search = null)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            var query = this.Context.ListingSale
                .Include(d => d.SaleProperty)
                .Include(d => d.SaleProperty.Community)
                .Include(d => d.SaleProperty.Community.Employees)
                .Include(d => d.StatusFieldsInfo)
                .FilterNotDeleted()
                .FilterByCompany(currentUser);

            query = this.FilterByCommunityEmployee(query);

            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.FilterBySearch(search);
            }

            return query;
        }
    }
}
