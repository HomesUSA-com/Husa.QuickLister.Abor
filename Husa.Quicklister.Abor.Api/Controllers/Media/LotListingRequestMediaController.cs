namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("lot-listing-requests/{entityId}/media")]
    public class LotListingRequestMediaController : MediaController<ILotListingRequestMediaService>
    {
        public LotListingRequestMediaController(
            ILotListingRequestMediaService requestMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<LotListingRequestMediaController> logger)
            : base(requestMediaService, logger)
        {
        }
    }
}
