namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
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
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using AlertExtension = Husa.Quicklister.Extensions.Data.Queries.Repositories;

    public class AlertQueriesRepository : AlertExtension.AlertQueriesRepository<SaleListing, DetailAlertQueryResult, MarketStatuses>, IAlertQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;

        public AlertQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserRepository userQueriesRepository,
            IPhotoServiceClient photoService,
            ILogger<AlertQueriesRepository> logger,
            IUserContextProvider userContext)
            : base(userQueriesRepository, logger, photoService, userContext)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override IReadOnlyDictionary<AlertType, Expression<Func<SaleListing, bool>>> AlertDictionary => AlertsQueryExtensions.AlertsDictionary;

        protected override Expression<Func<SaleListing, DetailAlertQueryResult>> DetailAlertQueryResultProjection => ListingSaleAlertsProjection.ProjectListingSaleQueryResult;

        protected override MarketCode MarketCode => MarketCode.Austin;

        protected override async Task<DataSet<DetailAlertQueryResult>> GetActiveEmployeesAsync(BaseAlertQueryFilter filter)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            var employeesQueriable = this.context.Community
                .FilterByCompany(currentUser)
                .Join(this.context.CommunityEmployee, comm => comm.Id, emp => emp.CommunityId, (community, employee) => new { community, employee })
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

        protected override Task<DataSet<DetailAlertQueryResult>> GetCompletedHomesWithoutPhotoRequestAsync(BaseAlertQueryFilter filter)
        {
            var query = this.GetSaleListingAlerts(filter.SearchBy)
                .FilterByStatus(SaleListing.ActivePhotoRequestListingStatuses)
                .Where(listing =>
                    !string.IsNullOrEmpty(listing.MlsNumber) && !listing.LastPhotoRequestId.HasValue &&
                    listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
                    listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow &&
                    !listing.IsPhotosDeclined);

            return this.ToDetailAlertQueryResultDataSet(query, filter);
        }

        protected override Task<DataSet<DetailAlertQueryResult>> GetXmlListingUpdatedWithoutRequestAsync(BaseAlertQueryFilter filter)
        {
            var query = this.GetSaleListingAlerts(filter.SearchBy)
               .Join(this.context.XmlRequestError, listing => listing.Id, error => error.ListingId, (listing, error) => listing)
               .Include(x => x.XmlRequestError);
            return this.ToDetailAlertQueryResultDataSet(query, filter);
        }

        protected override IQueryable<SaleListing> FilterByCommunityEmployee(IQueryable<SaleListing> query)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            if (currentUser.UserRole == UserRole.User && currentUser.EmployeeRole == RoleEmployee.SalesEmployee)
            {
                var communityIds = this.context.CommunityEmployee.Where(x => x.UserId == currentUser.Id && !x.IsDeleted).Select(x => x.CommunityId).ToList();
                query = query.Where(x => x.SaleProperty.CommunityId.HasValue && communityIds.Contains(x.SaleProperty.CommunityId.Value));
            }

            return query;
        }

        protected override IQueryable<SaleListing> GetSaleListingAlerts(string search = null)
        {
            var currentUser = this.UserContext.GetCurrentUser();
            var query = this.context.ListingSale
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
