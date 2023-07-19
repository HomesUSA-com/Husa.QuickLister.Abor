namespace Husa.Quicklister.Abor.Api.Contracts.Request.SalePropertyDetail
{
    using System;
    using Husa.Quicklister.Abor.Api.Contracts.ValidationAttributes;

    public class SalePropertyRequest
    {
        public string OwnerName { get; set; }

        [MarkedRequired]
        public Guid CompanyId { get; set; }

        public Guid CommunityId { get; set; }

        public Guid? PlanId { get; set; }
    }
}
