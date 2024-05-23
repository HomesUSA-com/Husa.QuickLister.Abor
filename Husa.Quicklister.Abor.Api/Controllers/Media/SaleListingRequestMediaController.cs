namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("sale-listing-requests/{listingRequestId}/media")]
    public class SaleListingRequestMediaController : MediaController<ISaleListingRequestMediaService>
    {
        private readonly ApplicationOptions options;

        public SaleListingRequestMediaController(
            ISaleListingRequestMediaService requestMediaService,
            IOptions<ApplicationOptions> options,
            ILogger<SaleListingRequestMediaController> logger)
            : base(requestMediaService, logger)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override int MediaLimitAllowed => this.options.MediaAllowed.SaleListingMaxAllowedMedia;
    }
}
