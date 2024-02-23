namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using Husa.Extensions.Api;
    using Husa.Extensions.Api.Cors;
    using Husa.Extensions.Logger.Enrichers;
    using Husa.Quicklister.Abor.Api.Configuration;
    using Husa.Quicklister.Extensions.Data.Documents.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .BindOptions()
                .ConfigureServiceBusOptions();

            services.AddLocalization();

            services
                .AddCommandRepositories()
                .AddQueriesRepositories()
                .AddCommonRepositories()
                .AddListingRequestsRepositories()
                .AddCosmosRepositories();

            services
                .ControllerConfiguration(this.Configuration)
                .RegisterAutoMapper();

            services
                .RegisterDelegatedServices()
                .AddApplicationServices()
                .RegisterMockedClients();

            services.AddHttpContextAccessor();
            services.AddHeaderPropagation(options => options.Headers.Add(CorrelationIdHeaderEnricher.HeaderKey));
            services.ConfigureApiClients(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHeaderPropagation();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureCultureAndLocalization();
            app.ConfigureCors();
        }
    }
}
