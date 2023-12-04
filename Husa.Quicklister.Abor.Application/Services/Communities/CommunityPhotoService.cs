namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class CommunityPhotoService : PhotoExtensions.CommunityPhotoService<CommunitySale, ICommunitySaleRepository>, ICommunityPhotoService
    {
        public CommunityPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            ICommunitySaleRepository communitySaleRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<CommunityPhotoService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               photoClient,
               busClient,
               traceIdProvider,
               communitySaleRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override MarketCode MarketCode => MarketCode.Austin;
    }
}
