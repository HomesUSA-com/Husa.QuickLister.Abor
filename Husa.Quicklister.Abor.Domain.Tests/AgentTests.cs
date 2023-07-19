namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class AgentTests
    {
        [Fact]
        public void CreateAgentMarketModifiedIsTheSameFails()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Agent(agent: null));
        }

        [Fact]
        public void UpdateAgentMarketModifiedIsTheSameFails()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var agentValueObject = new AgentValueObject
            {
                Email = "old-email@email.com",
                MarketModified = now,
            };
            var sut = new Agent(agentValueObject);
            var updatedAgent = new AgentValueObject
            {
                Email = "some-email@email.com",
                MarketModified = now,
            };

            // Act
            sut.UpdateInformation(updatedAgent);

            // Assert
            Assert.NotEqual(updatedAgent.Email, sut.AgentValue.Email);
        }

        [Fact]
        public void UpdateAgentSuccess()
        {
            // Arrange
            var firstName = "test-name";
            var lastName = "test-last-name";
            var fullName = $"{firstName} {lastName}";

            var now = DateTime.UtcNow;
            var agentValueObject = new AgentValueObject
            {
                Email = "old-email@email.com",
                MarketModified = now,
                FirstName = "homes",
                LastName = "usa",
            };
            var sut = new Agent(agentValueObject);
            var updatedAgent = new AgentValueObject
            {
                Email = "some-email@email.com",
                MarketModified = now.AddDays(1),
                FirstName = firstName,
                LastName = lastName,
            };

            // Act
            sut.UpdateInformation(updatedAgent);

            // Assert
            Assert.Equal(updatedAgent.Email, sut.AgentValue.Email);
            Assert.Equal(fullName, sut.FullName);
        }
    }
}
