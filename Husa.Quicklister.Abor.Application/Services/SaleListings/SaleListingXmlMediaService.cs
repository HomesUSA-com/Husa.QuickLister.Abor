namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using ExtensionsServices = Husa.Quicklister.Extensions.Application.Media;

    public class SaleListingXmlMediaService : ExtensionsServices.XmlMediaService<
        Plan,
        CommunitySale,
        SaleListing,
        IPlanRepository,
        ICommunitySaleRepository,
        IListingSaleRepository>, ISaleListingXmlMediaService
    {
        public SaleListingXmlMediaService(
            IXmlClient xmlClient,
            ExtensionsInterfaces.ISaleListingMediaService mediaService,
            IPlanRepository planRepository,
            ICommunitySaleRepository communitySaleRepository,
            IListingSaleRepository listingSaleRepository,
            IUserContextProvider userContextProvider,
            IProvideTraceId traceIdProvider,
            ILogger<SaleListingXmlMediaService> logger)
            : base(xmlClient, mediaService, traceIdProvider, planRepository, communitySaleRepository, listingSaleRepository, userContextProvider, logger)
        {
        }

        protected override MarketCode MarketCode => MarketCode.Austin;
    }
}
