namespace Husa.Quicklister.Abor.Api.Controllers.LotListing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Api.Configuration;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Request.LotListing;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.LotListing;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("lot-listings")]
    public class LotListingsController : Controller
    {
        private readonly ILotListingQueriesRepository lotListingQueriesRepository;
        private readonly ILotListingRequestQueriesRepository requestQueryRepository;
        private readonly IMediaService mediaService;
        private readonly ILotListingService listingService;
        private readonly ILogger<LotListingsController> logger;
        private readonly IMapper mapper;

        public LotListingsController(
            ILotListingQueriesRepository lotListingQueriesRepository,
            ILotListingRequestQueriesRepository requestQueryRepository,
            ILotListingService listingService,
            IMediaService mediaService,
            ILogger<LotListingsController> logger,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.lotListingQueriesRepository = lotListingQueriesRepository ?? throw new ArgumentNullException(nameof(lotListingQueriesRepository));
            this.listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
            this.mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            this.requestQueryRepository = requestQueryRepository ?? throw new ArgumentNullException(nameof(requestQueryRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ListingRequestFilter filters)
        {
            this.logger.LogInformation("Retrieving the listing with status: '{mlsStatus}'", filters.MlsStatus);

            var requestFilter = this.mapper.Map<ListingQueryFilter>(filters);
            var listingsSaleResult = await this.lotListingQueriesRepository.GetAsync(requestFilter);

            var data = this.mapper.Map<IEnumerable<ListingResponse>>(listingsSaleResult.Data);

            this.logger.LogInformation("Found '{listingCount}' sale listings", data.Count());

            return this.Ok(new DataSet<ListingResponse>(data, listingsSaleResult.Total));
        }

        [HttpGet("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.SalesEmployeeReadonly)]
        public async Task<IActionResult> GetListing([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Received request to GET sale listing with Id '{listingId}'.", listingId);
            var listing = await this.lotListingQueriesRepository.GetListing(listingId);
            var result = this.mapper.Map<LotListingDetailResponse>(listing);
            return this.Ok(result);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(QuickCreateListingRequest lotListing)
        {
            this.logger.LogInformation("Starting to add in Listing in ABOR with Address: {StreetNumber} {StreetName} ", lotListing.StreetNumber, lotListing.StreetName);
            var lotListingRequest = this.mapper.Map<QuickCreateListingDto>(lotListing);
            var queryResponse = await this.listingService.CreateAsync(lotListingRequest);

            if (queryResponse.Code == ResponseCode.Error)
            {
                return this.BadRequest(queryResponse);
            }

            return this.Ok(queryResponse.Result);
        }

        [HttpPut("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateListing([FromRoute] Guid listingId, LotListingDetailRequest saleListingRequest)
        {
            this.logger.LogInformation("Received request to UPDATE sale listing with Id '{listingId}'.", listingId);

            var lotListing = this.mapper.Map<LotListingDto>(saleListingRequest);

            await this.listingService.UpdateListing(listingId, lotListing);

            return this.Ok();
        }

        [HttpDelete("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteListing([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Received request to DELETE sale listing with Id '{listingId}'.", listingId);

            await this.listingService.DeleteListing(listingId);

            return this.Ok();
        }

        [HttpPatch("{listingId:guid}/change-community")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> ChangeCommunity([FromRoute] Guid listingId, [FromQuery][Required] Guid communityId)
        {
            this.logger.LogInformation("Starting the process to change the community linked to the listing Id '{listingId}' for the Community Id '{communityId}'", listingId, communityId);

            await this.listingService.ChangeCommunity(listingId, communityId);

            return this.Ok();
        }

        [HttpPut("{listingId:guid}/unlock")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UnlockListing(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to unlock listing Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            var queryResponse = await this.listingService.UnlockListing(listingId, cancellationToken);

            if (queryResponse.Code == ResponseCode.Error)
            {
                return this.BadRequest(queryResponse);
            }

            return this.Ok();
        }

        [HttpPut("{listingId:guid}/close")]
        [Authorize(Roles.MLSAdministrator)]
        public async Task<IActionResult> CloseListing(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to close listing Id {listingId}", listingId);
            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            var queryResponse = await this.listingService.CloseListing(listingId);
            if (queryResponse.Code == ResponseCode.Error)
            {
                return this.BadRequest(queryResponse);
            }

            return this.Ok();
        }

        [HttpPut("{listingId:guid}/mls-media")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin)]
        public Task ImportMediaFromMartketAsync([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Starting to import resources from market for listing with id {listingId}", listingId);

            return this.mediaService.ImportMediaFromMlsAsync(listingId);
        }

        [HttpPatch("{listingId:guid}/decline-photos")]
        public async Task<IActionResult> DeclinePhotosAsync([FromRoute][Required] Guid listingId)
        {
            this.logger.LogInformation("Photos declined for listing Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            await this.listingService.DeclinePhotos(listingId);

            return this.Ok();
        }

        [HttpGet("{listingId:guid}/listing-requests")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetRequestByListingAsync(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle query request by listing Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            var queryResponse = await this.requestQueryRepository.GetAsync(new() { ListingId = listingId }, cancellationToken);
            return this.Ok(this.mapper.Map<IEnumerable<ListingRequestQueryResponse>>(queryResponse.Data));
        }
    }
}
