namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Api.Controllers.PhotoRequest;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-listings/{entityId}/photo-requests")]
    public class SaleListingPhotoRequestsController : PhotoRequestsController<ISaleListingPhotoService>
    {
        public SaleListingPhotoRequestsController(ISaleListingPhotoService photoService, ILogger<SaleListingPhotoRequestsController> logger)
            : base(photoService, logger)
        {
        }
    }
}
