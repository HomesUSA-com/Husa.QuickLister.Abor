namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Document.Models;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Projections;
    using Husa.Quicklister.Abor.Data.Documents.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Options;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class LotListingRequestQueriesRepository : ExtensionRepositories.LotListingRequestQueriesRepository<LotListingRequest, ListingRequestQueryResult>, ILotListingRequestQueriesRepository
    {
        private readonly IAgentQueriesRepository agentQueriesRepository;

        public LotListingRequestQueriesRepository(
            CosmosClient cosmosClient,
            ICosmosLinqQuery cosmosLinqQuery,
            IOptions<DocumentDbSettings> options,
            IQueryMediaRepository mediaQueriesRepository,
            IAgentQueriesRepository agentQueriesRepository)
             : base(cosmosClient, cosmosLinqQuery, options, mediaQueriesRepository)
        {
            ArgumentNullException.ThrowIfNull(cosmosClient);
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
        }

        public override Expression<Func<LotListingRequest, ListingRequestQueryResult>> QueryResultProjection => LotRequestQueryProjection.ProjectionToLotRequest;

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
