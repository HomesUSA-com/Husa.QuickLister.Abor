namespace Husa.Quicklister.Abor.Application.Tests.Services.ListingRequests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Application.Services.ListingRequests;
    using Husa.Quicklister.Abor.Application.Tests.Providers;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Request;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class ListingRequestJsonImportServiceTests
    {
        private readonly Mock<ISaleListingRequestRepository> requestRepository = new();
        private readonly Mock<ISaleListingRequestMediaService> mediaService = new();
        private readonly Mock<ILogger<ListingRequestJsonImportService>> logger = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IRequestErrorRepository> pequestErrorRepository = new();
        private readonly ListingRequestJsonImportService sut;

        public ListingRequestJsonImportServiceTests()
        {
            this.sut = new(this.mediaService.Object, this.requestRepository.Object, this.userContextProvider.Object, this.pequestErrorRepository.Object, this.logger.Object);
        }

        [Fact]
        public async Task CreateRequestAsync_Success()
        {
            // Arrange
            var jsonListingId = Guid.NewGuid();
            var mlsNumber = "1062023";
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId: listingId);
            listing.MlsNumber = mlsNumber;
            var spec = JsonModelProviders.GetListingResponse(jsonListingId, listingId);

            var requestId = Guid.NewGuid();
            var listingRequest = ListingRequestProviders.GetSaleListingRequestMock(requestId, listingId: listingId, Guid.NewGuid(), requestState: ListingRequestState.Pending);
            var clonedRequest = ListingRequestProviders.GetSaleListingRequestMock(requestId, listingId: listingId, Guid.NewGuid(), requestState: ListingRequestState.Pending);
            listingRequest.Setup(p => p.Clone()).Returns(clonedRequest.Object);
            this.requestRepository
                .Setup(x => x.GetLastCompletedRequestAsync(listingId, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(listingRequest.Object)
                .Verifiable();

            var result = await this.sut.CreateRequestAsync(listing, spec);

            Assert.Equal(ResponseCode.Success, result.Code);
            this.requestRepository.Verify(r => r.AddDocumentAsync(It.IsAny<SaleListingRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
