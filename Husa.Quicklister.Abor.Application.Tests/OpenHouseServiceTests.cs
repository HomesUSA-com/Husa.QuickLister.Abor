namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.OpenHouse;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class OpenHouseServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ILogger<OpenHouseService>> logger = new();

        public OpenHouseServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new OpenHouseService(
                this.listingSaleRepository.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public IOpenHouseService Sut { get; set; }

        [Fact]
        public async Task WhenCallProcessOpenHousesFromDownloaderAsync_AndSalePropertyHasNotOpenHouse_OpenHouseIsAddedSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDto();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            listing.SetupGet(l => l.MlsNumber).Returns(mlsNumber);
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<SaleListingOpenHouse>()))
                .Returns(true);
            listing.SetupGet(l => l.SaleProperty).Returns(saleProperty.Object);

            this.listingSaleRepository
                .Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)))
                .ReturnsAsync(listing.Object).Verifiable();

            // Act
            await this.Sut.ProcessData(mlsNumber, openHousesDto);

            // Assert
            this.listingSaleRepository.Verify(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        [Fact]
        public async Task ProcessOpenHouseFromDownloaderWhenSalePropertyHasOpenHousesInfoNoDataIsAdded()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDto();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            listing.SetupGet(l => l.MlsNumber).Returns(mlsNumber);
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<SaleListingOpenHouse>()))
                .Returns(false);
            listing.SetupGet(l => l.SaleProperty).Returns(saleProperty.Object);

            this.listingSaleRepository
                .Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)))
                .ReturnsAsync(listing.Object).Verifiable();

            // Act
            await this.Sut.ProcessData(mlsNumber, openHousesDto);

            // Assert
            this.listingSaleRepository.Verify(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }

        [Fact]
        public async Task WhenCallProcessOpenHousesFromDownloaderAsync_AndSalePropertyNotFound_OpenHouseIsAddedSuccess()
        {
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDto();

            this.listingSaleRepository.Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber))).Verifiable();

            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ProcessData(mlsNumber, openHousesDto));

            this.listingSaleRepository.Verify(x => x.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }
    }
}
