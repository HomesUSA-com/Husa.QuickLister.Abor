namespace Husa.Quicklister.Abor.Api.Contracts.Response.Agent
{
    using System;

    public class AgentQueryResponse
    {
        public Guid Id { get; set; }

        public string AgentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }
        public string MemberStateLicense { get; set; }
        public string MlsId { get; set; }
    }
}
