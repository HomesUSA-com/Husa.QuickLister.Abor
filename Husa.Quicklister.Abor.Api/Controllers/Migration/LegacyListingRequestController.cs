namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Husa.Quicklister.Extensions.Application.Models.Migration;
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
        public async Task<ActionResult> MigrateFromV1([FromQuery] Guid? companyId = null, [FromQuery] string mlsNumber = null, [FromQuery] DateTime? fromDate = null, [FromQuery] bool updateRequest = false, [FromQuery] MarketStatuses? mlsStatus = null)
        {
            this.logger.LogInformation("Migrate listingRequest from v1 related to company {companyId}.", companyId);
            var filters = new MigrateListingRequestFilter()
            {
                UpdateRequest = updateRequest,
                FromDate = fromDate,
                MlsStatus = mlsStatus?.ToStringFromEnumMember(),
            };

            if (!string.IsNullOrWhiteSpace(mlsNumber))
            {
                await this.requestMigrationService.MigrateByMlsNumber(mlsNumber, filters);
            }
            else if (companyId.HasValue)
            {
                await this.requestMigrationService.MigrateByCompanyId(companyId.Value, filters);
            }

            return this.Ok();
        }
    }
}
