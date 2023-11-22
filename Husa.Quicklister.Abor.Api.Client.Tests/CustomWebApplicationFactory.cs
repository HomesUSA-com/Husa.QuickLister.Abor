namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using Husa.Extensions.Api.Configuration;
    using Husa.Quicklister.Abor.Api.Client.Tests.Authentication;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data;
    using Husa.Quicklister.Abor.Data.Queries;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly MockAuthenticationUser mockedUser = new(
            new Claim(type: "username", MockAuthenticationUser.Username),
            new Claim(ClaimTypes.NameIdentifier, MockAuthenticationUser.UserId.ToString()),
            new Claim(ClaimTypes.Role, Roles.MLSAdministrator));

        private HttpClient client;

        public HttpClient GetClient()
        {
            if (this.client == null)
            {
                // Default client option values are shown
                var clientOptions = new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                };
                this.client = this.CreateClient(clientOptions);
            }

            return this.client;
        }

        public T GetService<T>() => this.Services.GetRequiredService<T>();

        protected override IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((host, configuration) =>
            {
                configuration.Sources.Clear();

                configuration
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<TStartup>().UseTestServer());

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    services.AddTestAuthentication();
                    services.AddScoped(_ => this.mockedUser);
                    ConfigureTestDatabase(services);
                });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.client.Dispose();
        }

        private static void ConfigureTestDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase(databaseName: "HusaQuicklisterAbor", TestBootstrapper.InMemoryDatabaseRoot)
                .EnableSensitiveDataLogging());

            services.AddScoped<ApplicationQueriesDbContext>();
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                try
                {
                    dbContext.Database.EnsureCreated();
                    FillDatabase(dbContext);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test messages");
                    throw;
                }
            }
        }

        private static void FillDatabase(ApplicationDbContext dbContext)
        {
            var community = CommunityTestProvider.GetCommunityEntity(Factory.CommunityId, companyId: Factory.CompanyId);
            community.ProfileInfo.Name = Factory.CommunityName;

            dbContext.Community.Add(community);

            var communityAwaitingApproval = CommunityTestProvider.GetCommunityEntity(Factory.CommunityAwaitingApprovalId, companyId: Factory.CompanyId);
            communityAwaitingApproval.XmlStatus = XmlStatus.AwaitingApproval;
            dbContext.Community.Add(communityAwaitingApproval);

            var plan = PlanTestProvider.GetPlanEntity(Factory.PlanId, companyId: Factory.CompanyId);
            plan.BasePlan.Name = Factory.PlanName;
            dbContext.Plan.Add(plan);

            var planAwaitingApproval = PlanTestProvider.GetPlanEntity(Factory.PlanAwaitingApprovalId, companyId: Factory.CompanyId);
            planAwaitingApproval.XmlStatus = XmlStatus.AwaitingApproval;
            dbContext.Plan.Add(planAwaitingApproval);

            dbContext.SaveChanges();

            foreach (MarketStatuses status in Enum.GetValues(typeof(MarketStatuses)))
            {
                var listing = ListingTestProvider.GetListingEntity(companyId: Factory.CompanyId, communityId: Factory.CommunityId, planId: Factory.PlanId, marketStatuses: status);
                listing.SaleProperty.Plan = null;
                if (status == MarketStatuses.Closed)
                {
                    listing.SaleProperty.PropertyInfo.ConstructionStage = ConstructionStage.Complete;
                }

                dbContext.ListingSale.Add(listing);
            }

            var listingForXml = ListingTestProvider.GetListingEntity(listingId: Factory.ListingIdForXml, companyId: Factory.CompanyId, communityId: Factory.CommunityId, marketStatuses: MarketStatuses.Active);
            listingForXml.SaleProperty.AddressInfo.StreetName = "streetName";
            listingForXml.SaleProperty.AddressInfo.StreetNumber = "1234";
            listingForXml.SaleProperty.AddressInfo.ZipCode = "12345";
            listingForXml.SaleProperty.AddressInfo.City = Cities.LagoVista;
            dbContext.ListingSale.Add(listingForXml);

            var listingWithPhotoRequest = ListingTestProvider.GetListingEntity(listingId: Factory.ListingId, companyId: Factory.CompanyId, communityId: Factory.CommunityId, planId: Factory.PlanId, marketStatuses: MarketStatuses.Active);
            listingWithPhotoRequest.SaleProperty.Plan = null;
            listingWithPhotoRequest.SaleProperty.PropertyInfo.ConstructionCompletionDate = DateTime.UtcNow.AddDays(-1);
            listingWithPhotoRequest.MlsNumber = "123456";
            dbContext.ListingSale.Add(listingWithPhotoRequest);

            var listingWithInadequateRemarks = ListingTestProvider.GetListingEntity(listingId: Factory.ListingIdWithInadequateRemarks, companyId: Factory.CompanyId, communityId: Factory.CommunityId, planId: Factory.PlanId, marketStatuses: MarketStatuses.Active);
            listingWithInadequateRemarks.SaleProperty.Plan = null;
            listingWithInadequateRemarks.SaleProperty.PropertyInfo.ConstructionCompletionDate = DateTime.UtcNow.AddDays(-1);
            listingWithInadequateRemarks.MlsNumber = "123456";
            listingWithInadequateRemarks.SaleProperty.FeaturesInfo.PropertyDescription = "Property less than 100 characters";
            listingWithInadequateRemarks.LastPhotoRequestId = Guid.NewGuid();
            listingWithInadequateRemarks.LastPhotoRequestCreationDate = DateTime.UtcNow;
            dbContext.ListingSale.Add(listingWithInadequateRemarks);
            dbContext.SaveChanges();

            var listingWithOpenHouse = ListingTestProvider.GetListingEntity(listingId: Factory.ListingWithOpenHouse, companyId: Factory.CompanyId, communityId: Factory.CommunityId, planId: Factory.PlanId, marketStatuses: MarketStatuses.Active);
            listingWithOpenHouse.SaleProperty.Plan = null;
            listingWithOpenHouse.MlsNumber = "123456";
            listingWithOpenHouse.SaleProperty.OpenHouses = new List<SaleListingOpenHouse>()
            {
                new SaleListingOpenHouse(
                    Guid.NewGuid(),
                    Quicklister.Extensions.Domain.Enums.OpenHouseType.Monday,
                    startTime: TimeSpan.FromHours(2),
                    endTime: TimeSpan.FromHours(1),
                    refreshments: new List<Refreshments> { Refreshments.Beverages, Refreshments.Snacks }),
            };

            dbContext.ListingSale.Add(listingWithOpenHouse);
            dbContext.SaveChanges();

            var trackingReverseProspect = TrackingReverseProspectProvider.GetReverseProspectEntity(
                Factory.ListingId,
                Factory.UserId,
                Factory.CompanyId,
                "\r\nMLS#\r\nAddress\r\nAgentName\r\nEmail\r\nAgentID\r\nClientPublicID\r\nSearchID\r\nLastEmailed\r\nClientInterest\r\nInitiallyRanked\r\n\r\n11560730\r\n12232  Buckaroo Ranch\r\nLovetta McAlpin\r\n\r\n598390\r\nLCEJN\r\n\r\n10/26/2021\r\nInterested\r\n",
                ReverseProspectStatus.Available);
            dbContext.ReverseProspect.Add(trackingReverseProspect);
            dbContext.SaveChanges();

            var scrapedListing = ScrapedListingTestProvider.GetScrapedListingEntity(builderName: Factory.BuilderName);
            dbContext.ScrapedListing.Add(scrapedListing);
        }
    }
}
