namespace Husa.Quicklister.Abor.Api.Contracts.Request.Plan
{
    using System;

    public class CreatePlanRequest
    {
        private string name;
        public Guid CompanyId { get; set; }

        public string OwnerName { get; set; }

        public string Name
        {
            get { return this.name; }
            set { this.name = value.Trim(); }
        }
    }
}
