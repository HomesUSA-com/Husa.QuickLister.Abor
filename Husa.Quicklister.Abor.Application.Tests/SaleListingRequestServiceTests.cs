namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Application.Tests.Providers;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.SaleRequest;
    using Husa.Quicklister.Abor.Domain.Interfaces;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Application.Interfaces.Email;
    using Husa.Quicklister.Extensions.Application.Models.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using ExtensionsMediaService = Husa.Quicklister.Extensions.Application.Interfaces.Request.ISaleListingRequestMediaService;
    using ExtensionsUserRepository = Husa.Quicklister.Extensions.Domain.Repositories.IUserRepository;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingRequestServiceTests
    {
        private readonly Mock<ILogger<SaleListingRequestService>> logger = new();
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IEmailService> emailService = new();
        private readonly Mock<ExtensionsUserRepository> userQueriesRepository = new();
        private readonly Mock<ExtensionsMediaService> mediaService = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ISaleListingRequestRepository> saleRequestRepository = new();
        private readonly Mock<IListingSaleRepository> saleListingRepository = new();
        private readonly Mock<ICommunitySaleRepository> saleCommunityRepository = new();
        private readonly Mock<IProvideShowingTimeContacts> showingTimeContactsProvider = new();
        private readonly Mock<IRequestErrorRepository> pequestErrorRepository = new();

        public SaleListingRequestServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task UpdateRequestSuccess()
        {
            // Arrange
            const int listPrice = 230000;
            var requestId = Guid.NewGuid();
            var saleListingRequest = ListingRequestProviders.GetSaleListingRequestMock(requestId, Guid.NewGuid(), Guid.NewGuid());
            saleListingRequest.SetupProperty(sl => sl.ListPrice, initialValue: 0);
            saleListingRequest.SetupProperty(sl => sl.RequestState, initialValue: ListingRequestState.Processing);

            var saleListingRequestDto = new Mock<ListingSaleRequestDto>();
            var salePropertyDto = ListingTestProvider.GetSalePropertyDetailDto();

            saleListingRequestDto.SetupGet(sl => sl.SaleProperty).Returns(salePropertyDto);
            saleListingRequestDto.SetupGet(sl => sl.ListPrice).Returns(listPrice);

            var sut = this.GetSut();

            // Act
            var result = await sut.UpdateRequestAsync(saleListingRequest.Object, saleListingRequestDto.Object);

            // Assert
            Assert.NotNull(result);
            this.saleRequestRepository.Verify(
                r => r.UpdateDocumentAsync(
                    It.Is<Guid>(id => id == requestId),
                    It.IsAny<SaleListingRequest>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            Assert.Equal(listPrice, result.ListPrice);
            Assert.Equal(ListingRequestState.Pending, result.RequestState);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateListingRequestAsync_ShowingTimeFlag_ShouldHandleCorrectly(bool useShowingTime)
        {
            // Arrange
            var listingRequestId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var showingTime = TestModelProvider.ShowingTimeFaker();

            var listingSaleRequestDto = new ListingSaleRequestDto
            {
                UseShowingTime = useShowingTime,
                ListingSaleId = listingId,
                ShowingTime = this.fixture.Mapper.Map<ShowingTimeDto>(showingTime),
                SaleProperty = ListingTestProvider.GetSalePropertyDetailDto(),
                StatusFieldsInfo = new(),
            };

            var existingRequestMock = ListingRequestProviders.GetSaleListingRequestMock(listingRequestId, listingId);
            existingRequestMock.Setup(p => p.UpdateRequestInformation(It.IsAny<ListingRequestValueObject>(), It.IsAny<ListingStatusFieldsInfo>(), It.IsAny<SalePropertyValueObject>())).CallBase().Verifiable();
            var existingRequest = existingRequestMock.Object;

            this.saleRequestRepository.Setup(r => r.GetByIdAsync(listingRequestId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingRequest);

            this.userContextProvider.Setup(u => u.GetCurrentUserId()).Returns(userId);

            this.saleRequestRepository.Setup(r => r.UpdateDocumentAsync(
                    listingRequestId,
                    It.IsAny<SaleListingRequest>(),
                    userId,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await this.GetSut().UpdateListingRequestAsync(listingRequestId, listingSaleRequestDto);

            // Assert
            this.saleRequestRepository.Verify(r => r.GetByIdAsync(listingRequestId, It.IsAny<CancellationToken>()), Times.Once);

            if (useShowingTime)
            {
                this.showingTimeContactsProvider.Verify(s => s.GetScopedContacts(ContactScope.Listing, listingId), Times.Once);
            }
            else
            {
                this.showingTimeContactsProvider.Verify(s => s.GetScopedContacts(It.IsAny<ContactScope>(), It.IsAny<Guid>()), Times.Never);
            }

            this.saleRequestRepository.Verify(
                r => r.UpdateDocumentAsync(
                listingRequestId,
                It.IsAny<SaleListingRequest>(),
                userId,
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private SaleListingRequestService GetSut(IMapper mapper = null) => new(
            this.saleRequestRepository.Object,
            this.saleListingRepository.Object,
            this.mediaService.Object,
            this.userContextProvider.Object,
            this.saleCommunityRepository.Object,
            mapper ?? this.fixture.Mapper,
            this.logger.Object,
            this.fixture.Options.Object,
            this.emailService.Object,
            this.userQueriesRepository.Object,
            this.pequestErrorRepository.Object,
            this.showingTimeContactsProvider.Object);
    }
}
