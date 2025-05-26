namespace Husa.Quicklister.Abor.Api.Controllers.LotListing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.LotRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.LotRequest;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Extensions.Api.Controllers;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ListingRequestFilter = Husa.Quicklister.Extensions.Api.Contracts.Request.ListingRequest.ListingRequestFilter;

    [ApiController]
    [Route("lot-listing-requests")]
    public class LotListingRequestsController : ListingRequestsController<LotListingRequest, ILotListingRequestService>
    {
        private readonly ILotListingRequestQueriesRepository requestQueryRepository;
        private readonly ILotListingService listingService;
        private readonly ILotListingNotesService listingNotesService;
        private readonly IUserRepository userQueriesRepository;

        public LotListingRequestsController(
            ILotListingRequestQueriesRepository lotRequestQueryRepository,
            ILotListingService listingService,
            ILotListingNotesService listingNotesService,
            ILotListingRequestService requestService,
            IUserRepository userQueriesRepository,
            IMapper mapper,
            ILogger<LotListingRequestsController> logger)
            : base(requestService, mapper, logger)
        {
            this.requestQueryRepository = lotRequestQueryRepository ?? throw new ArgumentNullException(nameof(lotRequestQueryRepository));
            this.listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
            this.listingNotesService = listingNotesService ?? throw new ArgumentNullException(nameof(listingNotesService));
            this.userQueriesRepository = userQueriesRepository ?? throw new ArgumentNullException(nameof(userQueriesRepository));
        }

        [HttpGet]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetListRequestAsync([FromQuery] ListingRequestFilter requestFilter, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to get filtered ABOR lot listings request with company id {companyId}", requestFilter.CompanyId);
            var queryFilter = this.mapper.Map<ListingRequestQueryFilter>(requestFilter);
            var queryResponse = await this.requestQueryRepository.GetAsync(queryFilter, cancellationToken);
            await this.userQueriesRepository.FillUsersNameAsync(queryResponse.Data);
            var data = this.mapper.Map<IEnumerable<ListingRequestQueryResponse>>(queryResponse.Data);
            return this.Ok(new DocumentGridResponse<ListingRequestQueryResponse>(data, queryResponse.Total, queryResponse.ContinuationToken, queryFilter.ContinuationToken, queryFilter.CurrentToken));
        }

        [HttpGet("{id:guid}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetListRequestLotByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to get listing lot request with id: {id}", id);
            var queryResponse = await this.requestQueryRepository.GetRequestByIdAsync(id, cancellationToken);
            if (queryResponse is null)
            {
                this.logger.LogInformation("Listing lot request with {id} was not found.", id);
                return this.NotFound(id);
            }

            var response = this.mapper.Map<LotListingRequestDetailResponse>(queryResponse);
            return this.Ok(response);
        }

        [HttpGet("{id:guid}/summary")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly])]
        public async Task<IActionResult> GetRequestSummaryAsync(Guid id, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Start to handle summary of lot listing request with {listingRequestId}", id);
            var queryResponse = await this.requestQueryRepository.GetSummaryAsync(id, cancellationToken);

            if (queryResponse is null)
            {
                return this.NotFound(id);
            }

            var result = this.mapper.Map<IEnumerable<SummarySectionContract>>(queryResponse);

            return this.Ok(result);
        }

        [HttpPost("submit")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> SaveAndSubmitListingAsync(Guid listingId, LotListingRequestForUpdate listingLotForUpdate, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to update ABOR listing with id {lotListingId}", listingId);
            var listingLotDto = this.mapper.Map<LotListingDto>(listingLotForUpdate);
            await this.listingService.UpdateListing(listingId, listingLotDto);
            var result = await this.requestService.CreateRequestAsync(listingId, cancellationToken);

            return this.ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [RolesFilter([UserRole.MLSAdministrator])]
        public async Task<IActionResult> UpdateAsync(Guid id, LotListingRequestForUpdate listingLotForUpdate, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Starting to update ABOR listing request with id {listingRequestId}", id);
            var listingLotRequestDto = this.mapper.Map<LotListingRequestDto>(listingLotForUpdate);
            await this.requestService.UpdateListingRequestAsync(id, listingLotRequestDto, cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id:guid}/return")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> ReturnRequestAsync(Guid id, [FromBody] ListingRequestReturnContract returnRequest, CancellationToken cancellationToken = default)
        {
            await this.requestService.ChangeRequestStatus(id, ListingRequestState.Returned, returnRequest.ReturnReason, cancellationToken: cancellationToken);
            var request = await this.requestQueryRepository.GetRequestByIdAsync(id, cancellationToken);
            await this.listingNotesService.CreateNote(request.ListingId, title: "Returned", description: returnRequest.ReturnReason);
            return this.Ok();
        }

        [HttpPut("{id:guid}/complete")]
        [RolesFilter([UserRole.MLSAdministrator])]
        public async Task<IActionResult> CompleteRequestAsync(Guid id, string mlsNumber, ActionType actionType, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle complete of lot listing request with id {listingRequestId}", id);
            await this.ChangesRequestState(id, ListingRequestState.Processing, cancellationToken);
            var request = await this.requestQueryRepository.GetRequestByIdAsync(id, cancellationToken);
            await this.listingService.AssignMlsNumberAsync(request.ListingId, mlsNumber, request.MlsStatus, actionType);
            await this.ChangesRequestState(id, ListingRequestState.Completed, cancellationToken);
            return this.Ok();
        }
    }
}
