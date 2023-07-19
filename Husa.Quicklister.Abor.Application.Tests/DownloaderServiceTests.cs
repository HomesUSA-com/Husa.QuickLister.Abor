namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Downloader.Sabor.Api.Contracts.Response;
    using Husa.Downloader.Sabor.Client;
    using Husa.Downloader.Sabor.Client.Interfaces;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.MediaService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Application.Interfaces.Downloader;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Services;
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
        private readonly Mock<IDownloaderSaborClient> downloaderSaborClient = new();
        private readonly Mock<ILogger<DownloaderService>> logger = new();

        public DownloaderServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new DownloaderService(
                this.listingSaleRepository.Object,
                this.downloaderSaborClient.Object,
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
            var hoasDto = TestModelProvider.GetHoasDtoList();

            this.serviceSubscriptionClient.Setup(s => s.Company.GetAsync(
                It.IsAny<Request.CompanyRequest>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<Response.Company>(Array.Empty<Response.Company>(), total: 0))
            .Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<Response.Company>>(() => this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, hoasDto, sellingAgent));

            // Assert
            this.serviceSubscriptionClient.Verify();
            Assert.Equal(fullListingSaleDto.SaleProperty.SalePropertyInfo.OwnerName, notFoundException.Id);
        }

        [Fact]
        public async Task WhenCallProcessDataFromDownloaderAsyncAndListingNotExistsAndSuccess_ListingSaleIsCreatesOnDatabase()
        {
            // Arrange
            const string sellingAgent = "some-listing-agent";
            var agentId = Guid.NewGuid();
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var roomsDto = TestModelProvider.GetRoomsDtoList();
            var hoasDto = TestModelProvider.GetHoasDtoList();

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
                .Setup(r => r.GetAgentByLoginName(It.Is<string>(x => x == sellingAgent)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, hoasDto, sellingAgent);

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
            const string sellingAgent = "some-listing-agent";
            var agentId = Guid.NewGuid();
            var listingSaleId = Guid.NewGuid();
            var fullListingSaleDto = TestModelProvider.GetFullListingSaleDto();
            var roomsDto = TestModelProvider.GetRoomsDtoList();
            var hoasDto = TestModelProvider.GetHoasDtoList();

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
                .Setup(r => r.GetAgentByLoginName(It.Is<string>(x => x == sellingAgent)))
                .ReturnsAsync(agent.Object);

            // Act
            await this.Sut.ProcessDataFromDownloaderAsync(fullListingSaleDto, roomsDto, hoasDto, sellingAgent);

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
        public async Task ProcessMediaFromDownloaderThrowsNotFoundExceptionWhenListingDoesNotExistAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            this.listingSaleRepository
                .Setup(s => s.GetListingByMlsNumber(It.Is<string>(m => m.Equals(mlsNumber))))
                .ReturnsAsync((SaleListing)null)
                .Verifiable();

            // Act
            var notFoundException = await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ProcessMediaFromDownloaderAsync(
                mlsNumber,
                mediaDto: Array.Empty<ListingSaleMediaDto>()));

            // Assert
            this.listingSaleRepository.Verify();
            Assert.Equal(mlsNumber, notFoundException.Id);
        }

        [Fact(Skip = "fail")]
        public async Task ProcessMediaFromDownloaderNoMediaUpdatedWhenCountIsTheSameAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            var listingId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var saleListing = new Mock<SaleListing>();
            saleListing.SetupGet(sl => sl.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(s => s.GetListingByMlsNumber(It.Is<string>(m => m.Equals(mlsNumber))))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mediaDetail = new MediaDetail { Id = mediaId };
            this.mediaService
                .Setup(ms => ms.GetAsync(It.Is<Guid>(id => id == listingId)))
                .ReturnsAsync(new ResourceResponse
                {
                    Media = new[] { mediaDetail },
                })
                .Verifiable();
            var listingMedia = new ListingSaleMediaDto { MediaId = mediaId.ToString() };

            // Act
            await this.Sut.ProcessMediaFromDownloaderAsync(mlsNumber, mediaDto: new[] { listingMedia });

            // Assert
            this.listingSaleRepository.Verify();
            this.mediaService.Verify();
            this.mediaService.VerifyGet(ms => ms.Resource, Times.Never);
        }

        [Fact]
        public async Task ProcessMediaFromDownloaderMediaUpdatedWhenCountIsNotTheSameAsync()
        {
            // Arrange
            const string mlsNumber = "33669911";
            var listingId = Guid.NewGuid();
            var mediaId = Guid.NewGuid();
            var saleListing = new Mock<SaleListing>();
            saleListing.SetupGet(sl => sl.Id).Returns(listingId);
            this.listingSaleRepository
                .Setup(s => s.GetListingByMlsNumber(It.Is<string>(m => m.Equals(mlsNumber))))
                .ReturnsAsync(saleListing.Object)
                .Verifiable();
            var mediaDetail = new MediaDetail { Id = mediaId };
            var listingMedia = GetMediaToProcess();
            this.mediaService
                .Setup(ms => ms.GetAsync(It.Is<Guid>(id => id == listingId)))
                .ReturnsAsync(new ResourceResponse
                {
                    Media = new[] { mediaDetail },
                })
                .Verifiable();
            var resourceMock = new Mock<IResourceService>();
            this.mediaService
                .SetupGet(ms => ms.Resource)
                .Returns(resourceMock.Object);

            // Act
            await this.Sut.ProcessMediaFromDownloaderAsync(mlsNumber, mediaDto: listingMedia);

            // Assert
            this.listingSaleRepository.Verify();
            this.mediaService.VerifyGet(ms => ms.Resource, Times.AtLeast(1));
            this.mediaService.Verify(r => r.Resource.DeleteAsync(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
            resourceMock.Verify(r => r.BulkCreateAsync(It.Is<Guid>(id => id == listingId), It.IsAny<MarketCode>(), It.IsAny<IEnumerable<ListingSaleMediaDto>>(), It.IsAny<int>()), Times.Once);
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

            this.downloaderSaborClient.SetupGet(d => d.MlsMedia).Returns(mlsMediaResource.Object);

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

            this.downloaderSaborClient.SetupGet(d => d.MlsMedia).Returns(mlsMediaResource.Object);

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

        private static IEnumerable<ListingSaleMediaDto> GetMediaToProcess(int mediaToCreate = 5)
        {
            for (int i = 1; i <= mediaToCreate; i++)
            {
                yield return new()
                {
                    MediaId = Guid.NewGuid().ToString(),
                    Height = 10,
                    Width = 10,
                    Order = i,
                    Title = $"Title Image {i}",
                    Uri = "https://www.google.com/",
                };
            }
        }
    }
}
