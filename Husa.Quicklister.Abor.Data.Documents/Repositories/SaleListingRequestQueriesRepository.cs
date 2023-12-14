namespace Husa.Quicklister.Abor.Data.Documents.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting;
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
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Models;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Data.Documents.Specifications;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
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
            if (cosmosClient is null)
            {
                throw new ArgumentNullException(nameof(cosmosClient));
            }

            this.listingSaleQueriesRepository = listingSaleQueriesRepository ?? throw new ArgumentNullException(nameof(listingSaleQueriesRepository));
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.SaleContainer
                .GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.CosmosLinqQuery.GetFeedIterator(query);
            if (!queryIterator.HasMoreResults)
            {
                return null;
            }

            var requestEntity = (await queryIterator.ReadNextAsync(cancellationToken)).SingleOrDefault() ?? throw new NotFoundException<SaleListingRequest>(requestId);
            var queryResult = requestEntity.ToListingSaleRequestDetailQueryResult();
            var saleListing = await this.listingSaleQueriesRepository.GetListing(requestEntity.ListingSaleId) ?? throw new NotFoundException<SaleListing>(requestEntity.ListingSaleId);
            queryResult.IsFirstRequest = await this.CheckIsFirstListingRequestAsync(requestEntity.ListingSaleId, cancellationToken);
            if (saleListing.LockedStatus == LockedStatus.LockedBySystem)
            {
                queryResult.LockedByUsername = UserConstants.LockedBySystemLabel;
            }
            else
            {
                queryResult.LockedBy = saleListing.LockedBy;
                queryResult.LockedByUsername = saleListing.LockedByUsername;
            }

            queryResult.LockedStatus = saleListing.LockedStatus;
            queryResult.LockedOn = saleListing.LockedOn;
            if (queryResult.PublishInfo.PublishType is null)
            {
                queryResult.PublishInfo.PublishType = saleListing.PublishInfo.PublishType;
            }

            await this.userQueriesRepository.FillUserNameAsync(queryResult);

            queryResult.StatusFieldsInfo.AgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentId);
            queryResult.StatusFieldsInfo.SecondAgentMarketUniqueId = await this.agentQueriesRepository.GetAgentUniqueMarketIdAsync(queryResult.StatusFieldsInfo.AgentIdSecond);

            return queryResult;
        }

        public override Task<ListingRequestGridQueryResult<ListingSaleRequestQueryResult>> GetListingSaleRequestsAsync(SaleListingRequestQueryFilter queryFilter, CancellationToken cancellationToken = default)
        {
            if (queryFilter.RequestFieldChange.HasValue)
            {
                return this.GetRequestsByFieldChangeAsync(queryFilter.RequestFieldChange.Value, SaleRequestQueryProjection.ProjectionToRequestSale, cancellationToken);
            }

            return this.GetListingSaleRequestsAsync(queryFilter, SaleRequestQueryProjection.ProjectionToRequestSale, cancellationToken);
        }

        public override Task<IEnumerable<ListingSaleRequestQueryResult>> GetRequestsByListingSaleIdAsync(Guid listingSaleId, CancellationToken cancellationToken = default)
            => this.GetRequestsByListingSaleIdAsync(listingSaleId, SaleRequestQueryProjection.ProjectionToRequestSale, cancellationToken);

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

        protected override IQueryable<SaleListingRequest> FilterBySaleListingRequestQuery(IQueryable<SaleListingRequest> records, SaleListingRequestQueryFilter queryFilter)
            => records.FilterByQuery(queryFilter);

        protected override SummarySectionQueryResult TransformSummaryToQueryResult(SummarySection item)
        {
            var agentId = item.Fields.FirstOrDefault(f => f.FieldName == nameof(StatusFieldsRecord.AgentId));
            if (item.Name == StatusFieldsRecord.SummarySection && agentId != null)
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
