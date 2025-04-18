namespace Husa.Quicklister.Abor.Api
{
    using Husa.Extensions.Api;
    using Husa.Extensions.Api.Client;
    using Husa.Extensions.Api.Cors;
    using Husa.Extensions.Api.Middleware;
    using Husa.Extensions.EmailNotification;
    using Husa.Extensions.Logger.Enrichers;
    using Husa.Extensions.Media.Extensions;
    using Husa.Extensions.OpenAI.Configuration;
    using Husa.Extensions.Quickbooks.Extensions;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Abor.Api.Middlewares;
    using Husa.Quicklister.Extensions.Application.Configuration;
    using Husa.Quicklister.Extensions.Data.Documents.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .BindOptions()
                .AddServiceBusOptions()
                .BindIdentitySettings()
                .ConfigureOpenAiApiClient();

            services.AddLocalization();
            services.AddApplicationInsightsTelemetry();

            services.AddApplicationServices()
                .AddServiceBusHandlers()
                .RegisterDelegatedServices();

            services
                .ConfigureDatabase(this.Configuration)
                .ConfigureAzureBlobConnection(this.Configuration)
                .ConfigureHttpClients()
                .ConfigureMigrationClient();

            services
                .AddCommandRepositories()
                .AddQueriesRepositories()
                .AddCommonRepositories()
                .AddCosmosClientAndService()
                .AddCosmosRepositories();

            services
                .ConfigureRefitClient()
                .ConfigureInvoiceClient(this.Configuration);

            services
                .RegisterAutoMapper()
                .EmailNotificationRegister(this.Configuration);

            services.ControllerConfiguration(this.Configuration);

            services.AddHttpContextAccessor();
            services.AddHeaderPropagation(options => options.Headers.Add(CorrelationIdHeaderEnricher.HeaderKey));
            services.SwaggerConfiguration();
            services.ConfigureApiClients(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Husa.Quicklister.Abor.Api v1"));
            }

            app.UseMiddleware<ProductVersionHeaderMiddleware>();
            app.UseMiddleware<RequestCorrelationIdMiddleware>();
            app.UseHeaderPropagation();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureCors();
            app.ConfigureCultureAndLocalization();

            app.MigrateDatabase();
        }
    }
}
