namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Queries.Interfaces;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Data.Queries.Tests.Configuration;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
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
            Assert.Equal([MarketStatuses.Closed], mapping[KpiListingStatus.Sold]);
        }

        [Fact]
        public void FilterByPendingDate_WithListingsInsideRange_ReturnsOnlyThoseWithinRange()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var inside = now.AddDays(-1);
            var outside = now.AddDays(-10);

            var companyId = Guid.NewGuid();

            var listingInside = new SaleListing(
                MarketStatuses.Pending,
                streetName: "Street",
                streetNum: "1",
                unitNumber: string.Empty,
                city: default,
                state: default,
                zipCode: "00000",
                county: null,
                streetType: null,
                constructionCompletionDate: null,
                companyId: companyId,
                ownerName: string.Empty,
                communityId: null,
                planId: null,
                manuallyManaged: false)
            {
                StatusFieldsInfo = { PendingDate = inside },
            };

            var listingOutside = new SaleListing(
                MarketStatuses.Pending,
                streetName: "Street",
                streetNum: "2",
                unitNumber: string.Empty,
                city: default,
                state: default,
                zipCode: "00000",
                county: null,
                streetType: null,
                constructionCompletionDate: null,
                companyId: companyId,
                ownerName: string.Empty,
                communityId: null,
                planId: null,
                manuallyManaged: false)
            {
                StatusFieldsInfo = { PendingDate = outside },
            };

            var listingNull = new SaleListing(
                MarketStatuses.Pending,
                streetName: "Street",
                streetNum: "3",
                unitNumber: string.Empty,
                city: default,
                state: default,
                zipCode: "00000",
                county: null,
                streetType: null,
                constructionCompletionDate: null,
                companyId: companyId,
                ownerName: string.Empty,
                communityId: null,
                planId: null,
                manuallyManaged: false); // StatusFieldsInfo.PendingDate remains null

            var listings = new List<SaleListing> { listingInside, listingOutside, listingNull };

            var repo = this.GetInMemoryRepository();

            var method = repo.GetType().GetMethod(
                "FilterByPendingDate",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            var range = new DateRange(now.AddDays(-2), now.AddDays(2));

            // Act
            var resultQuery = method.Invoke(repo, new object[] { listings.AsQueryable(), range }) as IQueryable<SaleListing>;
            var result = resultQuery?.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, r => r.StatusFieldsInfo.PendingDate.HasValue && r.StatusFieldsInfo.PendingDate.Value == inside);
        }

        [Fact]
        public void FilterByPendingDate_WithNoMatchingListings_ReturnsEmpty()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var outside = now.AddDays(-10);

            var companyId = Guid.NewGuid();

            var listingOutside = new SaleListing(
                MarketStatuses.Pending,
                streetName: "Street",
                streetNum: "10",
                unitNumber: string.Empty,
                city: default,
                state: default,
                zipCode: "00000",
                county: null,
                streetType: null,
                constructionCompletionDate: null,
                companyId: companyId,
                ownerName: string.Empty,
                communityId: null,
                planId: null,
                manuallyManaged: false)
            {
                StatusFieldsInfo = { PendingDate = outside },
            };

            var listings = new List<SaleListing> { listingOutside };

            var repo = this.GetInMemoryRepository();

            var method = repo.GetType().GetMethod(
                "FilterByPendingDate",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);

            var range = new DateRange(now.AddDays(-2), now.AddDays(2));

            // Act
            var resultQuery = method.Invoke(repo, new object[] { listings.AsQueryable(), range }) as IQueryable<SaleListing>;
            var result = resultQuery?.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private KpiQueriesRepository GetInMemoryRepository() =>
            new KpiQueriesRepository(
                this.fixture.GetInMemoryDbContext<ApplicationQueriesDbContext>(),
                this.userContextProviderMock.Object,
                this.companyCacheRepositoryMock.Object,
                this.kpiQueriesRequestProviderMock.Object,
                this.loggerMock.Object);
    }
}
