namespace Husa.Quicklister.Abor.Api.Contracts.Request.Community
{
    using System;
    using System.Collections.Generic;

    public class CommunityEmployeesRequest
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
