namespace Husa.Quicklister.Abor.Api.Contracts.Request.Agent
{
    using System;

    public class AgentRequestFilter : BaseFilterRequest
    {
        public Guid CompanyId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Agency { get; set; }

        public string AgentId { get; set; }
    }
}
