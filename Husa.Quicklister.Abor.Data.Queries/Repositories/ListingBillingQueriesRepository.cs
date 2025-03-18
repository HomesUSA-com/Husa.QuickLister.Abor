namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Linq;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Husa.Quicklister.Extensions.Data.Queries.Models;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Data.Specifications;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    public class ListingBillingQueriesRepository : QueryListingBillingRepository
    {
        private readonly ApplicationQueriesDbContext context;

        public ListingBillingQueriesRepository(
            ApplicationQueriesDbContext context,
            ILogger<ListingBillingQueriesRepository> logger,
            IUserRepository userQueriesRepository,
            IServiceSubscriptionClient serviceSubscriptionClient)
            : base(serviceSubscriptionClient, userQueriesRepository, logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override MarketCode MarketCode => MarketCode.Austin;

        protected override IQueryable<SaleListingBillingQueryResult> GetListingBillingQueryResult(ListingBillingQueryFilter queryFilter)
        {
            return this.context.ListingSale
                .FilterNotDeleted()
                .FilterByBillingType(queryFilter.ActionType)
                .FilterBySearch(queryFilter.SearchBy)
                .FilterByBillingDate(queryFilter.From, queryFilter.To)
                .FilterByInvoice()
                .Select(BillingProjection.ProjectToListingSaleBillingQueryResult);
        }
    }
}
