namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Reports;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IScrapedListingQueriesRepository scrapedListingQueriesRepository;
        private readonly ILogger<ReportsController> logger;
        private readonly IMapper mapper;

        public ReportsController(
            IScrapedListingQueriesRepository scrapedListingQueriesRepository,
            IMapper mapper,
            ILogger<ReportsController> logger)
        {
            this.scrapedListingQueriesRepository = scrapedListingQueriesRepository ?? throw new ArgumentNullException(nameof(scrapedListingQueriesRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("comparison-report")]
        public async Task<IActionResult> GetComparisonReportAsync([FromQuery] ScrapedListingRequestFilter filter)
        {
            this.logger.LogInformation("Getting the comparison report with the following optiosn {@filters} in ABOR", filter);
            var requestFilter = this.mapper.Map<ScrapedListingQueryFilter>(filter);
            var queryResponse = await this.scrapedListingQueriesRepository.GetAsync(requestFilter);
            return this.Ok(this.mapper.Map<IEnumerable<ScrapedListingQueryResponse>>(queryResponse));
        }
    }
}
