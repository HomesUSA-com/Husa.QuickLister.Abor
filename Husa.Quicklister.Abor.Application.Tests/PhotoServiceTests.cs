namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Common.Exceptions;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.PhotoRequest;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public abstract class PhotoServiceTests<TPhotoService>
        where TPhotoService : IPhotoService
    {
        protected readonly Mock<IPhotoServiceClient> photoServiceClient = new();
        protected readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        protected readonly Mock<ServiceBusClient> client = new();
        protected readonly Mock<ServiceBusSender> sender = new();
        protected readonly Mock<IProvideTraceId> traceIdProvider = new();
        protected readonly ApplicationServicesFixture fixture;

        protected PhotoServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

            this.traceIdProvider = new Mock<IProvideTraceId>();
            this.traceIdProvider.Setup(u => u.TraceId).Returns(Guid.NewGuid().ToString());

            var serviceBusMessageBatch = ServiceBusModelFactory.ServiceBusMessageBatch(1, new List<ServiceBusMessage>());
            this.sender.Setup(u => u.CreateMessageBatchAsync(It.IsAny<CancellationToken>())).ReturnsAsync(serviceBusMessageBatch);
            this.client.Setup(u => u.CreateSender(It.IsAny<string>())).Returns(this.sender.Object);
        }

        public TPhotoService Sut { get; set; }

        [Fact]
        public async Task CreateAsyc_MessageSent_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var photoRequest = TestModelProvider.GetPhotoRequest(this.Sut.PropertyType);
            this.SetupValidEntityAndUser(entityId, userId);
            var company = TestModelProvider.GetCompanyDetail(photoRequest.CompanyId);
            company.PhotographyServiceInfo = new()
            {
                PhotographyServicesEnabled = true,
            };

            this.serviceSubscriptionClient
                .Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            // Act
            await this.Sut.CreateAsync(entityId, photoRequest);

            // Assert
            this.client.Verify(t => t.CreateSender(ApplicationServicesFixture.TestTopicName), Times.Once);
            this.sender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsyc_MessageSent_Error()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var photoRequest = TestModelProvider.GetPhotoRequest(this.Sut.PropertyType);
            this.SetupValidEntityAndUser(entityId, userId);
            var company = TestModelProvider.GetCompanyDetail(photoRequest.CompanyId);
            company.PhotographyServiceInfo = new()
            {
                PhotographyServicesEnabled = false,
            };

            this.serviceSubscriptionClient
                .Setup(x => x.Company.GetCompany(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.CreateAsync(entityId, photoRequest));
            this.sender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteById_MessageSent_Success()
        {
            var photoRequestId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            this.SetupValidEntityAndUser(entityId, userId);
            await this.Sut.DeleteByIdAsync(entityId, photoRequestId);

            this.client.Verify(t => t.CreateSender(ApplicationServicesFixture.TestTopicName), Times.Once);
            this.sender.Verify(t => t.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        protected async Task SetupAssignLatestPhotoRequest(Guid entityId)
        {
            var photorequestId = Guid.NewGuid();
            var creationDate = DateTime.UtcNow;

            // Act
            await this.Sut.AssignLatestPhotoRequest(entityId, photorequestId, creationDate);
        }

        protected abstract void SetupValidEntityAndUser(Guid entityId, Guid userId);
    }
}
