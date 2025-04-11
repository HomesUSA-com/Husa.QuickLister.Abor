namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("lot-listings/{entityId}/media")]
    public class LotListingMediaController : MediaController<ILotListingMediaService>
    {
        public LotListingMediaController(
            ILotListingMediaService listingMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<LotListingMediaController> logger)
            : base(listingMediaService, logger)
        {
        }

        [HttpPost("import")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> ImportMediaAsync([FromRoute] Guid entityId, [FromBody] IEnumerable<ImportMedia> media)
        {
            this.Logger.LogInformation("Importing resources for entity id {entityId}", entityId);

            await this.MediaService.Resource.ImportAsync(entityId, media);

            return this.Ok();
        }
    }
}
