namespace Husa.Quicklister.Abor.Data.Commands.Repositories
{
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using QLExtension = Husa.Quicklister.Extensions.Data.Repositories;

    public class AgentRepository : QLExtension.AgentRepository<ApplicationDbContext>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext context, IUserContextProvider userContextProvider, ILogger<OfficeRepository> logger)
            : base(context, userContextProvider, logger)
        {
        }
    }
}
