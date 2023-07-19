namespace Husa.Quicklister.Abor.Api.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Agent;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class AgentControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IAgentQueriesRepository> agentQueriesRepository;
        private readonly Mock<ILogger<AgentsController>> logger;
        public AgentControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.agentQueriesRepository = new Mock<IAgentQueriesRepository>();
            this.logger = new Mock<ILogger<AgentsController>>();
            this.Sut = new AgentsController(
                this.agentQueriesRepository.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public AgentsController Sut { get; set; }

        [Fact]
        public async Task GetAgentById_AgentFound_Success()
        {
            // Arrange
            var agentId = Guid.NewGuid();
            var agent = TestModelProvider.GetAgentQueryResult(agentId);

            this.agentQueriesRepository
            .Setup(u => u.GetAgentByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(agent)
            .Verifiable();

            // Act
            var result = await this.Sut.GetAgentByIdAsync(agentId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<AgentQueryResponse>(objectResult.Value);
            var agentDetail = Assert.IsAssignableFrom<AgentQueryResponse>(objectResult.Value);
            Assert.Equal(agentId, agentDetail.Id);
        }

        [Fact]
        public async Task GetAgentById_AgentNotFound_Error()
        {
            // Arrange
            var agentId = Guid.NewGuid();
            AgentQueryResult agent = null;

            this.agentQueriesRepository
            .Setup(u => u.GetAgentByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(agent)
            .Verifiable();

            // Act
            var result = await this.Sut.GetAgentByIdAsync(agentId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Null(objectResult.Value);
        }
    }
}
