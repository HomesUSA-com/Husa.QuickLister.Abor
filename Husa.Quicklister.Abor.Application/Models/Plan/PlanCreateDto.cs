namespace Husa.Quicklister.Abor.Application.Models.Plan
{
    using System;

    public class PlanCreateDto
    {
        public Guid CompanyId { get; set; }

        public string OwnerName { get; set; }

        public string Name { get; set; }
    }
}
