namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using System;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;

    public class CreateCommunityRequest
    {
        private string name;

        public Guid CompanyId { get; set; }

        public string OwnerName { get; set; }

        public Cities City { get; set; }

        public string Name
        {
            get { return this.name; }
            set { this.name = value.Trim(); }
        }

        public Counties? County { get; set; }
    }
}
