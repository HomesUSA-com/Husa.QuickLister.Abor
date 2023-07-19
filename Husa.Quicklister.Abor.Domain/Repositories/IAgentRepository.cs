namespace Husa.Quicklister.Abor.Domain.Repositories
{
    using System.Threading.Tasks;
    using Husa.Extensions.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;

    public interface IAgentRepository : IRepository<Agent>
    {
        Task<Agent> GetAgentByMarketUniqueId(string marketUniqueId);

        Task<Agent> GetAgentByLoginName(string loginName);
    }
}
