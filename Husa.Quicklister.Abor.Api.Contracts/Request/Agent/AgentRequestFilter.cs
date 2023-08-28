namespace Husa.Quicklister.Abor.Api.Contracts.Request.Agent
{
    using System;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;

    public class AgentRequestFilter : BaseFilterRequest
    {
        public Guid CompanyId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Agency { get; set; }

        public string AgentId { get; set; }
    }
}
