namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Options;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class CommunityHistoryQueriesRepository :
        ExtensionRepositories.CommunityQueriesRepository<CommunityHistory>,
        ICommunityHistoryQueriesRepository
    {
        public CommunityHistoryQueriesRepository(
            CosmosClient cosmosClient,
            ICosmosLinqQuery cosmosLinqQuery,
            IOptions<DocumentDbSettings> options,
            IUserCacheRepository userRepository)
             : base(cosmosClient, cosmosLinqQuery, options, userRepository)
        {
        }
    }
}
