namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public interface IPlanRepository : IRepository<Plan>
    {
        Task<Plan> GetPlan(string name, Guid companyId);

        Task<IEnumerable<Plan>> GetByLegacyIds(IEnumerable<Guid> legacyIds, Guid companyId);
    }
}
