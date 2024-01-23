namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("legacy-listing-requests")]
    public class LegacyListingRequestController : Controller
    {
        private readonly IListingRequestMigrationService requestMigrationService;
        private readonly ILogger<LegacyListingRequestController> logger;
        public LegacyListingRequestController(IListingRequestMigrationService requestMigrationService, ILogger<LegacyListingRequestController> logger)
        {
            this.requestMigrationService = requestMigrationService ?? throw new ArgumentNullException(nameof(requestMigrationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigrateFromV1([FromQuery] Guid? companyId = null, [FromQuery] string mlsNumber = null, [FromQuery] DateTime? fromDate = null, [FromQuery] bool updateRequest = false)
        {
            this.logger.LogInformation("Migrate listingRequest from v1 related to company {companyId}.", companyId);
            if (!string.IsNullOrWhiteSpace(mlsNumber))
            {
                await this.requestMigrationService.MigrateByMlsNumber(mlsNumber, updateRequest: updateRequest);
            }
            else if (companyId.HasValue)
            {
                await this.requestMigrationService.MigrateByCompanyId(companyId.Value, fromDate: fromDate, updateRequest: updateRequest);
            }

            return this.Ok();
        }
    }
}
