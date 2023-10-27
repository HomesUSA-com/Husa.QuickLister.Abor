namespace Husa.Quicklister.Abor.Api.Controllers.Migration
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("legacy-listings")]
    public class LegacyListingController : Controller
    {
        private readonly IMigrationQueryRepository migrationQueryRepository;
        private readonly ILogger<LegacyListingController> logger;
        public LegacyListingController(IMigrationQueryRepository migrationQueryRepository, ILogger<LegacyListingController> logger)
        {
            this.migrationQueryRepository = migrationQueryRepository ?? throw new ArgumentNullException(nameof(migrationQueryRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<ActionResult> GetListings([FromQuery] Guid? companyId, [FromQuery] DateTime? toModifiedOn)
        {
            this.logger.LogInformation("Get listings for locking in v1");
            var listings = await this.migrationQueryRepository.GetListingsToLock(companyId, toModifiedOn);
            return this.Ok(listings);
        }
    }
}
