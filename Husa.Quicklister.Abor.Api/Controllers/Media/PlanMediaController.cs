namespace Husa.Quicklister.Abor.Api.Controllers.Media
{
    using System;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Extensions.Api.Controllers.Media;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("plan/{entityId}/media")]
    [Route("plans/{entityId}/media")]
    public class PlanMediaController : MediaController<IPlanMediaService>
    {
        private readonly ApplicationOptions options;
        public PlanMediaController(IPlanMediaService planMediaService, IOptions<ApplicationOptions> options, ILogger<PlanMediaController> logger)
            : base(planMediaService, logger)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override int MediaLimitAllowed => this.options.MediaAllowed.PlanMaxAllowedMedia;
    }
}
