namespace Husa.Quicklister.Abor.Api.Client.Tests.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class MockAuthenticationUser
    {
        public const string Username = "test-user";

        public MockAuthenticationUser(params Claim[] claims) => this.Claims = claims.ToList();

        public static Guid UserId => Guid.Parse("44308925-5398-4a23-a0b6-33c9020b6f93");

        public IEnumerable<Claim> Claims { get; private set; }
    }
}
