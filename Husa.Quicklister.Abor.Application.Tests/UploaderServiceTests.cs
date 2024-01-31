namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.ReverseProspect;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.ReverseProspect.Api.Client;
    using Husa.ReverseProspect.Api.Contracts.Response;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class UploaderServiceTests
    {
        private readonly Mock<IListingSaleRepository> saleListingRepository = new();
        private readonly Mock<IReverseProspectClient> reverseProspectClient = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IReverseProspectRepository> reverseProspectRepository = new();
        private readonly Mock<ILogger<UploaderService>> logger;
        private readonly Mock<IMapper> mapper = new();

        public UploaderServiceTests(ApplicationServicesFixture fixture)
        {
            this.logger = new Mock<ILogger<UploaderService>>();
            this.Sut = new UploaderService(
                this.saleListingRepository.Object,
                this.reverseProspectRepository.Object,
                this.reverseProspectClient.Object,
                this.userContextProvider.Object,
                this.logger.Object,
                this.mapper.Object);
        }

        public UploaderService Sut { get; set; }

        [Fact]
        public async Task GetReverseProspectListingNotFoundListing()
        {
            var listingId = Guid.Empty;
            var usingDatabase = true;

            // Arrange
            this.saleListingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync((SaleListing)null);

            // Act && Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.GetReverseProspectListing(listingId, usingDatabase));
            this.saleListingRepository.Verify(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task GetReverseProspectListing()
        {
            var listingId = Guid.Empty;
            var userId = Guid.NewGuid();
            var usingDatabase = true;
            var listing = TestModelProvider.GetListingSaleEntity(listingId);
            var user = TestModelProvider.GetCurrentUser(userId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            var trackingReverseProspect = new ReverseProspect(
                listingId,
                userId,
                Guid.NewGuid(),
                "[{\"Agent\":\"Joe Corwin\", \"Email\":\"joe@joecorwin.com\", \"DateSent\":\"11/29/2021\", \"InterestLevel\":\"Interested\"}]",
                ReverseProspectStatus.Available);

            // Arrange
            this.userContextProvider
                .Setup(x => x.GetCurrentUserId())
                .Returns(userId);

            this.saleListingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing);

            this.reverseProspectRepository
                .Setup(x => x.AddAsync(It.IsAny<ReverseProspect>()))
                .Verifiable();

            this.reverseProspectRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(trackingReverseProspect)
                .Verifiable();

            this.reverseProspectRepository
                .Setup(x => x.GetReverseProspectByTrackingId(It.IsAny<Guid>()))
                .ReturnsAsync(trackingReverseProspect)
                .Verifiable();

            // Act
            var reverseProspectData = await this.Sut.GetReverseProspectListing(listingId, usingDatabase);

            // Assert
            this.saleListingRepository.Verify(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
            this.reverseProspectRepository.Verify(sl => sl.AddAsync(It.IsAny<ReverseProspect>()), Times.Once);
            Assert.NotEmpty(reverseProspectData.Results);
            Assert.Equal(ResponseCode.Success, reverseProspectData.Code);
        }

        [Fact]
        public async Task GetReverseProspectListingFound()
        {
            var listingId = Guid.Empty;
            var userId = Guid.NewGuid();
            var usingDatabase = true;
            var listing = TestModelProvider.GetListingSaleEntity(listingId);
            var user = TestModelProvider.GetCurrentUser(userId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            listing.MlsNumber = "12345678";

            var trackingReverseProspect = new ReverseProspectData()
            {
                Agent = "test",
                DateSent = DateTime.Now.ToString(),
                Email = "email@domein.com",
                InterestLevel = null,
            };

            // Arrange
            this.userContextProvider
                .Setup(x => x.GetCurrentUserId())
                .Returns(userId);

            this.saleListingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing);

            this.reverseProspectRepository
                .Setup(x => x.AddAsync(It.IsAny<ReverseProspect>()))
                .Verifiable();

            this.reverseProspectRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((ReverseProspect)null)
                .Verifiable();

            this.reverseProspectClient
                .Setup(x => x.ReverseProspectRequest.GetReverseProspectData(It.IsAny<MarketCode>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CommandResult<ReverseProspectData>.Success(trackingReverseProspect))
                .Verifiable();

            // Act
            var reverseProspectData = await this.Sut.GetReverseProspectListing(listingId, usingDatabase);

            // Assert
            this.saleListingRepository.Verify(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
            this.reverseProspectRepository.Verify(sl => sl.AddAsync(It.IsAny<ReverseProspect>()), Times.AtMost(2));
            Assert.NotEmpty(reverseProspectData.Results);
        }

        [Fact]
        public async Task GetReverseProspectListingErrorResponse()
        {
            var listingId = Guid.Empty;
            var userId = Guid.NewGuid();
            var usingDatabase = true;
            var listing = TestModelProvider.GetListingSaleEntity(listingId);
            var user = TestModelProvider.GetCurrentUser(userId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            listing.MlsNumber = "12345678";

            var response = CommandResult<ReverseProspectData>.Error("No data Found");
            response.Code = ResponseCode.Error;

            // Arrange
            this.userContextProvider
                .Setup(x => x.GetCurrentUserId())
                .Returns(userId);

            this.saleListingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing);

            this.reverseProspectRepository
                .Setup(x => x.AddAsync(It.IsAny<ReverseProspect>()))
                .Verifiable();

            this.reverseProspectRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync((ReverseProspect)null)
                .Verifiable();

            this.reverseProspectClient
                .Setup(x => x.ReverseProspectRequest.GetReverseProspectData(It.IsAny<MarketCode>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var reverseProspectData = await this.Sut.GetReverseProspectListing(listingId, usingDatabase);

            // Assert
            this.saleListingRepository.Verify(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
            this.reverseProspectRepository.Verify(sl => sl.AddAsync(It.IsAny<ReverseProspect>()), Times.AtMost(2));
            Assert.Empty(reverseProspectData.Results);
            Assert.Equal(ResponseCode.Error, reverseProspectData.Code);
        }
    }
}
