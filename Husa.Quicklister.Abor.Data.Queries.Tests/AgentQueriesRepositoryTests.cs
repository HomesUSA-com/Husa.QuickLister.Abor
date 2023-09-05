namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Office;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]
    public class AgentQueriesRepositoryTests
    {
        private readonly Mock<ILogger<AgentQueriesRepository>> logger = new();

        [Fact]
        public void NullContextFails()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new AgentQueriesRepository(null, null));
        }

        [Fact]
        public void NullLoggerFails()
        {
            // Arrange && Act && Assert
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot());
            var dbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            Assert.Throws<ArgumentNullException>(() => new AgentQueriesRepository(dbContext, null));
        }

        [Fact]
        public async Task GetAsyncSuccess()
        {
            // Arrange
            var officeId = "123456";
            var office = TestModelProvider.GetOfficeEntity(officeId);
            var agent = TestModelProvider.GetAgentEntity(null, officeId);
            var sut = this.GetInMemoryRepository(
                new List<Office> { office },
                new List<Agent> { agent });
            var queryFilter = new AgentQueryFilter();

            // Act
            var result = await sut.GetAsync(queryFilter);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetAsyncNotFoundFails()
        {
            // Arrange
            var officeId = "123456";
            var differentOfficeId = "123465";
            var office = TestModelProvider.GetOfficeEntity(officeId);
            var agent = TestModelProvider.GetAgentEntity(null, differentOfficeId);
            var sut = this.GetInMemoryRepository(
                new List<Office> { office },
                new List<Agent> { agent });
            var queryFilter = new AgentQueryFilter
            {
                CompanyId = Guid.NewGuid(),
            };

            // Act
            var result = await sut.GetAsync(queryFilter);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAgentByIdAsyncSuccess()
        {
            // Arrange
            var officeId = "123456";
            var office = TestModelProvider.GetOfficeEntity(officeId);
            var agent = TestModelProvider.GetAgentEntity(null, officeId);
            var sut = this.GetInMemoryRepository(
                new List<Office> { office },
                new List<Agent> { agent });

            // Act
            var result = await sut.GetAgentByIdAsync(agent.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAgentByIdAsyncNotFoundFails()
        {
            // Arrange
            var officeId = "123456";
            var differentOfficeId = "123465";
            var office = TestModelProvider.GetOfficeEntity(officeId);
            var agent = TestModelProvider.GetAgentEntity(null, differentOfficeId);
            var sut = this.GetInMemoryRepository(
                new List<Office> { office },
                new List<Agent> { agent });

            // Act
            var result = await sut.GetAgentByIdAsync(agent.Id);

            // Assert
            Assert.Null(result);
        }

        private AgentQueriesRepository GetInMemoryRepository(IEnumerable<Office> offices, IEnumerable<Agent> agents)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            foreach (var office in offices)
            {
                dbContext.Office.Add(office);
            }

            foreach (var agent in agents)
            {
                dbContext.Agent.Add(agent);
            }

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new AgentQueriesRepository(
                queriesDbContext,
                this.logger.Object);
        }
    }
}
