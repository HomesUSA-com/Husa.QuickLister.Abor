namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Lot;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class LotListingQueriesRepositoryTests
    {
        private readonly Mock<IUserContextProvider> userContextMock = new();
        private readonly Mock<ILogger<LotListingQueriesRepository>> loggerMock = new();
        private readonly Mock<IUserRepository> userQueriesRepositoryMock = new();

        [Fact]
        public async Task GetAsync_WithValidFilter_ReturnsDataSet()
        {
            // Arrange
            var queryFilter = new ListingQueryFilter
            {
                MlsStatus = new[] { MarketStatuses.Active },
                CommunityId = Guid.NewGuid(),
                MlsNumber = "123456",
            };

            this.SetupMlsAdmin();
            var listings = new List<LotListing>
            {
                new LotListing
                {
                    CommunityId = queryFilter.CommunityId.Value,
                    MlsNumber = queryFilter.MlsNumber,
                    MlsStatus = queryFilter.MlsStatus.First(),
                },
            };

            var community = CommunityTestProvider.GetCommunityEntity(queryFilter.CommunityId.Value);
            var sut = this.GetInMemoryRepository(listings, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetAsync(queryFilter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal(queryFilter.CommunityId, result.Data.First().CommunityId);
            Assert.Equal(queryFilter.MlsNumber, result.Data.First().MlsNumber);
            Assert.Equal(queryFilter.MlsStatus.First(), result.Data.First().MlsStatus);
        }

        [Fact]
        public async Task GetListing_WithValidListingId_ReturnsLotListingQueryDetailResult()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.SetupMlsAdmin();

            var lotListing = new LotListing
            {
                CommunityId = Guid.NewGuid(),
                MlsNumber = "123456",
                MlsStatus = MarketStatuses.Active,
                LockedStatus = LockedStatus.LockedBySystem,
                Id = listingId,
            };

            var community = CommunityTestProvider.GetCommunityEntity(lotListing.CommunityId.Value);
            var sut = this.GetInMemoryRepository(new[] { lotListing }, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
            Assert.Equal(lotListing.CommunityId, result.CommunityId);
            Assert.Equal(lotListing.MlsNumber, result.MlsNumber);
            Assert.Equal(lotListing.MlsStatus, result.MlsStatus);
            Assert.Equal(lotListing.LockedStatus, result.LockedStatus);
        }

        [Fact]
        public async Task GetListing_WithEmptySections()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            this.SetupMlsAdmin();

            var lotListing = new LotListing
            {
                CommunityId = Guid.NewGuid(),
                Id = listingId,
                ShowingInfo = null,
                SchoolsInfo = null,
                FeaturesInfo = null,
            };

            var community = CommunityTestProvider.GetCommunityEntity(lotListing.CommunityId.Value);
            var sut = this.GetInMemoryRepository(new[] { lotListing }, new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
        }

        private void SetupMlsAdmin()
        {
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContextMock.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }

        private LotListingQueriesRepository GetInMemoryRepository(IEnumerable<LotListing> listings, IEnumerable<CommunitySale> communities)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            foreach (var listing in listings)
            {
                dbContext.LotListing.Add(listing);
            }

            foreach (var community in communities)
            {
                dbContext.Community.Add(community);
            }

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);

            return new LotListingQueriesRepository(
                queriesDbContext,
                this.userContextMock.Object,
                this.loggerMock.Object,
                this.userQueriesRepositoryMock.Object);
        }
    }
}
