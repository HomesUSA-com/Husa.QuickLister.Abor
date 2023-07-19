namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using System;

    public class CommunityByNameFilter
    {
        public Guid CompanyId { get; set; }

        public string CommunityName { get; set; }
    }
}
