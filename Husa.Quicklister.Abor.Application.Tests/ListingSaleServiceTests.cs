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
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;
    using Husa.Xml.Api.Client.Interface;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;
    using ExtensionsInterfaces = Husa.Quicklister.Extensions.Application.Interfaces.Listing;

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
        private readonly Mock<IXmlClient> xXmlClient = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ILogger<SaleListingService>> logger = new();
        private readonly Mock<ExtensionsInterfaces.ISaleListingMediaService> listingMediaService = new();
        private readonly Mock<ISaleListingPhotoService> saleListingPhotoService = new();
        private readonly Mock<ExtensionsInterfaces.ILockLegacyListingService> legacyListingService = new();

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
                this.saleListingPhotoService.Object,
                this.legacyListingService.Object,
                this.xXmlClient.Object,
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

        [Theory]
        [InlineData(MarketStatuses.Active)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        [InlineData(MarketStatuses.Pending)]
        [InlineData(MarketStatuses.Hold)]
        public async Task QuickCreateAsync_ThereIsAvailableListingWithSameAddress_Exception(MarketStatuses existingListingStatus)
        {
            // Arrange
            var listingSale = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);
            listingSale.MlsStatus = existingListingStatus;
            var listingSaleDto = new QuickCreateListingDto()
            {
                StreetNumber = listingSale.SaleProperty.AddressInfo.StreetNumber,
                StreetName = listingSale.SaleProperty.AddressInfo.StreetName,
                ZipCode = listingSale.SaleProperty.AddressInfo.ZipCode,
                City = listingSale.SaleProperty.AddressInfo.City,
                UnitNumber = listingSale.SaleProperty.AddressInfo.UnitNumber,
            };
            this.listingSaleRepository
                .Setup(c => c.GetListing(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Cities>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(listingSale)
                .Verifiable();
            var result = await this.Sut.QuickCreateAsync(listingSaleDto, false);

            // Act & Assert
            Assert.Equal(ResponseCode.Error, result.Code);
        }

        [Theory]
        [InlineData(MarketStatuses.Active)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        [InlineData(MarketStatuses.Pending)]
        [InlineData(MarketStatuses.Hold)]
        public async Task UpdateListing_ThereIsAvailableListingWithSameAddress_Exception(MarketStatuses existingListingStatus)
        {
            // Arrange
            var listingSale = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);
            listingSale.MlsStatus = existingListingStatus;
            var listingSaleDto = TestModelProvider.GetSaleListingDto();
            listingSaleDto.SaleProperty.AddressInfo.StreetNumber = listingSale.SaleProperty.AddressInfo.StreetNumber;
            listingSaleDto.SaleProperty.AddressInfo.StreetName = listingSale.SaleProperty.AddressInfo.StreetName;
            listingSaleDto.SaleProperty.AddressInfo.ZipCode = listingSale.SaleProperty.AddressInfo.ZipCode;
            listingSaleDto.SaleProperty.AddressInfo.City = listingSale.SaleProperty.AddressInfo.City;
            listingSaleDto.SaleProperty.AddressInfo.UnitNumber = listingSale.SaleProperty.AddressInfo.UnitNumber;
            this.listingSaleRepository
                .Setup(c => c.GetListing(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Cities>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(listingSale)
                .Verifiable();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => this.Sut.UpdateListing(Guid.NewGuid(), listingSaleDto));
        }

        [Fact]
        public async Task UpdateListing_UpdateComplete_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(listingId, true, companyId);
            var listingSaleDto = TestModelProvider.GetSaleListingDto();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, userRole: UserRole.MLSAdministrator);

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listingSale)
                .Verifiable();

            this.SetupSubscriptionClientCompany(companyId, blockSquareFootage: true);

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
            var company = TestModelProvider.GetCompanyDetail();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            Mock.Get(listing).Setup(l => l.CanUnlock(It.IsAny<IUserContext>(), false)).Returns(true);

            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();

            this.serviceSubscriptionClient
                .Setup(x => x.Company
                    .GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

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

        [Fact]
        public async Task ImportListingInfoToPlan()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);
            this.listingSaleRepository
                 .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
            .ReturnsAsync(listingSale)
                 .Verifiable();
            listingSale.SaleProperty.SpacesDimensionsInfo = TestModelProvider.GetSpacesDimensionsInfo();

            // Act
            await this.Sut.CopyListingInfoToListingPlan(listingId);

            // Assert
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.StoriesTotal, listingSale.SaleProperty.Plan.BasePlan.StoriesTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.HalfBathsTotal, listingSale.SaleProperty.Plan.BasePlan.HalfBathsTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.FullBathsTotal, listingSale.SaleProperty.Plan.BasePlan.FullBathsTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.SqFtTotal, listingSale.SaleProperty.Plan.BasePlan.SqFtTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.DiningAreasTotal, listingSale.SaleProperty.Plan.BasePlan.DiningAreasTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.MainLevelBedroomTotal, listingSale.SaleProperty.Plan.BasePlan.MainLevelBedroomTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.OtherLevelsBedroomTotal, listingSale.SaleProperty.Plan.BasePlan.OtherLevelsBedroomTotal);
            Assert.Equal(listingSale.SaleProperty.SpacesDimensionsInfo.LivingAreasTotal, listingSale.SaleProperty.Plan.BasePlan.LivingAreasTotal);
        }

        [Fact]
        public async Task ImportListingInfoToCommunity()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listingSale = TestModelProvider.GetListingSaleEntity(Guid.NewGuid(), true);

            listingSale.SaleProperty.Community.AppointmentType = null;
            listingSale.AppointmentType = AppointmentType.AppointmentRequired;
            listingSale.SaleProperty.Community.AdditionalInstructions = null;
            listingSale.AdditionalInstructions = new() { NotesForApptStaff = "Note 1", NotesForShowingAgent = "Note 2" };
            listingSale.SaleProperty.Community.Financial.BuyersAgentCommission = null;
            listingSale.SaleProperty.FinancialInfo.BuyersAgentCommission = 100;
            listingSale.SaleProperty.Community.Property.County = Counties.Bexar;
            listingSale.SaleProperty.AddressInfo.County = Counties.Baylor;
            listingSale.SaleProperty.Community.Utilities.Fireplaces = 1;
            listingSale.SaleProperty.FeaturesInfo.Fireplaces = null;
            listingSale.SaleProperty.Community.Utilities.CoolingSystem = null;
            listingSale.SaleProperty.FeaturesInfo.CoolingSystem = new[] { CoolingSystem.CeilingFan };

            this.listingSaleRepository
                 .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>()))
            .ReturnsAsync(listingSale)
                 .Verifiable();

            await this.Sut.CopyListingInfoToCommunity(listingId);

            Assert.Equal(listingSale.SaleProperty.Community.AppointmentType, listingSale.AppointmentType);
            Assert.Equal(listingSale.SaleProperty.Community.AppointmentRestrictions, listingSale.AppointmentRestrictions);
            Assert.Equal(listingSale.SaleProperty.Community.Financial.BuyersAgentCommission, listingSale.SaleProperty.FinancialInfo.BuyersAgentCommission);
            Assert.NotEqual(listingSale.SaleProperty.Community.Property.County, listingSale.SaleProperty.AddressInfo.County);
            Assert.NotNull(listingSale.SaleProperty.Community.Utilities.Fireplaces);
            Assert.Equal(listingSale.SaleProperty.Community.Utilities.CoolingSystem, listingSale.SaleProperty.FeaturesInfo.CoolingSystem);
        }

        private async Task UnlockListing_Success(UserContext user, LockedStatus lockedStatus, Guid lockedBy)
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var listing = TestModelProvider.GetListingSaleEntity(listingId, true, user.CompanyId);
            listing.LockedBy = lockedBy;
            listing.LockedStatus = lockedStatus;
            Mock.Get(listing).Setup(l => l.CanUnlock(It.IsAny<IUserContext>(), false)).Returns(true);
            var company = TestModelProvider.GetCompanyDetail();

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            this.listingSaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == listingId), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(listing)
                .Verifiable();

            this.serviceSubscriptionClient
                .Setup(x => x.Company
                    .GetCompany(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company);

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

        private void SetupSubscriptionClientCompany(Guid companyId, ServiceCode? service = null, bool blockSquareFootage = false)
        {
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId, blockSquareFootage: blockSquareFootage);
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
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
