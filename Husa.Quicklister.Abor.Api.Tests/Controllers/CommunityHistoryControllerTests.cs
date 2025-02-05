namespace Husa.Quicklister.Abor.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Document.Models;
    using Husa.Quicklister.Abor.Api.Controllers.Community;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.Community;
    using Husa.Quicklister.Extensions.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Extensions.Data.Documents.Interfaces;
    using Husa.Quicklister.Extensions.Data.Documents.Models;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class CommunityHistoryControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<ICommunityHistoryQueriesRepository> queryRepository = new();
        private readonly Mock<ILogger<CommunityHistoryController>> logger = new();

        public CommunityHistoryControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new CommunityHistoryController(
                this.queryRepository.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public CommunityHistoryController Sut { get; set; }

        [Fact]
        public async Task GetAsync_Success()
        {
            // Arrange
            var requests = new List<CommunityHistoryQueryResult>()
            {
                new(),
                new(),
            };
            var requestsResult = new DocumentGridQueryResult<CommunityHistoryQueryResult>(requests, null);
            requestsResult.Total = requests.Count;

            this.queryRepository
                .Setup(u => u.GetAsync(It.IsAny<CommunityHistoryQueryFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(requestsResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetAsync(new CommunityHistoryFilter());

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DocumentGridResponse<CommunityHistoryQueryResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task GetSummaryAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            var requestsResult = new[]
            {
                new SummarySectionQueryResult() { Name = "section1" },
                new SummarySectionQueryResult() { Name = "section2" },
            };

            this.queryRepository
                .Setup(u => u.GetSummaryAsync(It.Is<Guid>(id => id == requestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(requestsResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetSummaryAsync(requestId);

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<IEnumerable<SummarySectionContract>>(okObjectResult.Value);
            Assert.Equal(2, result.Count());
        }
    }
}
