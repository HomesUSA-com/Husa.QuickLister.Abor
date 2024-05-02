namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
{
    using Azure.Messaging.ServiceBus;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Media.Interfaces;
    using Husa.MediaService.Client;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ExtensionServices = Husa.Quicklister.Extensions.Application.Media;

    public class ListingRequestMediaService : ExtensionServices.ListingRequestMediaService<SaleListingRequest, ISaleListingRequestRepository>, IListingRequestMediaService
    {
        public ListingRequestMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            ISaleListingRequestRepository saleRequestRepository,
            IMediaServiceClient mediaClient,
            ServiceBusClient client,
            IProvideTraceId traceIdProvider,
            IBlobService blobService,
            ICache cache,
            ILogger<ListingRequestMediaService> logger)
            : base(
                serviceBusSettings,
                userContextProvider,
                saleRequestRepository,
                mediaClient,
                client,
                traceIdProvider,
                blobService,
                cache,
                logger)
        {
        }

        public override MarketCode Market => MarketCode.Austin;
    }
}
