namespace Husa.Quicklister.Abor.Api.Contracts.Response.SalePropertyDetail
{
    using System;

    public class SalePropertyResponse
    {
        public string OwnerName { get; set; }

        public Guid? PlanId { get; set; }

        public string PlanName { get; set; }

        public Guid CommunityId { get; set; }

        public Guid CompanyId { get; set; }
    }
}
