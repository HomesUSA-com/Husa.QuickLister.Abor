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

        public Task<Agent> GetAgentByMemberStateLicense(string input)
        {
            this.logger.LogInformation("Starting to get agent with MemberStateLicense {marketUniqueId}", input);
            return this.context.Agent.FirstOrDefaultAsync(x => x.AgentValue.MemberStateLicense == input);
        }

        public Task<Agent> GetAgentByMlsId(string mlsId)
        {
            this.logger.LogInformation("Starting to get agent with mlsId {mlsId}", mlsId);
            return this.context.Agent.SingleOrDefaultAsync(x => x.AgentValue.MlsId == mlsId);
        }
    }
}
