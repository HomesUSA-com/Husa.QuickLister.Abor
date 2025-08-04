namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Specifications;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Linq.Specifications;
    using Husa.Extensions.ShowingTime.Data.Specifications;
    using Husa.Extensions.ShowingTime.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ShowingTimeContactQueriesRepository : IShowingTimeContactQueriesRepository, IProvideShowingTimeContacts
    {
        private readonly ShowingTimeContactProjection contactProjections;
        private readonly CommunityShowingTimeContactOrderProjection communityContactProjections;
        private readonly ListingShowingTimeContactOrderProjection listingContactProjections;
        private readonly ILogger<ShowingTimeContactQueriesRepository> logger;
        private readonly ApplicationQueriesDbContext context;
        private readonly IUserContextProvider userContext;

        public ShowingTimeContactQueriesRepository(
            ApplicationQueriesDbContext context,
            IUserContextProvider userContext,
            ShowingTimeContactProjection contactProjections,
            CommunityShowingTimeContactOrderProjection communityContactProjections,
            ListingShowingTimeContactOrderProjection listingContactProjections,
            ILogger<ShowingTimeContactQueriesRepository> logger)
        {
            this.context = context;
            this.userContext = userContext;
            this.contactProjections = contactProjections;
            this.communityContactProjections = communityContactProjections;
            this.listingContactProjections = listingContactProjections;
            this.logger = logger;
        }

        public async Task<DataSet<ShowingTimeContactQueryResult>> Search(ShowingTimeContactQueryFilter filters)
        {
            var data = await this.GetAsync(filters);

            return new DataSet<ShowingTimeContactQueryResult>(data, data.Count);
        }

        public async Task<ShowingTimeContactDetailQueryResult> GetContactById(Guid contactId)
        {
            var currentUser = this.userContext.GetCurrentUser();
            this.logger.LogInformation("Getting the showing time contacts for the user {@user}", currentUser);

            return await this.context.ShowingTimeContacts
                .FilterNotDeleted()
                .FilterByCompany(currentUser)
                .FilterById(contactId)
                .Select(this.contactProjections.ToShowingTimeContactDetailQueryResult)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<ShowingTimeContactQueryResult>> GetAsync(ShowingTimeContactQueryFilter filters)
        {
            var currentUser = this.userContext.GetCurrentUser();
            this.logger.LogInformation("Getting the showing time contacts for the user {@user}", currentUser);

            var companyId = filters.CompanyId ?? currentUser.CompanyId;

            var contacts = await this.context.ShowingTimeContacts.FilterNotDeleted()
                .Where(c => !companyId.HasValue || c.CompanyId == Guid.Empty || c.CompanyId == companyId.Value)
                .ApplyShowingSearchByFilter(filters.SearchBy)
                .ApplySortByFields(filters.SortBy)
                .ApplyPaginationFilter(filters.Skip, filters.Take, filters.IsForDownloading)
                .Select(this.contactProjections.ToShowingTimeContactQueryResult)
                .ToListAsync();

            var orders = filters.Scope.Equals(ContactScope.Community) ?
                await this.context.CommunityShowingTimeContacts
                .Where(f => !filters.LimitToScope || f.ScopeId == filters.ScopeId)
                .Select(this.communityContactProjections.ToScopedShowingTimeContactQueryResult)
                .ToListAsync()
                : await this.context.ListingShowingTimeContacts
                .Where(f => !filters.LimitToScope || f.ScopeId == filters.ScopeId)
                .Select(this.listingContactProjections.ToScopedShowingTimeContactQueryResult)
                .ToListAsync();

            return contacts.GroupJoin(orders, a => a.Id, b => b.ContactId, (a, b) => new
            {
                Contact = a,
                Scope = b,
            }).SelectMany(x => x.Scope.DefaultIfEmpty(), (x, y) =>
            {
                x.Contact.InScope = x.Contact.IsFixed || x.Scope.Any(s => s.ScopeId == filters.ScopeId);
                x.Contact.Order = x.Scope.FirstOrDefault(s => s.ScopeId == filters.ScopeId)?.Order ?? 0;
                return x.Contact;
            }).Where(x => !filters.LimitToScope || x.InScope)
            .Distinct()
            .ToList();
        }

        public async Task<ICollection<ShowingTimeContact>> GetDetailedAsync(ShowingTimeContactQueryFilter filters)
        {
            var data = await this.GetAsync(filters);
            var q = data.Join(this.context.ShowingTimeContacts, a => a.Id, b => b.Id, (a, b) => b);
            return q.ToList();
        }

        public Task<ICollection<ShowingTimeContact>> GetCompanyContacts(Guid companyId)
        {
            return this.GetDetailedAsync(new() { CompanyId = companyId, LimitToScope = false });
        }

        public Task<ICollection<ShowingTimeContact>> GetScopedContacts(ContactScope scope, Guid scopeId)
        {
            return this.GetDetailedAsync(new() { Scope = scope, ScopeId = scopeId, LimitToScope = true });
        }
    }
}
