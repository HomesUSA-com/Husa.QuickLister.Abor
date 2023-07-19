namespace Husa.Quicklister.Abor.Api.Tests.Plan
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Controllers.Media;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.MediaService.Api.Contracts.Request;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class PlanMediaControllerTests
    {
        private readonly Mock<IPlanMediaService> planMediaService;
        private readonly Mock<ILogger<PlanMediaController>> logger;

        public PlanMediaControllerTests(ApplicationServicesFixture fixture)
        {
            this.planMediaService = new Mock<IPlanMediaService>();
            this.logger = new Mock<ILogger<PlanMediaController>>();
            this.Sut = new PlanMediaController(this.planMediaService.Object, fixture.Options.Object, this.logger.Object);
        }

        public PlanMediaController Sut { get; set; }

        [Fact]
        public async Task GetResources_ResourcesFound_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var mediaId1 = Guid.NewGuid();
            var mediaDetail1 = TestModelProvider.GetMediaDetail(mediaId1);
            var mediaId2 = Guid.NewGuid();
            var mediaDetail2 = TestModelProvider.GetMediaDetail(mediaId2);
            var mediaDetailList = new List<MediaDetail>()
            {
                mediaDetail1,
                mediaDetail2,
            };

            var virtualTourList = new List<VirtualTourDetail>() { };

            var response = new ResourceResponse { Media = mediaDetailList, VirtualTour = virtualTourList };

            this.planMediaService.Setup(m => m.GetAsync(It.Is<Guid>(x => x == planId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(planId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Equal(2, result.Media.Count());
            Assert.Contains(result.Media, c => c.Id == mediaId1);
            this.planMediaService.Verify();
        }

        [Fact]
        public async Task GetResources_ResourcesEmpty_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var mediaDetailList = new List<MediaDetail>()
            {
            };

            var virtualTourList = new List<VirtualTourDetail>() { };

            var response = new ResourceResponse { Media = mediaDetailList, VirtualTour = virtualTourList };

            this.planMediaService.Setup(m => m.GetAsync(It.Is<Guid>(x => x == planId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(planId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Empty(result.Media);
            Assert.Empty(result.VirtualTour);
            this.planMediaService.Verify();
        }

        [Fact]
        public async Task UpdateResources_ResourcesUpdated_Success()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var media = TestModelProvider.MediaDetail(planId);

            this.planMediaService.Setup(m => m.Resource.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Request.Media>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.UpdateAsync(planId, mediaId, media);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.planMediaService.Verify(x => x.Resource.UpdateAsync(It.Is<Guid>(x => x == planId), It.Is<Guid>(x => x == mediaId), It.Is<Request.Media>(m => m == media)), Times.Once);
        }

        [Fact]
        public async Task GetMediaById_MediaFound_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var mediaDetail = TestModelProvider.GetMediaDetail(mediaId);

            this.planMediaService.Setup(m => m.Resource.GetById(It.Is<Guid>(x => x == planId), It.Is<Guid>(x => x == mediaId)))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(planId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<MediaDetail>(okObjectResult.Value);
            Assert.Equal(result.Id, mediaId);
            this.planMediaService.Verify();
        }

        [Fact]
        public async Task GetMediaById_MediaNotFound_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            MediaDetail mediaDetail = null;

            this.planMediaService.Setup(m => m.Resource.GetById(It.Is<Guid>(x => x == planId), It.Is<Guid>(x => x == mediaId)))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(planId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteById_MediaDeleted_Success()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var planId = Guid.NewGuid();

            this.planMediaService.Setup(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == planId), It.IsAny<Guid>(), It.IsAny<int>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResource(planId, mediaId));
            this.planMediaService.Verify(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == planId), It.Is<Guid>(x => x == mediaId), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteResources_MediaDeleted_Success()
        {
            // Arrange
            var planId = Guid.NewGuid();

            this.planMediaService.Setup(x => x.Resource.DeleteAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResources(planId));
            this.planMediaService.Verify(x => x.Resource.DeleteAsync(It.Is<Guid>(x => x == planId), It.IsAny<bool>()), Times.Once);
        }
    }
}
