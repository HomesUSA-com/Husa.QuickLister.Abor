namespace Husa.Quicklister.Abor.Application.Services.Plans
{
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.Logging;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class PlanPhotoService : PhotoExtensions.PlanPhotoService<Plan, IPlanRepository>, IPlanPhotoService
    {
        public PlanPhotoService(
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            IPhotoBusService busService,
            IPlanRepository planRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<PlanPhotoService> logger)
         : base(
               userContextProvider,
               photoClient,
               busService,
               planRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        protected override void SetCustomValuesToPropertyBusMessage(Plan entity, PropertyBusMessage message)
        {
            base.SetCustomValuesToPropertyBusMessage(entity, message);
            message.PlanName = entity.BasePlan.Name;
        }
    }
}
