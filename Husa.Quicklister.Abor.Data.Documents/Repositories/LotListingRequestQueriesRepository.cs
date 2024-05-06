namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Document.Models;
    using Husa.Extensions.Document.Specifications;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Data.Documents.Extensions;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.LotRequest;
    using Husa.Quicklister.Abor.Data.Documents.Projections;
    using Husa.Quicklister.Abor.Data.Documents.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Options;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class LotListingRequestQueriesRepository : ExtensionRepositories.LotListingRequestQueriesRepository<LotListingRequest, ListingRequestQueryResult>, ILotListingRequestQueriesRepository
    {
        private readonly IAgentQueriesRepository agentQueriesRepository;
        private readonly ILotListingQueriesRepository listingQueriesRepository;
        private readonly IUserRepository userQueriesRepository;

        public LotListingRequestQueriesRepository(
            CosmosClient cosmosClient,
            ICosmosLinqQuery cosmosLinqQuery,
            IOptions<DocumentDbSettings> options,
            IQueryMediaRepository mediaQueriesRepository,
            IAgentQueriesRepository agentQueriesRepository,
            IUserRepository userQueriesRepository,
            ILotListingQueriesRepository listingQueriesRepository)
             : base(cosmosClient, cosmosLinqQuery, options, mediaQueriesRepository)
        {
            ArgumentNullException.ThrowIfNull(cosmosClient);
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
            this.listingQueriesRepository = listingQueriesRepository ?? throw new ArgumentNullException(nameof(listingQueriesRepository));
        }

        public override Expression<Func<LotListingRequest, ListingRequestQueryResult>> QueryResultProjection => LotRequestQueryProjection.ProjectionToLotRequest;

        public async Task<LotListingRequestDetailQueryResult> GetRequestByIdAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.DbContainer
                .GetItemLinqQueryable<LotListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.CosmosLinqQuery.GetFeedIterator(query);
            if (!queryIterator.HasMoreResults)
            {
                return null;
            }

            var requestEntity = (await queryIterator.ReadNextAsync(cancellationToken)).SingleOrDefault() ?? throw new NotFoundException<LotListingRequest>(requestId);
            var queryResult = requestEntity.ToLotListingRequestDetailQueryResult();
            var listing = await this.listingQueriesRepository.GetListing(requestEntity.EntityId) ?? throw new NotFoundException<LotListing>(requestEntity.EntityId);
            queryResult.IsFirstRequest = await this.CheckIsFirstListingRequestAsync(requestEntity.EntityId, cancellationToken);
            queryResult.FillLockedInformation(listing);

            await this.userQueriesRepository.FillUserNameAsync(queryResult);

            queryResult.StatusFieldsInfo.AgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentId);
            queryResult.StatusFieldsInfo.SecondAgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentIdSecond);

            return queryResult;
        }

        protected override IQueryable<LotListingRequest> FilterByListingRequestQuery(IQueryable<LotListingRequest> records, ListingRequestQueryFilter queryFilter)
            => records.FilterByQuery(queryFilter);

        protected override SummarySectionQueryResult TransformSummaryToQueryResult(SummarySection item)
        {
            var agentId = item.Fields.FirstOrDefault(f => f.FieldName == nameof(SaleStatusFieldsRecord.AgentId));
            if (item.Name == SaleStatusFieldsRecord.SummarySection && agentId != null)
            {
                var newAgent = agentId.NewValue is not null ? this.agentQueriesRepository.GetAgentByIdAsync(Guid.Parse(agentId.NewValue.ToString())).Result : null;
                var oldAgent = agentId.OldValue is not null ? this.agentQueriesRepository.GetAgentByIdAsync(Guid.Parse(agentId.OldValue.ToString())).Result : null;

                var statusFields = item.Fields.ToList();
                statusFields.Add(new()
                {
                    FieldName = AgentQueryResult.SummaryField,
                    NewValue = newAgent?.SummaryValue,
                    OldValue = oldAgent?.SummaryValue,
                });
                item.Fields = statusFields;
            }

            return base.TransformSummaryToQueryResult(item);
        }
    }
}
