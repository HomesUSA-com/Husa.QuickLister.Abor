namespace Husa.Quicklister.Abor.Api.Client.Resources.LotListing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Quicklister.Abor.Api.Client.Interfaces.LotListing;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Microsoft.Extensions.Logging;

    public class ListingLotRequest : IListingLotRequest
    {
        private readonly QuicklisterAborClient client;
        private readonly ILogger<ListingLotRequest> logger;

        private readonly string baseUri;

        public ListingLotRequest(QuicklisterAborClient client, ILogger<ListingLotRequest> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/lot-listing-requests";
        }

        public async Task<DocumentGridResponse<ListingLotRequestQueryResponse>> GetListRequestAsync(SaleListingRequestFilter requestFilter, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing lots requests with filers {@requestFilter}", requestFilter);
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

            return await this.client.GetAsync<DocumentGridResponse<ListingLotRequestQueryResponse>>(endpoint, token);
        }

        public async Task<LotListingRequestDetailResponse> GetListRequestSaleByIdAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing lots request with id {id}", id);
            return await this.client.GetAsync<LotListingRequestDetailResponse>($"{this.baseUri}/{id}", token);
        }

        public async Task DeleteRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Delete ABOR listing lot request with id {id}", id);
            await this.client.DeleteAsync($"{this.baseUri}/{id}/delete", token);
        }

        public async Task ReturnRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Reject ABOR listing lot request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/return", token);
        }

        public async Task ApproveRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Approve ABOR listing lot request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/approve", token);
        }

        public async Task ProcessRequestAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Process ABOR listing lot request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/process", token);
        }

        public async Task CompleteRequestAsync(Guid id, string mlsNumber, CancellationToken token = default)
        {
            this.logger.LogInformation("Complete ABOR listing lot request with id {id}", id);
            await this.client.PutAsJsonAsync($"{this.baseUri}/{id}/complete", mlsNumber, token);
        }

        public async Task<IEnumerable<ListingLotRequestQueryResponse>> GetRequestByListingSaleIdAsync(Guid listingSaleId, CancellationToken token = default)
        {
            this.logger.LogInformation("Get ABOR listing lots requests with listing lot id {listingLotId}", listingSaleId);
            return await this.client.GetAsync<IEnumerable<ListingLotRequestQueryResponse>>($"{this.baseUri}/{listingSaleId}", token);
        }

        public async Task<IEnumerable<SummarySectionContract>> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("GET ABOR summary for listing lot request with id {id}", id);
            return await this.client.GetAsync<IEnumerable<SummarySectionContract>>($"{this.baseUri}/{id}/summary", cancellationToken);
        }

        public async Task<IEnumerable<object>> CreateAsync(Guid listingSaleId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Create ABOR request for listing lot with id {listingLotId}", listingSaleId);
            var endpoint = $"{this.baseUri}?listingLotId={listingSaleId}";
            return await this.client.PostAsJsonAsync<Guid, IEnumerable<object>>(endpoint, listingSaleId, cancellationToken);
        }

        public async Task<IEnumerable<object>> CreateTaxIdRequestAsync(Guid listingSaleId, string taxId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Create ABOR Tax Id request for listing sale with id {listingSaleId}", listingSaleId);
            var endpoint = $"{this.baseUri}/{listingSaleId}/tax-id-request";
            return await this.client.PostAsJsonAsync<string, IEnumerable<object>>(endpoint, taxId, cancellationToken);
        }

        public async Task<IEnumerable<Guid>> CreateRequestsByCommunityAsync(Guid communityId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Create ABOR requests for listings from community with id {communityId}", communityId);
            return await this.client.PostAsJsonAsync<Guid, IEnumerable<Guid>>($"{this.baseUri}/create-from-community", communityId, cancellationToken);
        }

        public Task MediaVerification(Guid listingRequestId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
