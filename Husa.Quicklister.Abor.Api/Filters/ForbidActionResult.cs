namespace Husa.Quicklister.Abor.Api.Filters
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class ForbidActionResult : ObjectResult
    {
        public ForbidActionResult(string errorMessage = null)
            : base(errorMessage ?? "User is not allowed to enter this page.")
        {
            this.StatusCode = (int)HttpStatusCode.Forbidden;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await base.ExecuteResultAsync(context);
        }
    }
}
