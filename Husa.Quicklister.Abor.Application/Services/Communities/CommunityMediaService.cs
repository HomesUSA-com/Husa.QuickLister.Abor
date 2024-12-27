namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CommunityMediaService : MediaServiceEntity<ICommunitySaleRepository, CommunitySale>, ICommunityMediaService
    {
        public CommunityMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            ICommunitySaleRepository communitySaleRepository,
            IBlobService blobService,
            ICache cache,
            ILogger<CommunityMediaService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               mediaServiceClient,
               busClient,
               traceIdProvider,
               communitySaleRepository,
               blobService,
               cache,
               logger)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.CommunityProfile;
    }
}
