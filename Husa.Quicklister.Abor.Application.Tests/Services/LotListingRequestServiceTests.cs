namespace Husa.Quicklister.Abor.Application.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.EmailNotification.Enums;
    using Husa.Extensions.EmailNotification.Services;
    using Husa.Quicklister.Abor.Application.Models.Request;
    using Husa.Quicklister.Abor.Application.Services;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Base;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest;
    using Husa.Quicklister.Abor.Domain.Entities.LotRequest.Records;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Abor.Domain.ValueObjects;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using ExtensionsMediaService = Husa.Quicklister.Extensions.Application.Interfaces.Request.ILotListingRequestMediaService;
    using ExtensionsUserRepository = Husa.Quicklister.Extensions.Domain.Repositories.IUserRepository;
    using MediaResponse = Husa.MediaService.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class LotListingRequestServiceTests
    {
        private readonly Mock<ILogger<LotListingRequestService>> logger = new();
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IEmailSender> emailSender = new();
        private readonly Mock<ExtensionsUserRepository> userQueriesRepository = new();
        private readonly Mock<ExtensionsMediaService> mediaService = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ILotListingRequestRepository> requestRepository = new();
        private readonly Mock<ILotListingRepository> listingRepository = new();
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();

        public LotListingRequestServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.fixture = fixture;
        }

        [Fact]
        public async Task CreateRequestAsync_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var lotListing = new Mock<LotListing>();
            lotListing.SetupGet(sl => sl.Id).Returns(listingId).Verifiable();

            var requestId = Guid.NewGuid();
            var lotListingRequest = this.GetLotListingRequestMock(requestId, listingId: listingId, Guid.NewGuid());
            this.SetupGenerateRequest(lotListing, userId, CommandSingleResult<LotListingRequest, ValidationResult>.Success(lotListingRequest.Object));

            this.userContextProvider
                .Setup(sl => sl.GetCurrentUserId())
                .Returns(userId);

            this.listingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => !filterByCompany)))
                .ReturnsAsync(lotListing.Object);

            this.requestRepository
                .Setup(sl => sl.CheckFirstListingRequestExistAsync(It.Is<Guid>(id => id == listingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            this.requestRepository
                .Setup(sl => sl.GetOpenRequestAsync(It.Is<Guid>(id => id == listingId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((LotListingRequest)null);

            var mediaDetail = TestModelProvider.GetMediaDetail(Guid.NewGuid());
            this.mediaService
                .Setup(sl => sl.GetListingResources(It.Is<Guid>(id => id == listingId)))
                .ReturnsAsync(new MediaResponse.ResourceResponse { Media = new[] { mediaDetail, mediaDetail, mediaDetail, mediaDetail } });

            var sut = this.GetSut();

            // Act
            var result = await sut.CreateRequestAsync(listingId, cancellationToken: default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResponseCode.Success, result.Code);
            this.listingRepository.Verify(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => !filterByCompany)), Times.Once);
            this.requestRepository.Verify(sl => sl.CheckFirstListingRequestExistAsync(It.Is<Guid>(id => id == listingId), It.IsAny<CancellationToken>()), Times.Once);
            this.requestRepository.Verify(sl => sl.AddDocumentAsync(It.Is<LotListingRequest>(sl => sl.EntityId == listingId), It.IsAny<CancellationToken>()), Times.Once);
            this.mediaService.Verify(sl => sl.CreateMediaRequestAsync(It.Is<Guid>(id => id == listingId), It.IsAny<Guid>(), It.Is<bool>(disposeBus => disposeBus)), Times.Once);
        }

        [Fact]
        public async Task ChangesRequestState_Returned_Success()
        {
            var requestId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = new Mock<LotListing>();
            listing.Setup(x => x.Id).Returns(listingId);
            var lotListingRequest = this.GetLotListingRequestMock(requestId, listingId: listingId, Guid.NewGuid(), requestState: ListingRequestState.Pending);

            this.requestRepository
                .Setup(sl => sl.GetByIdAsync(It.Is<Guid>(id => id == requestId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lotListingRequest.Object);
            this.listingRepository
                .Setup(sl => sl.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing.Object);
            this.serviceSubscriptionClient.Setup(u => u.User.GetUserDetail(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(
                new Husa.CompanyServicesManager.Api.Contracts.Response.UserDetail()
                {
                    Email = "email@test.com",
                    FirstName = "User",
                }).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.ChangeRequestStatus(requestId, ListingRequestState.Returned, reason: "reason");

            // Assert
            this.emailSender.Verify(
                sl => sl.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<EmailParameter, string>>(), It.IsAny<TemplateType>(), It.IsAny<string[]>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateRequestSuccess()
        {
            // Arrange
            const int listPrice = 230000;
            var requestId = Guid.NewGuid();
            var lotListingRequest = this.SetupListingRequestForUpdate(requestId);
            var lotListingRequestDto = this.GetLotListingRequestDto(listPrice);
            var sut = this.GetSut();

            // Act
            var result = await sut.UpdateRequestAsync(lotListingRequest, lotListingRequestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listPrice, result.ListPrice);
            Assert.Equal(ListingRequestState.Pending, result.RequestState);
        }

        [Fact]
        public async Task UpdateListingRequestAsync_Success()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var lotListingRequest = this.SetupListingRequestForUpdate(requestId);
            var lotListingRequestDto = this.GetLotListingRequestDto(listPrice: 230000);

            this.requestRepository
                .Setup(p => p.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lotListingRequest);

            var sut = this.GetSut();

            // Act
            await sut.UpdateListingRequestAsync(requestId, lotListingRequestDto);

            // Assert
            this.requestRepository.Verify(
                r => r.UpdateDocumentAsync(
                    It.Is<Guid>(id => id == requestId),
                    It.IsAny<LotListingRequest>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private LotListingRequestDto GetLotListingRequestDto(decimal listPrice) => new()
        {
            ListPrice = listPrice,
            StatusFieldsInfo = new()
            {
                PendingDate = DateTime.UtcNow,
            },
            AddressInfo = new()
            {
                City = Domain.Enums.Domain.Cities.Daffen,
            },
            PropertyInfo = new(),
            FinancialInfo = new(),
            FeaturesInfo = new(),
            ShowingInfo = new(),
            SchoolsInfo = new(),
        };

        private LotListingRequest SetupListingRequestForUpdate(Guid requestId)
        {
            var lotListingRequest = this.GetLotListingRequestMock(requestId, Guid.NewGuid(), Guid.NewGuid());
            lotListingRequest.SetupProperty(sl => sl.ListPrice, initialValue: 0);
            lotListingRequest.SetupProperty(sl => sl.RequestState, initialValue: ListingRequestState.Processing);
            lotListingRequest
                .Setup(p => p.UpdateRequestInformation(It.IsAny<ListingRequestValueObject>(), It.IsAny<ListingStatusFieldsInfo>(), It.IsAny<LotPropertyValueObject>()))
                .CallBase()
                .Verifiable();

            return lotListingRequest.Object;
        }

        private Mock<LotListingRequest> GetLotListingRequestMock(Guid requestId, Guid listingId, Guid? companyId = null, ListingRequestState? requestState = null)
        {
            var lotListingRequest = new Mock<LotListingRequest>();
            var lotStatusFieldsRecord = new LotStatusFieldsRecord();

            lotListingRequest.Setup(sl => sl.StatusFieldsInfo).Returns(lotStatusFieldsRecord).Verifiable();
            lotListingRequest.SetupGet(sl => sl.Id).Returns(requestId).Verifiable();
            lotListingRequest.SetupGet(sl => sl.EntityId).Returns(listingId).Verifiable();
            lotListingRequest.SetupGet(sl => sl.SysCreatedBy).Returns(Guid.NewGuid()).Verifiable();
            lotListingRequest.SetupGet(sl => sl.CompanyId).Returns(companyId ?? Guid.NewGuid()).Verifiable();

            if (requestState.HasValue)
            {
                lotListingRequest.SetupGet(sl => sl.RequestState).Returns(requestState.Value).Verifiable();
            }

            lotListingRequest.Setup(x => x.AddressInfo).Returns(new LotAddressRecord());

            return lotListingRequest;
        }

        private void SetupGenerateRequest(Mock<LotListing> saleListing, Guid userId, CommandSingleResult<LotListingRequest, ValidationResult> result)
        {
            saleListing
                .Setup(sl => sl.GenerateRequest(It.Is<Guid>(id => id == userId)))
                .Returns(result)
                .Verifiable();

            saleListing
                .Setup(sl => sl.GenerateRequestFromCommunity(It.IsAny<LotListingRequest>(), It.Is<Guid>(id => id == userId)))
                .Returns(result)
                .Verifiable();
        }

        private LotListingRequestService GetSut() => new(
            this.requestRepository.Object,
            this.listingRepository.Object,
            this.mediaService.Object,
            this.userContextProvider.Object,
            this.communityRepository.Object,
            this.fixture.Mapper,
            this.logger.Object,
            this.fixture.Options.Object,
            this.serviceSubscriptionClient.Object,
            this.emailSender.Object,
            this.userQueriesRepository.Object);
    }
}
