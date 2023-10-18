namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
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
        public async Task<ActionResult> MigrateFromV1([FromQuery][Required] Guid companyId)
        {
            this.logger.LogInformation("Migrate communitys from v1 related to company {companyId}.", companyId);
            await this.communityMigrationService.MigrateByCompanyId(companyId);
            return this.Ok();
        }
    }
}
