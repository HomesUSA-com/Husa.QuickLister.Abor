namespace Husa.Quicklister.Abor.Application.Services.SaleListings
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
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Media;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SaleListingMediaService : MediaServiceEntity<IListingSaleRepository, SaleListing>, ISaleListingMediaService
    {
        public SaleListingMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IMediaServiceClient mediaServiceClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IListingSaleRepository listingSaleRepository,
            IBlobService blobService,
            ICache cache,
            ILogger<SaleListingMediaService> logger,
            IMapper mapper)
         : base(
               serviceBusSettings,
               userContextProvider,
               mediaServiceClient,
               busClient,
               traceIdProvider,
               listingSaleRepository,
               blobService,
               cache,
               logger,
               mapper)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.Residential;
    }
}
