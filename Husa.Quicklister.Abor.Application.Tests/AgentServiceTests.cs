namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Application.Tests.Mocks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class AgentServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly AgentService agentService;
        private readonly Mock<IAgentRepository> agentRepository;
        private readonly Mock<ILogger<AgentService>> logger;

        public AgentServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.agentRepository = new AgentRepositoryMock();
            this.logger = new Mock<ILogger<AgentService>>();
            this.agentService = new AgentService(this.agentRepository.Object, this.fixture.Mapper, this.logger.Object);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndAgentNotExitsts_AgentIsCreatedSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            this.agentRepository.Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetAgentEntity()).Verifiable();
            await this.agentService.ProcessDataFromDownloaderAsync(TestModelProvider.GetAgentDto());

            this.agentRepository.Verify(r => r.GetAgentByMarketUniqueId(It.IsAny<string>()), Times.Once);
            this.agentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Agent>()), Times.Once);
            this.agentRepository.Verify(r => r.Attach(It.IsAny<Agent>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndAgentExists_AgentIsUpdatedSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            var agentDto = TestModelProvider.GetAgentDto();
            agentDto.MarketUniqueId = marketUniqueId;
            this.agentRepository.Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetAgentEntity()).Verifiable();

            await this.agentService.ProcessDataFromDownloaderAsync(agentDto);

            this.agentRepository.Verify(r => r.GetAgentByMarketUniqueId(It.IsAny<string>()), Times.Once);
            this.agentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Agent>()), Times.Once);
        }
    }
}
