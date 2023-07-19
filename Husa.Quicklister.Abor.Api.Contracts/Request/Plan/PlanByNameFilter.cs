namespace Husa.Quicklister.Abor.Api.Contracts.Request.Plan
{
    using System;

    public class PlanByNameFilter
    {
        public Guid CompanyId { get; set; }

        public string PlanName { get; set; }
    }
}
