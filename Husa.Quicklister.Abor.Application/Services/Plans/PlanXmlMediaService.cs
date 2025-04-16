namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting.Clients;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Microsoft.Extensions.Logging;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Media;
    public class PlanXmlMediaService : ExtensionsServices.XmlMediaService<
        Plan,
        CommunitySale,
        SaleListing,
        IPlanRepository,
        ICommunitySaleRepository,
        IListingSaleRepository>, IPlanXmlMediaService
    {
        public PlanXmlMediaService(
            IXmlClientWithoutToken xmlClient,
            IPlanMediaService mediaService,
            IPlanRepository planRepository,
            ICommunitySaleRepository communitySaleRepository,
            IListingSaleRepository listingSaleRepository,
            IUserContextProvider userContextProvider,
            IProvideTraceId traceIdProvider,
            ILogger<PlanXmlMediaService> logger)
            : base(xmlClient, mediaService, traceIdProvider, planRepository, communitySaleRepository, listingSaleRepository, userContextProvider, logger)
        {
        }

        protected override MarketCode MarketCode => MarketCode.Austin;
    }
}
