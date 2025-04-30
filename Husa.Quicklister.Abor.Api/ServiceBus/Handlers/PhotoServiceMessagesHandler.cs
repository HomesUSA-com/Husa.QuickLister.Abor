namespace Husa.Quicklister.Abor.Api.ServiceBus.Handlers
{
    using Husa.PhotoService.Domain.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    public class PhotoServiceMessagesHandler : Husa.Quicklister.Extensions.Api.ServiceBus.Handlers.PhotoServiceMessagesHandler
    {
        public PhotoServiceMessagesHandler(IPhotoServiceSubscriber photoRequestSubscriber, IServiceScopeFactory serviceProvider, ILogger<PhotoServiceMessagesHandler> logger)
            : base(photoRequestSubscriber, serviceProvider, logger)
        {
        }

        protected override IPhotoService ResolvePhotoService(IServiceScope scope, PhotoRequestType photoRequestType) => photoRequestType switch
        {
            PhotoRequestType.Residential => scope.ServiceProvider.GetRequiredService<ISaleListingPhotoService>(),
            PhotoRequestType.Lot => scope.ServiceProvider.GetRequiredService<ILotListingPhotoService>(),
            PhotoRequestType.Community => scope.ServiceProvider.GetRequiredService<ICommunityPhotoService>(),
            PhotoRequestType.Plan => scope.ServiceProvider.GetRequiredService<IPlanPhotoService>(),
            _ => null,
        };
    }
}
