namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    [ApiController]
    [Route("plans/{planId}/photo-requests")]
    public class PlanPhotoRequestsController : Controller
    {
        private readonly IPlanPhotoService photoService;
        private readonly ILogger<PlanPhotoRequestsController> logger;
        public PlanPhotoRequestsController(IPlanPhotoService photoService, ILogger<PlanPhotoRequestsController> logger)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid planId, [FromQuery] Request.PhotoRequestFilter filter)
        {
            this.logger.LogInformation("Starting to GET photo request  for the entity {planId}.", planId);
            var photoRequests = await this.photoService.GetAsync(planId, filter);
            return this.Ok(photoRequests);
        }

        [HttpGet("{photoRequestId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid planId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation("Starting to get the photorequest for plan entity {planId} and photoRequest Id '{photoRequestId}'", planId, photoRequestId);
            var photoRequest = await this.photoService.GetByIdAsync(planId, photoRequestId);
            return this.Ok(photoRequest);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid planId, [FromBody] Request.PhotoRequest photoRequest)
        {
            this.logger.LogInformation("Starting to create photorequest to plan with id {planId}", planId);
            await this.photoService.CreateAsync(planId, photoRequest);
            return this.Ok();
        }

        [HttpDelete("{photoRequestId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid planId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation("Starting to Delete photorequest: {photoRequestId} from plan with id: {planId}.", photoRequestId, planId);
            await this.photoService.DeleteByIdAsync(planId, photoRequestId);
            return this.Ok();
        }
    }
}
