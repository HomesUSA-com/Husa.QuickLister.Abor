namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("lot-listings/{entityId}/media")]
    public class LotListingMediaController : MediaController<ILotListingMediaService>
    {
        private readonly ApplicationOptions options;
        public LotListingMediaController(
            ILotListingMediaService listingMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<LotListingMediaController> logger)
            : base(listingMediaService, logger)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override int MediaLimitAllowed => this.options.MediaAllowed.LotListingMaxAllowedMedia;

        [HttpPost("import")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> ImportMediaAsync([FromRoute] Guid listingId, [FromBody] IEnumerable<ImportMedia> media)
        {
            this.Logger.LogInformation("Importing resources for entity id {listingId}", listingId);

            await this.MediaService.Resource.ImportAsync(listingId, media, mediaLimitAllowed: this.MediaLimitAllowed);

            return this.Ok();
        }
    }
}
