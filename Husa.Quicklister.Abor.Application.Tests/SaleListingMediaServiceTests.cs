namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Media;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class SaleListingMediaServiceTests : MediaServiceTests<ISaleListingMediaService>
    {
        private readonly Mock<IListingSaleRepository> listingSaleRepository;
        private readonly Mock<ILogger<SaleListingMediaService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;
        private readonly Mock<ICache> cache = new();
        private readonly Mock<IBlobService> blobService = new();
        private readonly Mock<IMapper> mapper = new();

        public SaleListingMediaServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<SaleListingMediaService>>();
            this.listingSaleRepository = new Mock<IListingSaleRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new SaleListingMediaService(
                this.busOptions.Object,
                this.userContextProvider.Object,
                this.mediaServiceClient.Object,
                this.busClient.Object,
                this.traceIdProvider.Object,
                this.listingSaleRepository.Object,
                this.blobService.Object,
                this.cache.Object,
                this.logger.Object,
                this.mapper.Object);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(listingId, userId);

            // Act
            await this.Sut.ValidateEntityAndUserCompany(listingId);

            // Assert
            this.listingSaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_EntityNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            SaleListing listing = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.listingSaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ValidateEntityAndUserCompany(listingId));
            this.listingSaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Never);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_IsNotCompanyEmployee_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingCompanyId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, createStub: false, listingCompanyId);

            var userId = Guid.NewGuid();
            var userCompanyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, userCompanyId);

            this.listingSaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ValidateEntityAndUserCompany(listingId));
            this.listingSaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        [Fact]
        public async Task BulkCreateResourcesSuccessAsync()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userCompanyId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, createStub: false, userCompanyId);
            var user = TestModelProvider.GetCurrentUser(userId, userCompanyId);

            this.listingSaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var listingMedia = new ListingSaleMediaDto
            {
                Height = 10,
                Width = 10,
                Order = 1,
                Title = "test media title",
                UploadKey = Guid.NewGuid().ToString(),
                Uri = "https://www.google.com/",
                MediaId = Guid.NewGuid().ToString(),
            };

            // Act
            await this.Sut.Resource.BulkCreateAsync(listingId, new[] { listingMedia }, 25);

            // Assert
            this.userContextProvider.Verify(r => r.GetCurrentUserId(), Times.Once);
        }

        protected override void SetupValidEntityAndUser(Guid entityId, Guid userId)
        {
            var companyId = Guid.NewGuid();

            var listing = TestModelProvider.GetListingSaleEntity(entityId, createStub: false, companyId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.listingSaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
