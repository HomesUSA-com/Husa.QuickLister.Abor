namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Property;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class CommunitySaleServiceTests
    {
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository = new();
        private readonly Mock<ICommunityHistoryService> communityHistoryService = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<ILogger<SaleCommunityService>> logger = new();
        private readonly Mock<ICommunityDeletionService> deletionService = new();

        public CommunitySaleServiceTests()
        {
            this.Sut = new SaleCommunityService(
                this.communitySaleRepository.Object,
                this.communityHistoryService.Object,
                this.serviceSubscriptionClient.Object,
                this.userContextProvider.Object,
                this.deletionService.Object,
                this.mapper.Object,
                this.logger.Object);
        }

        public ISaleCommunityService Sut { get; set; }

        [Fact]
        public async Task CreateCommunitySaleAsync_CreateComplete_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communitySaleDto = TestModelProvider.GetCommunitySaleCreateDto(companyId);
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);

            this.communitySaleRepository
                .Setup(c => c.Attach(It.IsAny<CommunitySale>()))
                .Callback<CommunitySale>(community => community.Id = communityId)
                .Verifiable();

            this.communitySaleRepository
                .Setup(c => c.SaveChangesAsync())
                .Verifiable();

            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            // Act
            var result = await this.Sut.CreateAsync(communitySaleDto);

            // Act and Assert
            this.communitySaleRepository.Verify();
            Assert.Equal(communityId, result.Result);
        }

        [Fact]
        public async Task UpdateCommunity_UpdateComplete_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var employee = TestModelProvider.GetCommunityEmployee(Guid.NewGuid(), communityId, userId);
            var employees = new List<CommunityEmployee>() { employee };
            var communityMock = TestModelProvider.GetCommunitySaleEntityMock(communityId, companyId);
            communityMock.Setup(x => x.Employees).Returns(employees);
            var communitySale = communityMock.Object;

            var communityDto = TestModelProvider.GetCommunityUpdateDto();
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.communitySaleRepository
                .Setup(c => c.GetById(
                    It.Is<Guid>(id => id == communityId),
                    It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act
            await this.Sut.UpdateCommunity(communityId, communityDto);

            // Assert
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UpdateCommunity_UpdateIncomplete_Error()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            CommunitySale communitySale = null;
            var communityDto = TestModelProvider.GetCommunityUpdateDto();

            this.communitySaleRepository
                .Setup(c => c.GetById(
                    It.Is<Guid>(id => id == communityId),
                    It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.UpdateCommunity(communityId, communityDto));
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CommunitySale>()), Times.Never);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task UpdateListingsAsync_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId);
            var saleProperty = new Mock<SaleProperty>();
            var listing = new Mock<SaleListing>();
            listing.Setup(x => x.MlsNumber).Returns("3544");
            listing.Setup(x => x.LockedStatus).Returns(LockedStatus.NoLocked);
            listing.Setup(x => x.MlsStatus).Returns(MarketStatuses.Active);
            saleProperty.Setup(c => c.SaleListings).Returns(new[] { listing.Object });
            listing.Setup(c => c.SaleProperty).Returns(saleProperty.Object);
            community.SaleProperties = new[] { saleProperty.Object };

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(community)
                .Verifiable();

            // Act
            await this.Sut.UpdateListingsAsync(communityId);

            // Assert
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
            saleProperty.Verify(r => r.ImportDataFromCommunity(It.Is<CommunitySale>(x => x.Id == communityId)), Times.Once);
        }
    }
}
