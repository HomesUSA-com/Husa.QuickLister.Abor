namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.QueryFilters;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Api.Client.Tests")]
    public class SaleListingRequestClientResourceTest
    {
        private readonly CustomWebApplicationFactory<TestStartup> customWebApplicationFactory;
        private readonly QuicklisterAborClient quicklisterClient;

        public SaleListingRequestClientResourceTest(CustomWebApplicationFactory<TestStartup> customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory ?? throw new ArgumentNullException(nameof(customWebApplicationFactory));
            var client = customWebApplicationFactory.GetClient();
            var loggerFactory = this.customWebApplicationFactory.Services.GetRequiredService<ILoggerFactory>();
            this.quicklisterClient = new QuicklisterAborClient(loggerFactory, client);
        }

        [Fact]
        public async Task GetSaleListingRequestSuccessAsync()
        {
            // Arrange
            const string continuationToken = "continuation-token";
            var requestId = Guid.NewGuid();
            var filter = new ListingSaleRequestFilter()
            {
                RequestState = ListingRequestState.Pending,
            };

            var requestResult = new ListingSaleRequestQueryResult
            {
                Address = "1234 sample",
                City = Cities.AlamoHeights,
                Id = requestId,
                ListPrice = 123000,
                ZipCode = "75035",
                MlsStatus = MarketStatuses.ActiveRFR,
            };

            var gridResult = new ListingRequestGridQueryResult<ListingSaleRequestQueryResult>(new[] { requestResult }, continuationToken);
            var repository = this.customWebApplicationFactory.Services.GetRequiredService<ISaleListingRequestQueriesRepository>();
            var repositoryMock = Mock.Get(repository);
            repositoryMock
                .Setup(repository => repository.GetListingSaleRequestsAsync(It.IsAny<ListingSaleRequestQueryFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(gridResult);

            // Act
            var listings = await this.quicklisterClient.ListingSaleRequest.GetListRequestAsync(filter);

            // Assert
            Assert.Equal(continuationToken, listings.ContinuationToken);
            var result = Assert.Single(listings.Data);
            Assert.Equal(requestResult.Id, result.Id);
            Assert.Equal(requestResult.MlsStatus, result.MlsStatus);
            Assert.Equal(requestResult.City, result.City);
            Assert.Equal(requestResult.ZipCode, result.ZipCode);
            Assert.Equal(requestResult.ListPrice, result.ListPrice);
            Assert.Equal(requestResult.Address, result.Address);
        }
    }
}
