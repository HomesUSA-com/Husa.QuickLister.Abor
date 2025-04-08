namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Plan;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Plan;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Application.Interfaces.JsonImport;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ExtensionController = Husa.Quicklister.Extensions.Api.Controllers.Plan;
    using ExtensionInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Plan;

    public class PlansController : ExtensionController.PlansController<IPlanService>
    {
        private readonly IPlanQueriesRepository planQueriesRepository;
        private readonly IMapper mapper;

        public PlansController(
            IPlanQueriesRepository planQueriesRepository,
            IPlanService planService,
            ExtensionInterfaces.IPlanXmlService planXmlService,
            IPlanJsonImportService planJsonImportService,
            ILogger<PlansController> logger,
            IMapper mapper)
            : base(planService, planXmlService, planJsonImportService, logger)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.planQueriesRepository = planQueriesRepository ?? throw new ArgumentNullException(nameof(planQueriesRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ProfileRequestFilter filter)
        {
            this.Logger.LogInformation("Starting to get plan profiles in ABOR filtered by {@filters}", filter);
            var requestFilter = this.mapper.Map<ProfileQueryFilter>(filter);
            var queryResponse = await this.planQueriesRepository.GetAsync(requestFilter);
            var data = this.mapper.Map<IEnumerable<PlanDataQueryResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<PlanDataQueryResponse>(data, queryResponse.Total));
        }

        [HttpGet("{planId}")]
        public async Task<IActionResult> GetPlanById([FromRoute] Guid planId)
        {
            this.Logger.LogInformation("Getting the plan detail with Id '{planId}'.", planId);

            var plan = await this.planQueriesRepository.GetPlanById(planId);

            var result = this.mapper.Map<PlanDetailResponse>(plan);

            return this.Ok(result);
        }

        [HttpGet("Name")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetPlanByName([FromQuery] PlanByNameFilter filters)
        {
            this.Logger.LogInformation("Retrieving the plans with the filters: {@filters}.", filters);

            var plan = await this.planQueriesRepository.GetPlanByByName(filters.CompanyId, filters.PlanName);

            var result = this.mapper.Map<PlanDetailResponse>(plan);

            if (result == null)
            {
                return this.Ok(new { });
            }

            return this.Ok(result);
        }

        [HttpPost]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin])]
        public async Task<IActionResult> CreateAsync(CreatePlanRequest createPlanRequest)
        {
            this.Logger.LogInformation("Adding plan profile with Name: {planName} and company {companyId}", createPlanRequest.Name, createPlanRequest.CompanyId);
            var planRequestDto = this.mapper.Map<PlanCreateDto>(createPlanRequest);
            var planId = await this.PlanService.CreateAsync(planRequestDto);
            return this.Ok(planId);
        }

        [HttpPut("{planId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> UpdatePlanAsync([FromRoute] Guid planId, [FromBody] UpdatePlanRequest updatePlanRequest)
        {
            this.Logger.LogInformation("Updating plan with id: {planId}", planId);
            var planRequestDto = this.mapper.Map<UpdatePlanDto>(updatePlanRequest);
            await this.PlanService.UpdatePlanAsync(planId, planRequestDto);

            return this.Ok();
        }

        [HttpGet("{planId:guid}/sale-listings/{listingId:guid}")]
        public async Task<IActionResult> GetPlanWithListingProjection([FromRoute][Required] Guid planId, [FromRoute][Required] Guid listingId)
        {
            this.Logger.LogInformation("Starting the process to import information from listing Id '{listingId}' to plan profile id '{planId}'", listingId, planId);
            var plan = await this.planQueriesRepository.GetByIdWithListingImportProjection(planId, listingId);
            var result = this.mapper.Map<PlanDetailResponse>(plan);
            return this.Ok(result);
        }
    }
}
