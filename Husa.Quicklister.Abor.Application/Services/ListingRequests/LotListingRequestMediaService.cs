namespace Husa.Quicklister.Abor.Application.Services.ListingRequests
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
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ApplicationOptions = Husa.Quicklister.Abor.Crosscutting.ApplicationOptions;
    using ExtensionServices = Husa.Quicklister.Extensions.Application.Media;

    public class LotListingRequestMediaService : ExtensionServices.ListingRequestMediaService<LotListingRequest, ILotListingRequestRepository>, ILotListingRequestMediaService
    {
        public LotListingRequestMediaService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IOptions<ApplicationOptions> options,
            IUserContextProvider userContextProvider,
            ILotListingRequestRepository requestRepository,
            IMediaServiceClient mediaClient,
            ServiceBusClient client,
            IProvideTraceId traceIdProvider,
            IBlobService blobService,
            ICache cache,
            ILogger<LotListingRequestMediaService> logger,
            IMapper mapper)
            : base(
                serviceBusSettings,
                options,
                userContextProvider,
                requestRepository,
                mediaClient,
                client,
                traceIdProvider,
                blobService,
                cache,
                logger,
                mapper)
        {
        }

        public override MarketCode Market => MarketCode.Austin;

        public override MediaType MediaType => MediaType.LotRequest;
    }
}
