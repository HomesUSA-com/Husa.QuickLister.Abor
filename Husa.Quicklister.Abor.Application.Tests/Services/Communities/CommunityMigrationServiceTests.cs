namespace Husa.Quicklister.Abor.Application.Tests.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Application.Interfaces.Community;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using MigrationContracts = Husa.Migration.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class CommunityMigrationServiceTests
    {
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();
        private readonly Mock<ILogger<CommunityMigrationService>> logger = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IMigrationClient> migrationClient = new();
        private readonly Mock<ICommunityPhotoService> photoService = new();
        private readonly Mock<ISaleCommunityService> communityService = new();
        private readonly Mock<ICommunityMediaService> mediaService = new();

        public CommunityMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new CommunityMigrationService(
                this.communityRepository.Object,
                this.migrationClient.Object,
                this.serviceSubscriptionClient.Object,
                this.photoService.Object,
                this.communityService.Object,
                this.mediaService.Object,
                this.logger.Object,
                fixture.Mapper);
        }

        private CommunityMigrationService Sut { get; set; }

        [Fact]
        public async Task MigrateByCompanyIdSuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var communitysReponse = MigrationCommunitiesResponse(20);
            this.migrationClient.Setup(m => m.Communities.GetAsync(It.Is<ProfileFilter>(x => x.CompanyId == legacyCompanyId), It.IsAny<CancellationToken>())).ReturnsAsync(communitysReponse).Verifiable();

            // Act
            await this.Sut.MigrateAsync(companyId, new()
            {
                CreateEntity = true,
            });

            // Assert
            this.communityRepository.Verify(r => r.Attach(It.IsAny<IEnumerable<CommunitySale>>()), Times.Once);
            this.communityRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task MigrateByCompanyId_UpdateCommunty_Suuccess()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var communitysReponse = MigrationCommunitiesResponse(20);
            this.migrationClient.Setup(m => m.Communities.GetAsync(It.Is<ProfileFilter>(x => x.CompanyId == legacyCompanyId), It.IsAny<CancellationToken>())).ReturnsAsync(communitysReponse).Verifiable();

            var communityId = Guid.NewGuid();
            var community = CommunityTestProvider.GetCommunityEntity(communityId);
            this.communityRepository.Setup(x => x.GetIdByLegacyId(It.IsAny<int>())).ReturnsAsync(communityId);
            this.communityRepository.Setup(x => x.GetById(It.Is<Guid>(id => id == communityId), It.IsAny<bool>())).ReturnsAsync(community);

            // Act
            await this.Sut.MigrateAsync(companyId, new()
            {
                UpdateEntity = true,
            });

            // Assert
            this.communityRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private static IEnumerable<MigrationContracts.Community.CommunityResponse> MigrationCommunitiesResponse(int communitysToAdd = 20)
        {
            for (int num = 1; num <= communitysToAdd; num++)
            {
                yield return new()
                {
                    LegacyCommunityId = num,
                    ProfileInfo = new()
                    {
                        Name = $"Community {num}",
                    },
                    PropertyInfo = new()
                    {
                        ZipCode = "54545",
                    },
                    SchoolsInfo = new(),
                    FinancialInfo = new(),
                    ShowingInfo = new(),
                    UtilitiesInfo = new(),
                    SaleOffice = new(),
                    EmailLeads = new(),
                };
            }
        }
    }
}
