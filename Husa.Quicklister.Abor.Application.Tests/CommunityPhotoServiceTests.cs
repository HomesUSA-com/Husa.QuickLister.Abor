namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Exceptions;
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

    public class CommunityPhotoServiceTests : PhotoServiceTests<ICommunityPhotoService>
    {
        private readonly Mock<ICommunitySaleRepository> communitySaleRepository;
        private readonly Mock<ILogger<CommunityPhotoService>> logger;
        private readonly Mock<IUserContextProvider> userContextProvider;

        public CommunityPhotoServiceTests(ApplicationServicesFixture fixture)
            : base(fixture)
        {
            this.logger = new Mock<ILogger<CommunityPhotoService>>();
            this.communitySaleRepository = new Mock<ICommunitySaleRepository>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.Sut = new CommunityPhotoService(
                this.fixture.BusOptions.Object,
                this.userContextProvider.Object,
                this.photoServiceClient.Object,
                this.client.Object,
                this.traceIdProvider.Object,
                this.communitySaleRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_Success()
        {
            // Arrange & Act
            var entityId = Guid.NewGuid();
            var community = TestModelProvider.GetCommunitySaleEntity(entityId);
            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(community)
                .Verifiable();

            await this.SetupAssignLatestPhotoRequest(entityId);

            // Assert
            this.communitySaleRepository.Verify(t => t.GetById(entityId, false), Times.Once);
        }

        [Fact]
        public async Task AssignLatestPhotoRequest_NotFoundException()
        {
            // Arrange
            var entityId = Guid.NewGuid();

            CommunitySale community = null;
            this.communitySaleRepository
                .Setup(x => x.GetById(It.IsAny<Guid>(), false))
                .ReturnsAsync(community)
                .Verifiable();

            var photorequestId = Guid.NewGuid();
            var creationDate = DateTime.UtcNow;

            // Act
            await Assert.ThrowsAsync<NotFoundException<CommunitySale>>(() => this.Sut.AssignLatestPhotoRequest(entityId, photorequestId, creationDate));
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
