namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Data.Queries.Projections;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Data.Queries.Tests.Seeders;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums.ShowingTime;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using DatabaseIds = Husa.Quicklister.Abor.Data.Queries.Tests.Seeders.ShowingTimeQueriesRepositoryDatabaseIds;

    public class ShowingTimeQueriesRepositoryTests
    {
        private readonly Mock<IUserContextProvider> userContextProvider;
        private readonly Mock<ILogger<ShowingTimeContactQueriesRepository>> logger = new();

        public ShowingTimeQueriesRepositoryTests()
        {
            var (dbContext, optionsBuilder) = ApplicationDbContextTestFactory
                .DbContextFactory<ApplicationDbContext>();
            dbContext.SeedShowingTimeQueriesRepositoryTestsDatabase();
            dbContext.SaveChanges();
            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            var contactProjections = new ShowingTimeContactProjection();
            var communityContactProjections = new CommunityShowingTimeContactOrderProjection();
            var listingContactProjections = new ListingShowingTimeContactOrderProjection();
            var userContext = new Mock<IUserContext>();
            this.userContextProvider = new Mock<IUserContextProvider>();
            this.userContextProvider
                .Setup(x => x.GetCurrentUser())
                .Returns(userContext.Object);
            this.Sut = new ShowingTimeContactQueriesRepository(
                queriesDbContext,
                this.userContextProvider.Object,
                contactProjections,
                communityContactProjections,
                listingContactProjections,
                this.logger.Object);
        }

        public ShowingTimeContactQueriesRepository Sut { get; private set; }

        [Fact]
        public async Task GetAsync_GetDefault()
        {
            var filters = new ShowingTimeContactQueryFilter
            {
                LimitToScope = false,
                CompanyId = DatabaseIds.CompanyTwoId,
            };
            var response = await this.Sut.Search(filters);

            Assert.Equal(1, response.Total);
            Assert.True(response.Data.FirstOrDefault()?.IsFixed);
        }

        [Fact]
        public async Task GetAsync_AllInCommunity()
        {
            var response = await this.Sut.GetScopedContacts(ContactScope.Community, DatabaseIds.CommunityOneId);

            Assert.Equal(2, response.Count);
        }

        [Fact]
        public async Task GetAsync_AllInCompany()
        {
            var response = await this.Sut.GetCompanyContacts(DatabaseIds.CompanyOneId);

            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async Task GetContactById_Success()
        {
            var response = await this.Sut.GetContactById(DatabaseIds.ContactOneId);
            Assert.NotNull(response?.Email);
        }

        [Fact]
        public async Task GetContactById_NotFound()
        {
            var nonExistentId = Guid.Parse("1b96e331-246a-478d-bcec-35f4aff5fcd5");
            var response = await this.Sut.GetContactById(nonExistentId);
            Assert.Null(response);
        }
    }
}
