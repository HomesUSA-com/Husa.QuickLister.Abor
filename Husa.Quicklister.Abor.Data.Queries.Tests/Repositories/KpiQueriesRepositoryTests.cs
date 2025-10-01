namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Reflection;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Data.Queries.Tests.Configuration;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class KpiQueriesRepositoryTests
    {
        private readonly ApplicationServicesFixture fixture = new();
        private readonly Mock<IUserContextProvider> userContextProviderMock = new();
        private readonly Mock<ICompanyCacheRepository> companyCacheRepositoryMock = new();
        private readonly Mock<IKpiQueriesRequestProvider> kpiQueriesRequestProviderMock = new();
        private readonly Mock<ILogger<KpiQueriesRepository>> loggerMock = new();

        [Fact]
        public void Constructor_ShouldInitialize()
        {
            var repo = new KpiQueriesRepository(
                this.fixture.GetInMemoryDbContext<ApplicationQueriesDbContext>(),
                this.userContextProviderMock.Object,
                this.companyCacheRepositoryMock.Object,
                this.kpiQueriesRequestProviderMock.Object,
                this.loggerMock.Object);

            Assert.NotNull(repo);
        }

        [Fact]
        public void GetKpiMarketStatusMapping_ShouldReturnExpectedMapping()
        {
            var repo = new KpiQueriesRepository(
                this.fixture.GetInMemoryDbContext<ApplicationQueriesDbContext>(),
                this.userContextProviderMock.Object,
                this.companyCacheRepositoryMock.Object,
                this.kpiQueriesRequestProviderMock.Object,
                this.loggerMock.Object);

            var mapping = repo.GetType().GetMethod(
                "GetKpiMarketStatusMapping",
                BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(repo, []) as Dictionary<KpiListingStatus, IEnumerable<MarketStatuses>>;

            Assert.NotNull(mapping);
            Assert.True(mapping.ContainsKey(KpiListingStatus.Active));
            Assert.True(mapping.ContainsKey(KpiListingStatus.Pending));
            Assert.True(mapping.ContainsKey(KpiListingStatus.Sold));

            Assert.Equal(SaleListing.ActiveListingStatuses, mapping[KpiListingStatus.Active]);
            Assert.Equal(SaleListing.PendingListingStatuses, mapping[KpiListingStatus.Pending]);
            Assert.Empty(mapping[KpiListingStatus.Sold]);
        }
    }
}
