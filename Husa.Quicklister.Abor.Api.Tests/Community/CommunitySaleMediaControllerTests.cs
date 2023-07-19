namespace Husa.Quicklister.Abor.Api.Tests.Community
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Enums;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Controllers.Media;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.MediaService.Api.Contracts.Request;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class CommunitySaleMediaControllerTests
    {
        private readonly Mock<ICommunityMediaService> communityMediaService;
        private readonly Mock<ILogger<SaleCommunityMediaController>> logger;

        public CommunitySaleMediaControllerTests(ApplicationServicesFixture fixture)
        {
            this.communityMediaService = new Mock<ICommunityMediaService>();
            this.logger = new Mock<ILogger<SaleCommunityMediaController>>();
            this.Sut = new SaleCommunityMediaController(this.communityMediaService.Object, fixture.Options.Object, this.logger.Object);
        }

        public SaleCommunityMediaController Sut { get; set; }

        [Fact]
        public async Task GetResources_ResourcesFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
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

            this.communityMediaService.Setup(m => m.GetAsync(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Equal(2, result.Media.Count());
            Assert.Contains(result.Media, c => c.Id == mediaId1);
            this.communityMediaService.Verify();
        }

        [Fact]
        public async Task GetResources_ResourcesEmpty_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var mediaDetailList = new List<MediaDetail>() { };
            var virtualTourList = new List<VirtualTourDetail>() { };

            var response = new ResourceResponse { Media = mediaDetailList, VirtualTour = virtualTourList };

            this.communityMediaService.Setup(m => m.GetAsync(It.Is<Guid>(x => x == entityId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(entityId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Empty(result.Media);
            Assert.Empty(result.VirtualTour);
            this.communityMediaService.Verify();
        }

        [Fact]
        public async Task CreateResources_ResourcesAdded_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var media = TestModelProvider.MediaDetail(entityId);

            this.communityMediaService.Setup(m => m.Resource.CreateAsync(It.IsAny<Guid>(), It.IsAny<MarketCode>(), It.IsAny<Request.Media>(), It.Is<int>(x => x == 5)))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.CreateAsync(entityId, media);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.communityMediaService.Verify(x => x.Resource.CreateAsync(It.Is<Guid>(x => x == entityId), It.IsAny<MarketCode>(), It.Is<Request.Media>(m => m == media), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateResources_ResourcesUpdated_Success()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var media = TestModelProvider.MediaDetail(entityId);

            this.communityMediaService.Setup(m => m.Resource.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Request.Media>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.UpdateAsync(entityId, mediaId, media);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.communityMediaService.Verify(x => x.Resource.UpdateAsync(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == mediaId), It.Is<Request.Media>(m => m == media)), Times.Once);
        }

        [Fact]
        public async Task GetMediaById_MediaFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var mediaDetail = TestModelProvider.GetMediaDetail(mediaId);

            this.communityMediaService.Setup(m => m.Resource.GetById(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == mediaId)))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(entityId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<MediaDetail>(okObjectResult.Value);
            Assert.Equal(result.Id, mediaId);
            this.communityMediaService.Verify();
        }

        [Fact]
        public async Task GetMediaById_MediaNotFound_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            MediaDetail mediaDetail = null;

            this.communityMediaService.Setup(m => m.Resource.GetById(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == mediaId)))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(entityId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteById_MediaDeleted_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            this.communityMediaService.Setup(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == entityId), It.IsAny<Guid>(), It.IsAny<int>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResource(entityId, mediaId));
            this.communityMediaService.Verify(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == entityId), It.Is<Guid>(x => x == mediaId), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteResources_MediaDeleted_Success()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            this.communityMediaService.Setup(x => x.Resource.DeleteAsync(It.Is<Guid>(x => x == entityId), It.IsAny<bool>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResources(entityId));
            this.communityMediaService.Verify(x => x.Resource.DeleteAsync(It.Is<Guid>(x => x == entityId), It.IsAny<bool>()), Times.Once);
        }
    }
}
