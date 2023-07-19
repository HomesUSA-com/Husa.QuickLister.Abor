namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class SaleListingPhotoServiceTests : PhotoServiceTests<ISaleListingPhotoService>
    {
        private readonly Mock<IListingSaleRepository> listingRepository;
        private readonly Mock<ILogger<SaleListingPhotoService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;

        public SaleListingPhotoServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<SaleListingPhotoService>>();
            this.listingRepository = new Mock<IListingSaleRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new SaleListingPhotoService(
                this.fixture.BusOptions.Object,
                this.userContextProvider.Object,
                this.photoServiceClient.Object,
                this.client.Object,
                this.traceIdProvider.Object,
                this.listingRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_Success()
        {
            // Arrange & Act
            var entityId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(entityId, createStub: false);
            this.listingRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(listing)
                .Verifiable();
            await this.SetupAssignLatestPhotoRequest(entityId);

            // Assert
            this.listingRepository.Verify(t => t.GetById(entityId, false), Times.Once);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_NotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            SaleListing listing = null;
            this.listingRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(listing)
                .Verifiable();

            var photorequestId = Guid.NewGuid();
            var creationDate = DateTime.UtcNow;

            // Act
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.AssignLatestPhotoRequest(entityId, photorequestId, creationDate));
        }

        protected override void SetupValidEntityAndUser(Guid entityId, Guid userId)
        {
            var companyId = Guid.NewGuid();

            var listing = TestModelProvider.GetListingSaleEntity(entityId, createStub: false, companyId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.listingRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
