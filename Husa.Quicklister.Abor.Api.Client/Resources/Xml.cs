namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Xml;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Xml;
    using Husa.Quicklister.Abor.Domain.Enums.Xml;
    using Microsoft.Extensions.Logging;

    public class Xml : IXml
    {
        private readonly ILogger<Xml> logger;
        private readonly QuicklisterAborClient client;

        private readonly string baseUri;

        public Xml(QuicklisterAborClient client, ILogger<Xml> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/xml-listings";
        }

        public Task<DataSet<XmlListingResponse>> GetListings(XmlListingFilterRequest filter, CancellationToken token = default)
        {
            var endpoint = this.baseUri
                .AddQueryString("skip", filter.Skip)
                .AddQueryString("take", filter.Take)
                .AddQueryString("isOnlyCount", filter.IsOnlyCount)
                .AddQueryString("importStatus", filter.ImportStatus);
            return this.client.GetAsync<DataSet<XmlListingResponse>>(endpoint, token);
        }

        public async Task ProcessListingAsync(Guid xmlListingId, ListActionType type, CancellationToken token = default)
        {
            this.logger.LogInformation("Process xml listing with id '{listingId}'", xmlListingId);
            var endpoint = $"{this.baseUri}/{xmlListingId}";
            await this.client.PostAsJsonAsync(endpoint, new { type }, token);
        }

        public async Task ListLaterAsync(Guid xmlListingId, DateTime listOn, CancellationToken token = default)
        {
            this.logger.LogInformation("Mark as list later the listing with id '{listingId}'", xmlListingId);
            var endpoint = $"{this.baseUri}/{xmlListingId}";
            await this.client.PatchAsJsonAsync(endpoint, listOn, token);
        }

        public async Task DeleteListingAsync(Guid xmlListingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Delete xml listing with id '{listingId}'", xmlListingId);
            var endpoint = $"{this.baseUri}/{xmlListingId}";
            await this.client.DeleteAsync(endpoint, token);
        }

        public async Task RestoreListingAsync(Guid xmlListingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Restore xml listing with id '{listingId}'", xmlListingId);
            var endpoint = $"{this.baseUri}/{xmlListingId}/restore";
            await this.client.PatchAsync(endpoint, null, token);
        }
    }
}
