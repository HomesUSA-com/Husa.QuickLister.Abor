namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Filters;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    [ApiController]
    [Route("sale-listings/{listingId}/photo-requests")]
    public class SaleListingPhotoRequestsController : Controller
    {
        private readonly ISaleListingPhotoService photoService;
        private readonly ILogger<SaleListingPhotoRequestsController> logger;
        public SaleListingPhotoRequestsController(ISaleListingPhotoService photoService, ILogger<SaleListingPhotoRequestsController> logger)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid listingId, [FromQuery] Request.PhotoRequestFilter filter)
        {
            this.logger.LogInformation($"Starting to GET photo request for the entity {listingId}.");
            var photoRequests = await this.photoService.GetAsync(listingId, filter);
            return this.Ok(photoRequests);
        }

        [HttpGet("{photoRequestId}")]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee, RoleEmployee.Readonly)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid listingId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation($"Starting to get the photorequest for entity {listingId} and photoRequest Id '{photoRequestId}'");
            var photoRequest = await this.photoService.GetByIdAsync(listingId, photoRequestId);
            return this.Ok(photoRequest);
        }

        [HttpPost]
        [ApiAuthorization(RoleEmployee.CompanyAdmin, RoleEmployee.SalesEmployee)]
        public async Task<IActionResult> CreateAsync([FromRoute] Guid listingId, [FromBody] Request.PhotoRequest photoRequest)
        {
            this.logger.LogInformation($"Starting to create photorequest to entity with id {listingId}");
            await this.photoService.CreateAsync(listingId, photoRequest);
            return this.Ok();
        }

        [HttpDelete("{photoRequestId}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid listingId, [FromRoute] Guid photoRequestId)
        {
            this.logger.LogInformation($"Starting to Delete photorequest: {photoRequestId} from entity with id: {listingId}.");
            await this.photoService.DeleteByIdAsync(listingId, photoRequestId);
            return this.Ok();
        }
    }
}
