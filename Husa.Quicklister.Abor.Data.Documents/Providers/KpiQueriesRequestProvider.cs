namespace Husa.Quicklister.Abor.Data.Documents.Providers
{
    using Husa.Extensions.Document.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Providers;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class KpiQueriesRequestProvider(
        CosmosClient cosmosClient,
        ICosmosLinqQuery cosmosLinqQuery,
        IOptions<DocumentDbSettings> options,
        ILogger<KpiQueriesRequestProvider> logger)
                : KpiQueryRequestsProvider<
                    SaleListing, SaleProperty, SaleListingRequest, MarketStatuses>(
              cosmosClient,
              cosmosLinqQuery,
              options.Value.DatabaseName,
              options.Value.SaleCollectionName,
              logger),
        IKpiQueriesRequestProvider
    {
    }
}
