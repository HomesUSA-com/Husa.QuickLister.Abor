namespace Husa.Quicklister.Abor.Api.Tests.Community
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request.Community;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Community;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class CommunityControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<ISaleCommunityService> communitySaleService = new();
        private readonly Mock<ICommunityQueriesRepository> communityQueriesRepository = new();
        private readonly Mock<ICommunityXmlService> communityXmlService = new();
        private readonly Mock<ISaleListingRequestService> listingSaleRequestService = new();
        private readonly Mock<ILogger<SaleCommunitiesController>> logger = new();
        private readonly Mock<ICommunityJsonImportService> communityJsonImportService = new();

        public CommunityControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleCommunitiesController(
                this.communityQueriesRepository.Object,
                this.logger.Object,
                this.communitySaleService.Object,
                this.listingSaleRequestService.Object,
                this.communityXmlService.Object,
                this.communityJsonImportService.Object,
                this.fixture.Mapper);
        }

        public SaleCommunitiesController Sut { get; set; }

        [Fact]
        public async Task CreateCommunity_CommunityCreated_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var communitySaleRequest = TestModelProvider.GetCommunitySaleRequest();

            this.communitySaleService
            .Setup(u => u.CreateAsync(It.IsAny<CommunitySaleCreateDto>()))
            .ReturnsAsync(CommandSingleResult<Guid, string>.Success(communityId))
            .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(communitySaleRequest);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(communityId, okResult.Value);
            this.communitySaleService.Verify();
        }

        [Fact]
        public async Task CreateCommunity_CommunityFails_Error()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var communitySaleRequest = TestModelProvider.GetCommunitySaleRequest();

            this.communitySaleService
            .Setup(u => u.CreateAsync(It.IsAny<CommunitySaleCreateDto>()))
            .ReturnsAsync(CommandSingleResult<Guid, string>.Success(communityId))
            .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(communitySaleRequest);

            // Assert
            Assert.NotNull(result);
            this.communitySaleService.Verify();
        }

        [Fact]
        public async Task UpdateCommunity_CallService_Succcess()
        {
            // Arrange
            var communitySaleRequest = new CommunitySaleRequest();
            var communityId = Guid.NewGuid();

            // Act
            var result = await this.Sut.UpdateCommunityAsync(communityId, communitySaleRequest);

            // Assert
            Assert.NotNull(result);
            this.communitySaleService.Verify(x => x.UpdateCommunity(communityId, It.IsAny<CommunitySaleDto>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_CommunitiesFound_Success()
        {
            // Arrange
            var communityId1 = Guid.NewGuid();
            var communityId2 = Guid.NewGuid();
            var community1 = TestModelProvider.GetCommunityQueryResult(communityId1);
            var community2 = TestModelProvider.GetCommunityQueryResult(communityId2);
            var communityQueryResult = new List<CommunityQueryResult>()
            {
                community1,
                community2,
            };
            var companyId = Guid.NewGuid();
            var communityByCompanyRequestFilter = TestModelProvider.GetCommunityByCompanyRequestFilter(companyId);
            var dataSet = new DataSet<CommunityQueryResult>(communityQueryResult, communityQueryResult.Count);

            this.communityQueriesRepository
            .Setup(c => c.GetAsync(It.IsAny<ProfileQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Action
            var actionResult = await this.Sut.GetAsync(communityByCompanyRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<CommunityDataQueryResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Total);
            this.communityQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetAsync_CommunitiesNotFound_Success()
        {
            // Arrange
            var communityQueryResult = new List<CommunityQueryResult>() { };
            var companyId = Guid.NewGuid();
            var communityByCompanyRequestFilter = TestModelProvider.GetCommunityByCompanyRequestFilter(companyId);
            var dataSet = new DataSet<CommunityQueryResult>(communityQueryResult, communityQueryResult.Count);

            this.communityQueriesRepository
            .Setup(c => c.GetAsync(It.IsAny<ProfileQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Action
            var actionResult = await this.Sut.GetAsync(communityByCompanyRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<CommunityDataQueryResponse>>(okObjectResult.Value);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Total);
            this.communityQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetCommunityById_CommunityFound_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunityDetailQueryResult(communityId);

            this.communityQueriesRepository
            .Setup(u => u.GetCommunityById(It.IsAny<Guid>()))
            .ReturnsAsync(community)
            .Verifiable();

            // Act
            var result = await this.Sut.GetCommunityById(communityId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<CommunitySaleResponse>(objectResult.Value);
            var communityDetail = Assert.IsAssignableFrom<CommunitySaleResponse>(objectResult.Value);
            Assert.Equal(communityId, communityDetail.Id);
        }

        [Fact]
        public async Task GetCommunityById_CommunityNotFound_Error()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            CommunityDetailQueryResult community = null;

            this.communityQueriesRepository
            .Setup(u => u.GetCommunityById(It.IsAny<Guid>()))
            .ReturnsAsync(community)
            .Verifiable();

            // Act
            var result = await this.Sut.GetCommunityById(communityId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task DeleteCommunity_CallService_Succcess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var deleteInCascade = false;

            this.communitySaleService.Setup(x => x.DeleteCommunity(It.Is<Guid>(x => x == communityId), It.Is<bool>(x => x == deleteInCascade))).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteCommunityAsync(communityId, deleteInCascade));
            this.communitySaleService.Verify(x => x.DeleteCommunity(communityId, deleteInCascade), Times.Once);
        }

        [Fact]
        public async Task SaveAndSubmitCommunityAsync_Succcess()
        {
            // Arrange
            var communitySaleRequest = new CommunitySaleRequest();
            var communityId = Guid.NewGuid();
            this.listingSaleRequestService
                .Setup(u => u.CreateRequestsFromCommunityAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CommandResult<CommunityRequestsResponse>.Success(new CommunityRequestsResponse()))
                .Verifiable();

            // Act
            var result = await this.Sut.SaveAndSubmitCommunityAsync(communityId, communitySaleRequest);

            // Assert
            Assert.NotNull(result);
            this.communitySaleService.Verify(x => x.UpdateCommunity(communityId, It.IsAny<CommunitySaleDto>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployees_EmployeesNotFound_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var newFilter = new BaseFilterRequest();
            var employees = new List<CommunityEmployeeQueryResult>();
            var dataSet = new DataSet<CommunityEmployeeQueryResult>(employees, employees.Count);

            this.communityQueriesRepository
                .Setup(c => c.GetCommunityEmployees(It.Is<Guid>(x => x == communityId), It.IsAny<string>()))
                .ReturnsAsync(dataSet)
                .Verifiable();

            // Act
            var result = await this.Sut.GetEmployeesAsync(communityId, newFilter);

            // Assert
            this.communityQueriesRepository.Verify();
            Assert.NotNull(result);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var employeeResult = Assert.IsAssignableFrom<DataSet<CommunityEmployeeResponse>>(okObjectResult.Value);
            Assert.Empty(employeeResult.Data);
        }

        [Fact]
        public async Task GetEmployees_EmployeesFound_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var employeeId1 = Guid.NewGuid();
            var employeeId2 = Guid.NewGuid();
            var newFilter = new BaseFilterRequest();
            var employee1 = TestModelProvider.GetCommunityEmployeeQueryResult(employeeId1, employeeId1, "Test");
            var employee2 = TestModelProvider.GetCommunityEmployeeQueryResult(employeeId2, employeeId2, "Test");
            var employees = new List<CommunityEmployeeQueryResult>() { employee1, employee2 };
            var dataSet = new DataSet<CommunityEmployeeQueryResult>(employees, employees.Count);

            this.communityQueriesRepository
                .Setup(c => c.GetCommunityEmployees(It.Is<Guid>(x => x == communityId), It.IsAny<string>()))
                .ReturnsAsync(dataSet)
                .Verifiable();

            // Act
            var result = await this.Sut.GetEmployeesAsync(communityId, newFilter);

            // Assert
            this.communityQueriesRepository.Verify();
            Assert.NotNull(result);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var employeeResult = Assert.IsAssignableFrom<DataSet<CommunityEmployeeResponse>>(okObjectResult.Value);
            Assert.Equal(2, employeeResult.Total);
        }

        [Fact]
        public async Task GetCommunityWithListingProjection_Succcess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunityDetailQueryResult(communityId);
            this.communityQueriesRepository
                .Setup(c => c.GetByIdWithListingImportProjection(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(community)
                .Verifiable();

            // Act
            var result = await this.Sut.GetCommunityWithListingProjection(communityId, listingId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<CommunitySaleResponse>(okObjectResult.Value);
        }
    }
}
