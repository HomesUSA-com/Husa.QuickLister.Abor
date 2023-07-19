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
    using Husa.PhotoService.Api.Contracts.Request;
    using Husa.PhotoService.Domain.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Alerts;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AlertQueriesRepository : IAlertQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserContextProvider userContext;
        private readonly IUserRepository userQueriesRepository;
        private readonly IPhotoServiceClient photoService;
        private readonly ILogger<AlertQueriesRepository> logger;

        public AlertQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserRepository userQueriesRepository,
            IPhotoServiceClient photoService,
            ILogger<AlertQueriesRepository> logger,
            IUserContextProvider userContext)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DataSet<DetailAlertQueryResult>> GetAsync(AlertType alertType, BaseAlertQueryFilter filter)
        {
            var alerts = await this.GetAlertsByTypeAsync(alertType, filter);
            if (filter.FillCommunityEmployees)
            {
                await this.FillCommunityEmployees(alerts.Data);
            }

            return alerts;
        }

        public async Task<int> GetTotal(IEnumerable<AlertType> alerts)
        {
            var currentUser = this.userContext.GetCurrentUser();

            var alertFilterExpressions = AlertsQueryExtensions.AlertsDictionary
                .Where(dictionary => alerts.Contains(dictionary.Key))
                .Where(dictionary => (dictionary.Key != AlertType.ExpiringListings && dictionary.Key != AlertType.OrphanListings) ||
                                     ((dictionary.Key == AlertType.ExpiringListings || dictionary.Key == AlertType.OrphanListings) && currentUser.IsMLSAdministrator))
                .Select(dictionary => dictionary.Value);

            var totalAlerts = 0;

            if (alertFilterExpressions.Any())
            {
                this.logger.LogInformation("Alert filter expression value: {alertTypes}", string.Join(",", alerts));

                totalAlerts = await this.GetListingSaleAlerts()
                .FilterByAlerts(alertFilterExpressions)
                .CountAsync();

                var totals = this.GetListingSaleAlerts()
                .GroupBy(saleListing => 1)
                .SelectAlertTotalsOrDefault(alerts, currentUser.IsMLSAdministrator);

                if (totals != null)
                {
                    totalAlerts = totals.Total;
                }
            }

            var alertsToRun = AlertsQueryExtensions.AlertsWithCustomQueries
                .Where(alertType => alerts.Contains(alertType))
                .ToList();

            foreach (var alert in alertsToRun)
            {
                var result = await this.GetAsync(alert, new() { IsOnlyCount = true });
                totalAlerts += result.Total;
            }

            return totalAlerts;
        }

        private async Task<DataSet<DetailAlertQueryResult>> GetAlertsByTypeAsync(AlertType alertType, BaseAlertQueryFilter filter) => alertType switch
        {
            AlertType.ActiveEmployees => await this.GetActiveEmployeesAsync(filter),
            AlertType.CompletedHomesWithoutPhotoRequest => await this.GetCompletedHomesWithoutPhotoRequestAsync(filter),
            AlertType.PastDuePhotoRequests => await this.GetPastDuePhotoRequestsAsync(filter),
            _ => await this.GetAlerts(filter, AlertsQueryExtensions.AlertsDictionary[alertType]),
        };

        private async Task<DataSet<DetailAlertQueryResult>> GetActiveEmployeesAsync(BaseAlertQueryFilter filter)
        {
            var currentUser = this.userContext.GetCurrentUser();
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

            var userIds = employees.Select(employee => employee.UserId);
            var users = await this.userQueriesRepository.GetUsersById(userIds);
            var activeEmployees = employees
                .Join(users, emp => emp.UserId, usr => usr.Id, (employee, user) =>
                {
                    employee.Name = $"{user.FirstName} {user.LastName}";
                    employee.Title = user.CompanyEmployees.Where(x => x.CompanyId == employee.CompanyId).Select(x => x.Title).FirstOrDefault();
                    return employee;
                }).Distinct();

            var count = activeEmployees.Count();
            if (filter.IsOnlyCount)
            {
                return new(Array.Empty<DetailAlertQueryResult>(), count);
            }

            var result = activeEmployees
                .Skip(filter.Skip * filter.Take)
                .Take(filter.Take)
                .Select(employee => employee).Distinct();

            return new(result, count);
        }

        private async Task<DataSet<DetailAlertQueryResult>> GetCompletedHomesWithoutPhotoRequestAsync(BaseAlertQueryFilter filter)
        {
            var currentUser = this.userContext.GetCurrentUser();
            var query = this.context.ListingSale
                .Include(p => p.SaleProperty)
                .Include(p => p.SaleProperty.Community)
                .Include(p => p.SaleProperty.Community.Employees)
                .FilterNotDeleted()
                .FilterByStatus(SaleListing.ActivePhotoRequestListingStatuses)
                .Where(listing =>
                    !string.IsNullOrEmpty(listing.MlsNumber) && !listing.LastPhotoRequestId.HasValue &&
                    listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.HasValue &&
                    listing.SaleProperty.PropertyInfo.ConstructionCompletionDate.Value <= DateTime.UtcNow &&
                    !listing.IsPhotosDeclined)
                .FilterByCompany(currentUser)
                .FilterBySearch(filter.SearchBy);

            query = this.FilterByCommunityEmployee(query);
            var count = await query.CountAsync();

            if (filter.IsOnlyCount)
            {
                return new(Array.Empty<DetailAlertQueryResult>(), count);
            }

            var listings = await query.Select(ListingSaleAlertsProjection.ProjectListingSaleQueryResult())
                .Skip(filter.Skip * filter.Take)
                .Take(filter.Take)
                .ToListAsync();
            await this.userQueriesRepository.FillUsersNameAsync(listings);
            return new(listings, count);
        }

        private async Task<DataSet<DetailAlertQueryResult>> GetPastDuePhotoRequestsAsync(BaseAlertQueryFilter filter)
        {
            var currentUser = this.userContext.GetCurrentUser();
            if (!currentUser.IsMLSAdministrator)
            {
                return new(Array.Empty<DetailAlertQueryResult>(), 0);
            }

            var request = await this.photoService.PhotoRequest.GetAsync(new PhotoRequestFilter()
            {
                PastDue = true,
                Type = new[] { PropertyType.Residential },
            });

            var listingsQuery = this.context.ListingSale
                    .Include(l => l.SaleProperty)
                    .FilterBySearch(filter.SearchBy);

            var photosQuery = request.Data
                .Join(
                    listingsQuery,
                    photo => photo.PropertyId,
                    listing => listing.Id,
                    (photo, listing) => new { photo, listing });

            var count = photosQuery.Count();

            if (filter.IsOnlyCount)
            {
                return new(Array.Empty<DetailAlertQueryResult>(), count);
            }

            var result = photosQuery
                .Select((request) => new DetailAlertQueryResult()
                {
                    ListingId = request.listing.Id,
                    PhotoRequestId = request.photo.Id,
                    MarketCode = MarketCode.SanAntonio,
                    MlsNumber = request.listing.MlsNumber,
                    Address = request.listing.SaleProperty.Address,
                    Subdivision = request.listing.SaleProperty.AddressInfo.Subdivision,
                    OwnerName = request.listing.SaleProperty.OwnerName,
                    AssignedTo = request.photo.PhotographerName,
                    AssignedOn = request.photo.AssignedOn,
                    ContactDate = request.photo.ContactDate,
                    ScheduleDate = request.photo.ScheduleDate,
                })
                .ToList();

            return new(result, count);
        }

        private async Task<DataSet<DetailAlertQueryResult>> GetAlerts(BaseAlertQueryFilter filter, Expression<Func<SaleListing, bool>> expression)
        {
            var query = this.GetListingSaleAlerts().Where(expression).FilterBySearch(filter.SearchBy);
            var alertCount = await query.CountAsync();
            if (filter.IsOnlyCount)
            {
                this.logger.LogInformation("Is only count request {alertCount}", alertCount);
                return new(Array.Empty<DetailAlertQueryResult>(), alertCount);
            }

            var listings = await query.Select(ListingSaleAlertsProjection.ProjectListingSaleQueryResult())
                .Skip(filter.Skip * filter.Take)
                .Take(filter.Take)
                .ToListAsync();

            await this.userQueriesRepository.FillUsersNameAsync(listings);
            return new(listings, alertCount);
        }

        private IQueryable<SaleListing> GetListingSaleAlerts()
        {
            var currentUser = this.userContext.GetCurrentUser();
            var query = this.context.ListingSale
                .Include(d => d.SaleProperty)
                .Include(d => d.SaleProperty.Community)
                .Include(d => d.SaleProperty.Community.Employees)
                .Include(d => d.StatusFieldsInfo)
                .FilterNotDeleted()
                .FilterByCompany(currentUser);

            return this.FilterByCommunityEmployee(query);
        }

        private IQueryable<SaleListing> FilterByCommunityEmployee(IQueryable<SaleListing> query)
        {
            var currentUser = this.userContext.GetCurrentUser();
            if (currentUser.UserRole == UserRole.User && currentUser.EmployeeRole == RoleEmployee.SalesEmployee)
            {
                var communityIds = this.context.CommunityEmployee.Where(x => x.UserId == currentUser.Id && !x.IsDeleted).Select(x => x.CommunityId).ToList();
                query = query.Where(x => x.SaleProperty.CommunityId.HasValue && communityIds.Contains(x.SaleProperty.CommunityId.Value));
            }

            return query;
        }

        private async Task FillCommunityEmployees(IEnumerable<DetailAlertQueryResult> alerts)
        {
            foreach (var alert in alerts)
            {
                var users = await this.userQueriesRepository.GetUsersById(alert.CommunityEmployees.Select(x => x.UserId));
                alert.CommunityEmployees = users.Select(x => new UserQueryResult() { UserId = x.Id, Email = x.Email, FullName = $"{x.FirstName} {x.LastName}" });
            }
        }
    }
}
