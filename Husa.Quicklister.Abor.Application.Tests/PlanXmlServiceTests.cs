namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Plan;
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
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IXmlClient> xmlClient = new();

        public PlanXmlServiceTests()
        {
            var xmlPlanClientMock = new Mock<IXmlPlan>();
            this.xmlClient.SetupGet(x => x.Plan).Returns(xmlPlanClientMock.Object);

            this.Sut = new PlanXmlService(
                this.xmlClient.Object,
                this.planSaleRepository.Object,
                this.userContextProvider.Object,
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

        private static PlanResponse GetPlanResponse(Guid xmlPlanId, Guid? planId = null) => new()
        {
            Id = xmlPlanId,
            Name = Faker.Company.Name(),
            PlanProfileId = planId,
        };
}
}
