namespace Husa.Quicklister.Abor.Data.Commands.Tests.Repositories
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Commands.Repositories;
    using Husa.Quicklister.Extensions.Crosscutting;
    using Microsoft.Azure.Cosmos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ListingSaleRepositoryTests
    {
        private readonly Mock<IUserContextProvider> userContext = new();
        private readonly Mock<ILogger<ListingSaleRepository>> logger = new();
        private readonly Mock<CosmosClient> cosmosClient = new();
        private readonly Mock<IOptions<DocumentDbSettings>> documentDbSettings = new();
        private readonly Mock<IOptions<Crosscutting.ApplicationOptions>> applicationOptions = new();

        public ListingSaleRepositoryTests()
        {
            this.documentDbSettings.SetupGet(o => o.Value).Returns(new DocumentDbSettings
            {
                DatabaseName = "TestDatabase",
                ListingChangesCollectionName = "TestCollection",
            });

            this.applicationOptions.SetupGet(o => o.Value).Returns(new Crosscutting.ApplicationOptions
            {
                FeatureFlags = new FeatureFlags
                {
                    DisableSavedChanges = true,
                },
            });
        }

        [Fact]
        public async Task GetByIdNoTracking_ReturnsListing_WhenExists()
        {
            // Arrange
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user);

            var listingId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId);

            var sut = this.GetInMemoryRepository(new[] { listing });

            // Act
            var result = await sut.GetByIdNoTracking(listingId, filterByCompany: false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
            Assert.NotNull(result.SaleProperty);
            Assert.NotNull(result.SaleProperty.Rooms);
            Assert.NotNull(result.SaleProperty.OpenHouses);
        }

        [Fact]
        public async Task GetByIdNoTracking_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user);

            var listing = ListingTestProvider.GetListingEntity(Guid.NewGuid());
            var sut = this.GetInMemoryRepository(new[] { listing });

            // Act
            var result = await sut.GetByIdNoTracking(Guid.NewGuid(), filterByCompany: false);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdNoTracking_FiltersByCompany_WhenFilterByCompanyIsTrue()
        {
            // Arrange
            var userCompanyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.User, companyId: userCompanyId);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user);

            var listingId = Guid.NewGuid();
            var otherCompanyId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: otherCompanyId);

            var sut = this.GetInMemoryRepository(new[] { listing });

            // Act
            var result = await sut.GetByIdNoTracking(listingId, filterByCompany: true);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdNoTracking_ReturnsListing_WhenFilterByCompanyIsTrueAndSameCompany()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.User, companyId: companyId);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user);

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);

            var sut = this.GetInMemoryRepository(new[] { listing });

            // Act
            var result = await sut.GetByIdNoTracking(listingId, filterByCompany: true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
            Assert.Equal(companyId, result.CompanyId);
        }

        private ListingSaleRepository GetInMemoryRepository(System.Collections.Generic.IEnumerable<Domain.Entities.Listing.SaleListing> listings)
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

            dbContext.SaveChanges();

            return new ListingSaleRepository(
                dbContext,
                this.cosmosClient.Object,
                this.userContext.Object,
                this.logger.Object,
                this.documentDbSettings.Object,
                this.applicationOptions.Object);
        }
    }
}
