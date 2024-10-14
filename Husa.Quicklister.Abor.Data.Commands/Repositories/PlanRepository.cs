namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        public PlanRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<PlanRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public async Task<Plan> GetPlan(string name, Guid companyId)
        {
            this.logger.LogInformation($"Starting to get the Plan name: {name} companyId: {companyId}");
            return await this.context.Plan
                .Where(p => p.BasePlan.Name == name && p.CompanyId == companyId)
                .SingleOrDefaultAsync();
        }

        public async Task<Guid?> GetIdByLegacyId(int legacyId)
        {
            this.logger.LogInformation("Starting to get the Plan by legacy id {legacyId}", legacyId);
            var plan = await this.context.Plan.FirstOrDefaultAsync(p => p.LegacyProfileId.HasValue && p.LegacyProfileId.Value == legacyId);
            return plan?.Id;
        }
    }
}
