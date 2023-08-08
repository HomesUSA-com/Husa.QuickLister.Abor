namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Agent;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Agent;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("agent")]
    [Route("agents")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentQueriesRepository agentQueriesRepository;
        private readonly ILogger<AgentsController> logger;
        private readonly IMapper mapper;

        public AgentsController(IAgentQueriesRepository agentQueriesRepository, IMapper mapper, ILogger<AgentsController> logger)
        {
            this.agentQueriesRepository = agentQueriesRepository ?? throw new ArgumentNullException(nameof(agentQueriesRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] AgentRequestFilter filter)
        {
            this.logger.LogInformation("Getting agents filtered by {@filters} in ABOR", filter);

            var requestFilter = this.mapper.Map<AgentQueryFilter>(filter);
            var queryResponse = await this.agentQueriesRepository.GetAsync(requestFilter);
            return this.Ok(this.mapper.Map<IEnumerable<AgentQueryResponse>>(queryResponse));
        }

        [HttpGet("{agentId:guid}")]
        public async Task<IActionResult> GetAgentByIdAsync([FromRoute] Guid agentId)
        {
            this.logger.LogInformation("Getting agent {agentId}.", agentId);

            var queryResponse = await this.agentQueriesRepository.GetAgentByIdAsync(agentId);
            return this.Ok(this.mapper.Map<AgentQueryResponse>(queryResponse));
        }
    }
}
