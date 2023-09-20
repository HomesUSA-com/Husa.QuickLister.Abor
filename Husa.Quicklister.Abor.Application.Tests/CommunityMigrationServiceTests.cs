namespace Husa.Quicklister.Abor.Application.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Response;
    using Husa.Extensions.Common.Exceptions;
    using Husa.Migration.Api.Client;
    using Husa.Quicklister.Abor.Application.Services.Communities;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Repositories;
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

        public CommunityMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new CommunityMigrationService(
                this.communityRepository.Object,
                this.migrationClient.Object,
                this.serviceSubscriptionClient.Object,
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
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var communitysReponse = MigrationCommunitiesResponse(20);
            this.migrationClient.Setup(m => m.Communities.GetByCompanyIdAsync(legacyCompanyId, It.IsAny<CancellationToken>())).ReturnsAsync(communitysReponse).Verifiable();

            // Act
            await this.Sut.MigrateByCompanyId(companyId);

            // Assert
            this.communityRepository.Verify(r => r.Attach(It.Is<IEnumerable<CommunitySale>>(x => x.Count() == communitysReponse.Count())), Times.Once);
            this.communityRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task MigrateByCompanyIdNoCommunitiesFound()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            this.migrationClient.Setup(m => m.Communities.GetByCompanyIdAsync(legacyCompanyId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<MigrationContracts.Community.CommunityResponse>()).Verifiable();

            // Act
            await this.Sut.MigrateByCompanyId(companyId);

            // Assert
            this.communityRepository.Verify(r => r.Attach(It.IsAny<IEnumerable<CommunitySale>>()), Times.Never);
        }

        [Fact]
        public async Task MigrateByCompanyIdNoLegacyCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = null;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CompanyDetail)null);

            // Act and Assert
            await Assert.ThrowsAsync<DomainException>(() => this.Sut.MigrateByCompanyId(companyId));
        }

        private static IEnumerable<MigrationContracts.Community.CommunityResponse> MigrationCommunitiesResponse(int communitysToAdd = 20)
        {
            for (int num = 1; num <= communitysToAdd; num++)
            {
                yield return new()
                {
                    Id = Guid.NewGuid(),
                    ProfileInfo = new()
                    {
                        Name = string.Join(" ", Faker.Lorem.Words(2)),
                    },
                    PropertyInfo = new()
                    {
                        ZipCode = Faker.Address.ZipCode()[..5],
                    },
                    SchoolsInfo = new(),
                    FinancialInfo = new(),
                    ShowingInfo = new(),
                    UtilitiesInfo = new(),
                    SaleOffice = new(),
                };
            }
        }
    }
}
