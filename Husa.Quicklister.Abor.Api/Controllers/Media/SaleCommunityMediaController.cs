namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("sale-communities/{communityId}/media")]
    public class SaleCommunityMediaController : Controller
    {
        private readonly ICommunityMediaService communityMediaService;
        private readonly ILogger<SaleCommunityMediaController> logger;
        private readonly ApplicationOptions options;
        public SaleCommunityMediaController(ICommunityMediaService communityMediaService, IOptions<ApplicationOptions> options, ILogger<SaleCommunityMediaController> logger)
        {
            this.communityMediaService = communityMediaService ?? throw new ArgumentNullException(nameof(communityMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.SalesEmployeeReadonly, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetResources([FromRoute] Guid communityId)
        {
            this.logger.LogInformation("Starting to get community media resources for the entity {communityId}", communityId);

            var resource = await this.communityMediaService.GetAsync(communityId);

            return this.Ok(resource);
        }

        [HttpGet("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetMediaById([FromRoute] Guid communityId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to get the media for community entity {communityId} and media Id '{mediaId}'", communityId, mediaId);

            var resource = await this.communityMediaService.Resource.GetById(communityId, mediaId);

            return this.Ok(resource);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid communityId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to add media to community with id {communityId}", communityId);

            await this.communityMediaService.Resource.CreateAsync(communityId, media, mediaLimitAllowed: this.options.MediaAllowed.SaleCommunityMaxAllowedMedia);

            return this.Ok();
        }

        [HttpPut]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ReplaceAsync([FromRoute] Guid communityId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to replace media with id {mediaId} for entity id {communityId}", media.Id, communityId);

            await this.communityMediaService.Resource.ReplaceAsync(communityId, media);

            return this.Ok();
        }

        [HttpDelete("{mediaId}")]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid communityId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to delete media fro community with id {mediaId}", mediaId);

            await this.communityMediaService.Resource.DeleteByIdAsync(communityId, mediaId, mediaLimitAllowed: this.options.MediaAllowed.SaleCommunityMaxAllowedMedia);

            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteResources([FromRoute] Guid communityId)
        {
            this.logger.LogInformation("Starting to delete media from community with community id {communityId}", communityId);

            await this.communityMediaService.Resource.DeleteAsync(communityId);

            return this.Ok();
        }

        [HttpPatch("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid communityId, [FromRoute] Guid mediaId, [FromBody] SimpleMedia media)
        {
            this.logger.LogInformation("Starting to update media for community with id {mediaId}", mediaId);

            await this.communityMediaService.Resource.UpdateAsync(communityId, mediaId, media);

            return this.Ok();
        }

        [HttpPatch]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateResourcesAsync([FromRoute] Guid communityId, [FromBody] IEnumerable<SimpleMedia> media)
        {
            this.logger.LogInformation("Starting to update {count} resources", media.Count());

            await this.communityMediaService.Resource.BulkUpdateAsync(communityId, media, mediaLimitAllowed: this.options.MediaAllowed.SaleCommunityMaxAllowedMedia);

            return this.Ok();
        }
    }
}
