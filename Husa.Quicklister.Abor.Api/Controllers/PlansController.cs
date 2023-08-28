namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Api.Configuration;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Plan;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("plans")]
    public class PlansController : Controller
    {
        private readonly IPlanQueriesRepository planQueriesRepository;
        private readonly IPlanService planService;
        private readonly IPlanXmlService planXmlService;
        private readonly ILogger<PlansController> logger;
        private readonly IMapper mapper;

        public PlansController(
            IPlanQueriesRepository planQueriesRepository,
            IPlanService planService,
            IPlanXmlService planXmlService,
            ILogger<PlansController> logger,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.planQueriesRepository = planQueriesRepository ?? throw new ArgumentNullException(nameof(planQueriesRepository));
            this.planService = planService ?? throw new ArgumentNullException(nameof(planService));
            this.planXmlService = planXmlService ?? throw new ArgumentNullException(nameof(planXmlService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PlanRequestFilter filter)
        {
            this.logger.LogInformation("Starting to get plan profiles in ABOR filtered by {@filters}", filter);
            var requestFilter = this.mapper.Map<PlanQueryFilter>(filter);
            var queryResponse = await this.planQueriesRepository.GetAsync(requestFilter);
            var data = this.mapper.Map<IEnumerable<PlanDataQueryResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<PlanDataQueryResponse>(data, queryResponse.Total));
        }

        [HttpGet("{planId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetPlanById([FromRoute] Guid planId)
        {
            this.logger.LogInformation("Getting the plan detail with Id '{planId}'.", planId);

            var plan = await this.planQueriesRepository.GetPlanById(planId);

            var result = this.mapper.Map<PlanDetailResponse>(plan);

            return this.Ok(result);
        }

        [HttpGet("Name")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetPlanByName([FromQuery] PlanByNameFilter filters)
        {
            this.logger.LogInformation("Retrieving the plans with the filters: {@filters}.", filters);

            var plan = await this.planQueriesRepository.GetPlanByByName(filters.CompanyId, filters.PlanName);

            var result = this.mapper.Map<PlanDetailResponse>(plan);

            if (result == null)
            {
                return this.Ok(new { });
            }

            return this.Ok(result);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> CreateAsync(CreatePlanRequest createPlanRequest)
        {
            this.logger.LogInformation("Adding plan profile with Name: {planName} and company {companyId}", createPlanRequest.Name, createPlanRequest.CompanyId);
            var planRequestDto = this.mapper.Map<PlanCreateDto>(createPlanRequest);
            var planId = await this.planService.CreateAsync(planRequestDto);
            return this.Ok(planId);
        }

        [HttpDelete("{planId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> DeletePlanAsync([FromRoute] Guid planId, [FromQuery] bool deleteInCascade = false)
        {
            this.logger.LogInformation("Deleting the plan profile with id {planId}", planId);

            await this.planService.DeletePlan(planId, deleteInCascade);

            return this.Ok();
        }

        [HttpPatch("{planId}/approve")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<IActionResult> ApprovePlanAsync(Guid planId)
        {
            this.logger.LogInformation("Approving a plan with id {planId} imported by xml", planId);
            await this.planXmlService.ApprovePlan(planId);
            return this.Ok();
        }

        [HttpPut("{planId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public async Task<IActionResult> UpdatePlanAsync([FromRoute] Guid planId, [FromBody] UpdatePlanRequest updatePlanRequest)
        {
            this.logger.LogInformation("Updating plan with id: {planId}", planId);
            var planRequestDto = this.mapper.Map<UpdatePlanDto>(updatePlanRequest);
            await this.planService.UpdatePlanAsync(planId, planRequestDto);

            return this.Ok();
        }

        [HttpGet("{planId:guid}/sale-listings/{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetPlanWithListingProjection([FromRoute][Required] Guid planId, [FromRoute][Required] Guid listingId)
        {
            this.logger.LogInformation("Starting the process to import information from listing Id '{listingId}' to plan profile id '{planId}'", listingId, planId);
            var plan = await this.planQueriesRepository.GetByIdWithListingImportProjection(planId, listingId);
            var result = this.mapper.Map<PlanDetailResponse>(plan);
            return this.Ok(result);
        }
    }
}
