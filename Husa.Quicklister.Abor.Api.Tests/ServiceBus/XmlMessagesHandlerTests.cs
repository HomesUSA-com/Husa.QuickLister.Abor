namespace Husa.Quicklister.Abor.Api.Tests.ServiceBus
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Api.ServiceBus.Handlers;
    using Husa.Quicklister.Abor.Api.ServiceBus.Subscribers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Husa.Xml.ServiceBus.Messages;
    using Husa.Xml.ServiceBus.Messages.Messages;
    using Microsoft.AspNetCore.HeaderPropagation;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class XmlMessagesHandlerTests
    {
        private readonly Mock<IXmlSubscriber> subscriberMock = new();
        private readonly Mock<ISubscriptionClient> subscriptionClientMock = new();
        private readonly Mock<ICommunityXmlService> communityXmlServiceMock = new();
        private readonly Mock<IPlanXmlService> planXmlServiceMock = new();
        private readonly Mock<ISaleListingXmlService> saleListingXmlService = new();
        private readonly Mock<IServiceScopeFactory> serviceScopeFactoryMock = new();
        private readonly Mock<ILogger<XmlMessagesHandler>> loggerMock = new();
        private readonly ApplicationServicesFixture fixture;

        public XmlMessagesHandlerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task HandleMessage_ImportSubdivisionMessage()
        {
            // Arrange
            var subdivisionMessage = new ImportSubdivisionMessage
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                CompanyName = Faker.Company.Name(),
                MarketCode = MarketCode.Austin,
            };
            var message = this.ArrangeMessage(subdivisionMessage);
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.communityXmlServiceMock.Verify(
                ds => ds.ImportEntity(
                    It.Is<Guid>(id => id == subdivisionMessage.CompanyId),
                    It.Is<string>(id => id == subdivisionMessage.CompanyName),
                    It.Is<Guid>(id => id == subdivisionMessage.Id)),
                Times.Once);
            this.planXmlServiceMock.Verify(
                ds => ds.ImportEntity(
                    It.Is<Guid>(id => id == subdivisionMessage.CompanyId),
                    It.Is<string>(id => id == subdivisionMessage.CompanyName),
                    It.Is<Guid>(id => id == subdivisionMessage.Id)),
                Times.Never);
        }

        [Fact]
        public async Task HandleMessage_ImportSubdivisionMessage_WrongMarket()
        {
            // Arrange
            var subdivisionMessage = new ImportSubdivisionMessage
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                CompanyName = Faker.Company.Name(),
                MarketCode = MarketCode.DFW,
            };
            var message = this.ArrangeMessage(subdivisionMessage);
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.communityXmlServiceMock.Verify(ds => ds.ImportEntity(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task HandleMessage_ImportPlanMessage()
        {
            // Arrange
            var planMessage = new ImportPlanMessage
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                CompanyName = Faker.Company.Name(),
                MarketCode = MarketCode.Austin,
            };
            var message = this.ArrangeMessage(planMessage);
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.planXmlServiceMock.Verify(
                ds => ds.ImportEntity(
                    It.Is<Guid>(id => id == planMessage.CompanyId),
                    It.Is<string>(id => id == planMessage.CompanyName),
                    It.Is<Guid>(id => id == planMessage.Id)),
                Times.Once);
            this.communityXmlServiceMock.Verify(
                ds => ds.ImportEntity(
                    It.Is<Guid>(id => id == planMessage.CompanyId),
                    It.Is<string>(id => id == planMessage.CompanyName),
                    It.Is<Guid>(id => id == planMessage.Id)),
                Times.Never);
        }

        [Fact]
        public async Task HandleMessage_UpdateXmlListingMessage()
        {
            // Arrange
            var importListingMessage = new ImportListingMessage
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                CompanyName = Faker.Company.Name(),
                MarketCode = MarketCode.Austin,
            };
            var message = this.ArrangeMessage(importListingMessage);
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.saleListingXmlService.Verify(
                x => x.UpdateListingFromXmlAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task HandleMessage_AutoMatchListingMessage()
        {
            // Arrange
            var autoMatchListingMessage = new AutoMatchListingMessage
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                CompanyName = Faker.Company.Name(),
                MarketCode = MarketCode.Austin,
            };
            var message = this.ArrangeMessage(autoMatchListingMessage);
            var sut = this.GetSut();

            // Act
            await sut.HandleMessage(message, cancellationToken: default);

            // Assert
            this.saleListingXmlService.Verify(
                x => x.AutoMatchListingFromXmlAsync(It.IsAny<Guid>()), Times.Once);
        }

        private Message ArrangeMessage<TMessage>(TMessage profileMessage)
            where TMessage : ImportProfileMessage
        {
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);

            var message = ApplicationServicesFixture.BuildBusMessage(profileMessage);
            this.subscriberMock
                .SetupGet(s => s.Client)
                .Returns(this.subscriptionClientMock.Object);
            this.ConfigureServiceProvider();

            return message;
        }

        private void ConfigureServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            var userProvider = new Mock<IUserProvider>();
            serviceCollection.AddSingleton(this.communityXmlServiceMock.Object);
            serviceCollection.AddSingleton(this.planXmlServiceMock.Object);
            serviceCollection.AddSingleton(this.saleListingXmlService.Object);
            serviceCollection.AddSingleton(userProvider.Object);
            serviceCollection.AddSingleton(new HeaderPropagationValues());

            var configureTraceId = new Mock<IConfigureTraceId>();
            serviceCollection.AddSingleton(configureTraceId.Object);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.SetupGet(sc => sc.ServiceProvider).Returns(serviceCollection.BuildServiceProvider());

            this.serviceScopeFactoryMock
                .Setup(sf => sf.CreateScope())
                .Returns(serviceScope.Object);
        }

        private XmlMessagesHandler GetSut() => new(
            this.subscriberMock.Object,
            this.serviceScopeFactoryMock.Object,
            this.fixture.XmlUserSettings.Object,
            this.fixture.Options.Object,
            this.loggerMock.Object);
    }
}
