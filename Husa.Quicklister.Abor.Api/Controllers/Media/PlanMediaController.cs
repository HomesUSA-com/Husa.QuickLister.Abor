namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("plan/{planId}/media")]
    [Route("plans/{planId}/media")]
    public class PlanMediaController : Controller
    {
        private readonly IPlanMediaService planMediaService;
        private readonly ILogger<PlanMediaController> logger;
        private readonly ApplicationOptions options;
        public PlanMediaController(IPlanMediaService planMediaService, IOptions<ApplicationOptions> options, ILogger<PlanMediaController> logger)
        {
            this.planMediaService = planMediaService ?? throw new ArgumentNullException(nameof(planMediaService));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetResources([FromRoute] Guid planId)
        {
            this.logger.LogInformation("Starting to get the media resources for the entity {planId}", planId);

            var resource = await this.planMediaService.GetAsync(planId);

            return this.Ok(resource);
        }

        [HttpGet("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetMediaById([FromRoute] Guid planId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to get the media for the entity {planId} and media Id '{mediaId}'", planId, mediaId);

            var resource = await this.planMediaService.Resource.GetById(planId, mediaId);

            return this.Ok(resource);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid planId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to add media to entity id {planId}", planId);

            await this.planMediaService.Resource.CreateAsync(planId, media, mediaLimitAllowed: this.options.MediaAllowed.PlanMaxAllowedMedia);

            return this.Ok();
        }

        [HttpPut]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ReplaceAsync([FromRoute] Guid planId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to replace media with id {mediaId} for entity id {planId}", media.Id, planId);

            await this.planMediaService.Resource.ReplaceAsync(planId, media);

            return this.Ok();
        }

        [HttpDelete("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid planId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to delete media with id {mediaId}", mediaId);

            await this.planMediaService.Resource.DeleteByIdAsync(planId, mediaId, mediaLimitAllowed: this.options.MediaAllowed.PlanMaxAllowedMedia);

            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteResources([FromRoute] Guid planId)
        {
            this.logger.LogInformation("Starting to delete media to the entity id {planId}", planId);

            await this.planMediaService.Resource.DeleteAsync(planId);

            return this.Ok();
        }

        [HttpPatch("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid planId, [FromRoute] Guid mediaId, [FromBody] SimpleMedia media)
        {
            this.logger.LogInformation("Starting to update media with id {mediaId}", mediaId);

            await this.planMediaService.Resource.UpdateAsync(planId, mediaId, media);

            return this.Ok();
        }

        [HttpPatch]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateResourcesAsync([FromRoute] Guid planId, [FromBody] IEnumerable<SimpleMedia> media)
        {
            this.logger.LogInformation("Starting to update {count} resources", media.Count());

            await this.planMediaService.Resource.BulkUpdateAsync(planId, media, mediaLimitAllowed: this.options.MediaAllowed.PlanMaxAllowedMedia);

            return this.Ok();
        }
    }
}
