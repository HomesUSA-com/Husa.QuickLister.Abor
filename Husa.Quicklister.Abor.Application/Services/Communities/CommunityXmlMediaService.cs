namespace Husa.Quicklister.Abor.Application.Services.Communities
{
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Media;
    public class CommunityXmlMediaService : ExtensionsServices.XmlMediaService<
        Plan,
        CommunitySale,
        SaleListing,
        IPlanRepository,
        ICommunitySaleRepository,
        IListingSaleRepository>, ICommunityXmlMediaService
    {
        public CommunityXmlMediaService(
            IXmlClientWithoutToken xmlClient,
            ICommunityMediaService mediaService,
            IPlanRepository planRepository,
            ICommunitySaleRepository communitySaleRepository,
            IListingSaleRepository listingSaleRepository,
            IUserContextProvider userContextProvider,
            IProvideTraceId traceIdProvider,
            ILogger<CommunityXmlMediaService> logger)
            : base(xmlClient, mediaService, traceIdProvider, planRepository, communitySaleRepository, listingSaleRepository, userContextProvider, logger)
        {
        }

        protected override MarketCode MarketCode => MarketCode.Austin;
    }
}
