namespace Husa.Quicklister.Abor.Data.Queries.Extensions
{
    using System.Linq;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public static class AgentQueryExtensions
    {
        public static IQueryable<AgentQueryResult> ApplySearchByAgentQueryFilter(this IQueryable<AgentQueryResult> query, AgentQueryFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));
            }

            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(x => x.LastName.Contains(filter.LastName));
            }

            if (!string.IsNullOrEmpty(filter.Agency))
            {
                query = query.Where(x => x.CompanyName.Contains(filter.Agency));
            }

            if (!string.IsNullOrEmpty(filter.AgentId))
            {
                query = query.Where(x => x.AgentId.Equals(filter.AgentId));
            }

            return query;
        }
    }
}
