namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("sale-listing-requests/{listingRequestId}/media")]
    public class SaleListingRequestMediaController : Controller
    {
        private readonly IListingRequestMediaService requestMediaService;
        private readonly ILogger<SaleListingRequestMediaController> logger;
        private readonly ApplicationOptions options;
        public SaleListingRequestMediaController(
            IListingRequestMediaService requestMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<SaleListingRequestMediaController> logger)
        {
            this.requestMediaService = requestMediaService ?? throw new ArgumentNullException(nameof(requestMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetResources([FromRoute] Guid listingRequestId)
        {
            this.logger.LogInformation("Starting to get the media resources for the entity {listingRequestId}", listingRequestId);

            var resource = await this.requestMediaService.GetAsync(listingRequestId);

            return this.Ok(resource);
        }

        [HttpGet("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetMediaById([FromRoute] Guid listingRequestId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to get the media for the entity {listingRequestId} and media Id '{mediaId}'", listingRequestId, mediaId);

            var resource = await this.requestMediaService.Resource.GetById(listingRequestId, mediaId);

            return this.Ok(resource);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid listingRequestId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to add media to entity id {listingRequestId}", listingRequestId);

            await this.requestMediaService.Resource.CreateAsync(listingRequestId, media, mediaLimitAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia);

            return this.Ok();
        }

        [HttpPut]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ReplaceAsync([FromRoute] Guid listingRequestId, [FromBody] Media media)
        {
            this.logger.LogInformation("Starting to replace media with id {mediaId} for entity id {listingRequestId}", media.Id, listingRequestId);

            await this.requestMediaService.Resource.ReplaceAsync(listingRequestId, media);

            return this.Ok();
        }

        [HttpDelete("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid listingRequestId, [FromRoute] Guid mediaId)
        {
            this.logger.LogInformation("Starting to delete media with id {mediaId}", mediaId);

            await this.requestMediaService.Resource.DeleteByIdAsync(listingRequestId, mediaId, mediaLimitAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia);

            return this.Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteResources([FromRoute] Guid listingRequestId)
        {
            this.logger.LogInformation("Starting to delete media to the entity id {listingRequestId}", listingRequestId);

            await this.requestMediaService.Resource.DeleteAsync(listingRequestId);

            return this.Ok();
        }

        [HttpPatch("{mediaId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid listingRequestId, [FromRoute] Guid mediaId, [FromBody] SimpleMedia media)
        {
            this.logger.LogInformation("Starting to update media with id {mediaId}", mediaId);

            await this.requestMediaService.Resource.UpdateAsync(listingRequestId, mediaId, media);

            return this.Ok();
        }

        [HttpPatch]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateResourcesAsync([FromRoute] Guid listingRequestId, [FromBody] IEnumerable<SimpleMedia> media)
        {
            this.logger.LogInformation("Starting to update {count} resources", media.Count());

            await this.requestMediaService.Resource.BulkUpdateAsync(listingRequestId, media, mediaLimitAllowed: this.options.MediaAllowed.SaleListingMaxAllowedMedia);

            return this.Ok();
        }
    }
}
