namespace Husa.Quicklister.Abor.Application.Tests.Services.Communities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Community;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyRequest = Husa.CompanyServicesManager.Api.Contracts.Request;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;
    using MigrationContracts = Husa.Migration.Api.Contracts.Response;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class CommunityHistoryMigrationServiceTests
    {
        private readonly Mock<ICommunityHistoryService> communityHistoryService = new();
        private readonly Mock<ICommunityHistoryRepository> communityHistoryRepository = new();
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ICommunityMigrationService> communityMigrationService = new();
        private readonly Mock<IMigrationClient> migrationClient = new();
        private readonly Mock<ILogger<CommunityHistoryMigrationService>> logger = new();

        public CommunityHistoryMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new CommunityHistoryMigrationService(
                this.communityHistoryService.Object,
                this.communityHistoryRepository.Object,
                this.communityRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.userContextProvider.Object,
                this.migrationClient.Object,
                this.communityMigrationService.Object,
                this.logger.Object,
                fixture.Mapper);
        }

        private CommunityHistoryMigrationService Sut { get; set; }

        [Fact]
        public async Task MigrateAsync_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var legacyCommunityId = 1;
            var communityId = Guid.NewGuid();
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var communitysReponse = MigrationCommunitiesResponse(legacyCommunityId, 20);
            this.migrationClient.Setup(m => m.CommunityHistory.GetAsync(It.IsAny<CommunityHistoryFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(communitysReponse).Verifiable();
            this.communityRepository.Setup(x => x.GetIdByLegacyId(legacyCommunityId)).ReturnsAsync(communityId);

            this.serviceSubscriptionClient
                .Setup(m => m.User.GetAsync(It.IsAny<CompanyRequest.UserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<CompanyResponse.User>(Array.Empty<CompanyResponse.User>(), 0));

            var community = new CommunitySale(companyId, "comNmae", "company") { Id = communityId };
            community.UpdateTrackValues(Guid.NewGuid());
            this.communityRepository.Setup(x => x.GetById(communityId, It.IsAny<bool>())).ReturnsAsync(community);

            // Act
            await this.Sut.MigrateAsync(companyDetail.Id, new()
            {
                CommunityId = legacyCommunityId,
            });

            // Assert
            this.communityHistoryService.Verify(x => x.AddRecordAsync(It.IsAny<CommunityHistory>(), It.IsAny<CancellationToken>()), Times.Exactly(communitysReponse.Count()));
        }

        private static IEnumerable<MigrationContracts.Community.CommunityHistoryResponse> MigrationCommunitiesResponse(int communityId, int communitysToAdd = 20)
        {
            for (int num = 1; num <= communitysToAdd; num++)
            {
                yield return new()
                {
                    LegacyCommunityHistoryId = num,
                    LegacyCommunityId = communityId,
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
                    SysModifiedOn = DateTime.Now.AddDays(1),
                };
            }
        }
    }
}
