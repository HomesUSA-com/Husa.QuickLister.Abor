namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Api.Controllers.PhotoRequest;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("lot-listings/{entityId}/photo-requests")]
    public class LotListingPhotoRequestsController : PhotoRequestsController<ILotListingPhotoService>
    {
        public LotListingPhotoRequestsController(ILotListingPhotoService photoService, ILogger<LotListingPhotoRequestsController> logger)
            : base(photoService, logger)
        {
        }
    }
}
