namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class PlanPhotoService : PhotoExtensions.PlanPhotoService<Plan, IPlanRepository>, IPlanPhotoService
    {
        public PlanPhotoService(
            IOptions<ServiceBusSettings> serviceBusSettings,
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            ServiceBusClient busClient,
            IProvideTraceId traceIdProvider,
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<PlanPhotoService> logger)
         : base(
               serviceBusSettings,
               userContextProvider,
               photoClient,
               busClient,
               traceIdProvider,
               planRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        public override MarketCode MarketCode => MarketCode.Austin;

        protected override void SetCustomValuesToPropertyBusMessage(Plan entity, PropertyBusMessage message)
        {
            base.SetCustomValuesToPropertyBusMessage(entity, message);
            message.PlanName = entity.BasePlan.Name;
        }
    }
}
