namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.JsonImport.Api.Client.Interface;
    using Husa.JsonImport.Api.Contracts.Request;
    using Husa.JsonImport.Api.Contracts.Response.Listing;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Data.Queries.Tests.Configuration;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Extensions.Data.Queries.Models.Json;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Enums.Json;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection(nameof(ApplicationServicesFixture))]

    public class QueryJsonRepositoryTest
    {
        private readonly Mock<IJsonImportClient> jsonClient = new();
        private readonly Mock<IUserContextProvider> userContex = new();
        private readonly ApplicationServicesFixture fixture;
        private readonly DbContextOptions<ApplicationDbContext> dbContextOptions;

        public QueryJsonRepositoryTest(ApplicationServicesFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .Options;
        }

        [Theory]
        [InlineData(SpecImportStatus.Available)]
        [InlineData(SpecImportStatus.Deleted)]
        [InlineData(SpecImportStatus.Imported)]
        public async Task GetAsync_AsEmployee_Success(SpecImportStatus importStatus)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userId, companyId, UserRole.User, RoleEmployee.SalesEmployee);
            this.userContex.Setup(u => u.GetCurrentUser()).Returns(user);
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            community.JsonImportStatus = JsonImportStatus.Approved;
            community.Employees.Add(new(userId, communityId, companyId));

            var sut = this.GetInMemoryRepository(new CommunitySale[] { community });
            var jsonListings = new SpecResponse[] { new(), new() };
            this.jsonClient
                .Setup(u => u.Spec.GetAsync(It.Is<SpecFilterRequest>(x => x.CommunityIds.Contains(communityId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DataSet<SpecResponse>(jsonListings.ToList(), jsonListings.Length));
            var filter = new JsonListingQueryFilter()
            {
                ImportStatus = importStatus,
            };

            // Act
            var result = await sut.GetAsync(filter);

            // Assert
            Assert.Equal(jsonListings.Length, result.Total);
            Assert.NotEmpty(result.Data);
        }

        private QueryJsonRepository GetInMemoryRepository(IEnumerable<CommunitySale> communities)
        {
            var dbContext = new ApplicationDbContext(this.dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.Community.AddRange(communities);
            dbContext.CommunityEmployee.AddRange(communities.SelectMany(x => x.Employees));
            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(this.dbContextOptions);
            return new QueryJsonRepository(
                queriesDbContext,
                this.jsonClient.Object,
                this.userContex.Object,
                this.fixture.Mapper);
        }
    }
}
