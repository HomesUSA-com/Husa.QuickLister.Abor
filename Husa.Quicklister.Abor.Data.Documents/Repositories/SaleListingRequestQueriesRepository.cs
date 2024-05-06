namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Document.Models;
    using Husa.Extensions.Document.Specifications;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Data.Documents.Extensions;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.ListingRequest;
    using Husa.Quicklister.Abor.Data.Documents.Projections;
    using Husa.Quicklister.Abor.Data.Documents.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest.Records;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Models;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Options;
    using ExtensionRepositories = Husa.Quicklister.Extensions.Data.Documents.Repositories;

    public class SaleListingRequestQueriesRepository : ExtensionRepositories.SaleListingRequestQueriesRepository<SaleListingRequest, ListingSaleRequestQueryResult>, ISaleListingRequestQueriesRepository
    {
        private readonly IListingSaleQueriesRepository listingSaleQueriesRepository;
        private readonly IAgentQueriesRepository agentQueriesRepository;
        private readonly IUserRepository userQueriesRepository;

        public SaleListingRequestQueriesRepository(
            CosmosClient cosmosClient,
            ICosmosLinqQuery cosmosLinqQuery,
            IOptions<DocumentDbSettings> options,
            IQueryMediaRepository mediaQueriesRepository,
            IListingSaleQueriesRepository listingSaleQueriesRepository,
            IAgentQueriesRepository agentQueriesRepository,
            IUserRepository userQueriesRepository)
             : base(cosmosClient, cosmosLinqQuery, options, mediaQueriesRepository)
        {
            ArgumentNullException.ThrowIfNull(cosmosClient);
            this.listingSaleQueriesRepository = listingSaleQueriesRepository ?? throw new ArgumentNullException(nameof(listingSaleQueriesRepository));
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public override Expression<Func<SaleListingRequest, ListingSaleRequestQueryResult>> QueryResultProjection => SaleRequestQueryProjection.ProjectionToRequestSale;

        public async Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.DbContainer
                .GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.CosmosLinqQuery.GetFeedIterator(query);
            if (!queryIterator.HasMoreResults)
            {
                return null;
            }

            var requestEntity = (await queryIterator.ReadNextAsync(cancellationToken)).SingleOrDefault() ?? throw new NotFoundException<SaleListingRequest>(requestId);
            var queryResult = requestEntity.ToListingSaleRequestDetailQueryResult();
            var saleListing = await this.listingSaleQueriesRepository.GetListing(requestEntity.EntityId) ?? throw new NotFoundException<SaleListing>(requestEntity.EntityId);
            queryResult.IsFirstRequest = await this.CheckIsFirstListingRequestAsync(requestEntity.EntityId, cancellationToken);
            queryResult.FillLockedInformation(saleListing);

            await this.userQueriesRepository.FillUserNameAsync(queryResult);

            queryResult.StatusFieldsInfo.AgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentId);
            queryResult.StatusFieldsInfo.SecondAgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentIdSecond);

            return queryResult;
        }

        public async Task<DataSet<ListingSaleBillingQueryResult>> GetBillableListingsAsync(ListingSaleBillingQueryFilter queryFilter, CancellationToken cancellationToken = default)
        {
            var billableListings = await this.listingSaleQueriesRepository.GetBillableListingsAsync(queryFilter);

            foreach (var listing in billableListings.Data)
            {
                var request = await this.GetFirstCompletedRequestAsync(listing.Id, sysModifiedOn: queryFilter.To, cancellationToken);
                if (request != null)
                {
                    listing.SysCreatedOn = request.SysCreatedOn;
                    listing.SysCreatedBy = request.SysCreatedBy;
                }
            }

            await this.userQueriesRepository.FillUsersNameAsync(billableListings.Data);

            var listingsFilteredBySearch = billableListings.Data.FilterBySearch(queryFilter.SearchBy);
            var total = listingsFilteredBySearch.Count();

            return new(listingsFilteredBySearch, total);
        }

        protected override IQueryable<ListingRequestForSummaryQueryResult<SaleListingRequest>> FilterByFieldChange(IQueryable<ListingRequestForSummaryQueryResult<SaleListingRequest>> requests, RequestFieldChange fieldChange)
            => fieldChange switch
            {
                RequestFieldChange.ListPrice =>
                    requests.Where(result => result.CurrentRequest.ListPrice != result.PreviousRequest.ListPrice),
                RequestFieldChange.CompletionDate =>
                    requests.Where(result =>
                        result.CurrentRequest.SaleProperty.PropertyInfo.ConstructionCompletionDate !=
                        result.PreviousRequest.SaleProperty.PropertyInfo.ConstructionCompletionDate),
                RequestFieldChange.ConstructionStage =>
                    requests.Where(result =>
                        result.CurrentRequest.SaleProperty.PropertyInfo.ConstructionStage !=
                        result.PreviousRequest.SaleProperty.PropertyInfo.ConstructionStage),
                _ => throw new NotImplementedException(),
            };

        protected override IQueryable<SaleListingRequest> FilterByListingRequestQuery(IQueryable<SaleListingRequest> records, ListingRequestQueryFilter queryFilter)
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
