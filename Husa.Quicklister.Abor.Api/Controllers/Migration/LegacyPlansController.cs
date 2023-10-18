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
        public async Task<ActionResult> MigrateFromV1([FromQuery][Required] Guid companyId)
        {
            this.logger.LogInformation("Migrate plans from v1 related to company {companyId}.", companyId);
            await this.planMigrationService.MigrateByCompanyId(companyId);
            return this.Ok();
        }
    }
}
