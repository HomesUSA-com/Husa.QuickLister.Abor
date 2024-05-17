namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Crosscutting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("lot-listings/{listingId}/media")]
    public class LotListingMediaController : Controller
    {
        private readonly ILotListingMediaService listingMediaService;
        private readonly ILogger<LotListingMediaController> logger;
        private readonly ApplicationOptions options;
        public LotListingMediaController(
            ILotListingMediaService listingMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<LotListingMediaController> logger)
        {
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.SalesEmployeeReadonly, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetResources([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting to get the media resources for the entity {listingId}", listingId);

            var resource = await this.listingMediaService.GetAsync(listingId);

            return this.Ok(resource);
        }

        [HttpGet("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetMediaById([FromRoute] Guid listingId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to get the media for the entity {listingId} and media Id '{mediaId}'", listingId, mediaId);

            var resource = await this.listingMediaService.Resource.GetById(listingId, mediaId);

            return this.Ok(resource);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid listingId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to add media to entity id {listingId}", listingId);

            await this.listingMediaService.Resource.CreateAsync(listingId, media, mediaLimitAllowed: this.options.MediaAllowed.LotListingMaxAllowedMedia);

            return this.Ok();
        }

        [HttpPut]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ReplaceAsync([FromRoute] Guid listingId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to replace media with id {mediaId} for entity id {listingId}", media.Id, listingId);

            await this.listingMediaService.Resource.ReplaceAsync(listingId, media);

            return this.Ok();
        }

        [HttpDelete("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid listingId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to delete media with id {mediaId}", mediaId);

            await this.listingMediaService.Resource.DeleteByIdAsync(listingId, mediaId, mediaLimitAllowed: this.options.MediaAllowed.LotListingMaxAllowedMedia);

            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteResources([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting to delete media to the entity id {listingId}", listingId);

            await this.listingMediaService.Resource.DeleteAsync(listingId);

            return this.Ok();
        }

        [HttpPatch("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid listingId, [FromRoute] Guid mediaId, [FromBody] SimpleMedia media)
        {
            this.logger.LogInformation("Starting to update media with id {mediaId}", mediaId);

            await this.listingMediaService.Resource.UpdateAsync(listingId, mediaId, media);

            return this.Ok();
        }

        [HttpPatch]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateResourcesAsync([FromRoute] Guid listingId, [FromBody] IEnumerable<SimpleMedia> media)
        {
            this.logger.LogInformation("Starting to update {count} resources", media.Count());

            await this.listingMediaService.Resource.BulkUpdateAsync(listingId, media, mediaLimitAllowed: this.options.MediaAllowed.LotListingMaxAllowedMedia);

            return this.Ok();
        }

        [HttpPost("import")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ImportMediaAsync([FromRoute] Guid listingId, [FromBody] IEnumerable<ImportMedia> media)
        {
            this.logger.LogInformation("Importing resources for entity id {listingId}", listingId);

            await this.listingMediaService.Resource.ImportAsync(listingId, media, mediaLimitAllowed: this.options.MediaAllowed.LotListingMaxAllowedMedia);

            return this.Ok();
        }
    }
}
