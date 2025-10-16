namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Data.Queries.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;

    public class KpiQueriesRepository
        : KpiQueryRepository<
            ApplicationQueriesDbContext,
            SaleListing,
            SaleProperty,
            CommunityEmployee,
            MarketStatuses,
            ListingStatusFieldsInfo>
    {
        public KpiQueriesRepository(
            ApplicationQueriesDbContext applicationDbContext,
            IUserContextProvider userContextProvider,
            ICompanyCacheRepository companyCacheRepository,
            IKpiQueriesRequestProvider kpiQueriesRequestProvider,
            ILogger<KpiQueriesRepository> logger)
            : base(applicationDbContext, userContextProvider, companyCacheRepository, kpiQueriesRequestProvider, logger)
        {
        }

        private static Dictionary<KpiListingStatus, IEnumerable<MarketStatuses>> KpiMarketStatusMapping =>
            new()
            {
                { KpiListingStatus.Active, SaleListing.ActiveListingStatuses },
                { KpiListingStatus.Pending, SaleListing.PendingListingStatuses },
                { KpiListingStatus.Sold, [MarketStatuses.Closed] },
            };

        protected override IQueryable<SaleListing> FilterByPendingDate(IQueryable<SaleListing> query, DateRange range)
            => query.FilterByPendingDate<SaleListing, ListingStatusFieldsInfo>(range);

        protected override Dictionary<KpiListingStatus, IEnumerable<MarketStatuses>> GetKpiMarketStatusMapping()
            => KpiMarketStatusMapping;
    }
}
