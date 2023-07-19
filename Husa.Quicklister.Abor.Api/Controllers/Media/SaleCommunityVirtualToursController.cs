namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-communities/{communityId}/virtual-tour")]
    public class SaleCommunityVirtualToursController : Controller
    {
        private readonly ICommunityMediaService communityMediaService;
        private readonly ILogger<SaleCommunityVirtualToursController> logger;
        public SaleCommunityVirtualToursController(ICommunityMediaService communityMediaService, ILogger<SaleCommunityVirtualToursController> logger)
        {
            this.communityMediaService = communityMediaService ?? throw new ArgumentNullException(nameof(communityMediaService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateVirtualTourAsync([FromRoute] Guid communityId, [FromBody] VirtualTour virtualTour)
        {
            this.logger.LogInformation("Starting to add Virtual tour to entity id {communityId}", communityId);

            await this.communityMediaService.VirtualTour.CreateAsync(communityId, virtualTour);

            return this.Ok();
        }

        [HttpDelete("{virtualTourId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> DeleteVirtualTour([FromRoute] Guid communityId, [FromRoute] Guid virtualTourId)
        {
            this.logger.LogInformation("Starting to delete virtual tour with id {virtualTourId}", virtualTourId);

            await this.communityMediaService.VirtualTour.DeleteById(communityId, virtualTourId);

            return this.Ok();
        }
    }
}
