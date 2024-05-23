namespace Husa.Quicklister.Abor.Api.Configuration
{
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Repositories;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class CosmosDbBootstrapper
    {
        public static IServiceCollection AddCosmosRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICommunityHistoryRepository, CommunityHistoryRepository>();
            services.AddScoped<ICommunityHistoryQueriesRepository, CommunityHistoryQueriesRepository>();
            services.AddScoped<ISaleListingRequestRepository, SaleListingRequestRepository>();
            services.AddScoped<ISaleListingRequestQueriesRepository, SaleListingRequestQueriesRepository>();
            services.AddScoped<ILotListingRequestRepository, LotListingRequestRepository>();
            services.AddScoped<ILotListingRequestQueriesRepository, LotListingRequestQueriesRepository>();

            return services;
        }
    }
}
