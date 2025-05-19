namespace Husa.Quicklister.Abor.Application.Services.SaleListings
{
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.PhotoService.ServiceBus.Messages.Contracts;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.Logging;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class SaleListingPhotoService : PhotoExtensions.SaleListingPhotoService<SaleListing, IListingSaleRepository>, ISaleListingPhotoService
    {
        public SaleListingPhotoService(
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            IPhotoBusService busService,
            IListingSaleRepository listingSaleRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<SaleListingPhotoService> logger)
         : base(
               userContextProvider,
               photoClient,
               busService,
               listingSaleRepository,
               serviceSubscriptionClient,
               logger)
        {
        }

        protected override void SetCustomValuesToPropertyBusMessage(SaleListing entity, PropertyBusMessage message)
        {
            base.SetCustomValuesToPropertyBusMessage(entity, message);
            message.PlanName = entity.SaleProperty.PlanId.HasValue ? entity.SaleProperty.Plan.BasePlan.Name : null;
        }
    }
}
