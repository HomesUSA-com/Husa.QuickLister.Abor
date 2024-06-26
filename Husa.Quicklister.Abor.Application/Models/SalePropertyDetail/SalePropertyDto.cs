namespace Husa.Quicklister.Abor.Application.Models.SalePropertyDetail
{
    using System;

    public class SalePropertyDto
    {
        public string OwnerName { get; set; }

        public Guid CompanyId { get; set; }

        public Guid CommunityId { get; set; }

        public Guid? PlanId { get; set; }
    }
}
