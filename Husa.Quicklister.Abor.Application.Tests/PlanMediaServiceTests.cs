namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Media.Interfaces;
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

    public class PlanMediaServiceTests : MediaServiceTests<IPlanMediaService>
    {
        private readonly Mock<IPlanRepository> planRepository;
        private readonly Mock<ILogger<PlanMediaService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;
        private readonly Mock<ICache> cache = new();
        private readonly Mock<IBlobService> blobService = new();

        public PlanMediaServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<PlanMediaService>>();
            this.planRepository = new Mock<IPlanRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new PlanMediaService(
                this.busOptions.Object,
                this.userContextProvider.Object,
                this.mediaServiceClient.Object,
                this.busClient.Object,
                this.traceIdProvider.Object,
                this.planRepository.Object,
                this.blobService.Object,
                this.cache.Object,
                this.logger.Object,
                fixture.Mapper);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(planId, userId);

            // Act
            await this.Sut.ValidateEntityAndUserCompany(planId);

            // Assert
            this.planRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_EntityNotFound_Error()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            Plan plan = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => this.Sut.ValidateEntityAndUserCompany(planId));
            this.planRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Never);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_IsNotCompanyEmployee_Error()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var planCompanyId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(planId, createStub: false, planCompanyId);

            var userId = Guid.NewGuid();
            var userCompanyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, userCompanyId);

            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ValidateEntityAndUserCompany(planId));
            this.planRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        protected override void SetupValidEntityAndUser(Guid entityId, Guid userId)
        {
            var companyId = Guid.NewGuid();

            var plan = TestModelProvider.GetPlanEntity(entityId, createStub: false, companyId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.planRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
