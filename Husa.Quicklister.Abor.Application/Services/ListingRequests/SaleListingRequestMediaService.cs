namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.MediaService.Client;
    using Husa.MediaService.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionServices = Husa.Quicklister.Extensions.Application.Media;

    public class SaleListingRequestMediaService : ExtensionServices.ListingRequestMediaService<SaleListingRequest, ISaleListingRequestRepository>, ISaleListingRequestMediaService
    {
        public SaleListingRequestMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            ISaleListingRequestRepository requestRepository,
            IMediaServiceClient mediaClient,
            ServiceBusClient client,
            IProvideTraceId traceIdProvider,
            IBlobService blobService,
            ICache cache,
            ILogger<SaleListingRequestMediaService> logger)
            : base(
                serviceBusSettings,
                userContextProvider,
                requestRepository,
                mediaClient,
                client,
                traceIdProvider,
                blobService,
                cache,
                logger)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.ListingRequest;
    }
}
