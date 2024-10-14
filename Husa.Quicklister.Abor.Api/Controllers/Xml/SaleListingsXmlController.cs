namespace Husa.Quicklister.Abor.Api.Controllers.Xml
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Xml;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Xml;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.Xml;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("xml-listings")]
    public class SaleListingsXmlController : Controller
    {
        private readonly ISaleListingXmlService listingXmlService;
        private readonly IQueryXmlRepository queryXmlRepository;
        private readonly ILogger<SaleListingsXmlController> logger;
        private readonly IMapper mapper;

        public SaleListingsXmlController(
            ISaleListingXmlService listingXmlService,
            IQueryXmlRepository queryXmlRepository,
            IMapper mapper,
            ILogger<SaleListingsXmlController> logger)
        {
            this.listingXmlService = listingXmlService ?? throw new ArgumentNullException(nameof(listingXmlService));
            this.queryXmlRepository = queryXmlRepository ?? throw new ArgumentNullException(nameof(queryXmlRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly, RoleEmployee.SalesEmployeeReadonly, RoleEmployee.CompanyAdminReadonly)]
        public async Task<IActionResult> GetListings([FromQuery] XmlListingFilterRequest filter)
        {
            this.logger.LogInformation("Starting to get xml listings {@filter}", filter);

            var queryFilter = this.mapper.Map<XmlListingQueryFilter>(filter);
            var result = await this.queryXmlRepository.GetAsync(queryFilter);
            var xmlListings = this.mapper.Map<IEnumerable<XmlListingResponse>>(result.Data);

            return this.Ok(new DataSet<XmlListingResponse>(xmlListings, result.Total));
        }

        [HttpPost("{xmlListingId:guid}")]
        public async Task<IActionResult> ProcessListing([FromRoute] Guid xmlListingId, ProcessListingRequest request)
        {
            this.logger.LogInformation("Importing information for the listing Id '{listingId}'.'", xmlListingId);
            var result = await this.listingXmlService.ProcessListingAsync(xmlListingId, request.Type);
            return this.Ok(result);
        }

        [HttpPatch("{xmlListingId:guid}/restore")]
        public async Task<IActionResult> RestoreListing([FromRoute] Guid xmlListingId)
        {
            this.logger.LogInformation("Restore listing with id '{listingId}'.'", xmlListingId);
            await this.listingXmlService.RestoreListingAsync(xmlListingId);
            return this.Ok();
        }

        [HttpPatch("{xmlListingId:guid}")]
        public async Task<IActionResult> ListLater([FromRoute] Guid xmlListingId, [FromBody][Required] DateTime listOn)
        {
            this.logger.LogInformation("List later the listing with id '{listingId}'.'", xmlListingId);
            await this.listingXmlService.ListLaterAsync(xmlListingId, listOn);
            return this.Ok();
        }

        [HttpDelete("{xmlListingId:guid}")]
        public async Task<IActionResult> DeleteListing([FromRoute] Guid xmlListingId)
        {
            this.logger.LogInformation("Delete xml listing by id '{listingId}'.'", xmlListingId);
            await this.listingXmlService.DeleteListingAsync(xmlListingId);
            return this.Ok();
        }
    }
}
