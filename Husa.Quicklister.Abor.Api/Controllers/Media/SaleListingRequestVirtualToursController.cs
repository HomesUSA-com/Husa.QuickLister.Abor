namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listing-requests/{listingRequestId}/virtual-tour")]
    public class SaleListingRequestVirtualToursController : Controller
    {
        private readonly IListingRequestMediaService requestMediaService;
        private readonly ILogger<SaleListingMediaController> logger;
        public SaleListingRequestVirtualToursController(IListingRequestMediaService requestMediaService, ILogger<SaleListingMediaController> logger)
        {
            this.requestMediaService = requestMediaService ?? throw new ArgumentNullException(nameof(requestMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid listingRequestId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {listingRequestId}", listingRequestId);

            await this.requestMediaService.VirtualTour.CreateAsync(listingRequestId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid listingRequestId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.requestMediaService.VirtualTour.DeleteById(listingRequestId, virtualTourId);

            return this.Ok();
        }
    }
}
