namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.CTX.Api.Contracts.Response;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class DownloaderMediaServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ISaleListingMediaService> mediaService = new();
        private readonly Mock<ILogger<MediaService>> logger = new();
        private readonly Mock<IDownloaderCtxClient> downloaderCtxClient = new();

        public DownloaderMediaServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new MediaService(
                this.listingSaleRepository.Object,
                this.mediaService.Object,
                this.downloaderCtxClient.Object,
                this.fixture.Options.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public IMediaService Sut { get; set; }

        [Fact]
        public async Task DownloadImagesSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var updatedOn = DateTime.UtcNow;
            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<IEnumerable<SaleListingOpenHouse>>()))
                .Returns(true);
            listing.SetupGet(l => l.SaleProperty).Returns(saleProperty.Object);

            var mediaDetailList = new Mock<List<MediaDetailResponse>>();
            mediaDetailList.Object.Add(new MediaDetailResponse
            {
                Uri = "www.google.com",
                EntityKey = "324324",
                Height = 35,
                Id = Guid.NewGuid(),
                Width = 34,
                Order = 1,
                ResidentialId = listingId,
                ResourceRecordKey = "3243ree",
            });

            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object).Verifiable();

            this.downloaderCtxClient
                .Setup(r => r.Residential.GetResidentialMediaByIdAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediaDetailList.Object).Verifiable();

            this.mediaService
                .Setup(r => r.Resource.DeleteAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Verifiable();

            this.mediaService
               .Setup(r => r.Resource.BulkCreateAsync(It.IsAny<Guid>(), It.IsAny<MarketCode>(), It.IsAny<IEnumerable<ListingSaleMediaDto>>(), It.IsAny<int>()))
               .Verifiable();

            // Act
            await this.Sut.ProcessData(listingId, updatedOn);

            // Assert
            this.listingSaleRepository.Verify(r => r.GetById(It.Is<Guid>(mls => mls == listingId), It.IsAny<bool>()), Times.Once);
            this.downloaderCtxClient.Verify(r => r.Residential.GetResidentialMediaByIdAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ProcessMediaFromDownloaderThrowsNotFoundExceptionWhenListingDoesNotExistAsync()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var updatedOn = DateTime.UtcNow;
            SaleListing listing = null;

            this.listingSaleRepository
                .Setup(r => r.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listing).Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ProcessData(
               listingId, updatedOn));

            // Assert
            this.listingSaleRepository.Verify();
            Assert.Equal(listingId, notFoundException.Id);
        }
    }
}
