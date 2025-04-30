namespace Husa.Quicklister.Abor.Api.Configuration
{
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.DependencyInjection;
    using ExtensionsPhoto = Husa.Quicklister.Extensions.Application.Services.PhotoServices;

    public static class PhotoServiceConfiguration
    {
        public static IServiceCollection ConfigurePhotoServices(this IServiceCollection services)
        {
            services.AddScoped<IPhotoBusService, ExtensionsPhoto.PhotoBusService<ApplicationOptions>>();
            services.AddScoped<ICommunityPhotoService, ExtensionsPhoto.CommunityPhotoService<CommunitySale, ICommunitySaleRepository>>();
            services.AddScoped<ILotListingPhotoService, LotListingPhotoService>();
            services.AddScoped<IPlanPhotoService, PlanPhotoService>();
            services.AddScoped<ISaleListingPhotoService, SaleListingPhotoService>();
            return services;
        }
    }
}
