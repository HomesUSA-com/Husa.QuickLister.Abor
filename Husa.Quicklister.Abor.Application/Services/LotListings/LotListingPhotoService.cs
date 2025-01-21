namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class LotListingPhotoService : PhotoExtensions.ListingPhotoService<LotListing, ILotListingRepository>, ILotListingPhotoService
    {
        public LotListingPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            ILotListingRepository listingRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<LotListingPhotoService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               photoClient,
               busClient,
               traceIdProvider,
               listingRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override MarketCode MarketCode => MarketCode.Austin;
        public override PropertyType PropertyType => PropertyType.Lot;
    }
}
