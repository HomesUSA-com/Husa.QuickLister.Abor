namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AgentRepository : Repository<Agent>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<AgentRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }

        public Task<Agent> GetAgentByMarketUniqueId(string marketUniqueId)
        {
            this.logger.LogInformation("Starting to get agent with Uid {marketUniqueId}", marketUniqueId);
            return this.context.Agent.SingleOrDefaultAsync(x => x.AgentValue.MarketUniqueId == marketUniqueId);
        }

        public Task<Agent> GetAgentByLoginName(string loginName)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                this.logger.LogWarning("Skipping the query retrieve the agent information since the variable {loginName} does not have a valid value", nameof(loginName));
                return Task.FromResult((Agent)null);
            }

            return this.context.Agent.SingleOrDefaultAsync(x => x.AgentValue.LoginName == loginName);
        }
    }
}
