namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Extensions.Linq.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using OfficeStatus = Husa.Downloader.CTX.Domain.Enums.OfficeStatus;

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
            this.logger.LogInformation("Getting the list of agents by {@filterOptions}", queryFilter);
            return await (from agent in this.context.Agent
                            join office in this.context.Office
                            on agent.AgentValue.OfficeId equals office.OfficeValue.MarketUniqueId
                            where agent.AgentValue.Status == MemberStatus.Active
                            where office.OfficeValue.Status == OfficeStatus.Active
                            select new AgentQueryResult
                            {
                                Id = agent.Id,
                                FirstName = agent.AgentValue.FirstName,
                                LastName = agent.AgentValue.LastName,
                                FullName = agent.FullName,
                                AgentId = agent.AgentValue.MarketUniqueId,
                                CompanyName = office.OfficeValue.Name,
                                MemberStateLicense = agent.AgentValue.MemberStateLicense,
                                MlsId = agent.AgentValue.MlsId,
                            })
                            .ApplySearchByAgentQueryFilter(queryFilter)
                            .ApplySortByFields(queryFilter.SortBy)
                            .ApplyPaginationFilter(queryFilter.Skip, queryFilter.Take)
                            .ToListAsync();
        }

        public async Task<AgentQueryResult> GetAgentByIdAsync(Guid agentId)
        {
            this.logger.LogInformation("Getting agent by id {agentId}", agentId);
            return await (from agent in this.context.Agent
                         join office in this.context.Office
                         on agent.AgentValue.OfficeId equals office.OfficeValue.MarketUniqueId
                         where agent.AgentValue.Status == MemberStatus.Active && office.OfficeValue.Status == OfficeStatus.Active && agent.Id == agentId
                         select new AgentQueryResult
                         {
                             Id = agent.Id,
                             FirstName = agent.AgentValue.FirstName,
                             LastName = agent.AgentValue.LastName,
                             FullName = agent.FullName,
                             AgentId = agent.AgentValue.MarketUniqueId,
                             CompanyName = office.OfficeValue.Name,
                             MemberStateLicense = agent.AgentValue.MemberStateLicense,
                             MlsId = agent.AgentValue.MlsId,
                         })
                        .SingleOrDefaultAsync();
        }

        public async Task<string> GetAgentUniqueMarketIdAsync(Guid? agentId)
        {
            if (agentId is null || agentId == Guid.Empty)
            {
                return string.Empty;
            }

            var agentInfo = await this.GetAgentByIdAsync((Guid)agentId);
            return agentInfo?.MlsId ?? string.Empty;
        }
    }
}
