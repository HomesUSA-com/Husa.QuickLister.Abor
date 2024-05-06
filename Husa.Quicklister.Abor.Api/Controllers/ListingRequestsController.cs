namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Entities.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class ListingRequestsController<TListingRequest, TListingRequestService> : Controller
        where TListingRequest : ListingRequest
        where TListingRequestService : IListingRequestService<TListingRequest>
    {
        protected readonly TListingRequestService requestService;
        protected readonly ILogger logger;
        protected readonly IMapper mapper;

        protected ListingRequestsController(
            TListingRequestService lotRequestService,
            IMapper mapper,
            ILogger logger)
        {
            this.requestService = lotRequestService ?? throw new ArgumentNullException(nameof(lotRequestService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle creation of request for listing with id {lotListingId}", listingId);
            var queryResponse = await this.requestService.CreateRequestAsync(listingId, cancellationToken);

            return queryResponse.Code == ResponseCode.Error ?
                this.BadRequest(queryResponse) :
                this.Ok(queryResponse.Result);
        }

        [HttpPut("{id:guid}/approve")]
        [ApiAuthorization(new RoleEmployee[0])]
        public Task<IActionResult> ApproveRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.ChangesRequestState(id, ListingRequestState.Approved, cancellationToken);
        }

        [HttpPut("{id:guid}/process")]
        [ApiAuthorization(new RoleEmployee[0])]
        public Task<IActionResult> ProcessRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.ChangesRequestState(id, ListingRequestState.Processing, cancellationToken);
        }

        [HttpPut("{id:guid}/pending")]
        [ApiAuthorization(new RoleEmployee[0])]
        public Task<IActionResult> PendingRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.ChangesRequestState(id, ListingRequestState.Pending, cancellationToken);
        }

        [HttpDelete("{id:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public Task<IActionResult> DeleteRequestAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Deleting listing request {id}", id);
            return this.ChangesRequestState(id, ListingRequestState.Deleted, cancellationToken);
        }

        protected async Task<IActionResult> ChangesRequestState(Guid id, ListingRequestState toState, CancellationToken cancellationToken = default)
        {
            await this.requestService.ChangeRequestStatus(id, toState, cancellationToken: cancellationToken);
            return this.Ok();
        }
    }
}
