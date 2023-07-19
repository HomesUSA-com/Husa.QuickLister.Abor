namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class PlanServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IPlanRepository> planRepository;
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient;
        private readonly Mock<ILogger<PlanService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;

        public PlanServiceTests(ApplicationServicesFixture fixture)
        {
            this.logger = new Mock<ILogger<PlanService>>();
            this.planRepository = new Mock<IPlanRepository>();
            this.serviceSubscriptionClient = new Mock<IServiceSubscriptionClient>();
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.userContextProvider = new Mock<IUserContextProvider>();
        }

        [Fact]
        public async Task CreatePlanAsync_CreateComplete_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var planDto = TestModelProvider.GetPlanCreateDto(companyId);
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);

            this.planRepository
                .Setup(c => c.Attach(It.IsAny<Plan>()))
                .Callback<Plan>(entity => entity.Id = planId)
                .Verifiable();

            this.planRepository
                .Setup(c => c.SaveChangesAsync())
                .Verifiable();

            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);
            var sut = this.GetSut();

            // Act
            var result = await sut.CreateAsync(planDto);

            // Act and Assert
            this.planRepository.Verify();
            Assert.IsType<Guid>(result);
            Assert.Equal(planId, result);
        }

        [Fact]
        public async Task DeletePlan_DeleteComplete_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(planId, true);
            var deleteInCascade = false;

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            var sut = this.GetSut();

            // Act
            await sut.DeletePlan(planId, deleteInCascade);

            // Assert
            this.planRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Plan>()), Times.Once);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task DeletePlan_DeleteFail_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            Plan plan = null;
            var deleteInCascade = false;

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            var sut = this.GetSut();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => sut.DeletePlan(planId, deleteInCascade));
            this.planRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Plan>()), Times.Never);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UpdatePlan_UpdateComplete_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(planId);
            var planDto = TestModelProvider.GetPlanDto();

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            var sut = this.GetSut();

            // Act
            await sut.UpdatePlanAsync(planId, planDto);

            // Assert
            this.planRepository.Verify(r => r.UpdateAsync(It.IsAny<Plan>()), Times.Once);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UpdatePlan_UpdateFail_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            Plan plan = null;
            var planDto = TestModelProvider.GetPlanDto();

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();
            var sut = this.GetSut();

            // Act andAssert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => sut.UpdatePlanAsync(planId, planDto));
            this.planRepository.Verify(r => r.SaveChangesAsync(It.IsAny<Plan>()), Times.Never);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        private PlanService GetSut() => new(
                this.planRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.userContextProvider.Object,
                this.fixture.Mapper,
                this.logger.Object);
    }
}
