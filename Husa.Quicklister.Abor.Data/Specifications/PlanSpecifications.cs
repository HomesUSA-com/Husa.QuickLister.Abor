namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System.Linq;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;

    public static class PlanSpecifications
    {
        public static IQueryable<Plan> ApplySearchByFilter(this IQueryable<Plan> query, string searchByFilter)
        {
            if (string.IsNullOrEmpty(searchByFilter))
            {
                return query;
            }

            return query.Where(x => x.BasePlan.Name.Contains(searchByFilter));
        }

        public static IQueryable<Plan> FilterByPlanName(this IQueryable<Plan> query, string planName)
        {
            return query.Where(p => p.BasePlan.Name.Equals(planName));
        }
    }
}
