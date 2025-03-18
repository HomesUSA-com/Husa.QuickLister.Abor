namespace Husa.Quicklister.Abor.Application.Services.LotListings
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
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ApplicationOptions = Husa.Quicklister.Abor.Crosscutting.ApplicationOptions;

    public class LotListingMediaService : MediaServiceEntity<ILotListingRepository, LotListing>, ILotListingMediaService
    {
        public LotListingMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IOptions<ApplicationOptions> options,
            IUserContextProvider userContextProvider,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            ILotListingRepository listingRepository,
            IBlobService blobService,
            ICache cache,
            ILogger<LotListingMediaService> logger,
            IMapper mapper)
         : base(
               serviceBusSettings,
               options,
               userContextProvider,
               mediaServiceClient,
               busClient,
               traceIdProvider,
               listingRepository,
               blobService,
               cache,
               logger,
               mapper)
        {
        }

        public override MarketCode Market => MarketCode.Austin;
        public override MediaType MediaType => MediaType.Lot;
    }
}
