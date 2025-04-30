namespace Husa.Quicklister.Abor.Application.Tests.Services.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.Sabor.Client;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Application.Services.Reports;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class DiscrepancyReportServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> saleListingRepository = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IXmlClient> xmlClient = new();
        private readonly Mock<IDownloaderCtxClient> downloaderCtxClient = new();
        private readonly Mock<IDownloaderSaborClient> downloaderSaborClient = new();
        private readonly Mock<ILogger<DiscrepancyReportService>> logger = new();
        public DiscrepancyReportServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task GetDiscrepancyAnalysisAsyncSuccess()
        {
            // Arrange
            var user = TestModelProvider.GetCurrentUser();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var listingIds = new List<Tuple<Guid, Guid, string>>
            {
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-1"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-2"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-3"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-4"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-5"),
            };
            var listings = DiscrepancyTestProvider.GenerateListing(listingIds);
            var xmlListings = DiscrepancyTestProvider.GenerateXmlListingResponse(listingIds);
            var trestleListings = DiscrepancyTestProvider.GenerateTrestleListingResponse(listingIds);
            this.saleListingRepository
                .Setup(c => c.GetListingsForDiscrepancyAsync(It.IsAny<bool>()))
                .ReturnsAsync(listings)
                .Verifiable();
            this.xmlClient.Setup(c => c.Listing.GetAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlListings)
                .Verifiable();
            this.downloaderCtxClient.Setup(c => c.Residential.GetListingsByMlsNumbers(It.IsAny<IEnumerable<string>>(), It.IsAny<MarketCode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(trestleListings)
                .Verifiable();
            var sut = this.GetSut();
            var result = await sut.GetDiscrepancyAnalysisAsync();

            // Assert
            this.saleListingRepository.Verify(r => r.GetListingsForDiscrepancyAsync(It.Is<bool>(b => !b)), Times.Once);
            this.xmlClient.Verify(r => r.Listing.GetAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            this.downloaderCtxClient.Verify(r => r.Residential.GetListingsByMlsNumbers(It.IsAny<IEnumerable<string>>(), It.IsAny<MarketCode>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(5, result.XmlListings);
            Assert.Equal(5, result.ListingsInBoth);
            Assert.Equal(5, result.ListingsPriceDiscrepancy);
            Assert.Equal(5, result.MlsListings);
        }

        [Fact]
        public async Task GetDiscrepancyDetailAsyncSuccess()
        {
            // Arrange
            var skip = 0;
            var top = 50;
            var user = TestModelProvider.GetCurrentUser();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var listingIds = new List<Tuple<Guid, Guid, string>>
            {
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-1"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-2"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-3"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-4"),
                new Tuple<Guid, Guid, string>(Guid.NewGuid(), Guid.NewGuid(), "mlsnumber-5"),
            };
            var listings = DiscrepancyTestProvider.GenerateListing(listingIds);
            var xmlListings = DiscrepancyTestProvider.GenerateXmlListingResponse(listingIds);
            var trestleListings = DiscrepancyTestProvider.GenerateTrestleListingResponse(listingIds);
            this.saleListingRepository
                .Setup(c => c.GetListingsForDiscrepancyAsync(It.IsAny<bool>()))
                .ReturnsAsync(listings)
                .Verifiable();
            this.xmlClient.Setup(c => c.Listing.GetAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(xmlListings)
                .Verifiable();
            this.downloaderCtxClient.Setup(c => c.Residential.GetListingsByMlsNumbers(It.IsAny<IEnumerable<string>>(), It.IsAny<MarketCode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(trestleListings)
                .Verifiable();
            var sut = this.GetSut();
            var result = await sut.GetDiscrepancyDetail(skip, top);

            // Assert
            this.saleListingRepository.Verify(r => r.GetListingsForDiscrepancyAsync(It.Is<bool>(b => b)), Times.Once);
            this.xmlClient.Verify(r => r.Listing.GetAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
            this.downloaderCtxClient.Verify(r => r.Residential.GetListingsByMlsNumbers(It.IsAny<IEnumerable<string>>(), It.IsAny<MarketCode>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(5, result.Total);
        }

        private DiscrepancyReportService GetSut()
        {
            return new DiscrepancyReportService(
                this.saleListingRepository.Object,
                this.userContextProvider.Object,
                this.xmlClient.Object,
                this.downloaderCtxClient.Object,
                this.downloaderSaborClient.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }
    }
}
