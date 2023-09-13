namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Downloader.CTX.Api.Client;
    using Husa.Downloader.CTX.Api.Client.Interface;
    using Husa.Downloader.CTX.Api.Contracts.Response;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Services.Downloader;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Agent;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Media;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using Request = Husa.CompanyServicesManager.Api.Contracts.Request;
    using Response = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class DownloaderServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<IAgentRepository> agentRepository = new();
        private readonly Mock<ISaleListingMediaService> mediaService = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IDownloaderCtxClient> downloaderCtxClient = new();
        private readonly Mock<ILogger<DownloaderService>> logger = new();

        public DownloaderServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new DownloaderService(
                this.listingSaleRepository.Object,
                this.downloaderCtxClient.Object,
                this.mediaService.Object,
                this.agentRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.fixture.Options.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public IDownloaderService Sut { get; set; }

        [Fact]
        public async Task ProcessDataFromDownloaderThrowsNotFoundExceptionWhenCompanyIsNotFoundAsync()
        {
            // Arrange
            const string sellingAgent = "some-listing-agent";
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var roomsDto = TestModelProvider.GetRoomsDtoList();

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(Array.Empty<Response.Company>(), total: 0))
            .Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<Response.Company>>(() => this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, sellingAgent));

            // Assert
            this.serviceSubscriptionClient.Verify();
            Assert.Equal(fullListingSaleDto.SaleProperty.SalePropertyInfo.OwnerName, notFoundException.Id);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndListingNotExistsAndSuccess_ListingSaleIsCreatesOnDatabase()
        {
            // Arrange
            const string agentMarketUniqueId = "some-agent-unique-id";
            var agentId = Guid.NewGuid();
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var roomsDto = TestModelProvider.GetRoomsDtoList();

            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            fullListingSaleDto.MlsNumber = mlsNumber;

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(TestModelProvider.GetCompanyInfo(), TestModelProvider.GetCompanyInfo().Count()))
                .Verifiable();

            this.listingSaleRepository.Setup(
                r => r.GetListingByLocationAsync(
                    It.Is<string>(x => x == mlsNumber),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            var agent = new Mock<Agent>();
            agent.SetupGet(a => a.Id).Returns(agentId);
            this.agentRepository
                .Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(x => x == agentMarketUniqueId)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, agentMarketUniqueId);

            // Assert
            this.serviceSubscriptionClient.Verify();
            this.listingSaleRepository.Verify();
            this.listingSaleRepository.Verify(x => x.Attach(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsynsAndListingExistsAndSuccess_ListingSaleIsUpdatedOnDatabase()
        {
            // Arrange
            const string agentMarketUniqueId = "some-agent-unique-id";
            var agentId = Guid.NewGuid();
            var listingSaleId = Guid.NewGuid();
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var roomsDto = TestModelProvider.GetRoomsDtoList();

            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            fullListingSaleDto.MlsNumber = mlsNumber;

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(TestModelProvider.GetCompanyInfo(), TestModelProvider.GetCompanyInfo().Count()));

            this.listingSaleRepository.Setup(
                r => r.GetListingByLocationAsync(
                    It.Is<string>(x => x == mlsNumber),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(TestModelProvider.GetListingSaleEntity(listingSaleId))
                .Verifiable();

            var agent = new Mock<Agent>();
            agent.SetupGet(a => a.Id).Returns(agentId);
            this.agentRepository
                .Setup(r => r.GetAgentByMarketUniqueId(It.Is<string>(x => x == agentMarketUniqueId)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, agentMarketUniqueId);

            // Assert
            this.serviceSubscriptionClient.Verify();
            this.listingSaleRepository.Verify();
            this.listingSaleRepository.Verify(x => x.Attach(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        [Fact]
        public async Task WhenCallProcessOpenHousesFromDownloaderAsync_AndSalePropertyHasNotOpenHouse_OpenHouseIsAddedSuccess()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDtoList();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            listing.SetupGet(l => l.MlsNumber).Returns(mlsNumber);
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<IEnumerable<SaleListingOpenHouse>>()))
                .Returns(true);
            listing.SetupGet(l => l.SaleProperty).Returns(saleProperty.Object);

            this.listingSaleRepository
                .Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)))
                .ReturnsAsync(listing.Object).Verifiable();

            // Act
            await this.Sut.ProcessOpenHouseFromDownloaderAsync(mlsNumber, openHousesDto);

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
            var openHousesDto = TestModelProvider.GetOpenHouseDtoList();

            var listing = new Mock<SaleListing>();
            listing.SetupGet(l => l.Id).Returns(listingId);
            listing.SetupGet(l => l.MlsNumber).Returns(mlsNumber);
            var saleProperty = new Mock<SaleProperty>();
            saleProperty
                .Setup(l => l.ImportOpenHouseInfoFromMarket(It.IsAny<IEnumerable<SaleListingOpenHouse>>()))
                .Returns(false);
            listing.SetupGet(l => l.SaleProperty).Returns(saleProperty.Object);

            this.listingSaleRepository
                .Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)))
                .ReturnsAsync(listing.Object).Verifiable();

            // Act
            await this.Sut.ProcessOpenHouseFromDownloaderAsync(mlsNumber, openHousesDto);

            // Assert
            this.listingSaleRepository.Verify(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }

        [Fact]
        public async Task WhenCallProcessOpenHousesFromDownloaderAsync_AndSalePropertyNotFound_OpenHouseIsAddedSuccess()
        {
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDtoList();

            this.listingSaleRepository.Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber))).Verifiable();

            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ProcessOpenHouseFromDownloaderAsync(mlsNumber, openHousesDto));

            this.listingSaleRepository.Verify(x => x.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }

        [Fact]
        public async Task WhenCallProcessOpenHousesFromDownloaderAsync_AndSalePropertyExistsAndHasOpenhouses_OpenHousesNotAdded()
        {
            var mlsNumber = Faker.RandomNumber.Next(10000, 19000).ToString();
            var openHousesDto = TestModelProvider.GetOpenHouseDtoList();
            this.listingSaleRepository
                .Setup(r => r.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)))
                .ReturnsAsync(TestModelProvider.GetListingSaleWithOpenHouses())
                .Verifiable();

            await this.Sut.ProcessOpenHouseFromDownloaderAsync(mlsNumber, openHousesDto);

            this.listingSaleRepository.Verify(x => x.GetListingByMlsNumber(It.Is<string>(mls => mls == mlsNumber)), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
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
        public async Task ImportMediaFromMlsStopsExecutionWhenNoMediaIsFoundForListingAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            var listingId = Guid.NewGuid();
            var saleListing = new Mock<SaleListing>();
            saleListing.SetupGet(sl => sl.Id).Returns(listingId);
            saleListing.SetupGet(sl => sl.MlsNumber).Returns(mlsNumber);
            this.listingSaleRepository
                .Setup(s => s.GetById(It.Is<Guid>(m => m.Equals(listingId)), It.IsAny<bool>()))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mlsMediaResource = new Mock<IMlsMedia>();
            mlsMediaResource
                .Setup(m => m.ImportSaleListingPhotosAsync(It.Is<string>(m => m.Equals(mlsNumber)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<MediaResponse>())
                .Verifiable();

            this.downloaderCtxClient.SetupGet(d => d.MlsMedia).Returns(mlsMediaResource.Object);

            // Act
            await this.Sut.ImportMediaFromMlsAsync(listingId);

            // Assert
            this.listingSaleRepository.Verify();
            mlsMediaResource.Verify();
            this.mediaService.Verify(m => m.GetAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task ImportMediaFromMlsImportsSuccessAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            var listingId = Guid.NewGuid();
            var saleListing = new Mock<SaleListing>();
            saleListing.SetupGet(sl => sl.Id).Returns(listingId);
            saleListing.SetupGet(sl => sl.MlsNumber).Returns(mlsNumber);
            this.listingSaleRepository
                .Setup(s => s.GetById(It.Is<Guid>(m => m.Equals(listingId)), It.IsAny<bool>()))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mlsMediaResource = new Mock<IMlsMedia>();
            var mediaResponse = new MediaResponse { MediaId = listingId.ToString(), Uri = "https://www.google.com/" };
            mlsMediaResource
                .Setup(m => m.ImportSaleListingPhotosAsync(It.Is<string>(m => m.Equals(mlsNumber)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { mediaResponse })
                .Verifiable();

            this.downloaderCtxClient.SetupGet(d => d.MlsMedia).Returns(mlsMediaResource.Object);

            var resourceMock = new Mock<IResourceService>();
            this.mediaService
                .SetupGet(ms => ms.Resource)
                .Returns(resourceMock.Object)
                .Verifiable();
            var mediaDetail = new MediaDetail { Id = listingId };
            this.mediaService
                .Setup(m => m.GetAsync(It.Is<Guid>(id => id == listingId)))
                .ReturnsAsync(new ResourceResponse { Media = new[] { mediaDetail } });

            // Act
            await this.Sut.ImportMediaFromMlsAsync(listingId);

            // Assert
            this.listingSaleRepository.Verify();
            mlsMediaResource.Verify();
            this.mediaService.Verify();
            this.mediaService.Verify(r => r.Resource.DeleteAsync(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
            resourceMock.Verify(r => r.BulkCreateAsync(It.Is<Guid>(id => id == listingId), It.IsAny<MarketCode>(), It.IsAny<IEnumerable<ListingSaleMediaDto>>(), It.IsAny<int>()), Times.Once);
        }
    }
}
