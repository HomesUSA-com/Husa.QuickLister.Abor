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
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.ValidationsRules;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ListingSaleRequestFilter = Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest.ListingSaleRequestFilter;

    [ApiController]
    [Route("sale-listing-requests")]
    public class SaleListingRequestsController : Controller
    {
        private readonly ISaleListingRequestQueriesRepository saleRequestQueryRepository;
        private readonly ISaleListingRequestService saleRequestService;
        private readonly ISaleListingService listingSaleService;
        private readonly ISaleListingNotesService listingNotesService;
        private readonly ILogger<SaleListingRequestsController> logger;
        private readonly IUserRepository userQueriesRepository;
        private readonly IMapper mapper;
        private readonly IValidateListingStatusChanges<ListingSaleRequestForUpdate> validateListingStatusChanges;

        public SaleListingRequestsController(
            ISaleListingRequestQueriesRepository saleRequestQueryRepository,
            ISaleListingService listingSaleService,
            ISaleListingNotesService listingNotesService,
            ISaleListingRequestService saleRequestService,
            IUserRepository userQueriesRepository,
            IMapper mapper,
            IValidateListingStatusChanges<ListingSaleRequestForUpdate> validateListingStatusChanges,
            ILogger<SaleListingRequestsController> logger)
        {
            this.saleRequestQueryRepository = saleRequestQueryRepository ?? throw new ArgumentNullException(nameof(saleRequestQueryRepository));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.listingNotesService = listingNotesService ?? throw new ArgumentNullException(nameof(listingNotesService));
            this.saleRequestService = saleRequestService ?? throw new ArgumentNullException(nameof(saleRequestService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.validateListingStatusChanges = validateListingStatusChanges ?? throw new ArgumentNullException(nameof(validateListingStatusChanges));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetListRequestAsync([FromQuery] ListingSaleRequestFilter requestFilter, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to get filtered ABOR sale listings request with company id {companyId}", requestFilter.CompanyId);
            var queryFilter = this.mapper.Map<ListingSaleRequestQueryFilter>(requestFilter);
            var queryResponse = await this.saleRequestQueryRepository.GetListingSaleRequestsAsync(queryFilter, cancellationToken);
            await this.userQueriesRepository.FillUsersNameAsync(queryResponse.Data);
            var data = this.mapper.Map<IEnumerable<ListingSaleRequestQueryResponse>>(queryResponse.Data);
            return this.Ok(new ListingRequestGridResponse<ListingSaleRequestQueryResponse>(data, queryResponse.Total, queryResponse.ContinuationToken, queryFilter.ContinuationToken, queryFilter.CurrentToken));
        }

        [HttpGet("{id:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
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
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Start to handle summary of sale listing request with {listingRequestId}", id);
            var queryResponse = await this.saleRequestQueryRepository.GetListingRequestSummaryAsync(id, cancellationToken);

            if (queryResponse is null)
            {
                return this.NotFound(id);
            }

            var result = this.mapper.Map<IEnumerable<SummarySectionContract>>(queryResponse);

            return this.Ok(result);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(Guid saleListingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle creation of request for sale listing with id {saleListingId}", saleListingId);
            var queryResponse = await this.saleRequestService.CreateRequestAsync(saleListingId, cancellationToken);

            return queryResponse.Code == ResponseCode.Error ?
                this.BadRequest(queryResponse) :
                this.Ok(queryResponse.Result);
        }

        [HttpPost("submit")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> SaveAndSubmitListingAsync(Guid saleListingId, ListingSaleRequestForUpdate listingSaleForUpdate, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to update ABOR listing with id {saleListingId}", saleListingId);
            var listingSaleDto = this.mapper.Map<ListingSaleRequestDto>(listingSaleForUpdate);
            await this.listingSaleService.SaveListingChanges(saleListingId, listingSaleDto);

            var requestValidations = this.validateListingStatusChanges.Validate(listingSaleForUpdate);
            if (!requestValidations.IsValid)
            {
                var errors = CommandResult<ValidationFailure>.Error("Request could not be created. Please check required fields.", requestValidations.Errors);
                return this.BadRequest(errors);
            }

            var result = await this.saleRequestService.CreateRequestAsync(saleListingId, cancellationToken);

            return result.Code == ResponseCode.Error ?
                this.BadRequest(result) :
                this.Ok(result.Result);
        }

        [HttpPut("{id:guid}")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> UpdateAsync(Guid id, ListingSaleRequestForUpdate listingSaleForUpdate, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Starting to update ABOR listing request with id {listingRequestId}", id);
            var listingSaleRequestDto = this.mapper.Map<ListingSaleRequestDto>(listingSaleForUpdate);
            await this.saleRequestService.UpdateListingRequestAsync(id, listingSaleRequestDto, cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id:guid}/return")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
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

        [HttpPut("{id:guid}/approve")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> ApproveRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle approve of listing sale request with id {listingRequestId}", id);
            var request = await this.saleRequestQueryRepository.GetListingSaleRequestByIdAndStatusAsync(id, ListingRequestState.Pending, cancellationToken);

            if (request is null)
            {
                return this.BadRequest(id);
            }

            await this.saleRequestService.ChangeRequestStatus(request, ListingRequestState.Approved, cancellationToken: cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id:guid}/process")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> ProcessRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle processing of listing sale request with id {listingRequestId}", id);
            var request = await this.saleRequestQueryRepository.GetListingSaleRequestByIdAndStatusAsync(id, ListingRequestState.Approved, cancellationToken);

            if (request is null)
            {
                return this.BadRequest(id);
            }

            await this.saleRequestService.ChangeRequestStatus(request, ListingRequestState.Processing, cancellationToken: cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id:guid}/complete")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> CompleteRequestAsync(Guid id, string mlsNumber, ActionType actionType, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle complete of sale listing request with id {listingRequestId}", id);
            var request = await this.saleRequestQueryRepository.GetListingSaleRequestByIdAndStatusAsync(id, ListingRequestState.Processing, cancellationToken);

            if (request is null)
            {
                return this.BadRequest(id);
            }

            await this.listingSaleService.AssignMlsNumberAsync(request.ListingSaleId, mlsNumber, request.MlsStatus, actionType);
            await this.saleRequestService.ChangeRequestStatus(request, ListingRequestState.Completed, cancellationToken: cancellationToken);
            return this.Ok();
        }

        [HttpDelete("{id:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation($"Start to handle delete of listing sale request with id: {id}");
            var openRequest = await this.saleRequestQueryRepository.GetListingRequestSaleAsyncOld(id, cancellationToken);

            if (openRequest is null)
            {
                this.logger.LogInformation("Listing sale request with {id} was not found.", id);
                return this.NotFound(id);
            }

            await this.saleRequestService.ChangeRequestStatus(openRequest, ListingRequestState.Deleted, cancellationToken: cancellationToken);
            return this.Ok();
        }
    }
}
