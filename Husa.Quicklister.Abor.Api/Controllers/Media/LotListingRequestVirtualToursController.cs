namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("lot-listing-requests/{entityId}/virtual-tour")]
    public class LotListingRequestVirtualToursController : VirtualToursController<ILotListingRequestMediaService>
    {
        public LotListingRequestVirtualToursController(ILotListingRequestMediaService requestMediaService, ILogger<LotListingRequestVirtualToursController> logger)
            : base(requestMediaService, logger)
        {
        }
    }
}
