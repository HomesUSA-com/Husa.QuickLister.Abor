namespace Husa.Quicklister.Abor.Api.Configuration
{
    using System;
    using AutoMapper;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.Sabor.Client;
    using Husa.Extensions.Api;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Authorization.Extensions;
    using Husa.Extensions.Cache.Extensions;
    using Husa.Extensions.Common;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.Media.Services;
    using Husa.Extensions.ServiceBus.Extensions;
    using Husa.MediaService.Client;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Client.Options;
    using Husa.Notes.Client;
    using Husa.PhotoService.Api.Client;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Mappings;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.ValidationsRules;
    using Husa.Quicklister.Abor.Application;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Application.Services.ListingRequests;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Application.Services.Reports;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Application.Services.ShowingTime;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Data;
    using Husa.Quicklister.Abor.Data.Commands.Repositories;
    using Husa.Quicklister.Abor.Data.Documents.Repositories;
    using Husa.Quicklister.Abor.Data.Queries;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Api.Configuration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Application.Interfaces.ShowingTime;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.ReverseProspect.Api.Client;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using ApplicationOptions = Husa.Quicklister.Abor.Crosscutting.ApplicationOptions;
    using IAgentQueriesRepository = Husa.Quicklister.Abor.Data.Queries.Interfaces.IAgentQueriesRepository;
    using InterfaceExtensions = Husa.Quicklister.Extensions.Application.Interfaces;
    using RepositoriesExtensions = Husa.Quicklister.Extensions.Domain.Repositories;

    public static class Bootstrapper
    {
        public static IServiceCollection BindOptions(this IServiceCollection services)
        {
            services
                .BindApplicationOptions<ApplicationOptions>(Husa.Extensions.Common.Enums.MarketCode.Austin)
                .BindDocumentDbSettings()
                .BindDownloaderSettings();

            return services;
        }

        public static IServiceCollection AddCommandRepositories(this IServiceCollection services)
        {
            services.AddScoped<IListingSaleRepository, ListingSaleRepository>();
            services.AddScoped<ICommunitySaleRepository, CommunitySaleRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IReverseProspectRepository, ReverseProspectRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<ILotListingRepository, LotListingRepository>();
            services.AddScoped<RepositoriesExtensions.IViolationWarningAlertRepository, ViolationWarningAlertRepository>();
            services.AddScoped<ILegacySaleListingRepository, LegacySaleListingRepository>();
            services.AddScoped<IShowingTimeContactRepository, ShowingTimeContactRepository>();
            services.AddScoped<RepositoriesExtensions.IRequestErrorRepository, RequestErrorRepository>();

            return services;
        }

        public static IServiceCollection AddCommonRepositories(this IServiceCollection services)
            => services.AddScoped<RepositoriesExtensions.IUserRepository, Husa.Quicklister.Extensions.Data.Queries.Repositories.UserRepository>();

        public static IServiceCollection AddQueriesRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ShowingTimeContactProjection>();
            services.AddSingleton<CommunityShowingTimeContactOrderProjection>();
            services.AddSingleton<ListingShowingTimeContactOrderProjection>();
            services.AddScoped<IListingSaleQueriesRepository, ListingSaleQueriesRepository>();
            services.AddScoped<IQueryCommunityEmployeeRepository, CommunityEmployeeQueriesRepository>();
            services.AddScoped<ICommunityQueriesRepository, CommunityQueriesRepository>();
            services.AddScoped<IPlanQueriesRepository, PlanQueriesRepository>();
            services.AddScoped<IAgentQueriesRepository, AgentQueriesRepository>();
            services.AddScoped<IAlertQueriesRepository, AlertQueriesRepository>();
            services.AddScoped<IScrapedListingQueriesRepository, ScrapedListingQueriesRepository>();
            services.AddScoped<IQueryXmlRepository, QueryXmlRepository>();
            services.AddScoped<IQueryJsonRepository, QueryJsonRepository>();
            services.AddScoped<IManagementTraceQueriesRepository, ManagementTraceQueriesRepository>();
            services.AddScoped<IMigrationQueryRepository, RequestMigrationQueryRepository>();
            services.AddScoped<ILotListingQueriesRepository, LotListingQueriesRepository>();
            services.AddScoped<IResidentialIdxQueriesRepository, ResidentialIdxQueriesRepository>();
            services.AddScoped<IShowingTimeContactQueriesRepository, ShowingTimeContactQueriesRepository>();
            services.AddScoped<IProvideShowingTimeContacts, ShowingTimeContactQueriesRepository>();
            services.AddScoped<IQueryListingBillingRepository, ListingBillingQueriesRepository>();
            services.AddScoped<IListingRequestBillingQueryRepository, ListingRequestBillingQueriesRepository>();
            services.AddExtensionRepositories();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddExtensionServices();
            services.AddScoped<InterfaceExtensions.Listing.ILegacyListingService, LegacyListingService>();
            services.AddTransient<IValidateListingStatusChanges<ListingSaleRequestForUpdate>, ListingSaleRequestWithStatusChangeValidator>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAgentServiceDownloader, AgentServiceDownloader>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IListingService, ListingService>();
            services.AddScoped<IMediaService, MediaService>();

            services.AddScoped<ISaleListingService, SaleListingService>();
            services.AddScoped<ISaleListingXmlService, SaleListingXmlService>();

            services.AddScoped<ILotListingService, LotListingService>();
            services.AddScoped<ILotListingRequestService, LotListingRequestService>();

            services.AddScoped<ICommunityHistoryService, CommunityHistoryService>();
            services.AddScoped<ISaleListingRequestService, SaleListingRequestService>();
            services.AddScoped<InterfaceExtensions.JsonImport.IListingRequestJsonImportService, ListingRequestJsonImportService>();
            services.AddScoped<InterfaceExtensions.Migration.IListingRequestMigrationService, ListingRequestMigrationService>();
            services.AddScoped<ISaleListingMigrationService, SaleListingMigrationService>();

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<InterfaceExtensions.JsonImport.IPlanJsonImportService, PlanJsonImportService>();
            services.AddScoped<InterfaceExtensions.Plan.IPlanXmlService, PlanXmlService>();
            services.AddScoped<InterfaceExtensions.Migration.IPlanMigrationService, PlanMigrationService>();
            services.AddScoped<InterfaceExtensions.Plan.IPlanDeletionService, PlanDeletionService>();

            services.AddScoped<ISaleCommunityService, SaleCommunityService>();
            services.AddScoped<InterfaceExtensions.Community.ICommunityXmlService, CommunityXmlService>();
            services.AddScoped<InterfaceExtensions.JsonImport.ICommunityJsonImportService, CommunityJsonImportService>();
            services.AddScoped<ICommunityMigrationService, CommunityMigrationService>();
            services.AddScoped<InterfaceExtensions.Migration.ICommunityHistoryMigrationService, CommunityHistoryMigrationService>();
            services.AddScoped<InterfaceExtensions.Community.ICommunityDeletionService, CommunityDeletionService>();

            services.AddScoped<IUploaderService, UploaderService>();
            services.AddScoped<IPlanXmlMediaService, PlanXmlMediaService>();
            services.AddScoped<ICommunityXmlMediaService, CommunityXmlMediaService>();
            services.AddScoped<ISaleListingXmlMediaService, SaleListingXmlMediaService>();
            services.AddScoped<IImportMlsMediaMessagingService, ImportMlsMediaMessagingService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<ISaleListingBillService, SaleListingBillService>();
            services.AddScoped<InterfaceExtensions.Reports.IDiscrepancyReportService, DiscrepancyReportService>();
            services.AddScoped<InterfaceExtensions.Listing.ICallForwardService, CallForwardService>();
            services.AddScoped<InterfaceExtensions.JsonImport.IListingJsonImportService, ListingJsonImportService>();
            services.AddScoped<IListingRequestXmlService<XmlListingDetailResponse>, ListingRequestXmlService>();

            services.AddScoped<IShowingTimeContactService, ShowingTimeContactService>();

            services.ConfigureLegacyListingService(Migration.Enums.MigrationMarketType.Austin);
            services.ConfigureMediaServices();
            services.ConfigureNoteServices();
            services.ConfigurePhotoServices();
            return services;
        }

        public static IServiceCollection ConfigureServiceBusOptions(this IServiceCollection services)
        {
            services
                .AddOptions<ServiceBusSettings>()
                .Configure<IConfiguration>((settings, config) => config.GetSection(ServiceBusSettings.Section).Bind(settings))
                .ValidateDataAnnotations();

            return services;
        }

        public static IServiceCollection AddServiceBusOptions(this IServiceCollection services)
        {
            services.ConfigureServiceBusOptions();

            services.AddScoped<ServiceBusClient>(provider =>
            {
                var busSettings = provider.GetRequiredService<IOptions<ServiceBusSettings>>().Value;
                var serviceBusClient = new ServiceBusClient(busSettings.ConnectionString);
                return serviceBusClient;
            });

            return services;
        }

        public static IServiceCollection RegisterDelegatedServices(this IServiceCollection services) => services
                .UseAuthorizationContext()
                .UseCache()
                .UseTraceIdProvider();

        public static IServiceCollection SwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Husa.Quicklister.Abor.Api", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton(ConfigureAutoMapper());

            return services;
        }

        public static IMapper ConfigureAutoMapper()
        {
            var config = MapperConfigurationExtensions.Configure
                .AddMapping<ListingsMappingProfile>()
                .Build();

            return config.CreateMapper();
        }

        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString(ApplicationDbContext.ConnectionName),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));
            services.AddScoped<ApplicationQueriesDbContext>();
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }

        public static void MigrateDatabase(this IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            using var quicklisterDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            quicklisterDbContext.Database.Migrate();
        }

        public static IServiceCollection ControllerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationOptions = configuration.GetSection(ApplicationOptions.Section).Get<ApplicationOptions>();
            services.CorsAndControllerConfiguration(configuration, corsAllowedOrigins: applicationOptions.QuicklisterUIUri);
            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, bool withTokenRequest = true)
        {
            services.AddHttpClient<IReverseProspectClient, ReverseProspectClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.ReverseProspect);
            }).AddHeaderPropagation();

            services.AddHttpClient<IServiceSubscriptionClient, ServiceSubscriptionClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                client.BaseAddress = new Uri(options.Services.CompanyServicesManager);
            }).ConfigureHeaderHandling(withTokenRequest);

            services.AddHttpClient<IDownloaderCtxClient, DownloaderCtxClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.Downloader);
            }).ConfigureHeaderHandling(withTokenRequest);

            services.AddHttpClient<IDownloaderSaborClient, DownloaderSaborClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.Downloader);
            }).ConfigureHeaderHandling(withTokenRequest);

            services.AddHttpClient<IPhotoServiceClient, PhotoServiceClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.PhotoService);
            }).AddHeaderPropagation();

            services.AddHttpClient<HusaClient<IMediaServiceClient>>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                client.BaseAddress = new Uri(options.Services.Media);
            }).AddHeaderPropagation();

            services.AddHttpClient<INotesClient, NotesClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.Notes);
            }).AddHeaderPropagation();

            services.AddScoped<IMediaServiceClient, MediaServiceClient>();

            services.AddHttpClient<IDownloaderSaborClient, DownloaderSaborClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.Downloader);
            }).ConfigureHeaderHandling(withTokenRequest);

            services.ConfigureJsonImportClient<ApplicationOptions>(withTokenRequest);
            services.ConfigureXmlClient<ApplicationOptions>();

            return services;
        }

        public static IServiceCollection ConfigureMigrationClient(this IServiceCollection services)
        {
            services
               .AddOptions<MigrationClientOptions>()
               .Configure<IConfiguration>((settings, config) => config.GetSection(MigrationClientOptions.Section).Bind(settings))
               .ValidateDataAnnotations();

            services.AddHttpClient<IMigrationClient, MigrationClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.MigrationService);
            });

            return services;
        }

        public static IServiceCollection AddServiceBusHandlers(this IServiceCollection services)
        {
            services.AddExtensionServiceBusHandlers();
            services.AddSingleton<IDownloaderSubscriber, DownloaderSubscriber>();
            services.AddSingleton<IDownloaderMessagesHandler, DownloaderMessagesHandler>();
            services.AddSingleton<Extensions.Api.ServiceBus.Handlers.IPhotoServiceMessagesHandler, PhotoServiceMessagesHandler>();
            services.AddSingleton<IXmlMessagesHandler, XmlMessagesHandler>();
            services.AddSingleton<IXmlSubscriber, XmlSubscriber>();
            services.AddSingleton<IMigrationMessagesHandler, MigrationMessagesHandler>();
            services.AddSingleton<IMigrationSubscriber, MigrationSubscriber>();

            return services;
        }

        public static IServiceCollection LocalRegisterEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEmail<ApplicationOptions>(configuration);
            return services;
        }

        private static IServiceCollection BindDocumentDbSettings(this IServiceCollection services)
        {
            services
                .AddOptions<DocumentDbSettings>()
                .Configure<IConfiguration>((settings, config) => config.GetSection(DocumentDbSettings.Section).Bind(settings));

            return services;
        }

        private static IServiceCollection BindDownloaderSettings(this IServiceCollection services)
        {
            services.AddOptions<DownloaderUserSettings>()
                .Configure<IConfiguration>((settings, config) => config.GetSection(DownloaderUserSettings.Section).Bind(settings));

            services.AddOptions<XmlUserSettings>()
                .Configure<IConfiguration>((settings, config) => config.GetSection(XmlUserSettings.Section).Bind(settings));

            return services;
        }
    }
}
