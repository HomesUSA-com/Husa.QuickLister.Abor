namespace Husa.Quicklister.Abor.Application.Interfaces.Plan
{
    using System;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Plan;

    public interface IPlanService
    {
        Task<Guid> CreateAsync(PlanCreateDto planDto);

        Task DeletePlan(Guid planId, bool deleteInCascade = false);

        Task UpdatePlanAsync(Guid planId, UpdatePlanDto updatePlanDto);
    }
}
