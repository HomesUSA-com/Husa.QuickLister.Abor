namespace Husa.Quicklister.Abor.Domain.Entities.Agent
{
    using System;
    using System.Collections.Generic;
    using Husa.Extensions.Domain.Entities;
    using Husa.Quicklister.Abor.Domain.ValueObjects;

    public class Agent : Entity
    {
        public Agent(AgentValueObject agent)
        {
            this.AgentValue = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        protected Agent()
        {
            this.AgentValue = new();
        }

        public AgentValueObject AgentValue { get; set; }

        public string FullName => $"{this.AgentValue.FirstName} {this.AgentValue.LastName}";

        public void UpdateInformation(AgentValueObject agentValue)
        {
            if (this.AgentValue == agentValue)
            {
                return;
            }

            this.AgentValue = agentValue;
        }

        protected override void DeleteChildren(Guid userId)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<object> GetEntityEqualityComponents()
        {
            yield return this.AgentValue;
        }
    }
}
