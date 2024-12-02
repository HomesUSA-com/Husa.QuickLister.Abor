namespace Husa.Quicklister.Abor.Application.Services
{
    using AutoMapper;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.CTX.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Microsoft.Extensions.Logging;
    using Cities = Husa.Quicklister.Abor.Domain.Enums.Domain.Cities;
    using QLExtensions = Husa.Quicklister.Extensions.Application.Services.Agents;

    public class AgentService : QLExtensions.AgentService<IOfficeRepository, Office, OfficeValueObject, Cities, StateOrProvince>, IAgentService
    {
        public AgentService(
            IAgentRepository agentRepository,
            IOfficeRepository officeRepository,
            IDownloaderCtxClient downloaderClient,
            ILogger<AgentService> logger,
            IMapper mapper)
            : base(agentRepository, officeRepository, downloaderClient, logger, mapper)
        {
        }
    }
}
