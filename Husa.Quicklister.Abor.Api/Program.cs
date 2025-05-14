namespace Husa.Quicklister.Abor.Api
{
    using System;
    using Figgle;
    using Husa.Extensions.Api;
    using Husa.Quicklister.Abor.Api.ServiceBus;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Standard.Render("QL ABOR STARTED!"));
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

            try
            {
                Log.Information("QL Api starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((host, configuration) =>
                {
                    configuration
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    if (host.HostingEnvironment.IsDevelopment())
                    {
                        configuration.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        configuration.AddUserSecrets<Startup>();
                    }

                    configuration.AddEnvironmentVariables();

                    if (!host.HostingEnvironment.IsDevelopment())
                    {
                        configuration.AddAppConfiguration(host, "QuicklisterAbor:Settings");
                    }
                })
                .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureServices((host, services) =>
                {
                    var featureFlags = host.Configuration.GetSection("Application:FeatureFlags").Get<FeatureFlags>();
                    if (featureFlags.EnableBusHandlers)
                    {
                        services.AddHostedService<Worker>();
                    }
                });
    }
}
