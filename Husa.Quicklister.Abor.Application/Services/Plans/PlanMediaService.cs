namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using AutoMapper;
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ApplicationOptions = Husa.Quicklister.Abor.Crosscutting.ApplicationOptions;

    public class PlanMediaService : MediaServiceEntity<IPlanRepository, Plan>, IPlanMediaService
    {
        public PlanMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IOptions<ApplicationOptions> options,
            IUserContextProvider userContextProvider,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IPlanRepository planRepository,
            IBlobService blobService,
            ICache cache,
            ILogger<PlanMediaService> logger,
            IMapper mapper)
         : base(
               serviceBusSettings,
               options,
               userContextProvider,
               mediaServiceClient,
               busClient,
               traceIdProvider,
               planRepository,
               blobService,
               cache,
               logger,
               mapper)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.PlanProfile;
    }
}
