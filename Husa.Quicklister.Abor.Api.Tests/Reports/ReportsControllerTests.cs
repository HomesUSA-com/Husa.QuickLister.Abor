namespace Husa.Quicklister.Abor.Api.Tests.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Contracts.Response.Reports;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Models;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class ReportsControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IScrapedListingQueriesRepository> scrapedListingQueriesRepository;
        private readonly Mock<ILogger<ReportsController>> logger;
        public ReportsControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.scrapedListingQueriesRepository = new Mock<IScrapedListingQueriesRepository>();
            this.logger = new Mock<ILogger<ReportsController>>();

            this.Sut = new ReportsController(
                this.scrapedListingQueriesRepository.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public ReportsController Sut { get; set; }

        [Fact]
        public void Repository_IsNull_Error()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new ReportsController(null, this.fixture.Mapper, this.logger.Object));
        }

        [Fact]
        public void Mapper_IsNull_Error()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new ReportsController(this.scrapedListingQueriesRepository.Object, null, this.logger.Object));
        }

        [Fact]
        public void Logger_IsNull_Error()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => new ReportsController(this.scrapedListingQueriesRepository.Object, this.fixture.Mapper, null));
        }

        [Fact]
        public async Task GetAsync_AllListings_Success()
        {
            // Arange
            var entry = TestModelProvider.GetScrapedListingQueryResult(null, 0, 0);
            var entry2 = TestModelProvider.GetScrapedListingQueryResult(null, 100000, 110000);
            var entry3 = TestModelProvider.GetScrapedListingQueryResult(null, 100000, 110000, "myTestBuilder2");
            var scrapedListingQueryResult = new List<ScrapedListingQueryResult>()
            {
                entry,
                entry2,
                entry3,
            };
            var scrapedListingRequestFilter = TestModelProvider.GetScrapedListingRequestFilter(string.Empty);

            this.scrapedListingQueriesRepository
                .Setup(c => c.GetAsync(It.IsAny<ScrapedListingQueryFilter>()))
                .ReturnsAsync(scrapedListingQueryResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetComparisonReportAsync(scrapedListingRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<ScrapedListingQueryResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
            this.scrapedListingQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetAsync_ListingFound_Success()
        {
            // Arange
            var entry = TestModelProvider.GetScrapedListingQueryResult(null, 0, 0);
            var entry2 = TestModelProvider.GetScrapedListingQueryResult(null, 100000, 110000);
            var scrapedListingQueryResult = new List<ScrapedListingQueryResult>()
            {
                entry,
                entry2,
            };
            var scrapedListingRequestFilter = TestModelProvider.GetScrapedListingRequestFilter();

            this.scrapedListingQueriesRepository
                .Setup(c => c.GetAsync(It.IsAny<ScrapedListingQueryFilter>()))
                .ReturnsAsync(scrapedListingQueryResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetComparisonReportAsync(scrapedListingRequestFilter);

            // Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<ScrapedListingQueryResponse>>(okObjectResult.Value);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            this.scrapedListingQueriesRepository.Verify();
        }

        [Fact]
        public async Task GetAsync_ListingNotFound_Error()
        {
            // Arange
            var scrapedListingQueryResult = new List<ScrapedListingQueryResult>();
            var scrapedListingRequestFilter = TestModelProvider.GetScrapedListingRequestFilter();

            this.scrapedListingQueriesRepository
                .Setup(c => c.GetAsync(It.IsAny<ScrapedListingQueryFilter>()))
                .ReturnsAsync(scrapedListingQueryResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetComparisonReportAsync(scrapedListingRequestFilter);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<ScrapedListingQueryResponse>>(objectResult.Value);
            Assert.Empty(result);
            this.scrapedListingQueriesRepository.Verify();
        }
    }
}
