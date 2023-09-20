namespace Husa.Quicklister.Abor.Data.Queries.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;

    public interface IAgentQueriesRepository
    {
        Task<IEnumerable<AgentQueryResult>> GetAsync(AgentQueryFilter queryFilter);

        Task<AgentQueryResult> GetAgentByIdAsync(Guid agentId);

        Task<string> GetAgentUniqueMarketIdAsync(Guid? agentId);
    }
}
