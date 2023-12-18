namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Alert;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;

    public class Alert : IAlert
    {
        private readonly ILogger<Alert> logger;
        private readonly QuicklisterAborClient client;

        private readonly string baseUri;

        public Alert(QuicklisterAborClient client, ILogger<Alert> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/alerts";
        }

        public async Task<int> GetAlertTotal(IEnumerable<AlertType> alerts, CancellationToken token = default)
        {
            this.logger.LogInformation("Get Alerts Total for {alertTypes}", string.Join(",", alerts));
            var endpoint = this.baseUri
                .AddQueryString("alerts", alerts);

            var response = await this.client.GetAsync<int>(endpoint, token);
            return response;
        }

        public Task<DataSet<AlertDetailResponse>> GetAsync(AlertType alertType, BaseAlertFilterRequest filters, CancellationToken token = default)
        {
            this.logger.LogInformation("Getting records for alert {alertType}", alertType);
            var endpoint = this.baseUri + $"/{alertType}"
                .AddQueryString("skip", filters.Skip)
                .AddQueryString("take", filters.Take)
                .AddQueryString("searchBy", filters.SearchBy)
                .AddQueryString("isOnlyCount", filters.IsOnlyCount);
            return this.client.GetAsync<DataSet<AlertDetailResponse>>(endpoint, token);
        }
    }
}
