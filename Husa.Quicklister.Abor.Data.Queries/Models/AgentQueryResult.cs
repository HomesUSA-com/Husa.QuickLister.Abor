namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;

    public class AgentQueryResult
    {
        public const string SummaryField = "AgentFullName";

        public Guid Id { get; set; }

        public string AgentId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }
        public string MemberStateLicense { get; set; }

        public string SummaryValue => $"{this.AgentId} - {this.FullName} - {this.CompanyName}";
    }
}
