namespace Husa.Quicklister.Abor.Api.Tests.Listing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Interfaces.Uploader;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Models.ReverseProspect;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Api.Contracts.Request;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class ListingSaleControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleQueriesRepository> saleListingQueriesRepository = new();
        private readonly Mock<ISaleListingRequestQueriesRepository> saleListingRequestQueriesRepository = new();
        private readonly Mock<IManagementTraceQueriesRepository> managementTraceQueriesRepository = new();
        private readonly Mock<IUploaderService> austinUploaderService = new();
        private readonly Mock<ISaleListingService> listingSaleService = new();
        private readonly Mock<ILogger<SaleListingsController>> logger = new();
        private readonly Mock<IMediaService> downloaderMediaService = new();
        private readonly Mock<HttpContext> httpContextMock = new();

        private readonly Guid userId = new("7c189de0-2493-44fb-b9da-30d1a6657f1c");

        public ListingSaleControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleListingsController(
                this.saleListingQueriesRepository.Object,
                this.saleListingRequestQueriesRepository.Object,
                this.managementTraceQueriesRepository.Object,
                this.listingSaleService.Object,
                this.austinUploaderService.Object,
                this.downloaderMediaService.Object,
                this.logger.Object,
                this.fixture.Mapper);
        }

        public SaleListingsController Sut { get; set; }

        [Theory]
        [InlineData(MarketStatuses.Active)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        public async Task GetListingSaleAsync_MarketStatuses_Success(MarketStatuses status)
        {
            // Arrange
            var listingId1 = Guid.NewGuid();
            var listingId2 = Guid.NewGuid();
            var listing1 = TestModelProvider.GetListingSaleQueryResult(listingId1);
            var listing2 = TestModelProvider.GetListingSaleQueryResult(listingId2);

            var listingSaleResponse = new List<ListingSaleQueryResult>()
            {
                listing1,
                listing2,
            };

            var filter = new ListingRequestFilter
            {
                MlsStatus = new List<MarketStatuses> { status },
                SearchBy = string.Empty,
                Skip = 0,
                Take = 100,
            };
            var dataSet = new DataSet<ListingSaleQueryResult>(listingSaleResponse, listingSaleResponse.Count);

            this.saleListingQueriesRepository
            .Setup(m => m.GetAsync(It.IsAny<ListingQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(filter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<ListingResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Total);
            this.saleListingQueriesRepository.Verify();
        }

        [Theory]
        [InlineData(MarketStatuses.Active)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        public async Task GetListingSaleAsync_MarketStatuses_EmptySuccess(MarketStatuses status)
        {
            // Arrange
            var listingSaleResponse = new List<ListingSaleQueryResult>() { };

            var filter = new ListingRequestFilter
            {
                MlsStatus = new List<MarketStatuses> { status },
                SearchBy = string.Empty,
                Skip = 0,
                Take = 100,
            };

            var dataSet = new DataSet<ListingSaleQueryResult>(listingSaleResponse, listingSaleResponse.Count);

            this.saleListingQueriesRepository
            .Setup(m => m.GetAsync(It.IsAny<ListingQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(filter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<ListingResponse>>(okObjectResult.Value);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Total);
            this.saleListingQueriesRepository.Verify();
        }

        [Theory]
        [InlineData(MarketStatuses.Active)]
        public async Task GeListingSaleAsyncSalesEmployyeReadOnlySuccess(MarketStatuses status)
        {
            // Arrange
            var listingId1 = Guid.NewGuid();
            var listingId2 = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var listing1 = TestModelProvider.GetListingSaleQueryResult(listingId1);
            listing1.CommunityId = communityId;
            var listing2 = TestModelProvider.GetListingSaleQueryResult(listingId2);
            listing2.CommunityId = communityId;
            var currentUserId = Guid.NewGuid();
            var currentUser = TestModelProvider.GetCurrentUser(userId: currentUserId, userRole: UserRole.User);
            currentUser.EmployeeRole = RoleEmployee.SalesEmployeeReadonly;
            var listingSaleResponse = new List<ListingSaleQueryResult>()
            {
                listing1,
                listing2,
            };
            var filter = new ListingRequestFilter
            {
                MlsStatus = new List<MarketStatuses> { status },
                SearchBy = string.Empty,
                Skip = 0,
                Take = 100,
                CommunityId = communityId,
            };
            var dataSet = new DataSet<ListingSaleQueryResult>(listingSaleResponse, listingSaleResponse.Count);
            this.saleListingQueriesRepository
            .Setup(m => m.GetAsync(It.IsAny<ListingQueryFilter>()))
            .ReturnsAsync(dataSet)
            .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(filter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<ListingResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result.Data);
            Assert.Equal(2, result.Total);
            this.saleListingQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetListing_ListingNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            ListingSaleQueryDetailResult listing = null;

            this.saleListingQueriesRepository
            .Setup(u => u.GetListing(It.IsAny<Guid>()))
            .ReturnsAsync(listing)
            .Verifiable();

            // Act
            var result = await this.Sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Null(objectResult.Value);
        }

        [Fact]
        public async Task GetListing_ListingFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleQueryDetail(listingId);

            this.saleListingQueriesRepository
            .Setup(u => u.GetListing(It.IsAny<Guid>()))
            .ReturnsAsync(listing)
            .Verifiable();

            // Act
            var result = await this.Sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.NotNull(objectResult.Value);
            Assert.IsType<ListingSaleDetailResponse>(objectResult.Value);
            var listngDetail = Assert.IsAssignableFrom<ListingSaleDetailResponse>(objectResult.Value);
            Assert.Equal(listingId, listngDetail.Id);
        }

        [Fact]
        public async Task CreateListing_ListingCreated_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var dataResponse = CommandSingleResult<Guid, string>.Success(listingId);
            var listingSaleRequest = TestModelProvider.GetListingSaleRequest(communityId: Guid.NewGuid());

            this.listingSaleService
            .Setup(u => u.CreateAsync(It.IsAny<QuickCreateListingDto>()))
            .ReturnsAsync(dataResponse)
            .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(listingSaleRequest);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(listingId, okResult.Value);
            this.listingSaleService.Verify();
        }

        [Fact]
        public async Task CreateListing_ListingFails_Error()
        {
            // Arrange
            var listingId = Guid.Empty;
            var listingSaleRequest = TestModelProvider.GetListingSaleRequest(communityId: Guid.NewGuid());
            var dataResponse = CommandSingleResult<Guid, string>.Error(listingId.ToString());

            this.listingSaleService
            .Setup(u => u.CreateAsync(It.IsAny<QuickCreateListingDto>()))
            .ReturnsAsync(dataResponse)
            .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(listingSaleRequest);

            // Assert
            Assert.NotNull(result);
            this.listingSaleService.Verify();
        }

        [Fact]
        public async Task UpdateLisitng_CallService_Succcess()
        {
            // Arrange
            var listingSaleRequest = new ListingSaleDetailRequest();
            var listingId = Guid.NewGuid();

            // Act
            var result = await this.Sut.UpdateListing(listingId, listingSaleRequest);

            // Assert
            Assert.NotNull(result);
            this.listingSaleService.Verify(x => x.UpdateListing(listingId, It.IsAny<SaleListingDto>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task ChangeCommunity_CallService_Succcess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var communityId = Guid.NewGuid();

            // Act
            var result = await this.Sut.ChangeCommunity(listingId, communityId);

            // Assert
            Assert.NotNull(result);
            this.listingSaleService.Verify(x => x.ChangeCommunity(listingId, communityId), Times.Once);
        }

        [Fact]
        public async Task ChangePlan_CallService_Succcess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var planId = Guid.NewGuid();

            // Act
            var result = await this.Sut.ChangePlan(listingId, planId);

            // Assert
            Assert.NotNull(result);
            this.listingSaleService.Verify(x => x.ChangePlan(listingId, planId, false), Times.Once);
        }

        [Fact]
        public async Task DeleteListing_ListingFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            // Act
            await this.Sut.DeleteListing(listingId);

            // Assert
            this.listingSaleService.Verify(x => x.DeleteListing(listingId), Times.Once);
        }

        [Fact]
        public async Task UnlockListing_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var expectedResponse = CommandResult<string>.Success("some-message");

            var sut = this.GetSut();
            this.listingSaleService
                .Setup(ls => ls.UnlockListing(It.Is<Guid>(id => id == listingId), default))
                .ReturnsAsync(expectedResponse)
                .Verifiable();

            // Act
            await sut.UnlockListing(listingId);

            // Assert
            this.listingSaleService.Verify();
            this.listingSaleService.Verify(x => x.UnlockListing(It.Is<Guid>(x => x == listingId), default), Times.Once);
        }

        [Fact]
        public async Task CloseListingCompleteSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var expectedResponse = CommandResult<string>.Success("some-message");
            var sut = this.GetSut();
            this.listingSaleService
                .Setup(ls => ls.CloseListing(It.Is<Guid>(id => id == listingId)))
                .ReturnsAsync(expectedResponse)
                .Verifiable();

            // Act
            await sut.CloseListing(listingId);

            // Assert
            this.listingSaleService.Verify();
            this.listingSaleService.Verify(x => x.CloseListing(It.Is<Guid>(x => x == listingId)), Times.Once);
        }

        [Fact]
        public async Task DeclinePhotosAsync_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            var sut = this.GetSut();
            this.listingSaleService
                .Setup(ls => ls.DeclinePhotos(It.Is<Guid>(id => id == listingId), default))
                .Verifiable();

            // Act
            await sut.DeclinePhotosAsync(listingId);

            // Assert
            this.listingSaleService.Verify();
            this.listingSaleService.Verify(x => x.DeclinePhotos(It.Is<Guid>(x => x == listingId), default), Times.Once);
        }

        [Fact]
        public async Task ReverseProspect_Complete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var trackingReverseProspect = new ReverseProspectInformationDto()
            {
                RequestedDate = DateTime.UtcNow,
                ReverseProspectData = new List<ReverseProspectDataDto>
                {
                    new ReverseProspectDataDto
                    {
                        Agent = "test",
                        DateSent = DateTime.Now.ToString(),
                        Email = "email@domein.com",
                        InterestLevel = null,
                    },
                },
            };

            this.austinUploaderService
                .Setup(x => x.GetReverseProspectListing(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CommandResult<ReverseProspectInformationDto>.Success(trackingReverseProspect));

            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, UserRole.MLSAdministrator.ToString()),
                new(ClaimTypes.NameIdentifier, this.userId.ToString()),
                new("username", "test-user"),
            };

            var claimIdentity = new ClaimsIdentity(claims, "fake-auth-type");
            var claimPrincipal = new ClaimsPrincipal();
            claimPrincipal.AddIdentity(claimIdentity);

            this.httpContextMock
              .Setup(a => a.Request.Headers["Authorization"])
              .Returns("fake-api-key");

            this.httpContextMock
             .SetupGet(ctx => ctx.User)
             .Returns(claimPrincipal);

            this.Sut.ControllerContext = new ControllerContext();
            this.Sut.ControllerContext.HttpContext = new DefaultHttpContext();
            this.Sut.ControllerContext.HttpContext = this.httpContextMock.Object;

            // Act
            var response = await this.Sut.GetReverseProspectInfo(listingId);

            // Assert
            this.listingSaleService.Verify();
            this.austinUploaderService.Verify(x => x.GetReverseProspectListing(It.Is<Guid>(x => x == listingId), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetXmlManagementInfo_NotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var filter = new BaseFilterRequest { Skip = 0, Take = 100, SortBy = "+SysCreatedOn" };
            var response = new DataSet<ManagementTraceQueryResult>(data: Array.Empty<ManagementTraceQueryResult>(), total: 0);

            this.managementTraceQueriesRepository
                .Setup(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var result = await this.Sut.GetXmlManagementInfo(listingId, filter);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var dataSet = Assert.IsAssignableFrom<DataSet<XmlManagementResponse>>(okResult.Value);
            Assert.Empty(dataSet.Data);
        }

        [Fact]
        public async Task GetXmlManagementInfo_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var filter = new BaseFilterRequest { Skip = 0, Take = 100, SortBy = "+SysCreatedOn" };
            var traceList = new ManagementTraceQueryResult()
            {
                Id = Guid.NewGuid(),
                SysCreatedOn = DateTime.UtcNow,
                SysModifiedBy = this.userId,
                ListingSaleId = listingId,
                IsManuallyManaged = Faker.Boolean.Random(),
                ModifiedBy = this.userId.ToString(),
                CreatedBy = this.userId.ToString(),
            };

            var response = new DataSet<ManagementTraceQueryResult>(
                data: new[] { traceList },
                total: 1);

            this.managementTraceQueriesRepository
            .Setup(u => u.GetAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(response)
            .Verifiable();

            // Act
            var result = await this.Sut.GetXmlManagementInfo(listingId, filter);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var dataSet = Assert.IsAssignableFrom<DataSet<XmlManagementResponse>>(okResult.Value);
            Assert.NotEmpty(dataSet.Data);
        }

        [Fact]
        public async Task GetListingsWithOpenHouse_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var requestFilter = new BaseFilterRequest { Skip = 0, Take = 100 };
            var queryResponse = new SaleListingOpenHouseQueryResult
            {
                Id = listingId,
                CompanyId = Guid.NewGuid(),
                MlsNumber = Faker.Lorem.Sentence(),
                OpenHouses = new List<OpenHousesQueryResult>()
                {
                    new OpenHousesQueryResult
                    {
                        EndTime = TimeSpan.FromHours(5),
                        StartTime = TimeSpan.FromHours(6),
                        Type = Extensions.Domain.Enums.OpenHouseType.Monday,
                    },
                },
            };

            var response = new DataSet<SaleListingOpenHouseQueryResult>(
                data: new[] { queryResponse },
                total: 1);

            this.saleListingQueriesRepository
                .Setup(u => u.GetListingsWithOpenHouse(It.IsAny<BaseQueryFilter>()))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var result = await this.Sut.GetListingsWithOpenHouseAsync(requestFilter);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var dataSet = Assert.IsAssignableFrom<DataSet<ListingSaleOpenHouseResponse>>(okResult.Value);
            Assert.NotEmpty(dataSet.Data);
        }

        private SaleListingsController GetSut() => new(
            this.saleListingQueriesRepository.Object,
            this.saleListingRequestQueriesRepository.Object,
            this.managementTraceQueriesRepository.Object,
            this.listingSaleService.Object,
            this.austinUploaderService.Object,
            this.downloaderMediaService.Object,
            this.logger.Object,
            this.fixture.Mapper);
    }
}
