namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Application.Tests.Mocks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Entities.Agent;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class AgentServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly AgentServiceDownloader agentService;
        private readonly Mock<IAgentRepository> agentRepository;
        private readonly Mock<ILogger<AgentServiceDownloader>> logger;

        public AgentServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.agentRepository = new AgentRepositoryMock();
            this.logger = new Mock<ILogger<AgentServiceDownloader>>();
            this.agentService = new AgentServiceDownloader(this.agentRepository.Object, this.fixture.Mapper, this.logger.Object);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndAgentNotExitsts_AgentIsCreatedSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            this.agentRepository.Setup(r => r.GetAgentByMlsId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetAgentEntity()).Verifiable();
            await this.agentService.ProcessDataFromDownloaderAsync(TestModelProvider.GetAgentDto());

            this.agentRepository.Verify(r => r.GetAgentByMlsId(It.IsAny<string>()), Times.Once);
            this.agentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Agent>()), Times.Once);
            this.agentRepository.Verify(r => r.Attach(It.IsAny<Agent>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndAgentExists_AgentIsUpdatedSuccess()
        {
            var marketUniqueId = Faker.RandomNumber.Next(10000, 90000).ToString();
            var agentDto = TestModelProvider.GetAgentDto();
            agentDto.MarketUniqueId = marketUniqueId;
            this.agentRepository.Setup(r => r.GetAgentByMlsId(It.Is<string>(muid => muid == marketUniqueId))).ReturnsAsync(TestModelProvider.GetAgentEntity()).Verifiable();

            await this.agentService.ProcessDataFromDownloaderAsync(agentDto);

            this.agentRepository.Verify(r => r.GetAgentByMlsId(It.IsAny<string>()), Times.Once);
            this.agentRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Agent>()), Times.Once);
        }
    }
}
