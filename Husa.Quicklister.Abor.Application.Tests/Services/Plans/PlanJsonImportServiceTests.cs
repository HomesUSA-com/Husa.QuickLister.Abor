namespace Husa.Quicklister.Abor.Application.Tests.Services.Plans
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Application.Tests.Providers;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.JsonImport;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class PlanJsonImportServiceTests
    {
        private readonly Mock<IPlanRepository> planSaleRepository = new();
        private readonly Mock<ILogger<PlanJsonImportService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IJsonImportClient> jsonClient = new();
        private readonly Mock<IServiceSubscriptionClient> companyClient = new();
        public PlanJsonImportServiceTests(ApplicationServicesFixture fixture)
        {
            var jsonPlanClientMock = new Mock<IJsonImportPlan>();
            this.jsonClient.SetupGet(x => x.Plan).Returns(jsonPlanClientMock.Object);
            this.Sut = new PlanJsonImportService(
                this.jsonClient.Object,
                this.planSaleRepository.Object,
                this.userContextProvider.Object,
                this.companyClient.Object,
                fixture.Options.Object,
                this.logger.Object);
        }

        public IPlanJsonImportService Sut { get; set; }

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
            this.companyClient.Setup(x => x.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CompanyResponse.CompanyDetail()
                {
                    SettingInfo = new() { DisableDirectSavingForApi = false },
                });

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
            Rooms = JsonModelProviders.GetRooms(),
        };
    }
}
