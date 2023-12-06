namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Extensions.Application.Interfaces;
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
