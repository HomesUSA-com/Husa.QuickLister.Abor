namespace Husa.Quicklister.Abor.Data.Queries.Models
{
    using System;

    public class SalePropertyQueryResult
    {
        public string OwnerName { get; set; }

        public Guid? PlanId { get; set; }

        public string PlanName { get; set; }

        public Guid? CommunityId { get; set; }

        public Guid? CompanyId { get; set; }
    }
}
