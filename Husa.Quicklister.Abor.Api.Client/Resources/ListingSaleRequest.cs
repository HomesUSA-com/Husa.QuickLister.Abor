namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Microsoft.Extensions.Logging;

    public class ListingSaleRequest : IListingSaleRequest
    {
        private readonly QuicklisterAborClient client;
        private readonly ILogger<ListingSaleRequest> logger;

        private readonly string baseUri;

        public ListingSaleRequest(QuicklisterAborClient client, ILogger<ListingSaleRequest> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/sale-listing-requests";
        }

        public async Task<DocumentGridResponse<ListingSaleRequestQueryResponse>> GetListRequestAsync(SaleListingRequestFilter requestFilter, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing sales requests with filers {@requestFilter}", requestFilter);
            var endpoint = this.baseUri
                .AddQueryString("companyId", requestFilter.CompanyId)
                .AddQueryString("requestState", requestFilter.RequestState)
                .AddQueryString("requestFieldChange", requestFilter.RequestFieldChange)
                .AddQueryString("searchFilter", requestFilter.SearchFilter)
                .AddQueryString("take", requestFilter.Take)
                .AddQueryString("sortBy", requestFilter.SortBy)
                .AddQueryString("continuationToken", requestFilter.ContinuationToken)
                .AddQueryString("currentToken", requestFilter.CurrentToken)
                .AddQueryString("isPrint", requestFilter.IsPrint);

            return await this.client.GetAsync<DocumentGridResponse<ListingSaleRequestQueryResponse>>(endpoint, token);
        }

        public async Task<ListingSaleRequestDetailResponse> GetListRequestSaleByIdAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing sales request with id {id}", id);
            return await this.client.GetAsync<ListingSaleRequestDetailResponse>($"{this.baseUri}/{id}", token);
        }

        public async Task DeleteRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Delete ABOR listing sale request with id {id}", id);
            await this.client.DeleteAsync($"{this.baseUri}/{id}/delete", token);
        }

        public async Task ReturnRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Reject ABOR listing sale request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/return", token);
        }

        public async Task ApproveRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Approve ABOR listing sale request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/approve", token);
        }

        public async Task ProcessRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Process ABOR listing sale request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/process", token);
        }

        public async Task CompleteRequestAsync(Guid id, string mlsNumber, CancellationToken token = default)
        {
            this.logger.LogInformation("Complete ABOR listing sale request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/complete", mlsNumber, token);
        }

        public async Task<IEnumerable<ListingSaleRequestQueryResponse>> GetRequestByListingSaleIdAsync(Guid listingSaleId, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing sales requests with listing sale id {listingSaleId}", listingSaleId);
            return await this.client.GetAsync<IEnumerable<ListingSaleRequestQueryResponse>>($"{this.baseUri}/{listingSaleId}", token);
        }

        public async Task<IEnumerable<SummarySectionContract>> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("GET ABOR summary for listing sale request with id {id}", id);
            return await this.client.GetAsync<IEnumerable<SummarySectionContract>>($"{this.baseUri}/{id}/summary", cancellationToken);
        }

        public async Task<IEnumerable<object>> CreateAsync(Guid listingSaleId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Create ABOR request for listing sale with id {listingSaleId}", listingSaleId);
            var endpoint = $"{this.baseUri}?listingSaleId={listingSaleId}";
            return await this.client.PostAsJsonAsync<Guid, IEnumerable<object>>(endpoint, listingSaleId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> CreateRequestsByCommunityAsync(Guid communityId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Create ABOR requests for listings from community with id {communityId}", communityId);
            return await this.client.PostAsJsonAsync<Guid, IEnumerable<Guid>>($"{this.baseUri}/create-from-community", communityId, cancellationToken);
        }
    }
}
