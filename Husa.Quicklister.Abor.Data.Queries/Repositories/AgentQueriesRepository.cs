namespace Husa.Quicklister.Abor.Data.Queries.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Linq.Specifications;
    using Husa.Quicklister.Abor.Data.Queries.Extensions;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Data.Queries.Models.Agent;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtensions = Husa.Quicklister.Extensions.Data.Queries.Repositories.Agent;

    public class AgentQueriesRepository : QLExtensions.AgentQueriesRepository<Office, ApplicationQueriesDbContext, OfficeValueObject, Cities, Downloader.CTX.Domain.Enums.StateOrProvince>, IAgentQueriesRepository
    {
        private readonly ApplicationQueriesDbContext context;
        private readonly ILogger<AgentQueriesRepository> logger;

        public AgentQueriesRepository(
            ILogger<AgentQueriesRepository> logger,
            ApplicationQueriesDbContext context)
            : base(logger, context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async override Task<IEnumerable<AgentQueryResult>> GetAsync(AgentQueryFilter queryFilter)
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
    }
}
