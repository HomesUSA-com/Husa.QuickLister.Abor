namespace Husa.Quicklister.Abor.Application.Tests.Services.ListingRequests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Migration.Api.Client;
    using Husa.Migration.Api.Contracts.Response;
    using Husa.Migration.Crosscutting.Enums;
    using Husa.Quicklister.Abor.Application.Interfaces.Request;
    using Husa.Quicklister.Abor.Application.Services.ListingRequests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Repositories;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Application.Test")]
    public class ListingRequestMigrationServiceTests
    {
        private readonly ApplicationServicesFixture fixture;
        private readonly Mock<IListingSaleRepository> listingRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IMigrationClient> migrationClient = new();
        private readonly Mock<IUserContextProvider> userContextProvider = new();
        private readonly Mock<ISaleListingRequestService> saleListingRequestService = new();
        private readonly Mock<ILogger<ListingRequestMigrationService>> logger = new();
        private readonly Mock<IAgentRepository> agentRepository = new();

        public ListingRequestMigrationServiceTests(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.userContextProvider.Setup(m => m.GetCurrentUserId()).Returns(Guid.NewGuid());
        }

        [Fact]
        public async Task MigrateByMlsNumber_Success()
        {
            var mlsNumber = "1062023";
            var listing = ListingTestProvider.GetListingEntity();
            listing.MlsNumber = mlsNumber;
            this.listingRepository.Setup(r => r.GetListingByMlsNumber(It.Is<string>(value => value == mlsNumber))).ReturnsAsync(listing).Verifiable();

            var migrationRequest = new SaleListingRequestResponse()
            {
                MlsNumber = mlsNumber,
                SaleProperty = new()
                {
                    OwnerName = "OwnerName",
                    AddressInfo = new(),
                    SpacesDimensionsInfo = new(),
                    FinancialInfo = new(),
                    FeaturesInfo = new(),
                    PropertyInfo = new(),
                    SchoolsInfo = new(),
                    SalesOfficeInfo = new(),
                    ShowingInfo = new(),
                },
                StatusFieldsInfo = new(),
                PublishInfo = new(),
            };
            this.migrationClient
                .Setup(m => m.ListingRequests.GetAsync(It.Is<MigrationMarketType>(x => x == MigrationMarketType.Austin), It.IsAny<int?>(), It.Is<string>(value => value == mlsNumber), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { migrationRequest });

            var sut = this.GetSut();

            await sut.MigrateByMlsNumber(mlsNumber);

            this.agentRepository.Verify(r => r.GetAgentByMarketUniqueId(It.IsAny<string>()), Times.Never);
            this.saleListingRequestService.Verify(r => r.GenerateRequestFromMigrationAsync(It.Is<SaleListingRequest>(x => x.MlsNumber == mlsNumber), It.IsAny<CancellationToken>()), Times.Once);
        }

        private ListingRequestMigrationService GetSut()
            => new(
                this.listingRepository.Object,
                this.migrationClient.Object,
                this.serviceSubscriptionClient.Object,
                this.saleListingRequestService.Object,
                this.userContextProvider.Object,
                this.agentRepository.Object,
                this.logger.Object,
                this.fixture.Mapper);
    }
}
