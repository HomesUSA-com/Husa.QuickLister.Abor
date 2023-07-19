namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class PlanPhotoServiceTests : PhotoServiceTests<IPlanPhotoService>
    {
        private readonly Mock<IPlanRepository> planRepository;
        private readonly Mock<ILogger<PlanPhotoService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;

        public PlanPhotoServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<PlanPhotoService>>();
            this.planRepository = new Mock<IPlanRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new PlanPhotoService(
                this.fixture.BusOptions.Object,
                this.userContextProvider.Object,
                this.photoServiceClient.Object,
                this.client.Object,
                this.traceIdProvider.Object,
                this.planRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_Success()
        {
            // Arrange & Act
            var entityId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(entityId);
            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(plan)
                .Verifiable();

            await this.SetupAssignLatestPhotoRequest(entityId);

            // Assert
            this.planRepository.Verify(t => t.GetById(entityId, false), Times.Once);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_NotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            Plan plan = null;
            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(plan)
                .Verifiable();

            var photorequestId = Guid.NewGuid();
            var creationDate = DateTime.UtcNow;

            // Act
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => this.Sut.AssignLatestPhotoRequest(entityId, photorequestId, creationDate));
        }

        protected override void SetupValidEntityAndUser(Guid entityId, Guid userId)
        {
            var companyId = Guid.NewGuid();

            var plan = TestModelProvider.GetPlanEntity(entityId, companyId: companyId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
