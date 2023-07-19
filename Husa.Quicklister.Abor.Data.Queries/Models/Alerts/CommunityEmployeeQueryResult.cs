namespace Husa.Quicklister.Abor.Data.Queries.Models.Alerts
{
    using System;

    public class CommunityEmployeeQueryResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CommunityId { get; set; }
        public string CommunityName { get; set; }
    }
}
