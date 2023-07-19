namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("plan/{planId}/virtual-tour")]
    [Route("plans/{planId}/virtual-tour")]
    public class PlanMediaVirtualToursController : Controller
    {
        private readonly IPlanMediaService planMediaService;
        private readonly ILogger<PlanMediaVirtualToursController> logger;
        public PlanMediaVirtualToursController(IPlanMediaService planMediaService, ILogger<PlanMediaVirtualToursController> logger)
        {
            this.planMediaService = planMediaService ?? throw new ArgumentNullException(nameof(planMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid planId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {planId}", planId);

            await this.planMediaService.VirtualTour.CreateAsync(planId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid planId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.planMediaService.VirtualTour.DeleteById(planId, virtualTourId);

            return this.Ok();
        }
    }
}
