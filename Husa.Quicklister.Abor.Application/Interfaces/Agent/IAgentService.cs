namespace Husa.Quicklister.Abor.Application.Interfaces.Agent
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Agent;

    public interface IAgentService
    {
        Task ProcessDataFromDownloaderAsync(AgentDto agentDto);
    }
}
