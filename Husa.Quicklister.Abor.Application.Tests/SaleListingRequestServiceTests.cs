namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.EmailNotification.Services;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Models.Community;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Interfaces;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using ExtensionsMediaService = Husa.Quicklister.Extensions.Application.Interfaces.Request.IListingRequestMediaService;
    using ExtensionsUserRepository = Husa.Quicklister.Extensions.Domain.Repositories.IUserRepository;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingRequestServiceTests
    {
        private readonly Mock<ILogger<SaleListingRequestService>> logger = new();
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IEmailSender> emailSender = new();
        private readonly Mock<ExtensionsUserRepository> userQueriesRepository = new();
        private readonly Mock<ExtensionsMediaService> mediaService = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ISaleListingRequestRepository> saleRequestRepository = new();
        private readonly Mock<IListingSaleRepository> saleListingRepository = new();
        private readonly Mock<ICommunitySaleRepository> saleCommunityRepository = new();

        public SaleListingRequestServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task CreateRequestsFromCommunity_Success()
        {
            // Arrange
            var lastListingRequest = new Mock<SaleListingRequest>();

            var userId = Guid.NewGuid();
            var saleListingRequestId = Guid.NewGuid();
            var unlockedListingId = Guid.NewGuid();
            var lockedListingBy = Guid.NewGuid();

            // Community
            var communityId = Guid.NewGuid();
            var saleCommunity = this.GetCommunityWithUnlockedListing(communityId, unlockedListingId, saleListingRequestId, lockedListingBy);
            this.saleCommunityRepository
                .Setup(sl => sl.GetCommunityByIdAsync(It.Is<Guid>(id => id == communityId)))
                .ReturnsAsync(saleCommunity)
                .Verifiable();
            this.saleRequestRepository
                .Setup(sl => sl.GetLastCompletedRequestAsync(It.Is<Guid>(id => id == unlockedListingId), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lastListingRequest.Object)
                .Verifiable();
            this.userContextProvider
                .Setup(sl => sl.GetCurrentUserId())
                .Returns(userId)
                .Verifiable();

            var sut = this.GetSut();

            // Act
            var result = await sut.CreateRequestsFromCommunityAsync(communityId, cancellationToken: default);

            // Assert
            Assert.Equal(ResponseCode.Success, result.Code);
            var objectResult = Assert.IsAssignableFrom<CommunityRequestsResponse>(result.Result);
            Assert.Single(objectResult.LockedListings);

            this.userQueriesRepository.Verify(sl => sl.FillUsersNameAsync(It.Is<IEnumerable<IProvideQuicklisterUserInfo>>(x => x.Any(x => x.LockedBy == lockedListingBy))), Times.Once);
            this.saleRequestRepository.Verify(sl => sl.AddListingSaleRequestAsync(It.Is<SaleListingRequest>(x => x.Id == saleListingRequestId), It.IsAny<CancellationToken>()), Times.Never);
            this.mediaService.Verify(sl => sl.CreateMediaRequestAsync(It.Is<Guid>(x => x == unlockedListingId), It.Is<Guid>(x => x == saleListingRequestId), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task UpdateRequestSuccess()
        {
            // Arrange
            const int listPrice = 230000;
            var requestId = Guid.NewGuid();
            var saleListingRequest = this.GetSaleListingRequestMock(requestId, Guid.NewGuid(), Guid.NewGuid());
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
                r => r.UpdateListingSaleRequestAsync(
                    It.Is<string>(id => id == requestId.ToString()),
                    It.IsAny<SaleListingRequest>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            Assert.Equal(listPrice, result.ListPrice);
            Assert.Equal(ListingRequestState.Pending, result.RequestState);
        }

        private CommunitySale GetCommunityWithUnlockedListing(Guid communityId, Guid unlockedListingId, Guid newRequestId, Guid lockedListingBy)
        {
            var saleCommunity = CommunityTestProvider.GetCommunityEntity(communityId);
            saleCommunity.Property.Subdivision = "CommunitySubdivision";
            var changes = new List<string> { nameof(saleCommunity.Property.Subdivision) };
            saleCommunity.UpdateChanges(nameof(saleCommunity.Property), changes);

            // Unlocked Listing
            var unlockedListingMock = TestModelProvider.GetListingSaleEntityMock(unlockedListingId, createStub: true, communityId: communityId);
            var saleListingRequest = this.GetSaleListingRequestMock(newRequestId, unlockedListingId, Guid.NewGuid());
            unlockedListingMock
                .Setup(x => x.GenerateRequestFromCommunity(It.IsAny<SaleListingRequest>(), It.IsAny<Guid>()))
                .Returns(CommandSingleResult<SaleListingRequest, ValidationResult>.Success(saleListingRequest.Object))
                .Verifiable();
            var unlockedListing = unlockedListingMock.Object;
            unlockedListing.LockedStatus = LockedStatus.NoLocked;
            unlockedListing.SaleProperty.AddressInfo.Subdivision = "ListingSubdivision";
            unlockedListing.SaleProperty.Community = saleCommunity;

            // Locked Listing
            var lockedListingId = Guid.NewGuid();
            var lockedListing = TestModelProvider.GetListingSaleEntity(lockedListingId, createStub: true, communityId: communityId);
            lockedListing.LockedBy = lockedListingBy;
            lockedListing.LockedStatus = LockedStatus.LockedByUser;

            saleCommunity.SaleProperties = new[] { unlockedListing.SaleProperty, lockedListing.SaleProperty };
            return saleCommunity;
        }

        private Mock<SaleListingRequest> GetSaleListingRequestMock(Guid requestId, Guid listingId, Guid companyId)
        {
            var saleListingRequest = new Mock<SaleListingRequest>();

            saleListingRequest.SetupGet(sl => sl.Id).Returns(requestId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.ListingId).Returns(listingId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.ListingSaleId).Returns(listingId).Verifiable();
            saleListingRequest.SetupGet(sl => sl.CompanyId).Returns(companyId).Verifiable();

            var salePropertyRecord = new Mock<SalePropertyRecord>();
            saleListingRequest.SetupGet(sl => sl.SaleProperty).Returns(salePropertyRecord.Object).Verifiable();
            return saleListingRequest;
        }

        private SaleListingRequestService GetSut() => new(
            this.saleRequestRepository.Object,
            this.saleListingRepository.Object,
            this.mediaService.Object,
            this.userContextProvider.Object,
            this.saleCommunityRepository.Object,
            this.fixture.Mapper,
            this.logger.Object,
            this.fixture.Options.Object,
            this.serviceSubscriptionClient.Object,
            this.emailSender.Object,
            this.userQueriesRepository.Object);
    }
}
