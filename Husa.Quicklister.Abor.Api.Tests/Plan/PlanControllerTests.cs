namespace Husa.Quicklister.Abor.Api.Tests.Plan
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Plan;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Models.Plan;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Plan;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using ExtensionInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Plan;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class PlanControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IPlanQueriesRepository> planQueriesRepository = new();
        private readonly Mock<ExtensionInterfaces.IPlanXmlService> planXmlService = new();
        private readonly Mock<ExtensionInterfaces.IPlanJsonImportService> planJsonImportService = new();
        private readonly Mock<IPlanService> planService = new();
        private readonly Mock<ILogger<PlansController>> logger = new();

        public PlanControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new PlansController(this.planQueriesRepository.Object, this.planService.Object, this.planXmlService.Object, this.planJsonImportService.Object, this.logger.Object, this.fixture.Mapper);
        }

        public PlansController Sut { get; set; }

        [Fact]
        public async Task GetAsync_PlansFound_Success()
        {
            // Arrange
            var planId1 = Guid.NewGuid();
            var planId2 = Guid.NewGuid();
            var plan1 = TestModelProvider.GetPlanQueryResult(planId1);
            var plan2 = TestModelProvider.GetPlanQueryResult(planId2);
            var planQueryResult = new List<PlanQueryResult>()
            {
                plan1,
                plan2,
            };
            var companyId = Guid.NewGuid();
            var planByCompanyRequestFilter = TestModelProvider.GetPlanByCompanyRequestFilter(companyId);
            var dataSet = new DataSet<PlanQueryResult>(planQueryResult, planQueryResult.Count);

            this.planQueriesRepository
            .Setup(c => c.GetAsync(It.IsAny<ProfileQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Action
            var actionResult = await this.Sut.GetAsync(planByCompanyRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<PlanDataQueryResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Total);
            this.planQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetAsync_PlansNotFound_Success()
        {
            // Arrange
            var planQueryResult = new List<PlanQueryResult>() { };
            var companyId = Guid.NewGuid();
            var planByCompanyRequestFilter = TestModelProvider.GetPlanByCompanyRequestFilter(companyId);
            var dataSet = new DataSet<PlanQueryResult>(planQueryResult, planQueryResult.Count);

            this.planQueriesRepository
            .Setup(c => c.GetAsync(It.IsAny<ProfileQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Action
            var actionResult = await this.Sut.GetAsync(planByCompanyRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<PlanDataQueryResponse>>(okObjectResult.Value);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Total);
            this.planQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetPlanById_PlanFound_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanDetailQueryResult(planId);

            this.planQueriesRepository
            .Setup(u => u.GetPlanById(It.IsAny<Guid>()))
            .ReturnsAsync(plan)
            .Verifiable();

            // Act
            var result = await this.Sut.GetPlanById(planId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<PlanDetailResponse>(objectResult.Value);
            var planDetail = Assert.IsAssignableFrom<PlanDetailResponse>(objectResult.Value);
            Assert.Equal(planId, planDetail.Id);
        }

        [Fact]
        public async Task GetPlanById_PlanNotFound_Error()
        {
            // Arrange
            var planId = Guid.NewGuid();
            PlanDetailQueryResult plan = null;

            this.planQueriesRepository
            .Setup(u => u.GetPlanById(It.IsAny<Guid>()))
            .ReturnsAsync(plan)
            .Verifiable();

            // Act
            var result = await this.Sut.GetPlanById(planId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task CreatePlan_PlanCreated_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var planRequest = TestModelProvider.GetPlanRequest();

            this.planService
            .Setup(u => u.CreateAsync(It.IsAny<PlanCreateDto>()))
            .ReturnsAsync(planId)
            .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(planRequest);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(planId, okResult.Value);
            this.planService.Verify();
        }

        [Fact]
        public async Task CreatePlan_PlanFails_Error()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var planRequest = TestModelProvider.GetPlanRequest();
            this.planService
                .Setup(u => u.CreateAsync(It.IsAny<PlanCreateDto>()))
                .ReturnsAsync(planId)
                .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(planRequest);

            // Assert
            Assert.NotNull(result);
            this.planService.Verify();
        }

        [Fact]
        public async Task DeletePlan_CallService_Succcess()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var deleteInCascade = false;

            this.planService.Setup(x => x.DeletePlan(It.Is<Guid>(x => x == planId), It.Is<bool>(x => x == deleteInCascade))).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeletePlanAsync(planId, deleteInCascade));
            this.planService.Verify(x => x.DeletePlan(planId, deleteInCascade), Times.Once);
        }

        [Fact]
        public async Task UpdatePlan_CallService_Succcess()
        {
            // Arrange
            var communitySaleRequest = TestModelProvider.GetPlanUpdateRequest();
            var planId = Guid.NewGuid();

            // Act
            var result = await this.Sut.UpdatePlanAsync(planId, communitySaleRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkResult>(result);
            this.planService.Verify(x => x.UpdatePlanAsync(planId, It.IsAny<UpdatePlanDto>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdWithListingImportProjection_Succcess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanDetailQueryResult(planId);

            this.planQueriesRepository
                .Setup(u => u.GetByIdWithListingImportProjection(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act
            var result = await this.Sut.GetPlanWithListingProjection(planId, listingId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<PlanDetailResponse>(okObjectResult.Value);
        }
    }
}
