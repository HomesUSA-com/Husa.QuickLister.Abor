namespace Husa.Quicklister.Abor.Application.Tests.Services.LotListings
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Request;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Services.LotListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.LotListings;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Lot;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class LotListingServiceTests
    {
        private readonly LotListingService sut;
        private readonly Mock<ILotListingRepository> lotListingRepositoryMock = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClientMock = new();
        private readonly Mock<ICommunitySaleRepository> communitySaleRepositoryMock = new();
        private readonly Mock<ILotListingMediaService> listingMediaServiceMock = new();
        private readonly Mock<IUserContextProvider> userContextProviderMock = new();
        private readonly Mock<ILotListingRequestRepository> lotListingRequestRepositoryMock = new();
        private readonly Mock<ILogger<LotListingService>> loggerMock = new();

        public LotListingServiceTests(ApplicationServicesFixture fixture)
        {
            this.sut = new LotListingService(
                this.lotListingRepositoryMock.Object,
                this.communitySaleRepositoryMock.Object,
                this.serviceSubscriptionClientMock.Object,
                this.userContextProviderMock.Object,
                this.listingMediaServiceMock.Object,
                this.lotListingRequestRepositoryMock.Object,
                fixture.Options.Object,
                fixture.Mapper,
                this.loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidLotListing_ReturnsSuccessResult()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var lotListing = new QuickCreateListingDto
            {
                CompanyId = companyId,
                CommunityId = communityId,
                IsManuallyManaged = true,
            };

            this.SetupSubscriptionClientCompany(companyId, ServiceCode.XMLImport);

            this.lotListingRepositoryMock.Setup(x => x.AddAsync(It.IsAny<LotListing>()))
               .ReturnsAsync(new LotListing { Id = Guid.NewGuid() });
            this.communitySaleRepositoryMock.Setup(x => x.GetById(communityId, It.IsAny<bool>()))
             .ReturnsAsync(new CommunitySale(companyId, "Community", "Company")
             {
                 Id = communityId,
             });

            // Act
            var result = await this.sut.CreateAsync(lotListing);

            // Assert
            Assert.Equal(ResponseCode.Success, result.Code);
            Assert.IsType<CommandSingleResult<Guid, string>>(result);
        }

        [Fact]
        public async Task UpdateListing_UpdateComplete_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = LotTestProvider.GetListingEntity(listingId, companyId: companyId);
            var listingDto = LotTestProvider.GetLotListingDto();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.MLSAdministrator);

            this.userContextProviderMock.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.lotListingRepositoryMock
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId);

            // Act
            await this.sut.UpdateListing(listingId, listingDto);

            // Assert
            this.lotListingRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<LotListing>()), Times.Once);
            this.lotListingRepositoryMock.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task ChangeCommunity_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            var listing = LotTestProvider.GetListingEntity(listingId, communityId: Guid.NewGuid(), companyId: companyId);
            this.lotListingRepositoryMock
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing)
                .Verifiable();

            this.communitySaleRepositoryMock.Setup(x => x.GetById(communityId, It.IsAny<bool>()))
             .ReturnsAsync(new CommunitySale(companyId, "Community", "Company")
             {
                 Id = communityId,
             });

            // Act
            await this.sut.ChangeCommunity(listingId, communityId);

            // Assert
            this.lotListingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<LotListing>()), Times.Once);
        }

        [Fact]
        public async Task AssignMlsNumberAsync_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            var listing = LotTestProvider.GetListingEntity(listingId);
            this.lotListingRepositoryMock
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing)
                .Verifiable();

            // Act
            await this.sut.AssignMlsNumberAsync(listingId, "MlsNumber", Domain.Enums.MarketStatuses.Active, Quicklister.Extensions.Domain.Enums.ActionType.NewListing);

            // Assert
            this.lotListingRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<LotListing>()), Times.Once);
        }

        [Fact]
        public async Task UnlockListing_WhenListingNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.lotListingRepositoryMock.Setup(x => x.GetById(listingId, true)).ReturnsAsync((LotListing)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException<LotListing>>(async () => await this.sut.UnlockListing(listingId));
        }

        [Fact]
        public async Task UnlockListing_WhenUserCannotUnlock_ThrowsDomainException()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing = new LotListing();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.User);
            this.lotListingRepositoryMock.Setup(x => x.GetById(listingId, true)).ReturnsAsync(listing);
            this.userContextProviderMock.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var company = TestModelProvider.GetCompanyDetail();

            this.serviceSubscriptionClientMock
               .Setup(x => x.Company
                   .GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(company);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(async () => await this.sut.UnlockListing(listingId));
        }

        [Fact]
        public async Task UnlockListing_ValidListing_ShouldUnlockListing()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.MLSAdministrator);
            var company = TestModelProvider.GetCompanyDetail();
            var listingSale = new LotListing { Id = listingId };

            this.serviceSubscriptionClientMock
               .Setup(x => x.Company
                   .GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(company);

            this.userContextProviderMock.Setup(x => x.GetCurrentUser()).Returns(user);
            this.lotListingRepositoryMock.Setup(x => x.GetById(listingId, true)).ReturnsAsync(listingSale);
            this.lotListingRequestRepositoryMock.Setup(x => x.CheckFirstListingRequestExistAsync(listingId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            // Act
            var result = await this.sut.UnlockListing(listingId);

            // Assert
            Assert.Equal(ResponseCode.Success, result.Code);
            Assert.Equal($"Unlocked LotListing with id {listingId}.", result.Result);
        }

        [Fact]
        public async Task UnlockListing_ListingHasOpenRequest_ReturnError()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.User);
            var company = TestModelProvider.GetCompanyDetail();
            user.EmployeeRole = RoleEmployee.CompanyAdmin;
            var listingSale = new LotListing { Id = listingId, LockedBy = userId, LockedStatus = Quicklister.Extensions.Domain.Enums.LockedStatus.LockedNotSubmitted };

            this.serviceSubscriptionClientMock
               .Setup(x => x.Company
                   .GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(company);

            this.userContextProviderMock.Setup(x => x.GetCurrentUser()).Returns(user);
            this.lotListingRepositoryMock.Setup(x => x.GetById(listingId, true)).ReturnsAsync(listingSale);
            this.lotListingRequestRepositoryMock.Setup(x => x.CheckFirstListingRequestExistAsync(listingId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await this.sut.UnlockListing(listingId);

            // Assert
            Assert.Equal(ResponseCode.Error, result.Code);
            Assert.Equal($"The LotListing {listingId} has an open request, cannot be unlocked.", result.Message);
        }

        [Fact]
        public async Task UpdateActionTypeAsync_ValidLotListing_UpdatesActionType()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var actionType = ActionType.PendingTransfer;
            var lotListing = new LotListing { Id = listingId };
            lotListing.PublishInfo.PublishType = ActionType.PendingTransfer;

            this.lotListingRepositoryMock.Setup(r => r.GetById(listingId, false)).ReturnsAsync(lotListing);

            // Act
            await this.sut.UpdateActionTypeAsync(listingId, actionType);

            // Assert
            Assert.Equal(actionType, lotListing.PublishInfo.PublishType);
            this.lotListingRepositoryMock.Verify(r => r.SaveChangesAsync(lotListing), Times.Once);
        }

        [Fact]
        public async Task UpdateActionTypeAsync_LotListingNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var actionType = ActionType.PendingTransfer;

            var mockRepository = new Mock<ILotListingRepository>();
            mockRepository.Setup(r => r.GetById(listingId, false)).ReturnsAsync((LotListing)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException<LotListing>>(async () =>
                await this.sut.UpdateActionTypeAsync(listingId, actionType));
        }

        private void SetupSubscriptionClientCompany(Guid companyId, ServiceCode? service = null)
        {
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            this.serviceSubscriptionClientMock
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var services = service.HasValue
                ? new CompanyResponse.ServiceSubscription[] { new() { ServiceCode = service.Value } }
                : Array.Empty<CompanyResponse.ServiceSubscription>();

            this.serviceSubscriptionClientMock
                .Setup(c => c.Company.GetCompanyServices(companyId, It.IsAny<FilterServiceSubscriptionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<CompanyResponse.ServiceSubscription>(services, services.Length));
        }
    }
}
