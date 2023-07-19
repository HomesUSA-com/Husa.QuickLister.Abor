namespace Husa.Quicklister.Abor.Data.Queries.Models.Community
{
    using System;

    public class CommunityEmployeeQueryResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommunityId { get; set; }
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
    }
}
