namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Media
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.MediaService.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Api.Controllers.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Media;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class SaleListingRequestVirtualToursControllerTests
    {
        private readonly Mock<ISaleListingRequestMediaService> mediaService = new();
        private readonly Mock<ILogger<SaleListingRequestVirtualToursController>> logger = new();

        public SaleListingRequestVirtualToursControllerTests()
        {
            var virtualTourService = new Mock<IVirtualTourService>();
            this.mediaService.Setup(x => x.VirtualTour).Returns(virtualTourService.Object);
            this.Sut = new SaleListingRequestVirtualToursController(this.mediaService.Object, this.logger.Object);
        }

        public SaleListingRequestVirtualToursController Sut { get; set; }

        [Fact]
        public async Task CreateVirtualTourAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            await this.Sut.CreateVirtualTourAsync(requestId, new VirtualTour()
            {
                Title = "Title",
                Uri = new Uri("https://sonarcloud.io/"),
            });

            // Assert
            this.mediaService.Verify(x => x.VirtualTour.CreateAsync(requestId, It.IsAny<VirtualTour>()), Times.Once);
        }

        [Fact]
        public async Task DeleteVirtualTour_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();

            // Act
            await this.Sut.DeleteVirtualTour(requestId, mediaId);

            // Assert
            this.mediaService.Verify(x => x.VirtualTour.DeleteById(requestId, mediaId), Times.Once);
        }
    }
}
