namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Api.Configuration;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-communities")]
    public class SaleCommunitiesController : Controller
    {
        private readonly ICommunityQueriesRepository communityQueriesRepository;
        private readonly ILogger<SaleCommunitiesController> logger;
        private readonly ISaleCommunityService communitySaleService;
        private readonly ICommunityXmlService communityXmlService;
        private readonly ISaleListingRequestService saleRequestService;
        private readonly IMapper mapper;

        public SaleCommunitiesController(
            ICommunityQueriesRepository communityQueriesRepository,
            ILogger<SaleCommunitiesController> logger,
            ISaleCommunityService communitySaleService,
            ISaleListingRequestService saleRequestService,
            ICommunityXmlService communityXmlService,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.communitySaleService = communitySaleService ?? throw new ArgumentNullException(nameof(communitySaleService));
            this.saleRequestService = saleRequestService ?? throw new ArgumentNullException(nameof(saleRequestService));
            this.communityXmlService = communityXmlService ?? throw new ArgumentNullException(nameof(communityXmlService));
            this.communityQueriesRepository = communityQueriesRepository ?? throw new ArgumentNullException(nameof(communityQueriesRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] CommunityRequestFilter filter)
        {
            this.logger.LogInformation($"Starting to get community profiles in ABOR");

            var requestFilter = this.mapper.Map<CommunityQueryFilter>(filter);
            var queryResponse = await this.communityQueriesRepository.GetAsync(requestFilter);
            var data = this.mapper.Map<IEnumerable<CommunityDataQueryResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<CommunityDataQueryResponse>(data, queryResponse.Total));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(CreateCommunityRequest communityRequest)
        {
            this.logger.LogInformation("Starting to add a community sale in ABOR with Name: {communityName} and company id {companyId}", communityRequest.Name, communityRequest.CompanyId);
            var communitySaleRequestDto = this.mapper.Map<CommunitySaleCreateDto>(communityRequest);
            var response = await this.communitySaleService.CreateAsync(communitySaleRequestDto);
            return this.Ok(response.Result);
        }

        [HttpGet("Name")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetCommunityByName([FromQuery] CommunityByNameFilter filters)
        {
            this.logger.LogInformation($"Retrieving the Community By Name: '{filters.CommunityName} from company id: {filters.CompanyId}'.");

            var community = await this.communityQueriesRepository.GetCommunityByName(filters.CompanyId, filters.CommunityName);

            var result = this.mapper.Map<CommunitySaleResponse>(community);

            if (result == null)
            {
                return this.Ok(new { });
            }

            return this.Ok(result);
        }

        [HttpGet("{communityId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetCommunityById([FromRoute] Guid communityId)
        {
            this.logger.LogInformation($"Received request to GET community detail with Id '{communityId}'.");

            var community = await this.communityQueriesRepository.GetCommunityById(communityId);

            var result = this.mapper.Map<CommunitySaleResponse>(community);

            return this.Ok(result);
        }

        [HttpPut("{communityId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateCommunityAsync(Guid communityId, [FromBody] CommunitySaleRequest communitySaleRequest)
        {
            this.logger.LogInformation("Starting to update the community sale in Abor with id {communityId}", communityId);

            var communitySale = this.mapper.Map<CommunitySaleDto>(communitySaleRequest);
            await this.communitySaleService.UpdateCommunity(communityId, communitySale);

            return this.Ok();
        }

        [HttpDelete("{communityId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> DeleteCommunityAsync(Guid communityId, [FromQuery] bool deleteInCascade = false)
        {
            this.logger.LogInformation($"Starting to delete the community sale in Abor with id {communityId}");

            await this.communitySaleService.DeleteCommunity(communityId, deleteInCascade);

            return this.Ok();
        }

        [HttpPut("{communityId}/submit")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> SaveAndSubmitCommunityAsync(Guid communityId, CommunitySaleRequest communitySaleRequest, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to submit community sale with id {communityId}", communityId);
            var communitySaleDto = this.mapper.Map<CommunitySaleDto>(communitySaleRequest);
            await this.communitySaleService.UpdateCommunity(communityId, communitySaleDto);
            var response = await this.saleRequestService.CreateRequestsFromCommunityAsync(communityId, cancellationToken);

            if (response.Code == ResponseCode.Information)
            {
                this.logger.LogInformation("Command result: {message} {community}", response.Message, communityId);
                return this.NotFound(response);
            }

            return this.Ok(response.Result);
        }

        [HttpPatch("{communityId}/approve")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<IActionResult> ApproveCommunityAsync(Guid communityId)
        {
            this.logger.LogInformation("Starting to approve a community sale with id {communityId} imported by xml", communityId);
            await this.communityXmlService.ApproveCommunity(communityId);
            return this.Ok();
        }

        [HttpGet("{communityId}/employees")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetEmployeesAsync([FromRoute] Guid communityId)
        {
            this.logger.LogInformation($"Start to get the employees for the community id {communityId}");
            var queryResponse = await this.communityQueriesRepository.GetCommunityEmployees(communityId);

            var data = this.mapper.Map<IEnumerable<CommunityEmployeeDataQueryResponse>>(queryResponse.Data);

            return this.Ok(new DataSet<CommunityEmployeeDataQueryResponse>(data, queryResponse.Total));
        }

        [HttpPost("{communityId}/employees")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> AddEmployeesAsync([FromRoute] Guid communityId, CommunityEmployeesRequest employees)
        {
            this.logger.LogInformation($"Start to create employees to the community id {communityId}");

            if (!employees.UserIds.Any())
            {
                this.logger.LogError($"The user list to added as employees of community {communityId} cannot be empty");
                return this.BadRequest();
            }

            var communityEmployeesRequestDto = this.mapper.Map<CommunityEmployeesCreateDto>(employees);

            communityEmployeesRequestDto.CommunityId = communityId;

            await this.communitySaleService.CreateEmployeesAsync(communityEmployeesRequestDto);

            return this.Ok();
        }

        [HttpDelete("{communityId}/employees")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteEmployeesAsync([FromRoute] Guid communityId, CommunityEmployeesDeleteRequest employees)
        {
            this.logger.LogInformation($"Start to delete employees to the community id {communityId}");

            if (!employees.UserIds.Any())
            {
                this.logger.LogError($"The user list to delete of community {communityId} cannot be empty");
                return this.BadRequest();
            }

            var communityEmployeesRequestDto = this.mapper.Map<CommunityEmployeesDeleteDto>(employees);

            communityEmployeesRequestDto.CommunityId = communityId;

            await this.communitySaleService.DeleteEmployeesAsync(communityEmployeesRequestDto);

            return this.Ok();
        }

        [HttpGet("{communityId:guid}/sale-listings/{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetCommunityWithListingProjection([FromRoute] Guid communityId, [FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting the process to import information from listing Id '{listingId}' to community id '{communityId}'", listingId, communityId);

            var community = await this.communityQueriesRepository.GetByIdWithListingImportProjection(communityId, listingId);

            var result = this.mapper.Map<CommunitySaleResponse>(community);

            return this.Ok(result);
        }
    }
}
