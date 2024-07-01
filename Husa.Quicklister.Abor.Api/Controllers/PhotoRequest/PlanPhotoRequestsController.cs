namespace Husa.Quicklister.Abor.Api.Controllers.PhotoRequest
{
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Extensions.Api.Controllers.PhotoRequest;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("plans/{entityId}/photo-requests")]
    public class PlanPhotoRequestsController : PhotoRequestsController<IPlanPhotoService>
    {
        public PlanPhotoRequestsController(IPlanPhotoService photoService, ILogger<PlanPhotoRequestsController> logger)
            : base(photoService, logger)
        {
        }
    }
}
