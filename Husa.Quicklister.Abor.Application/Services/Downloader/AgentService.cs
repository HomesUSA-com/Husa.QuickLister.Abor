namespace Husa.Quicklister.Abor.Application.Services.Downloader
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Models.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;

    public class AgentService : IAgentService
    {
        private readonly IAgentRepository agentRepository;
        private readonly ILogger<AgentService> logger;
        private readonly IMapper mapper;

        public AgentService(IAgentRepository agentRepository, IMapper mapper, ILogger<AgentService> logger)
        {
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessDataFromDownloaderAsync(AgentDto agentDto)
        {
            this.logger.LogInformation("Service agent is starting to process data from downloader for agent with agent id {agentMarketUniqueId}", agentDto.MarketUniqueId);
            var agent = await this.agentRepository.GetAgentByMarketUniqueId(agentDto.MarketUniqueId);
            var agentValue = this.mapper.Map<AgentValueObject>(agentDto);

            if (agent is null)
            {
                agent = new Agent(agentValue);
                this.agentRepository.Attach(agent);
            }
            else
            {
                agent.UpdateInformation(agentValue);
            }

            await this.agentRepository.SaveChangesAsync(agent);
        }
    }
}
