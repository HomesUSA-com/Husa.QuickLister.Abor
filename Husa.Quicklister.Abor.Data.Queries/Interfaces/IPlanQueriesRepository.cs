namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public interface IPlanQueriesRepository
    {
        Task<DataSet<PlanQueryResult>> GetAsync(PlanQueryFilter queryFilter);

        Task<PlanDetailQueryResult> GetPlanById(Guid id);

        Task<PlanDetailQueryResult> GetPlanByByName(Guid companyId, string planName);

        Task<PlanDetailQueryResult> GetByIdWithListingImportProjection(Guid id, Guid listingId);
    }
}
