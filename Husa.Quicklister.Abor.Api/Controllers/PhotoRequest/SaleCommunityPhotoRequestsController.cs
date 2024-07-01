namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Api.Controllers.PhotoRequest;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("sale-communities/{entityId}/photo-requests")]
    public class SaleCommunityPhotoRequestsController : PhotoRequestsController<ICommunityPhotoService>
    {
        public SaleCommunityPhotoRequestsController(ICommunityPhotoService photoService, ILogger<SaleCommunityPhotoRequestsController> logger)
            : base(photoService, logger)
        {
        }
    }
}
