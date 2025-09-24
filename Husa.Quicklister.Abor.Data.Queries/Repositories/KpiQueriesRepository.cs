namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;

    public class KpiQueriesRepository
        : KpiQueryRepository<
            ApplicationQueriesDbContext,
            SaleListing,
            SaleProperty,
            CommunityEmployee,
            MarketStatuses>
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
                { KpiListingStatus.Sold, Array.Empty<MarketStatuses>() },
            };

        protected override Dictionary<KpiListingStatus, IEnumerable<MarketStatuses>> GetKpiMarketStatusMapping()
            => KpiMarketStatusMapping;
    }
}
