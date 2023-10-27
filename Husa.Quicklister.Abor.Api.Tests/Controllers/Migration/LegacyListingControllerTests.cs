namespace Husa.Quicklister.Abor.Api.Tests.Controllers.Migration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Controllers.Migration;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class LegacyListingControllerTests
    {
        private readonly Mock<ILogger<LegacyListingController>> logger = new();
        private readonly Mock<IMigrationQueryRepository> migrationQueryRepository = new();

        public LegacyListingControllerTests()
        {
            this.Sut = new LegacyListingController(
                this.migrationQueryRepository.Object,
                this.logger.Object);
        }

        public LegacyListingController Sut { get; set; }

        [Fact]
        public async Task PendingRequestAsync_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var toModifiedOn = DateTime.Now;
            var listings = new ListingMigrationQueryResult[]
                {
                   new Mock<ListingMigrationQueryResult>().Object,
                   new Mock<ListingMigrationQueryResult>().Object,
                };
            this.migrationQueryRepository
                .Setup(u => u.GetListingsToLock(It.Is<Guid?>(x => x == companyId), It.Is<DateTime?>(x => x == toModifiedOn), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listings)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetListings(companyId, toModifiedOn);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<ListingMigrationQueryResult>>(okObjectResult.Value);
            Assert.Equal(listings.Length, result.Count());
        }
    }
}
