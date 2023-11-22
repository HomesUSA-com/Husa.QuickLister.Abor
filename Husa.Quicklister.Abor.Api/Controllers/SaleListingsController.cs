namespace Husa.Quicklister.Abor.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ReverseProspect;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listings")]
    public class SaleListingsController : Controller
    {
        private readonly IListingSaleQueriesRepository listingSaleQueriesRepository;
        private readonly ISaleListingRequestQueriesRepository saleRequestQueryRepository;
        private readonly IManagementTraceQueriesRepository managementTraceQueriesRepository;
        private readonly IUploaderService austinUploaderService;
        private readonly IMediaService mediaService;
        private readonly ISaleListingService listingSaleService;
        private readonly ILogger<SaleListingsController> logger;
        private readonly IMapper mapper;

        public SaleListingsController(
            IListingSaleQueriesRepository listingSaleQueriesRepository,
            ISaleListingRequestQueriesRepository saleRequestQueryRepository,
            IManagementTraceQueriesRepository managementTraceQueriesRepository,
            ISaleListingService listingSaleService,
            IUploaderService austinUploaderService,
            IMediaService mediaService,
            ILogger<SaleListingsController> logger,
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.austinUploaderService = austinUploaderService ?? throw new ArgumentNullException(nameof(austinUploaderService));
            this.listingSaleQueriesRepository = listingSaleQueriesRepository ?? throw new ArgumentNullException(nameof(listingSaleQueriesRepository));
            this.managementTraceQueriesRepository = managementTraceQueriesRepository ?? throw new ArgumentNullException(nameof(managementTraceQueriesRepository));
            this.listingSaleService = listingSaleService ?? throw new ArgumentNullException(nameof(listingSaleService));
            this.mediaService = mediaService ?? throw new ArgumentNullException(nameof(mediaService));
            this.saleRequestQueryRepository = saleRequestQueryRepository ?? throw new ArgumentNullException(nameof(saleRequestQueryRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ListingSaleRequestFilter filters)
        {
            this.logger.LogInformation("Retrieving the listing with status: '{filters.MlsStatus}'", filters.MlsStatus);

            var requestFilter = this.mapper.Map<ListingQueryFilter>(filters);
            var listingsSaleResult = await this.listingSaleQueriesRepository.GetAsync(requestFilter);

            var data = this.mapper.Map<IEnumerable<ListingSaleResponse>>(listingsSaleResult.Data);

            this.logger.LogInformation("Found '{listingCount}' sale listings", data.Count());

            return this.Ok(new DataSet<ListingSaleResponse>(data, listingsSaleResult.Total));
        }

        [HttpGet("Address")]
        public async Task<IActionResult> GetByAddressAsync([FromQuery] ListingSaleFilterByAddress filters)
        {
            this.logger.LogInformation("Retrieving the listing By Address: '{filters.StreetName} {filters.StreetNumber}, zipcode {filters.ZipCode}'", filters.StreetName, filters.StreetNumber, filters.ZipCode);
            var listing = await this.listingSaleQueriesRepository.GetListingByAddress(filters.StreetName, filters.StreetNumber, filters.ZipCode);

            if (listing == null)
            {
                return this.Ok(new ListingSaleDetailResponse());
            }

            var result = this.mapper.Map<ListingSaleDetailResponse>(listing);
            return this.Ok(result);
        }

        [HttpGet("open-house")]
        public async Task<IActionResult> GetListingsWithOpenHouseAsync([FromQuery] BaseFilterRequest filters)
        {
            this.logger.LogInformation("Retrieving listings with open house with the following filters: {filters}", filters);
            var requestFilter = this.mapper.Map<BaseQueryFilter>(filters);
            var listingsSaleResult = await this.listingSaleQueriesRepository.GetListingsWithOpenHouse(requestFilter);
            var data = this.mapper.Map<IEnumerable<ListingSaleOpenHouseResponse>>(listingsSaleResult.Data);
            this.logger.LogInformation("Found '{listingCount}' listings with Open House", data.Count());
            return this.Ok(new DataSet<ListingSaleOpenHouseResponse>(data, listingsSaleResult.Total));
        }

        [HttpGet("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetListing([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Received request to GET sale listing with Id '{listingId}'.", listingId);
            var listing = await this.listingSaleQueriesRepository.GetListing(listingId);
            var result = this.mapper.Map<ListingSaleDetailResponse>(listing);
            return this.Ok(result);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(ListingSaleRequest listingSale)
        {
            this.logger.LogInformation("Starting to add in Listing in ABOR with Address: {StreetNumber} {StreetName} ", listingSale.StreetNumber, listingSale.StreetName);
            var listingSaleRequest = this.mapper.Map<ListingSaleDto>(listingSale);
            var queryResponse = await this.listingSaleService.CreateAsync(listingSaleRequest);

            if (queryResponse.Code == ResponseCode.Error)
            {
                return this.BadRequest(queryResponse);
            }

            return this.Ok(queryResponse.Result);
        }

        [HttpPut("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateListing([FromRoute] Guid listingId, ListingSaleDetailRequest saleListingRequest)
        {
            this.logger.LogInformation("Received request to UPDATE sale listing with Id '{listingId}'.", listingId);

            var listingSale = this.mapper.Map<SaleListingDto>(saleListingRequest);

            await this.listingSaleService.UpdateListing(listingId, listingSale);

            return this.Ok();
        }

        [HttpDelete("{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteListing([FromRoute] Guid listingId)
        {
            this.logger.LogInformation("Received request to DELETE sale listing with Id '{listingId}'.", listingId);

            await this.listingSaleService.DeleteListing(listingId);

            return this.Ok();
        }

        [HttpPatch("{listingId:guid}/change-community")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> ChangeCommunity([FromRoute] Guid listingId, [FromQuery][Required] Guid communityId)
        {
            this.logger.LogInformation("Starting the process to change the community linked to the listing Id '{listingId}' for the Community Id '{communityId}'", listingId, communityId);

            await this.listingSaleService.ChangeCommunity(listingId, communityId);

            return this.Ok();
        }

        [HttpPatch("{listingId:guid}/change-plan")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> ChangePlan([FromRoute] Guid listingId, [FromQuery][Required] Guid planId, [FromQuery] bool updateRooms = false)
        {
            this.logger.LogInformation("Starting the process to change the plan profile linked to the listing Id '{listingId}' for the Plan Profile Id '{planId}'", listingId, planId);

            await this.listingSaleService.ChangePlan(listingId, planId, updateRooms);

            return this.Ok();
        }

        [HttpGet("{listingId:guid}/listing-requests")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> GetRequestByListingSaleAsync(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to handle query request by listing sale Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            var queryResponse = await this.saleRequestQueryRepository.GetRequestsByListingSaleIdAsync(listingId, cancellationToken);
            return this.Ok(this.mapper.Map<IEnumerable<ListingSaleRequestQueryResponse>>(queryResponse));
        }

        [HttpPut("{listingId:guid}/unlock")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UnlockListing(Guid listingId, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to unlock listing sale Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            var queryResponse = await this.listingSaleService.UnlockListing(listingId, cancellationToken);

            if (queryResponse.Code == ResponseCode.Error)
            {
                return this.BadRequest(queryResponse);
            }

            return this.Ok();
        }

        [HttpGet("{listingId:guid}/reverse-prospect")]
        public async Task<IActionResult> GetReverseProspectInfo([FromRoute][Required] Guid listingId, [FromQuery] bool usingDatabase = true, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Getting reverse prospect information for listing sale Id {listingId}", listingId);

            var result = await this.austinUploaderService.GetReverseProspectListing(listingId, usingDatabase, cancellationToken);

            if (result.Code == ResponseCode.Error)
            {
                return this.BadRequest(result);
            }

            var response = this.mapper.Map<ReverseProspectInformationResponse>(result.Result);

            return this.Ok(response);
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
            this.logger.LogInformation("Photos declined for listing sale Id {listingId}", listingId);

            if (listingId == Guid.Empty)
            {
                return this.BadRequest(listingId);
            }

            await this.listingSaleService.DeclinePhotos(listingId);

            return this.Ok();
        }

        [HttpGet("{listingId:guid}/management-traces")]
        public async Task<IActionResult> GetXmlManagementInfo([FromRoute][Required] Guid listingId, [FromQuery] BaseFilterRequest filter, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Getting xml management information for listing sale Id {listingId}'", listingId);
            var traces = await this.managementTraceQueriesRepository.GetAsync(listingId, filter.SortBy, filter.Skip, filter.Take);
            var response = this.mapper.Map<IEnumerable<XmlManagementResponse>>(traces.Data);
            var result = new DataSet<XmlManagementResponse>(data: response, traces.Total);
            return this.Ok(result);
        }

        [HttpPatch("{listingId:guid}/action-type")]
        [ApiAuthorization(new RoleEmployee[0])]
        public async Task<IActionResult> UpdateActionTypeAsync(Guid listingId, ActionTypeRequest listingRequestForUpdate, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Start to update action type from listing {listingId}", listingId);
            await this.listingSaleService.UpdateActionTypeAsync(listingId, listingRequestForUpdate.ActionType, cancellationToken);
            return this.Ok();
        }
    }
}
