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
    using Husa.Quicklister.Abor.Api.Controllers.Community;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Interfaces.JsonImport;
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

        [Fact]
        public async Task GetCommunityByName_WhenCommunityExists_ReturnsOkWithCommunity()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var communityName = "Test Community";
            var filter = new CommunityByNameFilter
            {
                CompanyId = companyId,
                CommunityName = communityName,
            };

            var communityData = new CommunityDetailQueryResult
            {
                Id = Guid.NewGuid(),
                Profile = new()
                {
                    Name = communityName,
                },
            };

            this.communityQueriesRepository
                .Setup(x => x.GetCommunityByName(companyId, communityName))
                .ReturnsAsync(communityData);

            // Act
            var result = await this.Sut.GetCommunityByName(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CommunitySaleResponse>(okResult.Value);
            Assert.Equal(communityName, returnValue.Name);
        }

        [Fact]
        public async Task GetCommunityByName_WhenCommunityDoesNotExist_ReturnsEmptyObject()
        {
            // Arrange
            var filter = new CommunityByNameFilter
            {
                CompanyId = Guid.NewGuid(),
                CommunityName = "NonExistent",
            };

            this.communityQueriesRepository
                .Setup(x => x.GetCommunityByName(filter.CompanyId, filter.CommunityName))
                .ReturnsAsync((CommunityDetailQueryResult)null);

            // Act
            var result = await this.Sut.GetCommunityByName(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<object>(okResult.Value);
        }
    }
}
