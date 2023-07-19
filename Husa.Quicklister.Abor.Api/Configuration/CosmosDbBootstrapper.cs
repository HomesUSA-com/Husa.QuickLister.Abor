namespace Husa.Quicklister.Abor.Api.Configuration
{
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Repositories;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Services;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    public static class CosmosDbBootstrapper
    {
        public static IServiceCollection AddListingRequestsRepositories(this IServiceCollection services)
        {
            services.AddSingleton<CosmosClient>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<DocumentDbSettings>>().Value;

                var cosmoClient = new CosmosClientBuilder(options.Endpoint, options.AuthToken)
                .WithCustomSerializer(BuildCosmosJsonDotNetSerializer())
                .Build();

                CreateDocumentContainerAsync(cosmoClient, options).GetAwaiter().GetResult();

                return cosmoClient;
            });

            services.AddTransient<ICosmosLinqQuery, CosmosLinqQuery>();
            services.AddScoped<ISaleListingRequestRepository, SaleListingRequestRepository>();
            services.AddScoped<ISaleListingRequestQueriesRepository, SaleListingRequestQueriesRepository>();

            return services;
        }

        public static CosmosJsonDotNetSerializer BuildCosmosJsonDotNetSerializer()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            var jsonConverter = new StringEnumConverter(
                namingStrategy: new CamelCaseNamingStrategy(),
                allowIntegerValues: true);
            jsonSerializerSettings.Converters.Add(jsonConverter);
            return new(jsonSerializerSettings);
        }

        private static async Task CreateDocumentContainerAsync(CosmosClient cosmosClient, DocumentDbSettings dbOptions)
        {
            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(dbOptions.DatabaseName);
            await database.Database.CreateContainerIfNotExistsAsync(dbOptions.SaleCollectionName, "/ListingSaleId");
        }
    }
}
