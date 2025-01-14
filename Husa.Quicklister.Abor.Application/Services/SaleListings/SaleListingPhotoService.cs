namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class SaleListingPhotoService : PhotoExtensions.SaleListingPhotoService<SaleListing, IListingSaleRepository>, ISaleListingPhotoService
    {
        public SaleListingPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IListingSaleRepository listingSaleRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<SaleListingPhotoService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               photoClient,
               busClient,
               traceIdProvider,
               listingSaleRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override MarketCode MarketCode => MarketCode.Austin;

        protected override void SetCustomValuesToPropertyBusMessage(SaleListing entity, PropertyBusMessage message)
        {
            base.SetCustomValuesToPropertyBusMessage(entity, message);
            message.PlanName = entity.SaleProperty.PlanId.HasValue ? entity.SaleProperty.Plan.BasePlan.Name : null;
        }
    }
}
