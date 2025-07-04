namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Uploader;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using ExtensionsContract = Husa.Quicklister.Extensions.Api.Contracts;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response;

    public class SaleListing : ISaleListing
    {
        private readonly ILogger<SaleListing> logger;
        private readonly QuicklisterAborClient client;

        private readonly string baseUri;

        public SaleListing(QuicklisterAborClient client, ILogger<SaleListing> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/sale-listings";
        }

        public async Task<Guid> CreateListing(Request.QuickCreateListingRequest listingSaleRequest, CancellationToken token = default)
        {
            this.logger.LogDebug("Creating listing {@saleListingRequest}", listingSaleRequest);
            var response = await this.client.PostAsJsonAsync<Request.QuickCreateListingRequest, Guid>(this.baseUri, listingSaleRequest, token);
            return response;
        }

        public async Task<IEnumerable<Response.ListingResponse>> GetAsync(Request.ListingRequestFilter filters, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting listings with the filter {@filters}", filters);
            var endpoint = this.GetListingsEndpoint(filters)
                .AddQueryString("mlsStatus", filters.MlsStatus)
                .AddQueryString("isCompleteHome", filters.IsCompleteHome);
            var response = await this.client.GetAsync<DataSet<Response.ListingResponse>>(endpoint, token);
            return response.Data;
        }

        public async Task<Response.ListingSaleDetailResponse> GetByAddressAsync(Request.ListingSaleFilterByAddress filter, CancellationToken token = default)
        {
            var endpoint = $"{this.baseUri}/Address";
            if (!string.IsNullOrEmpty(filter.StreetName) && !string.IsNullOrEmpty(filter.StreetNumber) && !string.IsNullOrEmpty(filter.ZipCode))
            {
                this.logger.LogInformation("Get listing by Address {streetName} {streetNumber}, zipcode {zipCode}.", filter.StreetName, filter.StreetNumber, filter.ZipCode);

                endpoint = endpoint
                .AddQueryString("streetName", filter.StreetName)
                .AddQueryString("streetNumber", filter.StreetNumber)
                .AddQueryString("zipCode", filter.ZipCode);
            }

            var response = await this.client.GetAsync<Response.ListingSaleDetailResponse>(endpoint, token);
            return response;
        }

        public async Task<DataSet<Response.ListingSaleOpenHouseResponse>> GetListingsWithOpenHouse(ExtensionsContract.Request.BaseFilterRequest filters, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting listings with Open House with the filter {@filter}", filters);
            var endpoint = $"{this.baseUri}/open-house"
                .AddQueryString("skip", filters.Skip)
                .AddQueryString("take", filters.Take);
            var response = await this.client.GetAsync<DataSet<Response.ListingSaleOpenHouseResponse>>(endpoint, token);
            return response;
        }

        public async Task<Response.ListingSaleDetailResponse> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Get listing with Id: {id}.", id);
            return await this.client.GetAsync<Response.ListingSaleDetailResponse>($"{this.baseUri}/{id}", token);
        }

        public async Task UpdateListing(Guid id, Request.ListingSaleDetailRequest saleListingRequest, CancellationToken token = default)
        {
            this.logger.LogInformation("Update listing with Id : {id}.", id);
            var endpoint = $"{this.baseUri}/{id}";
            await this.client.PutAsJsonAsync(endpoint, saleListingRequest, token);
        }

        public async Task<CommandResult<ReverseProspectResponse>> GetReverseProspect(Guid listingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Update listing with Id : {listingId}.", listingId);
            var endpoint = $"{this.baseUri}/{listingId}/reverse-prospect";
            var response = await this.client.GetAsync<CommandResult<ReverseProspectResponse>>(endpoint, token);
            return response;
        }

        public async Task UpdateActionTypeAsync(Guid listingId, ActionType actionType, CancellationToken token = default)
        {
            this.logger.LogInformation("Update action type from listing with id {listingId}", listingId);
            await this.client.PatchAsJsonAsync($"{this.baseUri}/{listingId}/action-type", new { actionType }, token);
        }

        public async Task<ExtensionsContract.Response.CallForwardResponse> GetForwardingPhoneFromMlsNumber(string mlsNumber, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting Forward phone for listing with MLS: {mlsNumber}.", mlsNumber);
            var endpoint = $"{this.baseUri}/call-forward/{mlsNumber}";
            var response = await this.client.GetAsync<ExtensionsContract.Response.CallForwardResponse>(endpoint, token);
            return response;
        }

        public async Task<IEnumerable<ExtensionsContract.Response.Listing.ListingResponse>> GetListings(ExtensionsContract.Request.Listing.IListingRequestFilter filters, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting listings with the filter {@filters}", filters);
            var endpoint = this.GetListingsEndpoint(filters);
            var response = await this.client.GetAsync<DataSet<ExtensionsContract.Response.Listing.ListingResponse>>(endpoint, token);
            return response.Data;
        }

        public Task<ExtensionsContract.Response.EmailLeadResponse> GetEmailLeads(Guid listingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting email leads for listing with id: {listingId}.", listingId);
            var endpoint = $"{this.baseUri}/{listingId}/email-leads";
            return this.client.GetAsync<ExtensionsContract.Response.EmailLeadResponse>(endpoint, token);
        }

        public Task UnlockUnsubmittedListings(CancellationToken token = default)
        {
            var endpoint = $"{this.baseUri}/unlock";
            return this.client.PatchAsync(endpoint, null, token: token);
        }

        public Task<IEnumerable<ListingLockedBySystemResponse>> GetListingLockedBySystemAsync(CancellationToken token = default)
        {
            this.logger.LogInformation("Getting listings awaiting for Mls Update");
            var endpoint = $"{this.baseUri}/locked-by-system";
            return this.client.GetAsync<IEnumerable<ExtensionsContract.Response.Listing.ListingLockedBySystemResponse>>(endpoint, token);
        }

        public Task AutomaticReverseProspect(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        private string GetListingsEndpoint(ExtensionsContract.Request.Listing.IListingRequestFilter filters)
        {
            return this.baseUri
                .AddQueryString("streetName", filters.StreetName)
                .AddQueryString("streetNumber", filters.StreetNumber)
                .AddQueryString("mlsNumber", filters.MlsNumber)
                .AddQueryString("zipCode", filters.ZipCode);
        }
    }
}
