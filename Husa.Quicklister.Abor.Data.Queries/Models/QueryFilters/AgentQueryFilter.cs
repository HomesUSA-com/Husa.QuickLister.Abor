namespace Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters
{
    using System;

    public class AgentQueryFilter : BaseQueryFilter
    {
        public Guid CompanyId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Agency { get; set; }

        public string AgentId { get; set; }
    }
}
