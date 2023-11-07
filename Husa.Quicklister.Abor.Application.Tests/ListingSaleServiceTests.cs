namespace Husa.Quicklister.Abor.Application.Tests
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
    using Husa.Extensions.Authorization.Models;
    using Husa.Extensions.Common.Classes;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class ListingSaleServiceTests
    {
        private readonly Mock<ISaleListingRequestRepository> saleRequestRepository = new();
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingSaleRepository = new();
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IPlanRepository> planRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ILogger<SaleListingService>> logger = new();
        private readonly Mock<ISaleListingMediaService> listingMediaService = new();

        public ListingSaleServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.Sut = new SaleListingService(
                this.saleRequestRepository.Object,
                this.listingSaleRepository.Object,
                this.communitySaleRepository.Object,
                this.planRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.userContextProvider.Object,
                this.listingMediaService.Object,
                this.fixture.Options.Object,
                this.fixture.Mapper,
                this.logger.Object);
        }

        public ISaleListingService Sut { get; set; }

        [Fact]
        public async Task CreateListingSaleAsync_WithoutComminityIdAndListingIdToImport_Fails()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId);
            listingSaleDto.CommunityId = null;
            listingSaleDto.ListingIdToImport = null;

            this.SetupSubscriptionClientCompany(companyId);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => this.Sut.CreateAsync(listingSaleDto));
        }

        [Fact]
        public async Task CreateListingAsync_WithXmlCompanyService_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, createStub: true, companyId: companyId);
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId, communityId: communityId, planId: planId);
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId, companyId: companyId);
            var plan = TestModelProvider.GetPlanEntity(planId, createStub: true, companyId: companyId);

            this.listingSaleRepository
                .Setup(c => c.AddAsync(It.IsAny<SaleListing>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId, ServiceCode.XMLImport);
            listingSaleDto.IsManuallyManaged = true;

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(communitySale)
                .Verifiable();

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(listingSaleDto);

            // Act and Assert
            this.listingSaleRepository.Verify();
            Assert.IsType<CommandSingleResult<Guid, string>>(result);
        }

        [Fact]
        public async Task CreateListingAsync_WithXmlCompanyService_Error()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId);

            this.SetupSubscriptionClientCompany(companyId, ServiceCode.XMLImport);
            listingSaleDto.IsManuallyManaged = false;

            // Act
            var result = await this.Sut.CreateAsync(listingSaleDto);

            // Act and Assert
            this.listingSaleRepository.Verify();
            var response = Assert.IsType<CommandSingleResult<Guid, string>>(result);
            Assert.Equal(ResponseCode.Error, response.Code);
        }

        [Fact]
        public async Task CreateListingSaleAsync_CreateComplete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var planId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, createStub: true, companyId: companyId);
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId, communityId: communityId, planId: planId);
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId, companyId: companyId);
            var plan = TestModelProvider.GetPlanEntity(planId, createStub: true, companyId: companyId);

            this.listingSaleRepository
                .Setup(c => c.AddAsync(It.IsAny<SaleListing>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId);

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(communitySale)
                .Verifiable();

            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>()))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act
            var result = await this.Sut.CreateAsync(listingSaleDto);

            // Act and Assert
            this.listingSaleRepository.Verify();
            var response = Assert.IsType<CommandSingleResult<Guid, string>>(result);
            Assert.Equal(ResponseCode.Success, response.Code);
        }

        [Fact]
        public async Task CreateListingSaleAsync_CloneComplete_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, createStub: true, companyId: companyId);
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId);
            var listingIdToImport = Guid.NewGuid();
            listingSaleDto.ListingIdToImport = listingIdToImport;
            var listingToImport = TestModelProvider.GetListingSaleEntity(listingIdToImport, true, companyId);

            this.listingSaleRepository
                .Setup(c => c.AddAsync(It.IsAny<SaleListing>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            this.listingSaleRepository
                .Setup(l => l.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listingToImport)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId);

            // Act
            var result = await this.Sut.CreateAsync(listingSaleDto);

            // Act and Assert
            this.listingSaleRepository.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
            Assert.IsType<CommandSingleResult<Guid, string>>(result);
            Assert.Equal(listingToImport.SaleProperty.AddressInfo.City, listingSale.SaleProperty.AddressInfo.City);
            Assert.Equal(listingToImport.SaleProperty.SpacesDimensionsInfo, listingSale.SaleProperty.SpacesDimensionsInfo);
            Assert.Equal(listingToImport.SaleProperty.ShowingInfo, listingSale.SaleProperty.ShowingInfo);
        }

        [Fact]
        public async Task CreateListingSaleAsync_CloneIncomplete_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, createStub: true, companyId: companyId);
            var listingSaleDto = TestModelProvider.GetListingSaleDto(companyId);
            var listingIdToImport = Guid.NewGuid();
            listingSaleDto.ListingIdToImport = listingIdToImport;
            SaleListing listingToImport = null;

            this.listingSaleRepository
                .Setup(c => c.AddAsync(It.IsAny<SaleListing>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            this.listingSaleRepository
                .Setup(l => l.GetById(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(listingToImport)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId);

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.CreateAsync(listingSaleDto));
            this.listingSaleRepository.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task UpdateListing_UpdateComplete_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true);
            var listingSaleDto = TestModelProvider.GetSaleListingDto();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act
            await this.Sut.UpdateListing(listingId, listingSaleDto);

            // Assert
            this.listingSaleRepository.Verify(r => r.UpdateAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UpdateListing_CreateIncomplete_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            SaleListing listingSale = null;
            var listingSaleDto = TestModelProvider.GetSaleListingDto();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.UpdateListing(listingId, listingSaleDto));
            this.listingSaleRepository.Verify(r => r.UpdateAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task DeleteListing_ListingNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            SaleListing listingSale = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.DeleteListing(listingId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task DeleteListing_ListingFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();

            // Act
            await this.Sut.DeleteListing(listingId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(
                c => c.GetById(
                    It.Is<Guid>(id => id == listingId),
                    It.Is<bool>(filterByCompany => filterByCompany)),
                Times.Once);
        }

        [Fact]
        public async Task GetEntity_GetById_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            SaleListing listingSale = null;
            var listingSaleResponse = TestModelProvider.GetListingSaleEntity(listingId, true);

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSaleResponse)
                .Verifiable();

            // Act
            var response = await this.Sut.GetEntity(listingSale, listingId);

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<SaleListing>(response);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task GetEntity_GetByEntity_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true);

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act
            var response = await this.Sut.GetEntity(listingSale, Guid.Empty);

            // Assert
            Assert.NotNull(response);
            Assert.IsAssignableFrom<SaleListing>(response);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Never);
        }

        [Fact]
        public async Task ChangeCommunity_CommunityChanged_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var communityId = Guid.NewGuid();
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId, companyId);
            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();
            communitySale.CompanyId = listingSale.SaleProperty.CompanyId;

            // Act
            await this.Sut.ChangeCommunity(listingId, communityId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task ChangeCommunity_CommunityNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true);
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var communityId = Guid.NewGuid();
            CommunitySale communitySale = null;
            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.ChangeCommunity(listingId, communityId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task ChangeCommunity_ListingSaleNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            SaleListing listingSale = null;
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var communityId = Guid.NewGuid();
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId);
            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ChangeCommunity(listingId, communityId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Never);
        }

        [Fact]
        public async Task ChangePlan_PlanChanged_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(planId, true, companyId);
            plan.CompanyId = listingSale.SaleProperty.CompanyId;
            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act
            await this.Sut.ChangePlan(listingId, planId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task ChangePlan_PlanNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true);
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var planId = Guid.NewGuid();
            Plan plan = null;
            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<Plan>>(() => this.Sut.ChangePlan(listingId, planId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task ChangePlan_ListingSaleNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            SaleListing listingSale = null;
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var planId = Guid.NewGuid();
            var plan = TestModelProvider.GetPlanEntity(planId, true);
            this.planRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(plan)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.ChangePlan(listingId, planId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.planRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == planId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Never);
        }

        [Fact]
        public async Task WhenCallSaveListingChangesAndListingSaleNotExists_ThrowNotFoundArgument()
        {
            var listingSaleId = Guid.NewGuid();
            var listingSaleRequestDto = TestModelProvider.GetListingSaleRequestDto();

            this.listingSaleRepository.Setup(r => r.GetById(It.Is<Guid>(id => id == listingSaleId), It.Is<bool>(filterByCompany => filterByCompany))).ReturnsAsync(TestModelProvider.GetListingSaleEntity(listingSaleId)).Verifiable();
            this.listingSaleRepository.Setup(r => r.SaveChangesAsync(It.IsAny<SaleListing>())).Verifiable();
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.SaveListingChanges(Guid.NewGuid(), listingSaleRequestDto));
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
        }

        [Fact]
        public async Task WhenCallSaveListingChangesAndListingsaleExists_ListingSaleIsSuccessUpdated()
        {
            var listingSaleId = Guid.NewGuid();
            var listingSaleRequestDto = TestModelProvider.GetListingSaleRequestDto();

            this.listingSaleRepository.Setup(r => r.GetById(It.Is<Guid>(id => id == listingSaleId), It.Is<bool>(filterByCompany => filterByCompany))).ReturnsAsync(TestModelProvider.GetListingSaleEntity(listingSaleId)).Verifiable();
            this.listingSaleRepository.Setup(r => r.SaveChangesAsync(It.IsAny<SaleListing>())).Verifiable();

            var result = await this.Sut.SaveListingChanges(listingSaleId, listingSaleRequestDto);
            this.listingSaleRepository.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once);
            this.listingSaleRepository.Verify(x => x.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UnlockListing_ListingNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            SaleListing listingSale = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.UnlockListing(listingId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UnlockListing_ListingFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);

            var userId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            Mock.Get(listing).Setup(l => l.CanUnlock(It.IsAny<IUserContext>())).Returns(true);

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();

            // Act
            await this.Sut.UnlockListing(listingId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(
                c => c.GetById(
                    It.Is<Guid>(id => id == listingId),
                    It.Is<bool>(filterByCompany => filterByCompany)),
                Times.Once);
        }

        [Fact]
        public async Task UnlockNotSubmittedListingAsCompanyAdmin_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);
            user.EmployeeRole = RoleEmployee.CompanyAdmin;

            await this.UnlockListing_Success(user, LockedStatus.LockedNotSubmitted, user.Id);
        }

        [Fact]
        public async Task UnlockNotSubmittedListingAsCompanyAdmin_LockByOtherUser_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);
            user.EmployeeRole = RoleEmployee.CompanyAdmin;

            await this.UnlockListing_Success(user, LockedStatus.LockedNotSubmitted, Guid.NewGuid());
        }

        [Fact]
        public async Task UnlockNotSubmittedListingAsSalesEmployee_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);
            user.EmployeeRole = RoleEmployee.SalesEmployee;

            await this.UnlockListing_Success(user, LockedStatus.LockedNotSubmitted, userId);
        }

        [Fact]
        public async Task UnlockListingLockedByUserAsMlsAdmin_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, UserRole.MLSAdministrator);

            await this.UnlockListing_Success(user, LockedStatus.LockedByUser, Guid.NewGuid());
        }

        [Fact]
        public async Task UnlockListingLockedBySystemAsMlsAdmin_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, UserRole.MLSAdministrator);

            await this.UnlockListing_Success(user, LockedStatus.LockedBySystem, userId);
        }

        [Fact]
        public async Task DeclinePhotos_ListingFound_Success()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);

            var userId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listing)
                .Verifiable();

            // Act
            await this.Sut.DeclinePhotos(listingId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(
                c => c.GetById(
                    It.Is<Guid>(id => id == listingId),
                    It.IsAny<bool>()),
                Times.Once);
        }

        [Fact]
        public async Task DeclinePhotos_ListingNotFound_Error()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            SaleListing listingSale = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<SaleListing>>(() => this.Sut.DeclinePhotos(listingId));
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Never);
            this.listingSaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task UpdateActionTypeSuccess()
        {
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            var newActionType = ActionType.Comparable;

            // Act
            await this.Sut.UpdateActionTypeAsync(listingId, newActionType);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
        }

        private async Task UnlockListing_Success(UserContext user, LockedStatus lockedStatus, Guid lockedBy)
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true, user.CompanyId);
            listing.LockedBy = lockedBy;
            listing.LockedStatus = lockedStatus;
            Mock.Get(listing).Setup(l => l.CanUnlock(It.IsAny<IUserContext>())).Returns(true);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();

            // Act
            await this.Sut.UnlockListing(listingId);

            // Assert
            this.listingSaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<SaleListing>()), Times.Once);
            this.listingSaleRepository.Verify(
                c => c.GetById(
                    It.Is<Guid>(id => id == listingId),
                    It.Is<bool>(filterByCompany => filterByCompany)),
                Times.Once);
        }

        private void SetupSubscriptionClientCompany(Guid companyId, ServiceCode? service = null)
        {
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var services = service.HasValue
                ? new CompanyResponse.ServiceSubscription[] { new() { ServiceCode = service.Value } }
                : Array.Empty<CompanyResponse.ServiceSubscription>();

            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompanyServices(companyId, It.IsAny<FilterServiceSubscriptionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<CompanyResponse.ServiceSubscription>(services, services.Length));
        }
    }
}
