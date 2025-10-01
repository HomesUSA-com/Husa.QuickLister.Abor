namespace Husa.Quicklister.Abor.Api.Configuration
{
    using Husa.Quicklister.Abor.Application.Services.ShowingTime;
    using Husa.Quicklister.Abor.Data.Commands.Repositories;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.ShowingTime;
    using Microsoft.Extensions.DependencyInjection;
    using ShowingTimeExtensions = Husa.Quicklister.Extensions.Application.Services.ShowingTime;

    public static class ShowingTimeConfiguration
    {
        public static IServiceCollection ConfigureShowingTime(this IServiceCollection services)
        {
            services.AddSingleton<ShowingTimeContactProjection>();
            services.AddSingleton<CommunityShowingTimeContactOrderProjection>();
            services.AddSingleton<ListingShowingTimeContactOrderProjection>();

            services.AddScoped<IShowingTimeContactRepository, ShowingTimeContactRepository>();
            services.AddScoped<IShowingTimeContactQueriesRepository, ShowingTimeContactQueriesRepository>();
            services.AddScoped<IProvideShowingTimeContacts, ShowingTimeContactQueriesRepository>();

            services.AddScoped<IShowingTimeContactService, ShowingTimeContactService>();
            services.AddScoped<IShowingTimeService, ShowingTimeExtensions.ShowingTimeService<SaleListing, CommunitySale, IListingSaleRepository, ICommunitySaleRepository>>();
            return services;
        }
    }
}
