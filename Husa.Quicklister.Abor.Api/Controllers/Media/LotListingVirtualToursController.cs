namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("lot-listings/{entityId}/virtual-tour")]
    public class LotListingVirtualToursController : VirtualToursController<ILotListingMediaService>
    {
        public LotListingVirtualToursController(ILotListingMediaService requestMediaService, ILogger<LotListingVirtualToursController> logger)
            : base(requestMediaService, logger)
        {
        }
    }
}
