namespace Husa.Quicklister.Abor.Application.Interfaces.Downloader
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Models.Agent;

    public interface IAgentServiceDownloader
    {
        Task ProcessDataFromDownloaderAsync(AgentDto agentDto);
    }
}
