namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Specifications;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AgentQueriesRepository : IAgentQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly ILogger<AgentQueriesRepository> logger;

        public AgentQueriesRepository(
            ApplicationQueriesDbContext context,
            ILogger<AgentQueriesRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AgentQueryResult>> GetAsync(AgentQueryFilter queryFilter)
        {
            var filterOptions = !queryFilter.CompanyId.Equals(Guid.Empty) ? $"with company id {queryFilter.CompanyId}" : string.Empty;
            this.logger.LogInformation($"Starting to get the ABOR list agents {filterOptions}");

            return await (from agent in this.context.Agent
                            join office in this.context.Office
                            on agent.AgentValue.OfficeId equals office.OfficeValue.MarketUniqueId
                            where agent.AgentValue.Status == "Active"
                            where office.OfficeValue.Status == "Active"
                            select new AgentQueryResult
                            {
                                Id = agent.Id,
                                FirstName = agent.AgentValue.FirstName,
                                LastName = agent.AgentValue.LastName,
                                FullName = agent.FullName,
                                AgentId = agent.AgentValue.LoginName,
                                CompanyName = office.OfficeValue.Name,
                            })
                            .ApplySearchByAgentQueryFilter(queryFilter)
                            .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take)
                            .ApplySortByFields(queryFilter.SortBy)
                            .ToListAsync();
        }

        public async Task<AgentQueryResult> GetAgentByIdAsync(Guid agentId)
        {
            this.logger.LogInformation($"Starting to get ABOR agent with id: {agentId}");

            return await (from agent in this.context.Agent
                         join office in this.context.Office
                         on agent.AgentValue.OfficeId equals office.OfficeValue.MarketUniqueId
                         where agent.AgentValue.Status == "Active" && office.OfficeValue.Status == "Active" && agent.Id == agentId
                         select new AgentQueryResult
                         {
                             Id = agent.Id,
                             FirstName = agent.AgentValue.FirstName,
                             LastName = agent.AgentValue.LastName,
                             FullName = agent.FullName,
                             AgentId = agent.AgentValue.LoginName,
                             CompanyName = office.OfficeValue.Name,
                         })
                        .SingleOrDefaultAsync();
        }
    }
}
