namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("sale-communities/{entityId}/media")]
    public class SaleCommunityMediaController : MediaController<ICommunityMediaService>
    {
        private readonly ApplicationOptions options;
        public SaleCommunityMediaController(ICommunityMediaService communityMediaService, IOptions<ApplicationOptions> options, ILogger<SaleCommunityMediaController> logger)
            : base(communityMediaService, logger)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override int MediaLimitAllowed => this.options.MediaAllowed.SaleCommunityMaxAllowedMedia;
    }
}
