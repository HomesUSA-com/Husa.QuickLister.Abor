namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("legacy-communities")]
    public class LegacyCommunitiesController : Controller
    {
        private readonly ICommunityMigrationService communityMigrationService;
        private readonly ILogger<LegacyCommunitiesController> logger;

        public LegacyCommunitiesController(ICommunityMigrationService communityMigrationService, ILogger<LegacyCommunitiesController> logger)
        {
            this.communityMigrationService = communityMigrationService ?? throw new ArgumentNullException(nameof(communityMigrationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigrateFromV1([FromQuery][Required] Guid companyId, [FromQuery] bool? createCommunity, [FromQuery] bool? updateCommunity, [FromQuery] int? legacyCommunityId = null, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate communitys from v1 related to company {companyId}.", companyId);
            await this.communityMigrationService.MigrateAsync(companyId, new()
            {
                LegacyEntityId = legacyCommunityId,
                CreateEntity = createCommunity ?? false,
                UpdateEntity = updateCommunity ?? false,
                FromDate = fromDate,
            });

            return this.Ok();
        }

        [HttpPut("employees")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigrateEmployees([FromQuery] Guid? companyId = null, [FromQuery] Guid? communityId = null, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate community photo requests from v1 related to company {companyId}", companyId);
            await this.communityMigrationService.MigrateEmployees(communityId, companyId, fromDate);
            return this.Ok();
        }

        [HttpPut("photo")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigratePhotoRequests([FromQuery][Required] Guid companyId, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate community photo requests from v1 related to company {companyId}", companyId);
            await this.communityMigrationService.MigratePhotoRequests(companyId, fromDate);
            return this.Ok();
        }
    }
}
