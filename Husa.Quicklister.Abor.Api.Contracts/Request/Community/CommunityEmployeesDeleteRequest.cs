namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using System;
    using System.Collections.Generic;

    public class CommunityEmployeesDeleteRequest
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
