namespace Husa.Quicklister.Abor.Api.Controllers.Community
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
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
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.JsonImport;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums.Xml;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ExtensionController = Husa.Quicklister.Extensions.Api.Controllers.Community;

    public class SaleCommunitiesController : ExtensionController.SaleCommunitiesController<ISaleCommunityService>
    {
        private readonly ICommunityQueriesRepository communityQueriesRepository;
        private readonly ISaleListingRequestService saleRequestService;

        public SaleCommunitiesController(
            ICommunityQueriesRepository communityQueriesRepository,
            ILogger<SaleCommunitiesController> logger,
            ISaleCommunityService communityService,
            ISaleListingRequestService saleRequestService,
            ICommunityXmlService communityXmlService,
            ICommunityJsonImportService communityJsonImportService,
            IMapper mapper)
            : base(communityService, communityXmlService, communityJsonImportService, logger, mapper)
        {
            this.saleRequestService = saleRequestService ?? throw new ArgumentNullException(nameof(saleRequestService));
            this.communityQueriesRepository = communityQueriesRepository ?? throw new ArgumentNullException(nameof(communityQueriesRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ProfileRequestFilter filter)
        {
            this.Logger.LogInformation("Starting to get community profiles in ABOR");

            var requestFilter = this.Mapper.Map<ProfileQueryFilter>(filter);
            var queryResponse = await this.communityQueriesRepository.GetAsync(requestFilter);
            var data = this.Mapper.Map<IEnumerable<CommunityDataQueryResponse>>(queryResponse.Data);
            return this.Ok(new DataSet<CommunityDataQueryResponse>(data, queryResponse.Total));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync(CreateCommunityRequest communityRequest)
        {
            this.Logger.LogInformation("Starting to add a community sale in ABOR with Name: {communityName} and company id {companyId}", communityRequest.Name, communityRequest.CompanyId);
            var communitySaleRequestDto = this.Mapper.Map<CommunitySaleCreateDto>(communityRequest);
            var response = await this.CommunityService.CreateAsync(communitySaleRequestDto);
            return this.Ok(response.Result);
        }

        [HttpGet("Name")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetCommunityByName([FromQuery] CommunityByNameFilter filters)
        {
            this.Logger.LogInformation("Retrieving the communities with the filters: {@filters}.", filters);

            var community = await this.communityQueriesRepository.GetCommunityByName(filters.CompanyId, filters.CommunityName);

            var result = this.Mapper.Map<CommunitySaleResponse>(community);

            if (result == null)
            {
                return this.Ok(new());
            }

            return this.Ok(result);
        }

        [HttpGet("{communityId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.SalesEmployeeReadonly, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetCommunityById([FromRoute] Guid communityId)
        {
            this.Logger.LogInformation("Received request to GET community detail with Id '{communityId}'.", communityId);

            var community = await this.communityQueriesRepository.GetCommunityById(communityId);
            SubdivisionResponse subdivision = null;
            if (community?.XmlStatus != XmlStatus.NotFromXml)
            {
                try
                {
                    subdivision = await this.CommunityXmlService.GetSubdivisonByCommunityId(communityId, MarketCode.Austin);
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode != HttpStatusCode.NotFound)
                    {
                        throw;
                    }
                }
            }

            var result = this.Mapper.Map<CommunitySaleResponse>(community);
            if (subdivision != null)
            {
                result.XmlSubdivisionId = subdivision.Id;
            }

            return this.Ok(result);
        }

        [HttpPut("{communityId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> UpdateCommunityAsync(Guid communityId, [FromBody] CommunitySaleRequest communitySaleRequest)
        {
            this.Logger.LogInformation("Updating the community sale in Abor with id {communityId}", communityId);

            var communitySale = this.Mapper.Map<CommunitySaleDto>(communitySaleRequest);
            await this.CommunityService.UpdateCommunity(communityId, communitySale);

            return this.Ok();
        }

        [HttpPut("{communityId}/submit")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> SaveAndSubmitCommunityAsync(Guid communityId, CommunitySaleRequest communitySaleRequest, CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Submitting community sale with id {communityId}", communityId);
            var communitySaleDto = this.Mapper.Map<CommunitySaleDto>(communitySaleRequest);
            await this.CommunityService.UpdateCommunity(communityId, communitySaleDto, isSubmitted: true);
            var response = await this.saleRequestService.CreateRequestsFromCommunityAsync(communityId, cancellationToken);

            if (response.Code == ResponseCode.Information)
            {
                this.Logger.LogInformation("Command result: {message} {community}", response.Message, communityId);
                return this.NotFound(response);
            }

            return this.Ok(response.Result);
        }

        [HttpGet("{communityId:guid}/sale-listings/{listingId:guid}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.SalesEmployeeReadonly, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetCommunityWithListingProjection([FromRoute] Guid communityId, [FromRoute] Guid listingId)
        {
            this.Logger.LogInformation("Starting the process to import information from listing Id '{listingId}' to community id '{communityId}'", listingId, communityId);

            var community = await this.communityQueriesRepository.GetByIdWithListingImportProjection(communityId, listingId);

            var result = this.Mapper.Map<CommunitySaleResponse>(community);

            return this.Ok(result);
        }
    }
}
