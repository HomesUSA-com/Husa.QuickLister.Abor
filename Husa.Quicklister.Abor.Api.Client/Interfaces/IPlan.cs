namespace Husa.Quicklister.Abor.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Request = Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Response = Husa.Quicklister.Abor.Api.Contracts.Response.Plan;

    public interface IPlan
    {
        Task<Guid> CreatePlan(Request.CreatePlanRequest planRequest, CancellationToken token = default);

        Task<IEnumerable<Response.PlanDataQueryResponse>> GetAsync(Request.PlanRequestFilter filter, CancellationToken token = default);

        Task<Response.PlanDetailResponse> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<Response.PlanDetailResponse> GetByNameAsync(Request.PlanByNameFilter filter, CancellationToken token = default);

        Task UpdatePlan(Guid id, Request.UpdatePlanRequest planRequest, CancellationToken token = default);

        Task<Response.PlanDetailResponse> GetPlanWithListingProjection(Guid planId, Guid listingId, CancellationToken token = default);

        Task ApprovePlan(Guid id, CancellationToken token = default);
    }
}
