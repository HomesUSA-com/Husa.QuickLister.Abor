namespace Husa.Quicklister.Abor.Api.Client.Tests.Authentication
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuthenticationServiceCollectionExtensions
    {
        public const string Scheme = "TestAuth";

        public static AuthenticationBuilder AddTestAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(Scheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services
                .AddAuthentication(Scheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(Scheme, options => { });
        }
    }
}
