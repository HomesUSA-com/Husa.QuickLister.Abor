namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Media
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Controllers.Media;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.MediaService.Api.Contracts.Request;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class SaleListingRequestMediaControllerTests
    {
        private readonly Mock<IListingRequestMediaService> mediaService = new();
        private readonly Mock<ILogger<SaleListingRequestMediaController>> logger = new();

        public SaleListingRequestMediaControllerTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new SaleListingRequestMediaController(this.mediaService.Object, fixture.Options.Object, this.logger.Object);
        }

        public SaleListingRequestMediaController Sut { get; set; }

        [Fact]
        public async Task GetResources_ResourcesFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
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

            this.mediaService.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(response).Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(listingId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Equal(2, result.Media.Count());
            Assert.Contains(result.Media, c => c.Id == mediaId1);
            this.mediaService.Verify();
        }

        [Fact]
        public async Task GetResources_ResourcesEmpty_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mediaDetailList = new List<MediaDetail>()
            {
            };
            var virtualTourList = new List<VirtualTourDetail>() { };
            var response = new ResourceResponse { Media = mediaDetailList, VirtualTour = virtualTourList };

            this.mediaService.Setup(m => m.GetAsync(It.Is<Guid>(x => x == listingId)))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetResources(listingId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<ResourceResponse>(okObjectResult.Value);
            Assert.Empty(result.Media);
            Assert.Empty(result.VirtualTour);
            this.mediaService.Verify();
        }

        [Fact]
        public async Task CreateResources_ResourcesAdded_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var simpleMedia = TestModelProvider.MediaDetail(listingId);

            this.mediaService.Setup(m => m.Resource.CreateAsync(It.IsAny<Guid>(), It.IsAny<Request.Media>(), It.IsAny<int>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.CreateAsync(listingId, simpleMedia);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.mediaService.Verify(x => x.Resource.CreateAsync(It.Is<Guid>(x => x == listingId), It.Is<Request.Media>(m => m == simpleMedia), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ReplaceAsync_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var media = TestModelProvider.MediaDetail(listingId);

            this.mediaService.Setup(m => m.Resource.ReplaceAsync(It.IsAny<Guid>(), It.IsAny<Request.Media>())).Verifiable();

            // Act
            var actionResult = await this.Sut.ReplaceAsync(listingId, media);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.mediaService.Verify(x => x.Resource.ReplaceAsync(It.Is<Guid>(x => x == listingId), It.Is<Request.Media>(m => m == media)), Times.Once);
        }

        [Fact]
        public async Task UpdateResources_ResourcesUpdated_Success()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var media = TestModelProvider.MediaDetail(listingId);

            this.mediaService.Setup(m => m.Resource.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Request.Media>()))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.UpdateAsync(listingId, mediaId, media);

            // Assert
            Assert.IsAssignableFrom<OkResult>(actionResult);
            this.mediaService.Verify(x => x.Resource.UpdateAsync(It.Is<Guid>(x => x == listingId), It.Is<Guid>(x => x == mediaId), It.Is<Request.Media>(m => m == media)), Times.Once);
        }

        [Fact]
        public async Task GetMediaById_MediaFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var mediaDetail = TestModelProvider.GetMediaDetail(mediaId);

            this.mediaService.Setup(m => m.Resource.GetById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(listingId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<MediaDetail>(okObjectResult.Value);
            Assert.Equal(result.Id, mediaId);
            this.mediaService.Verify();
        }

        [Fact]
        public async Task GetMediaById_MediaNotFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            MediaDetail mediaDetail = null;

            this.mediaService.Setup(m => m.Resource.GetById(It.Is<Guid>(x => x == listingId), It.Is<Guid>(x => x == mediaId)))
                .ReturnsAsync(mediaDetail)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetMediaById(listingId, mediaId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteById_MediaDeleted_Success()
        {
            // Arrange
            var mediaId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.mediaService.Setup(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == listingId), It.IsAny<Guid>(), It.IsAny<int>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResource(listingId, mediaId));
            this.mediaService.Verify(x => x.Resource.DeleteByIdAsync(It.Is<Guid>(x => x == listingId), It.Is<Guid>(x => x == mediaId), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteResources_MediaDeleted_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.mediaService.Setup(x => x.Resource.DeleteAsync(It.Is<Guid>(x => x == listingId), It.IsAny<bool>())).Verifiable();

            // Act and Assert
            Assert.IsAssignableFrom<OkResult>(await this.Sut.DeleteResources(listingId));
            this.mediaService.Verify(x => x.Resource.DeleteAsync(It.Is<Guid>(x => x == listingId), It.IsAny<bool>()), Times.Once);
        }
    }
}
