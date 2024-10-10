namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Moq;
    using Xunit;
    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]
    public class ManagementTraceQueriesRepositoryTest
    {
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IUserContextProvider> userContext = new();

        [Fact]
        public async Task GetAsync_Success()
        {
            // Arrange
            this.SetupMlsAdmin();

            var saleListingId = Guid.NewGuid();
            var saleListing = ListingTestProvider.GetListingEntity(saleListingId);
            var community = CommunityTestProvider.GetCommunityEntity(saleListing.SaleProperty.CommunityId);
            var trace = new ManagementTrace(saleListing, Guid.NewGuid(), Faker.Boolean.Random());

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { saleListing },
                new List<CommunitySale> { community },
                new List<ManagementTrace> { trace });

            // Act
            var result = await sut.GetAsync(saleListingId, "+SysCreatedOn", 0, 50);

            // Assert
            Assert.NotEmpty(result.Data);
            Assert.Equal(saleListingId, result.Data.First().ListingSaleId);
        }

        private ManagementTraceQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings, IEnumerable<CommunitySale> communities, IEnumerable<ManagementTrace> traces)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            foreach (var listing in listings)
            {
                dbContext.ListingSale.Add(listing);
            }

            foreach (var community in communities)
            {
                dbContext.Community.Add(community);
            }

            foreach (var trace in traces)
            {
                dbContext.ManagementTrace.Add(trace);
            }

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new ManagementTraceQueriesRepository(queriesDbContext, this.userQueriesRepository.Object);
        }

        private void SetupMlsAdmin()
        {
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }
    }
}
