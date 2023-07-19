namespace Husa.Quicklister.Abor.Api.Client.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Microsoft.Extensions.Logging;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response.Plan;

    public class Plan : IPlan
    {
        private readonly ILogger<Plan> logger;
        private readonly QuicklisterAborClient client;

        private readonly string baseUri;

        public Plan(QuicklisterAborClient client,  ILogger<Plan> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.baseUri = "api/plans";
        }

        public async Task<Guid> CreatePlan(Request.CreatePlanRequest planRequest, CancellationToken token = default)
        {
            this.logger.LogInformation("Create Plan.");
            var response = await this.client.PostAsJsonAsync<Request.CreatePlanRequest, Guid>(this.baseUri, planRequest, token);
            return response;
        }

        public async Task<IEnumerable<Response.PlanDataQueryResponse>> GetAsync(PlanRequestFilter filter, CancellationToken token = default)
        {
            this.logger.LogInformation("Get plan.");
            var endpoint = this.baseUri.AddQueryString("searchBy", filter.SearchBy);
            var response = await this.client.GetAsync<DataSet<Response.PlanDataQueryResponse>>(endpoint, token);
            return response.Data;
        }

        public async Task<Response.PlanDetailResponse> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Get plan with Id: {id}.", id);
            return await this.client.GetAsync<Response.PlanDetailResponse>($"{this.baseUri}/{id}", token);
        }

        public async Task<Response.PlanDetailResponse> GetByNameAsync(Request.PlanByNameFilter filter, CancellationToken token = default)
        {
            this.logger.LogInformation("Retrieving the Plan By Name: '{PlanName} from company id: {CompanyId}'.", filter.PlanName, filter.CompanyId);
            var endpoint = $"{this.baseUri}/Name";

            if (!string.IsNullOrEmpty(filter.PlanName) && filter.CompanyId != Guid.Empty)
            {
                endpoint = $"{endpoint}?companyId={filter.CompanyId}&planName={filter.PlanName}";
            }

            var response = await this.client.GetAsync<Response.PlanDetailResponse>(endpoint, token);
            return response;
        }

        public async Task UpdatePlan(Guid id, Request.UpdatePlanRequest planRequest, CancellationToken token = default)
        {
            this.logger.LogInformation("Update Plan with Id : {id}.", id);
            var endpoint = $"{this.baseUri}/{id}";
            await this.client.PutAsJsonAsync(endpoint, planRequest, token);
        }

        public async Task<Response.PlanDetailResponse> GetPlanWithListingProjection(Guid planId, Guid listingId, CancellationToken token = default)
        {
            this.logger.LogInformation("Starting the process to import information from listing Id '{listingId}' to plan profile id '{planId}'", listingId, planId);
            return await this.client.GetAsync<Response.PlanDetailResponse>($"{this.baseUri}/{planId}/sale-listings/{listingId}", token);
        }

        public async Task ApprovePlan(Guid id, CancellationToken token = default)
        {
            this.logger.LogInformation("Approve plan with Id : {id}.", id);
            var endpoint = $"{this.baseUri}/{id}/approve";
            await this.client.PatchAsync(endpoint, null, token: token);
        }
    }
}
