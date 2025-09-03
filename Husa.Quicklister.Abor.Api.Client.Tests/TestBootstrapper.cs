namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Request;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Document.Interfaces;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Extensions.OpenAI;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.MediaService.Client;
    using Husa.Notes.Client;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Api.Client.Tests.Authentication;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Interfaces.Media;
    using Husa.ReverseProspect.Api.Client;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Request;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Azure.Cosmos;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using CompanyUser = Husa.CompanyServicesManager.Api.Contracts.Response.User;
    using ServiceSubscriptionResponse = Husa.CompanyServicesManager.Api.Contracts.Response.ServiceSubscription;

    public static class TestBootstrapper
    {
        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new();

        public static IServiceCollection RegisterMockedClients(this IServiceCollection services)
        {
            services.AddSingleton(new Mock<IReverseProspectClient>().Object);
            services.MockServiceSubscriptionClient();
            services.MockXmlClient();
            services.AddSingleton(new Mock<IDownloaderCtxClient>().Object);
            services.AddSingleton(new Mock<IJsonImportClient>().Object);
            services.AddSingleton(new Mock<IPhotoServiceClient>().Object);
            services.AddSingleton(new Mock<IMediaServiceClient>().Object);
            services.AddSingleton(new Mock<IBlobService>().Object);
            services.AddSingleton(new Mock<INotesClient>().Object);
            services.AddSingleton(new Mock<IEmailService>().Object);
            services.AddSingleton(new Mock<IMediaService>().Object);
            services.AddSingleton(new Mock<IOpenAIClient>().Object);
            services.AddSingleton(new Mock<ISaleListingRequestQueriesRepository>().Object);
            services.MockCosmosClient();
            services.AddScoped<ServiceBusClient>(provider =>
            {
                var serviceBusClient = new Mock<ServiceBusClient>();
                serviceBusClient.SetupAllProperties();
                return serviceBusClient.Object;
            });
            services.AddSingleton(UserContextProviderMock.SetupUserContext().Object);

            return services;
        }

        private static void MockServiceSubscriptionClient(this IServiceCollection services)
        {
            var serviceSubscription = new Mock<IServiceSubscriptionClient>();
            var companyMock = new Mock<ICompany>();
            companyMock.SetupAllProperties();
            var companyDetail = TestModelProvider.GetCompanyDetail(Factory.CompanyId);
            companyMock.Setup(s => s.GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(companyDetail);

            var companyServices = Array.Empty<ServiceSubscriptionResponse>();
            companyMock
                .Setup(c => c.GetCompanyServices(It.IsAny<Guid>(), It.IsAny<FilterServiceSubscriptionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<ServiceSubscriptionResponse>(companyServices, companyServices.Length));
            companyMock.Setup(s => s.GetAsync(It.IsAny<CompanyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Husa.CompanyServicesManager.Api.Contracts.Response.Company>([new() { Id = Factory.CompanyId }], 1));
            serviceSubscription.SetupGet(s => s.Company).Returns(companyMock.Object);
            serviceSubscription.SetupGet(s => s.Corporation).Returns(new Mock<ICorporation>().Object);

            var userMock = new Mock<IUser>();
            userMock.SetupAllProperties();
            var responseUser = new CompanyUser
            {
                Id = MockAuthenticationUser.UserId,
                UserName = MockAuthenticationUser.Username,
            };

            userMock
                .Setup(s => s.GetAsync(It.IsAny<UserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<CompanyUser>(new[] { responseUser }, total: 1));

            serviceSubscription.SetupGet(s => s.User).Returns(userMock.Object);
            serviceSubscription.SetupGet(s => s.Employee).Returns(new Mock<IEmployee>().Object);
            services.AddSingleton(serviceSubscription.Object);
        }

        private static void MockCosmosClient(this IServiceCollection services)
        {
            var cosmosClient = new Mock<CosmosClient>();
            var container = new Mock<Container>();
            var mockCosmosLinqQuery = new Mock<ICosmosLinqQuery>();
            var feedIterator = new Mock<FeedIterator<SaleListingRequest>>();
            mockCosmosLinqQuery.Setup(x => x.GetFeedIterator(It.IsAny<IQueryable<SaleListingRequest>>())).Returns(feedIterator.Object);

            container.Setup(x => x.GetItemLinqQueryable<SaleListingRequest>(It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>(), It.IsAny<CosmosLinqSerializerOptions>()))
                                    .Returns(Enumerable.Empty<SaleListingRequest>().AsQueryable().OrderBy(x => 0));
            cosmosClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(container.Object);
            services.AddSingleton(cosmosClient.Object);
            services.AddSingleton(mockCosmosLinqQuery.Object);
        }

        private static void MockXmlClient(this IServiceCollection services)
        {
            var xmlListingClientMock = new Mock<IXmlListing>();
            var xmlListings = new List<XmlListingResponse>()
                {
                    new XmlListingResponse(),
                };
            xmlListingClientMock.SetupAllProperties();
            xmlListingClientMock.Setup(s => s.GetAsync(It.IsAny<ListingRequestFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<XmlListingResponse>(xmlListings, xmlListings.Count));

            var xmlPlanClientMock = new Mock<IXmlPlan>();
            xmlPlanClientMock.SetupAllProperties();
            var xmlSubdivisionClientMock = new Mock<IXmlSubdivision>();
            xmlSubdivisionClientMock.SetupAllProperties();

            var xmlClientMock = new Mock<IXmlClient>();
            xmlClientMock.SetupGet(s => s.Listing).Returns(xmlListingClientMock.Object);
            xmlClientMock.SetupGet(s => s.Plan).Returns(xmlPlanClientMock.Object);
            xmlClientMock.SetupGet(s => s.Subdivision).Returns(xmlSubdivisionClientMock.Object);
            services.AddSingleton(xmlClientMock.Object);
        }
    }
}
