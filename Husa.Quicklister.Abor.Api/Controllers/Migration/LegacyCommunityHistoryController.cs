namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Husa.Quicklister.Extensions.Application.Models.Migration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("legacy-community-history")]
    public class LegacyCommunityHistoryController : Controller
    {
        private readonly ICommunityHistoryMigrationService migrationService;
        private readonly ILogger<LegacyCommunityHistoryController> logger;
        public LegacyCommunityHistoryController(ICommunityHistoryMigrationService migrationService, ILogger<LegacyCommunityHistoryController> logger)
        {
            this.migrationService = migrationService ?? throw new ArgumentNullException(nameof(migrationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigrateFromV1([FromQuery][Required] Guid companyId, [FromQuery] int? communityId = null, [FromQuery] DateTime? fromDate = null, [FromQuery] bool updateRecord = false)
        {
            this.logger.LogInformation("Migrate community history from v1 related to company {companyId}.", companyId);
            var filters = new MigrateCommunityHistoryRequestFilter()
            {
                CommunityId = communityId,
                FromDate = fromDate,
                UpdateRecord = updateRecord,
            };

            await this.migrationService.MigrateAsync(companyId, filters);

            return this.Ok();
        }
    }
}
