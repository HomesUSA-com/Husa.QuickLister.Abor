namespace Husa.Quicklister.Abor.Application.Interfaces.Plan
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using IPlanExtensions = Husa.Quicklister.Extensions.Application.Interfaces.Plan.IPlanService;

    public interface IPlanService : IPlanExtensions
    {
        Task<Guid> CreateAsync(PlanCreateDto planDto);

        Task UpdatePlanAsync(Guid planId, UpdatePlanDto updatePlanDto);
    }
}
