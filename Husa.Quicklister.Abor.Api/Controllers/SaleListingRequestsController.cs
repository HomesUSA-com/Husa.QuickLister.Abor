namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation.Results;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.ValidationsRules;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Entities.ShowingTime;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Extensions.Api.Controllers;
    using Husa.Quicklister.Extensions.Api.Filters;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    [ApiController]
    [Route("sale-listing-requests")]
    public class SaleListingRequestsController : ListingRequestsController<SaleListingRequest, ISaleListingRequestService, ShowingTimeContact>
    {
        private readonly ISaleListingRequestQueriesRepository saleRequestQueryRepository;
        private readonly ISaleListingRequestService saleRequestService;
        private readonly ISaleListingService listingSaleService;
        private readonly ISaleListingNotesService listingNotesService;
        private readonly IUserRepository userQueriesRepository;
        private readonly IValidateListingStatusChanges<ListingSaleRequestForUpdate> validateListingStatusChanges;

        public SaleListingRequestsController(
            ISaleListingRequestQueriesRepository saleRequestQueryRepository,
            ISaleListingService listingSaleService,
            ISaleListingNotesService listingNotesService,
            ISaleListingRequestService saleRequestService,
            IUserRepository userQueriesRepository,
            IMapper mapper,
            IValidateListingStatusChanges<ListingSaleRequestForUpdate> validateListingStatusChanges,
            IShowingTimeContactQueriesRepository showingTimeContactQueriesRepository,
            ILogger<SaleListingRequestsController> logger)
            : base(showingTimeContactQueriesRepository, saleRequestService, mapper, logger)
        {
            this.saleRequestQueryRepository = saleRequestQueryRepository ?? throw new ArgumentNullException(nameof(saleRequestQueryRepository));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.listingNotesService = listingNotesService ?? throw new ArgumentNullException(nameof(listingNotesService));
            this.saleRequestService = saleRequestService ?? throw new ArgumentNullException(nameof(saleRequestService));
            this.validateListingStatusChanges = validateListingStatusChanges ?? throw new ArgumentNullException(nameof(validateListingStatusChanges));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        [HttpGet]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetListRequestAsync([FromQuery] SaleListingRequestFilter requestFilter, CancellationToken cancellationToken = default)
        {
            StringValues continuationToken = string.Empty;
            this.logger.LogInformation("Starting to get filtered ABOR sale listings request with company id {companyId}", requestFilter.CompanyId);
            var queryFilter = this.mapper.Map<SaleListingRequestQueryFilter>(requestFilter);
            this.Request?.Headers?.TryGetValue("Continuation-Token", out continuationToken);
            if (!string.IsNullOrEmpty(continuationToken))
            {
                queryFilter.ContinuationToken = continuationToken;
            }

            var queryResponse = await this.saleRequestQueryRepository.GetAsync(queryFilter, cancellationToken);
            await this.userQueriesRepository.FillUsersNameAsync(queryResponse.Data);
            var data = this.mapper.Map<IEnumerable<ListingSaleRequestQueryResponse>>(queryResponse.Data);
            return this.Ok(new DocumentGridResponse<ListingSaleRequestQueryResponse>(data, queryResponse.Total, queryResponse.ContinuationToken, queryFilter.ContinuationToken, queryFilter.CurrentToken));
        }

        [HttpGet("{id:guid}")]
        [ContinuationTokenFilter]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetListRequestSaleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to get listing sale request with id: {id}", id);
            var queryResponse = await this.saleRequestQueryRepository.GetListingRequestSaleAsync(id, cancellationToken);
            if (queryResponse is null)
            {
                this.logger.LogInformation("Listing sale request with {id} was not found.", id);
                return this.NotFound(id);
            }

            var response = this.mapper.Map<ListingSaleRequestDetailResponse>(queryResponse);
            return this.Ok(response);
        }

        [HttpGet("{id:guid}/summary")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Start to handle summary of sale listing request with {listingRequestId}", id);
            var queryResponse = await this.saleRequestQueryRepository.GetSummaryAsync(id, cancellationToken);

            if (queryResponse is null)
            {
                return this.NotFound(id);
            }

            var result = this.mapper.Map<IEnumerable<SummarySectionContract>>(queryResponse);

            return this.Ok(result);
        }

        [HttpPost("submit")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> SaveAndSubmitListingAsync(Guid listingId, ListingSaleRequestForUpdate listingSaleForUpdate, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to update ABOR listing with id {saleListingId}", listingId);
            var listingSaleDto = this.mapper.Map<ListingSaleRequestDto>(listingSaleForUpdate);
            await this.listingSaleService.SaveListingChanges(listingId, listingSaleDto);

            var requestValidations = this.validateListingStatusChanges.Validate(listingSaleForUpdate);
            if (!requestValidations.IsValid)
            {
                var errors = CommandResult<ValidationFailure>.Error("Request could not be created. Please check required fields.", requestValidations.Errors);
                return this.ToActionResult(errors);
            }

            var result = await this.saleRequestService.CreateRequestAsync(listingId, cancellationToken);

            if (!result.HasErrors())
            {
                await this.listingSaleService.CopyListingInfoToCommunity(listingId);
            }

            return this.ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [RolesFilter([UserRole.MLSAdministrator])]
        public async Task<IActionResult> UpdateAsync(Guid id, ListingSaleRequestForUpdate listingSaleForUpdate, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Starting to update ABOR listing request with id {listingRequestId}", id);
            var listingSaleRequestDto = this.mapper.Map<ListingSaleRequestDto>(listingSaleForUpdate);
            await this.saleRequestService.UpdateListingRequestAsync(id, listingSaleRequestDto, cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id:guid}/return")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> ReturnRequestAsync(Guid id, [FromBody] ListingRequestReturnContract returnRequest, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Rejecting sale listing request {listingRequestId}", id);
            var request = await this.saleRequestQueryRepository.GetListingSaleRequestByIdAndStatusAsync(id, ListingRequestState.Pending, cancellationToken);
            if (request is null)
            {
                return this.BadRequest(id);
            }

            await this.saleRequestService.ChangeRequestStatus(request, ListingRequestState.Returned, returnRequest.ReturnReason, cancellationToken: cancellationToken);
            await this.listingNotesService.CreateNote(request.ListingSaleId, title: "Returned", description: returnRequest.ReturnReason);
            return this.Ok();
        }

        [HttpPut("{id:guid}/complete")]
        [RolesFilter([UserRole.MLSAdministrator])]
        public async Task<IActionResult> CompleteRequestAsync(Guid id, string mlsNumber, ActionType actionType, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle complete of sale listing request with id {listingRequestId}", id);
            var request = await this.saleRequestQueryRepository.GetListingSaleRequestByIdAndStatusAsync(id, ListingRequestState.Processing, cancellationToken);

            if (request is null)
            {
                return this.BadRequest(id);
            }

            await this.listingSaleService.CopyListingInfoToListingPlan(request.ListingSaleId);

            await this.listingSaleService.AssignMlsNumberAsync(request.ListingSaleId, mlsNumber, request.MlsStatus, actionType);
            await this.saleRequestService.ChangeRequestStatus(request, ListingRequestState.Completed, cancellationToken: cancellationToken);
            return this.Ok();
        }
    }
}
