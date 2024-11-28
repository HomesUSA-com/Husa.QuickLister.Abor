namespace Husa.Quicklister.Abor.Data.Specifications
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Husa.Extensions.Linq;
    using Husa.Quicklister.Extensions.Domain.Entities.Agent;

    public static class AgentSpecifications
    {
        public static Expression<Func<Agent, bool>> FilterByCompanyId(Guid companyId)
        {
            return BaseSpecifications.BaseFilter<Agent>().AndIf(!companyId.Equals(Guid.Empty), a => a.CompanyId == companyId);
        }

        public static IQueryable<Agent> ApplySearchByFilter(this IQueryable<Agent> query, string searchBy)
        {
            if (string.IsNullOrEmpty(searchBy))
            {
                return query;
            }

            return query.Where(x => x.AgentValue.MarketUniqueId == searchBy
                || x.AgentValue.FirstName.Contains(searchBy)
                || x.AgentValue.LastName.Contains(searchBy)
                || x.AgentValue.Email.Contains(searchBy));
        }
    }
}
