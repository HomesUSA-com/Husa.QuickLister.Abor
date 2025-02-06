namespace Husa.Quicklister.Abor.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Document.Models;
    using Husa.Quicklister.Abor.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest.SaleRequest;
    using Husa.Quicklister.Abor.Api.Controllers;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Api.ValidationsRules;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.SaleRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class SaleListingRequestsControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<ISaleListingRequestQueriesRepository> saleRequestQueryRepository = new();
        private readonly Mock<ISaleListingService> listingSaleService = new();
        private readonly Mock<ISaleListingNotesService> listingNotesService = new();
        private readonly Mock<ISaleListingRequestService> saleRequestService = new();
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IValidateListingStatusChanges<ListingSaleRequestForUpdate>> validateListingStatusChanges = new();
        private readonly Mock<ILogger<SaleListingRequestsController>> logger = new();
        private readonly Mock<IShowingTimeContactQueriesRepository> showingTimeContactQueriesRepository = new();

        public SaleListingRequestsControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleListingRequestsController(
                this.saleRequestQueryRepository.Object,
                this.listingSaleService.Object,
                this.listingNotesService.Object,
                this.saleRequestService.Object,
                this.userQueriesRepository.Object,
                this.fixture.Mapper,
                this.validateListingStatusChanges.Object,
                this.showingTimeContactQueriesRepository.Object,
                this.logger.Object);
        }

        public SaleListingRequestsController Sut { get; set; }

        [Fact]
        public async Task GetListRequestAsync_Success()
        {
            // Arrange
            var requests = new List<ListingSaleRequestQueryResult>()
            {
                new(),
                new(),
            };
            var requestsResult = new DocumentGridQueryResult<ListingSaleRequestQueryResult>(requests, null);
            requestsResult.Total = requests.Count;

            this.saleRequestQueryRepository
                .Setup(u => u.GetAsync(It.IsAny<SaleListingRequestQueryFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(requestsResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetListRequestAsync(new SaleListingRequestFilter());

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DocumentGridResponse<ListingSaleRequestQueryResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task PendingRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            await this.Sut.PendingRequestAsync(requestId);

            // Assert
            this.saleRequestService.Verify(x => x.ChangeRequestStatus(It.Is<Guid>(x => x == requestId), It.Is<ListingRequestState>(x => x == ListingRequestState.Pending), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ProcessRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            await this.Sut.ProcessRequestAsync(requestId);

            // Assert
            this.saleRequestService.Verify(x => x.ChangeRequestStatus(It.Is<Guid>(x => x == requestId), It.Is<ListingRequestState>(x => x == ListingRequestState.Processing), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ApproveRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();

            // Act
            await this.Sut.ApproveRequestAsync(requestId);

            // Assert
            this.saleRequestService.Verify(x => x.ChangeRequestStatus(It.Is<Guid>(x => x == requestId), It.Is<ListingRequestState>(x => x == ListingRequestState.Approved), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
