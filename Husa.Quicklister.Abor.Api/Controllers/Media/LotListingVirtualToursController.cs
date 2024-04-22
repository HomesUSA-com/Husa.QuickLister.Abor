namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("lot-listings/{listingId}/virtual-tour")]
    public class LotListingVirtualToursController : Controller
    {
        private readonly ILotListingMediaService listingMediaService;
        private readonly ILogger<LotListingVirtualToursController> logger;
        public LotListingVirtualToursController(
            ILotListingMediaService listingMediaService,
            ILogger<LotListingVirtualToursController> logger)
        {
            this.listingMediaService = listingMediaService ?? throw new ArgumentNullException(nameof(listingMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid listingId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {listingId}", listingId);

            await this.listingMediaService.VirtualTour.CreateAsync(listingId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid listingId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.listingMediaService.VirtualTour.DeleteById(listingId, virtualTourId);

            return this.Ok();
        }
    }
}
