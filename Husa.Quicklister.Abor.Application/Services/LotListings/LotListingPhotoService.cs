namespace Husa.Quicklister.Abor.Application.Services.LotListings
{
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.Logging;
    using PhotoExtensions = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public class LotListingPhotoService : PhotoExtensions.LotListingPhotoService<LotListing, ILotListingRepository>, ILotListingPhotoService
    {
        public LotListingPhotoService(
            IUserContextProvider userContextProvider,
            IPhotoServiceClient photoClient,
            IPhotoBusService busService,
            ILotListingRepository listingRepository,
            IServiceSubscriptionClient serviceSubscriptionClient,
            ILogger<LotListingPhotoService> logger)
         : base(
               userContextProvider,
               photoClient,
               busService,
               listingRepository,
               serviceSubscriptionClient,
               logger)
        {
        }
    }
}
