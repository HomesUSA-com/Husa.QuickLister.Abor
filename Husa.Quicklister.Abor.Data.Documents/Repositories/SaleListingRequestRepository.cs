namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using Husa.Extensions.Document.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Options;
    using ExtensionsRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class SaleListingRequestRepository : ExtensionsRepositories.SaleListingRequestRepository<SaleListingRequest, DocumentDbSettings>, ISaleListingRequestRepository
    {
        public SaleListingRequestRepository(
            CosmosClient cosmosClient,
            ICosmosLinqQuery cosmosLinqQuery,
            IOptions<DocumentDbSettings> options)
            : base(cosmosClient, cosmosLinqQuery, options)
        {
        }
    }
}
