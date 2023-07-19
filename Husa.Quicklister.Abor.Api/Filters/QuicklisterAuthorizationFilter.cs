namespace Husa.Quicklister.Abor.Api.Filters
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Extensions;
    using Husa.Extensions.Authorization.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class QuicklisterAuthorizationFilter : AuthorizationFilter
    {
        private readonly IServiceSubscriptionClient serviceSubscriptionClient;

        public QuicklisterAuthorizationFilter(IUserProvider userProvider, IServiceSubscriptionClient serviceSubscriptionClient, ILogger<QuicklisterAuthorizationFilter> logger)
            : base(userProvider, logger)
        {
            this.serviceSubscriptionClient = serviceSubscriptionClient ?? throw new ArgumentNullException(nameof(serviceSubscriptionClient));
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                this.logger.LogInformation("Skipping the filter and continue as Anonymous request");
                return;
            }

            var role = context.HttpContext.User.GetUserRole();
            if (role != UserRole.Photographer)
            {
                await base.OnAuthorizationAsync(context);
                return;
            }

            this.logger.LogError("User {useRole} can't access to Quicklister", role);
            context.Result = new ForbidActionResult();
        }

        public override async Task GetCompanyEmployeeAsync(AuthorizationFilterContext context, IUserContext user)
        {
            if (user.UserRole != UserRole.User)
            {
                return;
            }

            if (!user.CompanyId.HasValue)
            {
                this.logger.LogError("CompanyId isn't provided");
                context.Result = new UnauthorizedObjectResult("CompanyId isn't provided");
                return;
            }

            var userEmployee = await this.serviceSubscriptionClient.Employee.GetEmployeeByUserAndCompany(user.Id, user.CompanyId.Value);

            if (userEmployee is null)
            {
                this.logger.LogError("The user with id: '{userId}' doesn't exist.", user.Id);
                context.Result = new UnauthorizedObjectResult($"The user with id:'{user.Id}' doesn't exist.");
                return;
            }

            user.EmployeeRole = userEmployee.RoleName;
        }
    }
}
