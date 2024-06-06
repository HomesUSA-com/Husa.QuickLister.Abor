namespace Husa.Quicklister.Abor.Application.Tests.Services.SaleListings
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Common.Classes;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Request;
    using Husa.Migration.Api.Contracts.Response.SaleListing;
    using Husa.Migration.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Listing;
    using Husa.Quicklister.Abor.Application.Models;
    using Husa.Quicklister.Abor.Application.Services.SaleListings;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Husa.Quicklister.Extensions.Crosscutting.Providers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class SaleListingMigrationServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly IOptions<ServiceBusSettings> serviceBusSettings;
        private readonly Mock<IListingSaleRepository> listingRepository = new();
        private readonly Mock<IMigrationClient> migrationClient = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ISaleListingService> saleListingService = new();
        private readonly Mock<IAgentRepository> agentRepository = new();
        private readonly Mock<ILogger<SaleListingMigrationService>> logger = new();
        private readonly Mock<IPlanRepository> planRepository = new();
        private readonly Mock<ICommunitySaleRepository> communityRepository = new();
        private readonly Mock<ISaleListingPhotoService> photoService = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<IProvideTraceId> traceIdProvider = new();
        private readonly Mock<ServiceBusClient> busClient = new();

        public SaleListingMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.serviceBusSettings = Options.Create(new ServiceBusSettings { MigrationService = new ServiceBusOptions { TopicName = "TopicName", SubscriptionName = "SubscriptionName" } });
        }

        [Fact]
        public async Task MigrateListings_CreateListing_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var legacyCompanyId = 1;
            var companyDetail = TestModelProvider.GetCompanyDetail();
            companyDetail.LegacyId = legacyCompanyId;
            this.serviceSubscriptionClient
                .Setup(c => c.Company.GetCompany(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyDetail);

            var listingReponse = new SaleListingResponse()
            {
                SaleProperty = MigrationProvider.GetSaleProperty(),
                StatusFieldsInfo = MigrationProvider.GetStatusFieldsResponse(),
            };
            this.migrationClient.Setup(m => m.SaleListings.GetAsync(It.IsAny<MigrationMarketType>(), It.Is<ListingFilter>(x => x.CompanyId == legacyCompanyId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { listingReponse }).Verifiable();

            var listingId = Guid.NewGuid();
            this.saleListingService.Setup(m => m.CreateAsync(It.IsAny<QuickCreateListingDto>())).ReturnsAsync(CommandSingleResult<Guid, string>.Success(listingId)).Verifiable();
            var listing = ListingTestProvider.GetListingEntity(listingId: listingId);
            this.listingRepository.Setup(m => m.GetById(It.Is<Guid>(id => id == listingId), It.IsAny<bool>())).ReturnsAsync(listing).Verifiable();

            var sut = this.GetSut();

            // Act
            await sut.MigrateListings(companyId, new()
            {
                CreateListing = true,
            });

            // Assert
            this.saleListingService.Verify(r => r.CreateAsync(It.IsAny<QuickCreateListingDto>()), Times.Once);
        }

        private SaleListingMigrationService GetSut()
            => new(
                this.listingRepository.Object,
                this.migrationClient.Object,
                this.serviceSubscriptionClient.Object,
                this.saleListingService.Object,
                this.agentRepository.Object,
                this.planRepository.Object,
                this.communityRepository.Object,
                this.photoService.Object,
                this.userContextProvider.Object,
                this.traceIdProvider.Object,
                this.busClient.Object,
                this.serviceBusSettings,
                this.logger.Object,
                this.fixture.Mapper);
    }
}
