namespace Husa.Quicklister.Abor.Api.Tests.PhotoRequest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.PhotoService.Api.Contracts.Response;
    using Husa.Quicklister.Abor.Api.Contracts.Response.PhotoRequest;
    using Husa.Quicklister.Abor.Api.Controllers.PhotoRequest;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class SaleListingPropertyControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleQueriesRepository> listingSaleQueriesRepository;
        private readonly Mock<ILogger<SaleListingPropertiesController>> logger;
        public SaleListingPropertyControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ??
              throw new ArgumentNullException(nameof(fixture));
            this.listingSaleQueriesRepository = new Mock<IListingSaleQueriesRepository>();
            this.logger = new Mock<ILogger<SaleListingPropertiesController>>();
            this.Sut = new SaleListingPropertiesController(this.listingSaleQueriesRepository.Object, this.fixture.Mapper, this.logger.Object);
        }

        public SaleListingPropertiesController Sut { get; set; }

        [Fact]
        public async Task GetProperty_Success()
        {
            //// Arrange
            var listingId = Guid.NewGuid();
            var propertyResponse = new Property
            {
                StreetName = "test",
                StreetNum = "1234",
                CompletedPhotoRequestCount = 1,
                Id = listingId,
            };

            this.listingSaleQueriesRepository.Setup(m => m.GetListingPhotoProperty(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(propertyResponse)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(listingId, listingId);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<PhotoRequestPropertyResponse>(okObjectResult.Value);
            Assert.NotNull(result);
            Assert.Equal(result.Id, listingId);
            this.listingSaleQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetProperty_Null()
        {
            //// Arrange
            var listingId = Guid.NewGuid();
            Property propertyResponse = null;
            this.listingSaleQueriesRepository.Setup(m => m.GetListingPhotoProperty(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(propertyResponse)
                .Verifiable();

            //// Act
            var actionResult = await this.Sut.GetAsync(listingId, listingId);
            //// Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            Assert.Null(okObjectResult.Value);
            this.listingSaleQueriesRepository.Verify();
        }
    }
}
