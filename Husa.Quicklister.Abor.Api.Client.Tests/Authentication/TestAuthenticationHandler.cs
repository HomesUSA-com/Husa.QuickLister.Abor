namespace Husa.Quicklister.Abor.Api.Client.Tests.Authentication
{
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly MockAuthenticationUser mockedAuthenticationUser;

        public TestAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            MockAuthenticationUser mockedAuthenticationUser)
            : base(options, logger, encoder, clock)
        {
            this.mockedAuthenticationUser = mockedAuthenticationUser;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!this.mockedAuthenticationUser.Claims.Any())
            {
                return Task.FromResult(AuthenticateResult.Fail("Mock auth user not configured."));
            }

            // create the identity and authenticate the request
            var identity = new ClaimsIdentity(this.mockedAuthenticationUser.Claims, AuthenticationServiceCollectionExtensions.Scheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationServiceCollectionExtensions.Scheme);

            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }
    }
}
