namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("legacy-listings")]
    public class LegacyListingController : Controller
    {
        private readonly IMigrationQueryRepository migrationQueryRepository;
        private readonly ISaleListingMigrationService listingMigrationService;
        private readonly ILogger<LegacyListingController> logger;
        public LegacyListingController(
            IMigrationQueryRepository migrationQueryRepository,
            ISaleListingMigrationService listingMigrationService,
            ILogger<LegacyListingController> logger)
        {
            this.migrationQueryRepository = migrationQueryRepository ?? throw new ArgumentNullException(nameof(migrationQueryRepository));
            this.listingMigrationService = listingMigrationService ?? throw new ArgumentNullException(nameof(listingMigrationService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> GetListings([FromQuery][Required] Guid companyId, [FromQuery] string mlsNumber = null, [FromQuery] bool createListing = false, [FromQuery] bool updateListing = false, [FromQuery] MarketStatuses? mlsStatus = null, [FromQuery] DateTime? fromDate = null)
        {
            this.logger.LogInformation("Migrate listings from v1 related to company {companyId}.", companyId);
            await this.listingMigrationService.MigrateListings(companyId, new()
            {
               MlsNumber = mlsNumber,
               MlsStatus = mlsStatus?.ToStringFromEnumMember(),
               CreateListing = createListing,
               UpdateListing = updateListing,
               FromDate = fromDate,
            });
            return this.Ok();
        }

        [HttpGet("lock")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> GetLockedListings([FromQuery] Guid? companyId, [FromQuery] DateTime? toModifiedOn)
        {
            this.logger.LogInformation("Get listings for locking in v1");
            var listings = await this.migrationQueryRepository.GetListingsToLock(companyId, toModifiedOn);
            return this.Ok(listings);
        }

        [HttpPut("photo")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> MigratePhotoRequests([FromQuery][Required] Guid companyId)
        {
            this.logger.LogInformation("Migrate residential photo requests from v1 related to company {companyId}", companyId);
            await this.listingMigrationService.MigratePhotoRequests(companyId);
            return this.Ok();
        }
    }
}
