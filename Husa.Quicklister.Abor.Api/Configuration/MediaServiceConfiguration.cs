namespace Husa.Quicklister.Abor.Api.Configuration
{
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Application.Media;
    using Microsoft.Extensions.DependencyInjection;

    public static class MediaServiceConfiguration
    {
        public static IServiceCollection ConfigureMediaServices(this IServiceCollection services)
        {
            services.AddScoped<ICommunityMediaService, CommunityMediaService<CommunitySale, ApplicationOptions, ICommunitySaleRepository>>();
            services.AddScoped<IPlanMediaService, PlanMediaService<Plan, ApplicationOptions, IPlanRepository>>();
            services.AddScoped<ISaleListingMediaService, SaleListingMediaService<SaleListing, ApplicationOptions, IListingSaleRepository>>();
            services.AddScoped<ISaleListingRequestMediaService, SaleListingRequestMediaService<SaleListingRequest, ApplicationOptions, ISaleListingRequestRepository>>();
            services.AddScoped<ILotListingMediaService, LotListingMediaService<LotListing, ApplicationOptions, ILotListingRepository>>();
            services.AddScoped<ILotListingRequestMediaService, LotListingRequestMediaService<LotListingRequest, ApplicationOptions, ILotListingRequestRepository>>();
            return services;
        }
    }
}
