namespace Husa.Quicklister.Abor.Application.Tests.Mocks
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Moq;

    public class AgentRepositoryMock : Mock<IAgentRepository>
    {
        public AgentRepositoryMock()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.Setup(r => r.AddAsync(It.IsAny<Agent>())).ReturnsAsync(TestModelProvider.GetAgentEntity()).Verifiable();
            this.Setup(r => r.SaveChangesAsync(It.IsAny<Agent>())).Returns(Task.CompletedTask).Verifiable();
        }
    }
}
