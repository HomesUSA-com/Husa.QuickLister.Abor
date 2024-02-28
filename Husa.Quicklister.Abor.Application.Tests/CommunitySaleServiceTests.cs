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
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Husa.Quicklister.Extensions.Application.Models.Community;
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

        public CommunitySaleServiceTests()
        {
            this.Sut = new SaleCommunityService(
                this.communitySaleRepository.Object,
                this.communityHistoryService.Object,
                this.serviceSubscriptionClient.Object,
                this.userContextProvider.Object,
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
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
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
        public async Task DeleteCommunity_DeleteComplete_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var communitySale = TestModelProvider.GetCommunitySaleEntity(communityId);
            var deleteInCascade = false;

            this.communitySaleRepository
                .Setup(c => c.GetById(
                    It.Is<Guid>(id => id == communityId),
                    It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            // Act
            await this.Sut.DeleteCommunity(communityId, deleteInCascade);

            // Assert
            this.communitySaleRepository.Verify(r => r.UpdateAsync(It.IsAny<CommunitySale>()), Times.Once);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task DeleteCommunity_DeleteFail_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            CommunitySale communitySale = null;
            var deleteInCascade = false;

            this.communitySaleRepository
                .Setup(c => c.GetById(
                    It.Is<Guid>(id => id == communityId),
                    It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(communitySale)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.DeleteCommunity(communityId, deleteInCascade));
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CommunitySale>()), Times.Never);
            this.communitySaleRepository.Verify(c => c.GetById(It.Is<Guid>(id => id == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
        }

        [Fact]
        public async Task AddCommunityEmployees_CommunityFound_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid>()
            {
                userId1,
                userId2,
            };

            var employeesDto = new CommunityEmployeesCreateDto() { UserIds = userIds, CommunityId = communityId, CompanyId = companyId };

            var community = new Mock<CommunitySale>();

            Mock.Get(community.Object)
                .Setup(c => c.AddCommunityEmployees(It.Is<IEnumerable<Guid>>(e => e == userIds))).Verifiable();

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(community.Object)
                .Verifiable();

            this.communitySaleRepository
                .Setup(c => c.SaveChangesAsync(It.Is<CommunitySale>(comm => comm == community.Object)))
                .Verifiable();

            // Act
            await this.Sut.CreateEmployeesAsync(employeesDto);

            // Assert
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(community.Object), Times.Once);
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task AddCommunityEmployees_CommunityNotFound_Exception()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid>()
            {
                userId1,
                userId2,
            };

            var employeesDto = new CommunityEmployeesCreateDto() { UserIds = userIds, CommunityId = communityId, CompanyId = companyId };

            CommunitySale community = null;

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(community)
                .Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.CreateEmployeesAsync(employeesDto));
            this.communitySaleRepository.Verify();
        }

        [Fact]
        public async Task DeleteCommunityEmployees_CommunityFound_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var employeeId1 = Guid.NewGuid();
            var employeeId2 = Guid.NewGuid();
            var employeeIds = new List<Guid>()
            {
                employeeId1,
                employeeId2,
            };

            CommunityEmployee employee1 = TestModelProvider.GetCommunityEmployee(employeeId1, communityId, Guid.NewGuid());
            CommunityEmployee employee2 = TestModelProvider.GetCommunityEmployee(employeeId2, communityId, Guid.NewGuid());

            var communityEmployees = new List<CommunityEmployee>() { employee1, employee2 };

            var employeesDto = new CommunityEmployeesDeleteDto() { UserIds = employeeIds, CommunityId = communityId };

            var communityMock = TestModelProvider.GetCommunitySaleEntityMock(communityId, companyId);
            communityMock.Setup(x => x.Employees).Returns(communityEmployees);
            var community = communityMock.Object;

            this.communitySaleRepository
                .Setup(c => c.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()))
                .ReturnsAsync(community)
                .Verifiable();

            this.communitySaleRepository
                .Setup(c => c.SaveChangesAsync(It.Is<CommunitySale>(comm => comm == community)))
                .Verifiable();

            // Act
            await this.Sut.DeleteEmployeesAsync(employeesDto);

            // Assert
            this.communitySaleRepository.Verify(r => r.SaveChangesAsync(community), Times.Once);
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>()), Times.Once);
        }
    }
}
