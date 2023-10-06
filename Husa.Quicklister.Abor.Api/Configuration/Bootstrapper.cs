namespace Husa.Quicklister.Abor.Api.Configuration
{
    using System;
    using AutoMapper;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Api;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Api.Cors;
    using Husa.Extensions.Api.Mvc;
    using Husa.Extensions.Authorization.Extensions;
    using Husa.Extensions.Cache.Extensions;
    using Husa.Extensions.Common;
    using Husa.Extensions.Common.Exceptions.Filters;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.Media.Services;
    using Husa.MediaService.Client;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Client.Options;
    using Husa.Notes.Client;
    using Husa.PhotoService.Api.Client;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Filters;
    using Husa.Quicklister.Abor.Api.Mappings;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.ValidationsRules;
    using Husa.Quicklister.Abor.Application;
    using Husa.Quicklister.Abor.Application.Interfaces.Agent;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Notes;
    using Husa.Quicklister.Abor.Application.Interfaces.Office;
    using Husa.Quicklister.Abor.Application.Interfaces.OpenHouse;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Application.Media;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Application.Services.ListingRequests;
    using Husa.Quicklister.Abor.Application.Services.Notes;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Data;
    using Husa.Quicklister.Abor.Data.Commands.Repositories;
    using Husa.Quicklister.Abor.Data.Queries;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Migration;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Husa.ReverseProspect.Api.Client;
    using Husa.Xml.Api.Client;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;

    public static class Bootstrapper
    {
        public static IServiceCollection BindOptions(this IServiceCollection services)
        {
            services
                .BindApplicationSettings()
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

            return services;
        }

        public static IServiceCollection AddCommonRepositories(this IServiceCollection services) => services.AddScoped<IUserRepository, UserRepository>();

        public static IServiceCollection AddQueriesRepositories(this IServiceCollection services)
        {
            services.AddScoped<IListingSaleQueriesRepository, ListingSaleQueriesRepository>();
            services.AddScoped<ICommunityQueriesRepository, CommunityQueriesRepository>();
            services.AddScoped<IPlanQueriesRepository, PlanQueriesRepository>();
            services.AddScoped<IQueryMediaRepository, QueryMediaRepository>();
            services.AddScoped<IAgentQueriesRepository, AgentQueriesRepository>();
            services.AddScoped<IAlertQueriesRepository, AlertQueriesRepository>();
            services.AddScoped<IScrapedListingQueriesRepository, ScrapedListingQueriesRepository>();
            services.AddScoped<IQueryXmlRepository, QueryXmlRepository>();
            services.AddScoped<IManagementTraceQueriesRepository, ManagementTraceQueriesRepository>();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidateListingStatusChanges<ListingSaleRequestForUpdate>, ListingSaleRequestWithStatusChangeValidator>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IDownloaderService, DownloaderService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IOpenHouseService, OpenHouseService>();

            services.AddScoped<ISaleListingService, SaleListingService>();
            services.AddScoped<ISaleListingNotesService, SaleListingNotesService>();
            services.AddScoped<ISaleListingPhotoService, SaleListingPhotoService>();
            services.AddScoped<ISaleListingXmlService, SaleListingXmlService>();
            services.AddScoped<ISaleListingMediaService, SaleListingMediaService>();

            services.AddScoped<ISaleListingRequestService, SaleListingRequestService>();
            services.AddScoped<IListingRequestMediaService, ListingRequestMediaService>();
            services.AddScoped<IListingRequestMigrationService, ListingRequestMigrationService>();

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPlanPhotoService, PlanPhotoService>();
            services.AddScoped<IPlanXmlService, PlanXmlService>();
            services.AddScoped<IPlanMigrationService, PlanMigrationService>();
            services.AddScoped<IPlanMediaService, PlanMediaService>();
            services.AddScoped<IPlanNotesService, PlanNotesService>();

            services.AddScoped<ISaleCommunityService, SaleCommunityService>();
            services.AddScoped<ICommunityPhotoService, CommunityPhotoService>();
            services.AddScoped<ICommunityXmlService, CommunityXmlService>();
            services.AddScoped<ICommunityMigrationService, CommunityMigrationService>();
            services.AddScoped<ICommunityMediaService, CommunityMediaService>();
            services.AddScoped<ICommunityNotesService, CommunityNotesService>();

            services.AddScoped<IUploaderService, UploaderService>();
            services.AddScoped<INotesBusService, NotesBusService>();
            services.AddScoped<IXmlMediaService, XmlMediaService>();
            services.AddScoped<IMediaMessagingService, MediaMessagingService>();
            services.AddScoped<IBlobService, BlobService>();

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

        public static IServiceCollection AddMvcOptions(this IServiceCollection services)
        {
            services.AddScoped<ExceptionLoggingFilterAttribute>();
            services.AddScoped<QuicklisterAuthorizationFilter>();

            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ExceptionLoggingFilterAttribute>();
                    options.Filters.Add<QuicklisterAuthorizationFilter>();
                });
            return services;
        }

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

            services
                .RegisterCors(new[] { applicationOptions.QuicklisterUIUri })
                .AddMvcOptions()
                .AddControllersWithViews(options => options.AddControllerPrefixConventions("api"))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions
                        .SetConfiguration()
                        .Converters.Add(new TimeSpanToStringConverter());
                });

            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, bool withTokenRequest = true)
        {
            services.AddHttpClient<IReverseProspectClient, ReverseProspectClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.ReverseProspect);
            }).AddHeaderPropagation();

            services.AddHttpClient<IServiceSubscriptionClient, ServiceSubscriptionClient>(async (provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                await client.ConfigureClientAsync(provider, options.Services.CompanyServicesManager);
            }).ConfigureHeaderHandling(withTokenRequest);

            services.AddHttpClient<IDownloaderCtxClient, DownloaderCtxClient>(async (provider, client) =>
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

            return services;
        }

        public static IServiceCollection ConfigureXmlClient(this IServiceCollection services, bool withTokenRequest = true)
        {
            services
                .AddHttpClient<IXmlClient, XmlClient>(async (provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                    await client.ConfigureClientAsync(provider, options.Services.XmlService);
                })
                .ConfigureHeaderHandling(withTokenRequest);

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
            services.AddSingleton<IDownloaderSubscriber, DownloaderSubscriber>();
            services.AddSingleton<IDownloaderMessagesHandler, DownloaderMessagesHandler>();
            services.AddSingleton<IPhotoServiceMessagesHandler, PhotoServiceMessagesHandler>();
            services.AddSingleton<IPhotoServiceSubscriber, PhotoServiceSubscriber>();
            services.AddSingleton<IXmlMessagesHandler, XmlMessagesHandler>();
            services.AddSingleton<IXmlSubscriber, XmlSubscriber>();

            return services;
        }

        public static void ConfigureLocalization(this IApplicationBuilder app)
        {
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = new[] { ApplicationOptions.ApplicationCultureInfo },
                SupportedUICultures = new[] { ApplicationOptions.ApplicationCultureInfo },
                DefaultRequestCulture = new RequestCulture(ApplicationOptions.ApplicationCultureInfo),
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null,
            };

            app.UseRequestLocalization(localizationOptions);
        }

        private static IServiceCollection BindApplicationSettings(this IServiceCollection services)
        {
            services
                .AddOptions<ApplicationOptions>()
                .Configure<IConfiguration>((settings, config) => config.GetSection(ApplicationOptions.Section).Bind(settings));

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

        private static IServiceCollection UseTraceIdProvider(this IServiceCollection services)
        {
            return services
                   .AddScoped<TraceIdProvider>()
                   .AddScoped<IProvideTraceId>(x => x.GetRequiredService<TraceIdProvider>())
                   .AddScoped<IConfigureTraceId>(x => x.GetRequiredService<TraceIdProvider>());
        }
    }
}
