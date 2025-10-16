namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Specifications;
    using Husa.Extensions.Common.Classes;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Specifications;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using AlertExtension = Husa.Quicklister.Extensions.Data.Queries.Repositories;

    public class AlertQueriesRepository : AlertExtension.AlertQueriesRepository<ApplicationQueriesDbContext, SaleListing, DetailAlertQueryResult, MarketStatuses>, IAlertQueriesRepository
    {
        public AlertQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserRepository userQueriesRepository,
            IPhotoServiceClient photoService,
            ICompanyCacheRepository companyRepository,
            IXmlClient xmlClient,
            ILogger<AlertQueriesRepository> logger,
            IUserContextProvider userContext)
            : base(userQueriesRepository, logger, photoService, companyRepository, xmlClient, userContext, context)
        {
        }

        protected override IReadOnlyDictionary<AlertType, Expression<Func<SaleListing, bool>>> AlertDictionary => AlertsQueryExtensions.AlertsDictionary;

        protected override Expression<Func<SaleListing, DetailAlertQueryResult>> DetailAlertQueryResultProjection => ListingSaleAlertsProjection.ProjectListingSaleQueryResult;

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
            return query
                .FilterByCommunityEmployee<SaleListing, SaleProperty, CommunityEmployee>(this.Context.CommunityEmployee, currentUser);
        }

        protected override IQueryable<SaleListing> GetSaleListingAlerts(string search = null)
        {
            var query = this.Context.ListingSale
                .Include(d => d.SaleProperty)
                .Include(d => d.SaleProperty.Community)
                .Include(d => d.SaleProperty.Community.Employees)
                .Include(d => d.StatusFieldsInfo)
                .FilterNotDeleted();

            query = this.FilterByCommunityEmployee(query)
                .FilterByAvailableCompanies(
                    this.UserContext.GetCurrentUser(),
                    this.companyRepository);

            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.FilterBySearch(search);
            }

            return query;
        }

        protected override IQueryable<SaleListing> FilterByActive(IQueryable<SaleListing> query)
        {
            return query.Where(l => l.MlsStatus == MarketStatuses.Active);
        }
    }
}
