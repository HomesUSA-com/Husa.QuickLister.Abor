namespace Husa.Quicklister.Abor.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Document.Models;
    using Husa.Quicklister.Abor.Api.Contracts.Response.ListingRequest;
    using Husa.Quicklister.Abor.Api.Controllers.LotListing;
    using Husa.Quicklister.Abor.Api.Tests.Configuration;
    using Husa.Quicklister.Abor.Application.Interfaces.Lot;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Models.Lot;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Data.Documents.Interfaces;
    using Husa.Quicklister.Abor.Data.Documents.Models;
    using Husa.Quicklister.Abor.Data.Documents.Models.LotRequest;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Api.Contracts.Request.ListingRequest;
    using Husa.Quicklister.Extensions.Api.Contracts.Response;
    using Husa.Quicklister.Extensions.Application.Interfaces.Notes;
    using Husa.Quicklister.Extensions.Data.Documents.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]
    public class LotListingRequestsControllerTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<ILotListingRequestQueriesRepository> requestQueryRepository = new();
        private readonly Mock<ILotListingService> listingLotService = new();
        private readonly Mock<ILotListingNotesService> listingNotesService = new();
        private readonly Mock<ILotListingRequestService> requestService = new();
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<ILogger<LotListingRequestsController>> logger = new();

        public LotListingRequestsControllerTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new LotListingRequestsController(
                this.requestQueryRepository.Object,
                this.listingLotService.Object,
                this.listingNotesService.Object,
                this.requestService.Object,
                this.userQueriesRepository.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public LotListingRequestsController Sut { get; set; }

        [Fact]
        public async Task GetListRequestAsync_Success()
        {
            // Arrange
            var requests = new List<ListingRequestQueryResult>()
            {
                new(),
                new(),
            };
            var requestsResult = new DocumentGridQueryResult<ListingRequestQueryResult>(requests, null);
            requestsResult.Total = requests.Count;

            this.requestQueryRepository
                .Setup(u => u.GetAsync(It.IsAny<ListingRequestQueryFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(requestsResult)
                .Verifiable();

            // Act
            var actionResult = await this.Sut.GetListRequestAsync(new ListingRequestFilter());

            // Assert
            Assert.NotNull(actionResult);
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<DocumentGridResponse<ListingRequestQueryResponse>>(okObjectResult.Value);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task SaveAndSubmitListingAsync_Success()
        {
            this.requestService
                .Setup(x => x.CreateRequestAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CommandSingleResult<Guid, System.ComponentModel.DataAnnotations.ValidationResult>.Success(Guid.NewGuid()))
                .Verifiable();

            // Act
            await this.Sut.SaveAndSubmitListingAsync(Guid.NewGuid(), new()
            {
                StatusFieldsInfo = new(),
                PublishInfo = new(),
                AddressInfo = new(),
                PropertyInfo = new(),
                FeaturesInfo = new(),
                FinancialInfo = new(),
                ShowingInfo = new(),
                SchoolsInfo = new(),
            });

            // Assert
            this.listingLotService.Verify(x => x.UpdateListing(It.IsAny<Guid>(), It.IsAny<LotListingDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateListingRequestAsync_Success()
        {
            // Act
            await this.Sut.UpdateAsync(Guid.NewGuid(), new()
            {
                StatusFieldsInfo = new(),
                PublishInfo = new(),
                AddressInfo = new(),
                PropertyInfo = new(),
                FeaturesInfo = new(),
                FinancialInfo = new(),
                ShowingInfo = new(),
                SchoolsInfo = new(),
            });

            // Assert
            this.requestService.Verify(x => x.UpdateListingRequestAsync(It.IsAny<Guid>(), It.IsAny<LotListingRequestDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CompleteRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            this.requestQueryRepository
                .Setup(sl => sl.GetRequestByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LotListingRequestDetailQueryResult()
                {
                    Id = requestId,
                    ListingId = listingId,
                    MlsStatus = MarketStatuses.Active,
                });

            // Act
            await this.Sut.CompleteRequestAsync(listingId, "12763", ActionType.NewListing);

            // Assert
            this.requestService.Verify(x => x.ChangeRequestStatus(It.IsAny<Guid>(), It.Is<ListingRequestState>(x => x == ListingRequestState.Processing), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            this.listingLotService.Verify(x => x.AssignMlsNumberAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<MarketStatuses>(), It.IsAny<ActionType>()), Times.Once);
            this.requestService.Verify(x => x.ChangeRequestStatus(It.IsAny<Guid>(), It.Is<ListingRequestState>(x => x == ListingRequestState.Completed), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReturnRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            this.requestQueryRepository
                .Setup(sl => sl.GetRequestByIdAsync(It.Is<Guid>(id => id == requestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LotListingRequestDetailQueryResult()
                {
                    Id = requestId,
                });

            // Act
            await this.Sut.ReturnRequestAsync(requestId, new()
            {
                ReturnReason = "reason",
            });

            // Assert
            this.requestService.Verify(x => x.ChangeRequestStatus(It.Is<Guid>(x => x == requestId), It.Is<ListingRequestState>(x => x == ListingRequestState.Returned), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            this.listingNotesService.Verify(x => x.CreateNote(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
