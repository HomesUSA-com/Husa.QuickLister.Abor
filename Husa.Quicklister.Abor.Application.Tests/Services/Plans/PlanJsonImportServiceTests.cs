namespace Husa.Quicklister.Abor.Application.Tests.Services.Plans
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class PlanJsonImportServiceTests
    {
        private readonly Mock<IPlanRepository> planSaleRepository = new();
        private readonly Mock<ILogger<PlanJsonImportService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IJsonImportClient> jsonClient = new();
        public PlanJsonImportServiceTests()
        {
            var jsonPlanClientMock = new Mock<IJsonImportPlan>();
            this.jsonClient.SetupGet(x => x.Plan).Returns(jsonPlanClientMock.Object);
            this.Sut = new PlanJsonImportService(
                this.jsonClient.Object,
                this.planSaleRepository.Object,
                this.userContextProvider.Object,
                this.logger.Object);
        }

        public IPlanJsonImportService Sut { get; set; }

        [Fact]
        public async Task ImportPlan_UpdatePlan_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var jsonPlanId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var planSale = TestModelProvider.GetPlanEntity(planId);
            this.jsonClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == jsonPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(jsonPlanId, planId))
                .Verifiable();
            this.planSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync(planSale)
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, jsonPlanId);

            // Assert
            this.planSaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()), Times.Once);
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportPlan_UpdatePlan_NotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var jsonPlanId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            this.jsonClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == jsonPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(jsonPlanId, planId))
                .Verifiable();
            this.planSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync((Plan)null)
                .Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => this.Sut.ImportEntity(companyId, companyName, jsonPlanId));
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task ImportPlan_CreateNewPlan_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var jsonPlanId = Guid.NewGuid();
            this.jsonClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == jsonPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(jsonPlanId))
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, jsonPlanId);

            // Assert
            this.planSaleRepository.Verify(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
            this.planSaleRepository.Verify(r => r.Attach(It.IsAny<Plan>()), Times.Once);
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private static PlanResponse GetPlanResponse(Guid jsonPlanId, Guid? qlId = null) => new()
        {
            Id = jsonPlanId,
            Name = Faker.Company.Name(),
            Stories = 3,
            HalfBaths = 4,
            Bedrooms = 5,
            QuicklisterId = qlId,
        };
    }
}
