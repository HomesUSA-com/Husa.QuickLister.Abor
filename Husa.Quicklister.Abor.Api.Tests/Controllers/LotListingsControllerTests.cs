namespace Husa.Quicklister.Abor.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Document.Models;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Controllers.LotListing;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Media;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.Listing;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class LotListingsControllerTests
    {
        private readonly Mock<ILotListingQueriesRepository> lotListingQueriesRepository = new();
        private readonly Mock<ILotListingRequestQueriesRepository> requestQueryRepository = new();
        private readonly Mock<IMediaService> mediaService = new();
        private readonly Mock<ILotListingService> listingService = new();
        private readonly Mock<ILogger<LotListingsController>> logger = new();

        public LotListingsControllerTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new LotListingsController(
                this.lotListingQueriesRepository.Object,
                this.requestQueryRepository.Object,
                this.listingService.Object,
                this.mediaService.Object,
                this.logger.Object,
                fixture.Mapper);
        }

        protected LotListingsController Sut { get; }

        [Fact]
        public async Task GetRequestByListingAsync_Success()
        {
            var listingId = Guid.NewGuid();
            var listings = new ListingRequestQueryResult[]
            {
                new(),
                new(),
            };

            this.requestQueryRepository
                .Setup(x => x.GetAsync(It.Is<ListingRequestQueryFilter>(x => x.ListingId == listingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DocumentGridQueryResult<ListingRequestQueryResult>(listings, null))
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetRequestByListingAsync(listingId);

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<ListingRequestQueryResponse>>(okObjectResult.Value);
            Assert.Equal(listings.Length, result.Count());
        }

        [Fact]
        public async Task UpdateMlsNumberAsync_Success()
        {
            var listingId = Guid.NewGuid();
            var mlsNumberRequest = new MlsNumberRequest()
            {
                MlsNumber = "1234567",
            };

            // Act
            var actionResult = await this.Sut.UpdateMlsNumber(listingId, mlsNumberRequest);

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsAssignableFrom<OkResult>(actionResult);
        }
    }
}
