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
    [Route("legacy-plans")]
    public class LegacyPlansController : Controller
    {
        private readonly IPlanMigrationService planMigrationService;
        private readonly ILogger<LegacyPlansController> logger;
        public LegacyPlansController(IPlanMigrationService planMigrationService, ILogger<LegacyPlansController> logger)
        {
            this.planMigrationService = planMigrationService ?? throw new ArgumentNullException(nameof(planMigrationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigrateFromV1([FromQuery][Required] Guid companyId, [FromQuery] bool? createPlan, [FromQuery] bool? updatePlan, [FromQuery] int? legacyPlanId = null, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate plans from v1 related to company {companyId}.", companyId);
            await this.planMigrationService.MigrateAsync(companyId, new()
            {
                LegacyEntityId = legacyPlanId,
                CreateEntity = createPlan ?? false,
                UpdateEntity = updatePlan ?? false,
                FromDate = fromDate,
            });
            return this.Ok();
        }

        [HttpPut("photo")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigratePhotoRequests([FromQuery][Required] Guid companyId, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate plan photo requests from v1 related to company {companyId}", companyId);
            await this.planMigrationService.MigratePhotoRequests(companyId, fromDate);
            return this.Ok();
        }
    }
}
