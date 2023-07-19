namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    [ApiController]
    [Route("sale-communities/{communityId}/photo-requests")]
    public class SaleCommunityPhotoRequestsController : Controller
    {
        private readonly ICommunityPhotoService photoService;
        private readonly ILogger<SaleCommunityPhotoRequestsController> logger;
        public SaleCommunityPhotoRequestsController(ICommunityPhotoService photoService, ILogger<SaleCommunityPhotoRequestsController> logger)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid communityId, [FromQuery] Request.PhotoRequestFilter filter)
        {
            this.logger.LogInformation($"Starting to GET photo request  for the entity {communityId}.");
            var photoRequests = await this.photoService.GetAsync(communityId, filter);
            return this.Ok(photoRequests);
        }

        [HttpGet("{photoRequestId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid communityId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation($"Starting to get the photorequest for community entity {communityId} and photoRequest Id '{photoRequestId}'");
            var photoRequest = await this.photoService.GetByIdAsync(communityId, photoRequestId);
            return this.Ok(photoRequest);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid communityId, [FromBody] Request.PhotoRequest photoRequest)
        {
            this.logger.LogInformation($"Starting to create photorequest to community with id {communityId}");
            await this.photoService.CreateAsync(communityId, photoRequest);
            return this.Ok();
        }

        [HttpDelete("{photoRequestId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid communityId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation($"Starting to Delete photorequest: {photoRequestId} from community with id: {communityId}.");
            await this.photoService.DeleteByIdAsync(communityId, photoRequestId);
            return this.Ok();
        }
    }
}
