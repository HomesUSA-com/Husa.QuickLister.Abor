namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System;

    public class BasePropertyRequest
    {
        public Guid CompanyId { get; set; }

        public Guid CommunityId { get; set; }

        public Guid PlanId { get; set; }
    }
}
