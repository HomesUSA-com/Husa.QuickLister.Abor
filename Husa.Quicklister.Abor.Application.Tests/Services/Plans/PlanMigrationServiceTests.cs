namespace Husa.Quicklister.Abor.Application.Tests.Services.Plans
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Request;
    using Husa.Quicklister.Abor.Application.Interfaces.Plan;
    using Husa.Quicklister.Abor.Application.Services.Plans;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Entities.Plan;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using MigrationContracts = Husa.Migration.Api.Contracts.Response;
    using MigrationEnums = Husa.Migration.Enums;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class PlanMigrationServiceTests
    {
        private readonly Mock<IPlanRepository> planRepository = new();
        private readonly Mock<ILogger<PlanMigrationService>> logger = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IMigrationClient> migrationClient = new();
        private readonly Mock<IPlanPhotoService> photoService = new();
        private readonly Mock<IPlanMediaService> mediaService = new();

        public PlanMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.Sut = new PlanMigrationService(
                this.planRepository.Object,
                this.migrationClient.Object,
                this.serviceSubscriptionClient.Object,
                this.photoService.Object,
                this.mediaService.Object,
                this.logger.Object,
                fixture.Mapper);
        }

        private PlanMigrationService Sut { get; set; }

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

            var plansReponse = MigrationPlansResponse(20);
            this.migrationClient.Setup(m => m.Plans.GetAsync(It.Is<ProfileFilter>(x => x.CompanyId == legacyCompanyId), It.IsAny<CancellationToken>())).ReturnsAsync(plansReponse).Verifiable();

            // Act
            await this.Sut.MigrateAsync(companyId, new()
            {
                CreateEntity = true,
            });

            // Assert
            this.planRepository.Verify(r => r.Attach(It.Is<IEnumerable<Plan>>(x => x.Count() == plansReponse.Count())), Times.Once);
            this.planRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task MigrateByCompanyId_UpdatePlans_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var companyDetail = TestModelProvider.GetCompanyDetail(companyId);
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var plansReponse = MigrationPlansResponse(20);
            this.migrationClient.Setup(m => m.Plans.GetAsync(It.Is<ProfileFilter>(x => x.CompanyId == legacyCompanyId), It.IsAny<CancellationToken>())).ReturnsAsync(plansReponse).Verifiable();

            var planId = Guid.NewGuid();
            var plan = PlanTestProvider.GetPlanEntity(planId);
            this.planRepository.Setup(x => x.GetIdByLegacyId(It.IsAny<int>())).ReturnsAsync(planId);
            this.planRepository.Setup(x => x.GetById(It.Is<Guid>(id => id == planId), It.IsAny<bool>())).ReturnsAsync(plan);

            // Act
            await this.Sut.MigrateAsync(companyId, new()
            {
                UpdateEntity = true,
            });

            // Assert
            this.planRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        private static IEnumerable<MigrationContracts.PlanResponse> MigrationPlansResponse(int plansToAdd = 20)
        {
            for (int num = 1; num <= plansToAdd; num++)
            {
                yield return new()
                {
                    Id = num,
                    PlanName = $"Plan {num}",
                    Rooms = new List<MigrationContracts.RoomResponse>
                    {
                        new MigrationContracts.RoomResponse
                        {
                            Level = "1",
                            Length = 6,
                            Width = 7,
                            RoomType = MigrationEnums.RoomType.MasterBath,
                        },
                        new MigrationContracts.RoomResponse
                        {
                            Level = "0",
                            Length = 6,
                            Width = 7,
                            RoomType = MigrationEnums.RoomType.MasterBedroom,
                        },
                    },
                };
            }
        }
    }
}
