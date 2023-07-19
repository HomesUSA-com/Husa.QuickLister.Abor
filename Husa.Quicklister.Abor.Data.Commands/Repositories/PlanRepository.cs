namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System;
    using System.Collections.Generic;
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

        public async Task<IEnumerable<Plan>> GetByLegacyIds(IEnumerable<Guid> legacyIds, Guid companyId)
        {
            this.logger.LogInformation("Starting to get the Plan by legacy ids and companyId: {companyId}", companyId);
            return await this.context.Plan
                .Where(p => p.LegacyId.HasValue && legacyIds.Contains(p.LegacyId.Value) && p.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
