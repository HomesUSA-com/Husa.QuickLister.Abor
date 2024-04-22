namespace Husa.Quicklister.Abor.Api.Tests.PhotoRequest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.PhotoService.Api.Contracts.Response;
    using Husa.PhotoService.Domain.Enums;
    using Husa.Quicklister.Abor.Api.Controllers.PhotoRequest;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.PhotoService.Api.Contracts.Request;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Api.Test")]
    public class PlanPhotoRequestControllerTests
    {
        private readonly Mock<IPlanPhotoService> photoService;
        private readonly Mock<ILogger<PlanPhotoRequestsController>> logger;

        public PlanPhotoRequestControllerTests()
        {
            this.photoService = new Mock<IPlanPhotoService>();
            this.logger = new Mock<ILogger<PlanPhotoRequestsController>>();
            this.Sut = new PlanPhotoRequestsController(this.photoService.Object, this.logger.Object);
        }

        public PlanPhotoRequestsController Sut { get; set; }

        private static PropertyType PhotoType => PropertyType.Plan;

        [Fact]
        public async Task GetAsync_ResourcesFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var photoRequestFilter = new Request.PhotoRequestFilter
            {
                Skip = 0,
                Take = 100,
                Type = new[] { PhotoType },
                Status = new List<PhotoRequestStatus>
                {
                    PhotoRequestStatus.Pending,
                },
            };

            var photo1 = TestModelProvider.GetPhotoRequestResponse(PhotoType.ToPhotoRequestType());
            var photo2 = TestModelProvider.GetPhotoRequestResponse(PhotoType.ToPhotoRequestType());

            var list = new List<PhotoRequestResponse>
            {
                photo1,
                photo2,
            };

            var response = new DataSet<PhotoRequestResponse>(list, list.Count);

            this.photoService.Setup(m => m.GetAsync(It.IsAny<Guid>(), photoRequestFilter))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(entityId, photoRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<PhotoRequestResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Total);
            Assert.Contains(result.Data, c => c.Id == photo1.Id);
            this.photoService.Verify();
        }

        [Fact]
        public async Task GetResources_ResourcesEmpty_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            var photoRequestFilter = new Request.PhotoRequestFilter
            {
                Skip = 0,
                Take = 100,
                Type = new[] { PhotoType },
                Status = new List<PhotoRequestStatus>
                {
                    PhotoRequestStatus.Pending,
                },
            };

            var response = new DataSet<PhotoRequestResponse>(new List<PhotoRequestResponse>(), 0);

            this.photoService.Setup(m => m.GetAsync(It.IsAny<Guid>(), photoRequestFilter))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(entityId, photoRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DataSet<PhotoRequestResponse>>(okObjectResult.Value);
            Assert.Empty(result.Data);
            this.photoService.Verify();
        }

        [Fact]
        public async Task GetByIdAsync_PhotoFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var response = TestModelProvider.GetPhotoRequestDetail();
            var photoRequestId = response.Id;

            this.photoService.Setup(m => m.GetByIdAsync(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == photoRequestId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetByIdAsync(entityId, photoRequestId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<PhotoRequestDetail>(okObjectResult.Value);
            Assert.Equal(result.Id, photoRequestId);
            this.photoService.Verify();
        }

        [Fact]
        public async Task GetByIdAsync_PhotoNotFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var photoRequestId = Guid.NewGuid();

            PhotoRequestDetail photoDetail = null;

            this.photoService.Setup(m => m.GetByIdAsync(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == photoRequestId)))
                .ReturnsAsync(photoDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetByIdAsync(entityId, photoRequestId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public async Task CreateAsync_PhotoRequestAdded_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var photoRequest = TestModelProvider.GetPhotoRequest();

            this.photoService.Setup(m => m.CreateAsync(It.IsAny<Guid>(), It.IsAny<Request.PhotoRequest>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.CreateAsync(communityId, photoRequest);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.photoService.Verify(x => x.CreateAsync(It.Is<Guid>(x => x == communityId), It.Is<Request.PhotoRequest>(m => m == photoRequest)), Times.Once);
        }

        [Fact]
        public async Task DeleteById_PhotoRequestDeleted_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var photoRequestId = Guid.NewGuid();

            this.photoService.Setup(x => x.DeleteByIdAsync(It.Is<Guid>(x => x == communityId), It.Is<Guid>(x => x == photoRequestId))).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteById(communityId, photoRequestId));
            this.photoService.Verify(x => x.DeleteByIdAsync(It.Is<Guid>(x => x == communityId), It.Is<Guid>(x => x == photoRequestId)), Times.Once);
        }
    }
}
