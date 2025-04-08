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
    [Route("lot-listing-requests/{listingRequestId}/virtual-tour")]
    public class LotListingRequestVirtualToursController : Controller
    {
        private readonly ILotListingRequestMediaService requestMediaService;
        private readonly ILogger<LotListingRequestVirtualToursController> logger;
        public LotListingRequestVirtualToursController(ILotListingRequestMediaService requestMediaService, ILogger<LotListingRequestVirtualToursController> logger)
        {
            this.requestMediaService = requestMediaService ?? throw new ArgumentNullException(nameof(requestMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid listingRequestId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {listingRequestId}", listingRequestId);

            await this.requestMediaService.VirtualTour.CreateAsync(listingRequestId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [RolesFilter(employeeRoles: [RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee])]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid listingRequestId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.requestMediaService.VirtualTour.DeleteById(listingRequestId, virtualTourId);

            return this.Ok();
        }
    }
}
