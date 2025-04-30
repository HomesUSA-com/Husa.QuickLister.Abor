namespace Husa.Quicklister.Abor.Application.Tests.Services.Plans
{
    using System;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Photo;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    public class PlanPhotoServiceTests
    {
        private readonly Mock<IUserContextProvider> mockUserContextProvider = new();
        private readonly Mock<IPhotoServiceClient> mockPhotoClient = new();
        private readonly Mock<IPhotoBusService> mockBusService = new();
        private readonly Mock<IPlanRepository> mockPlanRepository = new();
        private readonly Mock<IServiceSubscriptionClient> mockServiceSubscriptionClient = new();
        private readonly Mock<ILogger<PlanPhotoService>> mockLogger = new();
        private readonly PlanPhotoService sut;

        public PlanPhotoServiceTests()
        {
            this.sut = new PlanPhotoService(
                 this.mockUserContextProvider.Object,
                 this.mockPhotoClient.Object,
                 this.mockBusService.Object,
                 this.mockPlanRepository.Object,
                 this.mockServiceSubscriptionClient.Object,
                 this.mockLogger.Object);
        }

        [Fact]
        public async Task GetMigratePropertyMessage_WithValidPlan_SetsCorrectPlanName()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var planName = "Test Plan";
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var plan = new Plan(companyId, planName, "Owner");
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.MLSAdministrator);

            this.mockUserContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.mockPlanRepository.Setup(x => x.GetById(planId, It.IsAny<bool>())).ReturnsAsync(plan);
            this.mockBusService
                .Setup(b => b.GetPropertyBusMessage(planId, It.IsAny<Request.Property>()))
                .Returns(new Husa.PhotoService.ServiceBus.Messages.Contracts.PropertyBusMessage())
                .Verifiable();

            var property = new Request.Property()
            {
                Id = planId,
            };

            // Act
            var result = await this.sut.GetMigratePropertyMessage(property, 1, companyId, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyId, result.CompanyId);
            Assert.Equal(PhotoService.Domain.Enums.PropertyType.Plan, result.Property.Type);
        }
    }
}
