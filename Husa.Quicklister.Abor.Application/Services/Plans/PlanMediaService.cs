namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Media.Interfaces;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class PlanMediaService : MediaServiceEntity<IPlanRepository, Plan>, IPlanMediaService
    {
        public PlanMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IPlanRepository planRepository,
            IBlobService blobService,
            ICache cache,
            ILogger<PlanMediaService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               mediaServiceClient,
               busClient,
               traceIdProvider,
               planRepository,
               blobService,
               cache,
               logger)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.PlanProfile;
    }
}
