namespace Husa.Quicklister.Abor.Api.Controllers
{
    using AutoMapper;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Microsoft.Extensions.Logging;
    using ExtensionController = Husa.Quicklister.Extensions.Api.Controllers.Agent;

    public class AgentsController : ExtensionController.AgentsController<IAgentService, IAgentQueriesRepository>
    {
        public AgentsController(IAgentQueriesRepository agentQueriesRepository, IAgentService agentService, IMapper mapper, ILogger<AgentsController> logger)
            : base(agentQueriesRepository, agentService, mapper, logger)
        {
        }
    }
}
