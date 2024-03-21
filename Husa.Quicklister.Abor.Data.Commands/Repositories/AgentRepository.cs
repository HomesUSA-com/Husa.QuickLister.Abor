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
            var newId = input.Length > 0 && input[0] != '0' ? input.PadLeft(input.Length + 1, '0') : input;
            this.logger.LogInformation("Starting to get agent with Uid {marketUniqueId}", newId);
            return this.context.Agent.FirstOrDefaultAsync(x => x.AgentValue.MemberStateLicense == newId);
        }
    }
}
