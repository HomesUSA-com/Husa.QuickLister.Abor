namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Husa.Xml.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class PlanXmlServiceTests
    {
        private readonly Mock<IPlanRepository> planSaleRepository = new();
        private readonly Mock<ILogger<PlanXmlService>> logger = new();
        private readonly Mock<IXmlClient> xmlClient = new();

        public PlanXmlServiceTests()
        {
            var xmlPlanClientMock = new Mock<IXmlPlan>();
            this.xmlClient.SetupGet(x => x.Plan).Returns(xmlPlanClientMock.Object);

            this.Sut = new PlanXmlService(
                this.xmlClient.Object,
                this.planSaleRepository.Object,
                this.logger.Object);
        }

        public IPlanXmlService Sut { get; set; }

        [Fact]
        public async Task ImportPlanWithoutPlanIdSuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var xmlPlanId = Guid.NewGuid();

            this.xmlClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(xmlPlanId))
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, xmlPlanId);

            // Assert
            this.planSaleRepository.Verify(r => r.Attach(It.Is<Plan>(c => c.CompanyId == companyId)), Times.Once);
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportPlanWithPlanIdSuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var xmlPlanId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var planSale = TestModelProvider.GetPlanEntity(planId);

            this.xmlClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(xmlPlanId, planId))
                .Verifiable();
            this.planSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync(planSale)
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, xmlPlanId);

            // Assert
            this.planSaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()), Times.Once);
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ImportPlanWithPlanIdNotFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var xmlPlanId = Guid.NewGuid();
            var planId = Guid.NewGuid();

            this.xmlClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetPlanResponse(xmlPlanId, planId))
                .Verifiable();
            this.planSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync((Plan)null)
                .Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => this.Sut.ImportEntity(companyId, companyName, xmlPlanId));
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(1, GarageDescription.OneCarGarage)]
        [InlineData(2, GarageDescription.TwoCarGarage)]
        [InlineData(3, GarageDescription.ThreeCarGarage)]
        [InlineData(4, GarageDescription.FourPlusCarGarage)]
        [InlineData(5, GarageDescription.FourPlusCarGarage)]
        public async Task ImportPlanWithPlanIdAndGarageDescriptionSuccess(int totalGarages, GarageDescription garageDescription)
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyName = Faker.Company.Name();
            var xmlPlanId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var planSale = TestModelProvider.GetPlanEntity(planId, createStub: true);
            var planMocked = Mock.Get(planSale);
            planMocked.SetupProperty(p => p.BasePlan, initialValue: new BasePlan("plan-name", companyName));
            var xmlPlanResponse = GetPlanResponse(xmlPlanId, planId);
            xmlPlanResponse.Garage = totalGarages;

            this.xmlClient
                .Setup(x => x.Plan.GetByIdAsync(It.Is<Guid>(id => id == xmlPlanId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlPlanResponse)
                .Verifiable();
            this.planSaleRepository
                .Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync(planSale)
                .Verifiable();

            // Act
            await this.Sut.ImportEntity(companyId, companyName, xmlPlanId);

            // Assert
            this.planSaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()), Times.Once);
            this.planSaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            planMocked.Verify(p => p.UpdateBasePlanInformation(It.Is<BasePlan>(b => b.GarageDescription.Single() == garageDescription)), Times.Once);
        }

        private static PlanResponse GetPlanResponse(Guid xmlPlanId, Guid? planId = null) => new()
        {
            Id = xmlPlanId,
            Name = Faker.Company.Name(),
            PlanProfileId = planId,
        };
}
}
