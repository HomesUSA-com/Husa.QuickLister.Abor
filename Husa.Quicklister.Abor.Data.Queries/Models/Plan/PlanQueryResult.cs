namespace Husa.Quicklister.Abor.Data.Queries.Models.Plan
{
    using System;
    using Husa.Extensions.Common.Enums;

    public class PlanQueryResult : BaseQueryResult
    {
        public Guid CompanyId { get; set; }

        public string Name { get; set; }

        public MarketCode Market { get; set; }
    }
}
