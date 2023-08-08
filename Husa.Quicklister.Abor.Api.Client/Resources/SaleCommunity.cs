namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community;
    using Microsoft.Extensions.Logging;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request.Community;

    public class SaleCommunity : ISaleCommunity
    {
        private readonly ILogger<SaleCommunity> logger;
        private readonly QuicklisterAborClient client;

        private readonly string baseUri;

        public SaleCommunity(QuicklisterAborClient client, ILogger<SaleCommunity> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/sale-communities";
        }

        public async Task<Guid> CreateCommunity(Request.CreateCommunityRequest communityRequest, CancellationToken token = default)
        {
            this.logger.LogDebug("Create Community {@communityRequest}", communityRequest);
            var response = await this.client.PostAsJsonAsync<Request.CreateCommunityRequest, Guid>(this.baseUri, communityRequest, token);
            return response;
        }

        public async Task<IEnumerable<CommunityDataQueryResponse>> GetAsync(CommunityRequestFilter filter, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting communities with the filter {@filters}", filter);
            var endpoint = this.baseUri
                .AddQueryString("name", filter.Name)
                .AddQueryString("searchBy", filter.SearchBy)
                .AddQueryString("skip", filter.Skip)
                .AddQueryString("take", filter.Take)
                .AddQueryString("isOnlyCount", filter.IsOnlyCount)
                .AddQueryString("sortBy", filter.SortBy)
                .AddQueryString("xmlStatus", filter.XmlStatus);

            var response = await this.client.GetAsync<DataSet<CommunityDataQueryResponse>>(endpoint, token);
            return response.Data;
        }

        public async Task<CommunitySaleResponse> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Get community with Id: {id}.", id);
            var endpoint = $"{this.baseUri}/{id}";
            return await this.client.GetAsync<CommunitySaleResponse>(endpoint, token);
        }

        public async Task<CommunitySaleResponse> GetByNameAsync(Request.CommunityByNameFilter filter, CancellationToken token = default)
        {
            this.logger.LogInformation("Retrieving the Community By Name: '{CommunityName} from company id: {CompanyId}'.", filter.CommunityName, filter.CompanyId);
            var endpoint = $"{this.baseUri}/Name";

            if (!string.IsNullOrEmpty(filter.CommunityName) && filter.CompanyId != Guid.Empty)
            {
                endpoint = endpoint
                    .AddQueryString("companyId", filter.CompanyId)
                    .AddQueryString("planName", filter.CommunityName);
            }

            var response = await this.client.GetAsync<CommunitySaleResponse>(endpoint, token);
            return response;
        }

        public async Task UpdateCommunity(Guid id, Request.CommunitySaleRequest communitySaleRequest, CancellationToken token = default)
        {
            this.logger.LogInformation("Update Community with Id : {id}.", id);
            var endpoint = $"{this.baseUri}/{id}";
            await this.client.PutAsJsonAsync(endpoint, communitySaleRequest, token);
        }

        public async Task ApproveCommunity(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Approve Community with Id : {id}.", id);
            var endpoint = $"{this.baseUri}/{id}/approve";
            await this.client.PatchAsync(endpoint, null, token: token);
        }

        public async Task<CommunitySaleResponse> GetCommunityWithListingProjection(Guid communityId, Guid listingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Get community with Id: {communityId} and information from listing {listingId}", communityId, listingId);
            return await this.client.GetAsync<CommunitySaleResponse>($"{this.baseUri}/{communityId}/sale-listings/{listingId}", token);
        }
    }
}
