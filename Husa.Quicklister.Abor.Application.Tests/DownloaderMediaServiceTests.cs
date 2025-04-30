namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.CTX.Api.Contracts.Response;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Husa.Quicklister.Extensions.ServiceBus.Contracts;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class DownloaderMediaServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ExtensionsInterfaces.ISaleListingMediaService> mediaService = new();
        private readonly Mock<ILogger<MediaService>> logger = new();
        private readonly Mock<IDownloaderCtxClient> downloaderCtxClient = new();
        private readonly Mock<IImportMlsMediaMessagingService> saleListingMediaMessagingService = new();
        private readonly Mock<IUserContextProvider> userContext = new();
        private readonly Mock<IProvideTraceId> provideTraceId = new();

        public DownloaderMediaServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new MediaService(
                this.listingSaleRepository.Object,
                this.mediaService.Object,
                this.downloaderCtxClient.Object,
                this.saleListingMediaMessagingService.Object,
                this.userContext.Object,
                this.provideTraceId.Object,
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
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<SaleListingOpenHouse>()))
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
               .Setup(r => r.Resource.BulkCreateAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<ListingSaleMediaDto>>()))
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

        [Fact]
        public async Task ImportMediaFromMlsThrowsNotFoundExceptionWhenListingDoesNotExistAsync()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.listingSaleRepository
                .Setup(s => s.GetById(It.Is<Guid>(m => m.Equals(listingId)), It.IsAny<bool>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ImportMediaFromMlsAsync(listingId));

            // Assert
            this.listingSaleRepository.Verify();
            Assert.Equal(listingId, notFoundException.Id);
        }

        [Fact]
        public async Task ImportMediaFromMlsImportsSuccessAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var saleListing = new Mock<SaleListing>();
            saleListing.SetupGet(sl => sl.Id).Returns(listingId);
            saleListing.SetupGet(sl => sl.MlsNumber).Returns(mlsNumber);
            this.listingSaleRepository
                .Setup(s => s.GetById(It.Is<Guid>(m => m.Equals(listingId)), It.IsAny<bool>()))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            this.userContext
                .Setup(s => s.GetCurrentUserId())
                .Returns(userId);

            // Act
            await this.Sut.ImportMediaFromMlsAsync(listingId);

            // Assert
            this.listingSaleRepository.Verify();
            this.saleListingMediaMessagingService.Verify(
                x => x.SendMessage(
                    It.IsAny<IEnumerable<ImportMlsMediaMessage>>(),
                    It.Is<string>(
                        m => m.Equals(userId.ToString())),
                    It.Is<MarketCode>(m => m.Equals(MarketCode.Austin)),
                    It.IsAny<string>(),
                    It.Is<bool>(m => m.Equals(true))),
                Times.Once);
        }
    }
}
