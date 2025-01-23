namespace Husa.Quicklister.Abor.Api.Controllers.Community
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Community;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("community-history")]
    public class CommunityHistoryController : Controller
    {
        private readonly ICommunityHistoryQueriesRepository queriesRepository;
        private readonly ILogger<CommunityHistoryController> logger;
        private readonly IMapper mapper;

        public CommunityHistoryController(
            ICommunityHistoryQueriesRepository queriesRepository,
            IMapper mapper,
            ILogger<CommunityHistoryController> logger)
        {
            this.queriesRepository = queriesRepository ?? throw new ArgumentNullException(nameof(queriesRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetAsync([FromQuery] CommunityHistoryFilter requestFilter, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to get filtered community history");
            var queryFilter = this.mapper.Map<CommunityHistoryQueryFilter>(requestFilter);
            var queryResponse = await this.queriesRepository.GetAsync(queryFilter, cancellationToken);
            var data = this.mapper.Map<IEnumerable<CommunityHistoryQueryResponse>>(queryResponse.Data);
            return this.Ok(new DocumentGridResponse<CommunityHistoryQueryResponse>(data, queryResponse.Total, queryResponse.ContinuationToken, queryFilter.ContinuationToken, queryFilter.CurrentToken));
        }

        [HttpGet("{id:guid}/summary")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetSummaryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle summary of community record with {id}", id);
            var queryResponse = await this.queriesRepository.GetSummaryAsync(id, cancellationToken);
            var result = this.mapper.Map<IEnumerable<SummarySectionContract>>(queryResponse);
            return this.Ok(result);
        }
    }
}
