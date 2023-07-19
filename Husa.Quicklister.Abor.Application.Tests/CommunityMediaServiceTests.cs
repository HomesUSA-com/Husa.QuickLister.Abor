namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Extensions.Media.Interfaces;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]

    public class CommunityMediaServiceTests : MediaServiceTests<ICommunityMediaService>
    {
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository;
        private readonly Mock<ILogger<CommunityMediaService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;
        private readonly Mock<IBlobService> blobService = new();
        private readonly Mock<ICache> cache = new();

        public CommunityMediaServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<CommunityMediaService>>();
            this.communitySaleRepository = new Mock<ICommunitySaleRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new CommunityMediaService(
                this.busOptions.Object,
                this.userContextProvider.Object,
                this.mediaServiceClient.Object,
                this.busClient.Object,
                this.traceIdProvider.Object,
                this.communitySaleRepository.Object,
                this.blobService.Object,
                this.cache.Object,
                this.logger.Object);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_Success()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            this.SetupValidEntityAndUser(communityId, userId);

            // Act
            await this.Sut.ValidateEntityAndUserCompany(communityId);

            // Assert
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_EntityNotFound_Error()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            CommunitySale community = null;
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(community)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.ValidateEntityAndUserCompany(communityId));
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Never);
        }

        [Fact]
        public async Task ValidateEntityAndUserCompany_IsNotCompanyEmployee_Error()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var communityCompanyId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(communityId, communityCompanyId);

            var userId = Guid.NewGuid();
            var userCompanyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, userCompanyId);

            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(community)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();

            // Act and Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.ValidateEntityAndUserCompany(communityId));
            this.communitySaleRepository.Verify(r => r.GetById(It.Is<Guid>(x => x == communityId), It.Is<bool>(filterByCompany => filterByCompany)), Times.Once);
            this.userContextProvider.Verify(r => r.GetCurrentUser(), Times.Once);
        }

        protected override void SetupValidEntityAndUser(Guid entityId, Guid userId)
        {
            var companyId = Guid.NewGuid();

            var community = TestModelProvider.GetCommunitySaleEntity(entityId, companyId);
            var user = TestModelProvider.GetCurrentUser(userId, companyId);

            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.Is<bool>(filterByCompany => filterByCompany)))
                .ReturnsAsync(community)
                .Verifiable();
            this.userContextProvider.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
