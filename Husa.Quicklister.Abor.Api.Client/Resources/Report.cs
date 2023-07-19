namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Microsoft.Extensions.Logging;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response.Reports;

    public class Report : IReport
    {
        private readonly ILogger<Report> logger;
        private readonly QuicklisterAborClient client;
        private readonly string baseUri;

        public Report(QuicklisterAborClient client, ILogger<Report> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/reports";
        }

        public async Task<IEnumerable<Response.ScrapedListingQueryResponse>> GetComparisonReportAsync(
            ScrapedListingRequestFilter filter,
            CancellationToken token = default)
        {
            this.logger.LogInformation("Get comparison report from {BuilderName}", filter.BuilderName);
            var endpoint = $"{this.baseUri}/comparison-report"
                .AddQueryString("builderName", filter.BuilderName)
                .AddQueryString("searchBy", filter.SortBy);

            var response = await this.client.GetAsync<IEnumerable<Response.ScrapedListingQueryResponse>>(endpoint, token);
            return response;
        }
    }
}
