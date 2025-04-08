namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listings/{listingId}/virtual-tour")]
    public class SaleListingVirtualToursController : Controller
    {
        private readonly ISaleListingMediaService listingMediaService;
        private readonly ILogger<SaleListingVirtualToursController> logger;
        public SaleListingVirtualToursController(
            ISaleListingMediaService listingMediaService,
            ILogger<SaleListingVirtualToursController> logger)
        {
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid listingId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {listingId}", listingId);

            await this.listingMediaService.VirtualTour.CreateAsync(listingId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid listingId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.listingMediaService.VirtualTour.DeleteById(listingId, virtualTourId);

            return this.Ok();
        }
    }
}
