namespace Husa.Quicklister.Abor.Api.Contracts.Response.Plan
{
    using System;
    using Husa.Extensions.Common.Enums;

    public class PlanDataQueryResponse
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public string OwnerName { get; set; }

        public string Name { get; set; }

        public MarketCode Market { get; set; }

        public DateTime SysCreatedOn { get; set; }

        public Guid? SysCreatedBy { get; set; }

        public DateTime? SysModifiedOn { get; set; }

        public Guid? SysModifiedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}
