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
    using Husa.Quicklister.Abor.Data.Documents.QueryFilters;
    using Husa.Quicklister.Abor.Data.Documents.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Specifications;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using Microsoft.Extensions.Options;

    public class SaleListingRequestQueriesRepository : ISaleListingRequestQueriesRepository
    {
        private readonly Container saleContainer;
        private readonly ICosmosLinqQuery cosmosLinqQuery;
        private readonly IQueryMediaRepository mediaQueriesRepository;
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
        {
            if (cosmosClient is null)
            {
                throw new ArgumentNullException(nameof(cosmosClient));
            }

            var dbOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.saleContainer = cosmosClient.GetContainer(dbOptions.DatabaseName, dbOptions.SaleCollectionName);
            this.cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
            this.mediaQueriesRepository = mediaQueriesRepository ?? throw new ArgumentNullException(nameof(mediaQueriesRepository));
            this.listingSaleQueriesRepository = listingSaleQueriesRepository ?? throw new ArgumentNullException(nameof(listingSaleQueriesRepository));
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        public async Task<ListingRequestGridQueryResult<ListingSaleRequestQueryResult>> GetListingSaleRequestsAsync(ListingSaleRequestQueryFilter queryFilter, CancellationToken cancellationToken = default)
        {
            var queryRequestOptions = new QueryRequestOptions() { MaxItemCount = queryFilter.Take };
            var continuationToken = queryFilter.IsPrint ? null : queryFilter.ContinuationToken;

            var query = this.saleContainer.GetItemLinqQueryable<SaleListingRequest>(false, continuationToken, queryRequestOptions)
                .FilterByQuery(queryFilter)
                .Select(SaleRequestQueryProjection.ProjectionToRequestSale);

            var queryCount = await this.saleContainer.GetItemLinqQueryable<SaleListingRequest>(false)
                .FilterByQuery(queryFilter)
                .CountAsync(cancellationToken);

            var requests = await ReadDocumentFeedToGrid(query, cancellationToken: cancellationToken);
            requests.Total = queryCount;

            return requests;
        }

        public async Task<SaleListingRequest> GetListingRequestSaleAsyncOld(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.saleContainer
                .GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query);
            if (queryIterator.HasMoreResults)
            {
                return (await queryIterator.ReadNextAsync(cancellationToken)).FirstOrDefault();
            }

            return null;
        }

        public async Task<ListingSaleRequestDetailQueryResult> GetListingRequestSaleAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.saleContainer
                .GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query);
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

        public async Task<IEnumerable<SummarySectionQueryResult>> GetListingRequestSummaryAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var query = this.saleContainer
                .GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId);

            var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query);
            if (!queryIterator.HasMoreResults)
            {
                throw new NotFoundException<SaleListingRequest>(requestId);
            }

            var currentRequest = (await queryIterator.ReadNextAsync(cancellationToken)).FirstOrDefault() ?? throw new NotFoundException<SaleListingRequest>(requestId);
            var oldCompleteRequest = await this.GetLastCompletedRequestAsync(currentRequest.ListingSaleId, currentRequest.SysModifiedOn, cancellationToken);
            var summary = currentRequest.GetSummary(oldCompleteRequest);
            var summaryQueryResult = summary
                .Where(item => item != null)
                .Select(this.TransformSummaryToQueryResult)
                .ToList();

            var summaryItem = await this.GetMediaSection(currentRequest, oldCompleteRequest);
            if (summaryItem != null)
            {
                summaryQueryResult.AddRange(summaryItem);
            }

            return summaryQueryResult;
        }

        public async Task<IEnumerable<ListingSaleRequestQueryResult>> GetRequestsByListingSaleIdAsync(Guid listingSaleId, CancellationToken cancellationToken = default)
        {
            var results = new List<ListingSaleRequestQueryResult>();
            var queryRequestOptions = new QueryRequestOptions() { MaxItemCount = -1 };
            var query = this.saleContainer
                .GetItemLinqQueryable<SaleListingRequest>(
                    allowSynchronousQueryExecution: true,
                    continuationToken: null,
                    requestOptions: queryRequestOptions)
                .FilterByNonDeleted()
                .FilterByListingId(listingSaleId)
                .OrderByDescending(x => x.SysCreatedOn)
                .Select(SaleRequestQueryProjection.ProjectionToRequestSale);

            using (var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query))
            {
                if (queryIterator.HasMoreResults)
                {
                    var queryResult = await queryIterator.ReadNextAsync(cancellationToken);
                    results.AddRange(queryResult.ToList());
                }
            }

            return results;
        }

        public async Task<SaleListingRequest> GetListingSaleRequestByIdAndStatusAsync(Guid requestId, ListingRequestState requestState, CancellationToken cancellationToken = default)
        {
            var query = this.saleContainer.GetItemLinqQueryable<SaleListingRequest>()
                .FilterById(requestId)
                .FilterByStatus(requestState);

            using (var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query))
            {
                if (queryIterator.HasMoreResults)
                {
                    return (await queryIterator.ReadNextAsync(cancellationToken)).FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<SaleListingRequest> GetLastCompletedRequestAsync(Guid listingSaleId, DateTime? sysModifiedOn, CancellationToken cancellationToken = default)
        {
            var query = this.saleContainer
                    .GetItemLinqQueryable<SaleListingRequest>(false)
                    .FilterByListingId(listingSaleId)
                    .Where(request => request.SysModifiedOn < sysModifiedOn)
                    .Where(request => SaleListingRequest.ValidStates.Contains(request.RequestState))
                    .OrderByDescending(x => x.SysModifiedOn);

            using (var feedIterator = query.ToFeedIterator())
            {
                if (feedIterator.HasMoreResults)
                {
                    return (await feedIterator.ReadNextAsync(cancellationToken)).FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<SaleListingRequest> GetFirstCompletedRequestAsync(Guid listingSaleId, DateTime? sysModifiedOn, CancellationToken cancellationToken = default)
        {
            var validStates = new[] { ListingRequestState.Approved, ListingRequestState.Processing, ListingRequestState.Completed };
            var query = this.saleContainer
                    .GetItemLinqQueryable<SaleListingRequest>(false)
                    .FilterByListingId(listingSaleId)
                    .Where(request => request.SysModifiedOn < sysModifiedOn)
                    .Where(x => validStates.Contains(x.RequestState))
                    .OrderBy(x => x.SysModifiedOn);

            using (var feedIterator = query.ToFeedIterator())
            {
                if (feedIterator.HasMoreResults)
                {
                    return (await feedIterator.ReadNextAsync(cancellationToken)).FirstOrDefault();
                }
            }

            return null;
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

            return billableListings;
        }

        private static async Task<ListingRequestGridQueryResult<T>> ReadDocumentFeedToGrid<T>(IQueryable<T> query, string continuationToken = null, CancellationToken cancellationToken = default)
            where T : class
        {
            var results = new List<T>();
            using (var feedIterator = query.ToFeedIterator())
            {
                if (feedIterator.HasMoreResults)
                {
                    var queryResults = await feedIterator.ReadNextAsync(cancellationToken);
                    continuationToken = queryResults.ContinuationToken;
                    results.AddRange(queryResults.ToList());
                }
            }

            return new(results, continuationToken);
        }

        private SummarySectionQueryResult TransformSummaryToQueryResult(SummarySection item)
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

            return new SummarySectionQueryResult
            {
                Name = item.Name,
                Fields = item.Fields.Select(summaryField => new SummaryFieldQueryResult
                {
                    FieldName = summaryField.FieldName,
                    NewValue = summaryField.NewValue,
                    OldValue = summaryField.OldValue,
                }),
            };
        }

        private async Task<IEnumerable<SummarySectionQueryResult>> GetMediaSection(SaleListingRequest currentRequest, SaleListingRequest oldCompleteRequest)
        {
            var mediaSections = await this.mediaQueriesRepository.GetMediaSummary(currentRequest.Id, oldCompleteRequest?.Id);

            if (mediaSections == null)
            {
                return null;
            }

            var summaryItems = mediaSections.Select(mediaSection => new SummarySectionQueryResult
            {
                Name = mediaSection.Name,
                Fields = mediaSection.Fields.Select(x => new SummaryFieldQueryResult
                {
                    FieldName = x.FieldName,
                    NewValue = x.NewValue,
                    OldValue = x.OldValue,
                }),
            });
            return summaryItems;
        }

        private async Task<bool> CheckIsFirstListingRequestAsync(Guid listingSaleId, CancellationToken cancellationToken = default)
        {
            var result = false;
            var query = this.saleContainer.GetItemLinqQueryable<SaleListingRequest>()
                .FilterByNonDeleted()
                .FilterByListingId(listingSaleId);

            var queryIterator = this.cosmosLinqQuery.GetFeedIterator(query);
            if (queryIterator.HasMoreResults)
            {
                result = (await queryIterator.ReadNextAsync(cancellationToken)).Count == 1;
            }

            return result;
        }
    }
}
