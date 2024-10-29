namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]

    public class AlertQueriesRepositoryTest
    {
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IPhotoServiceClient> photoService = new();
        private readonly Mock<IUserContextProvider> userContex = new();
        private readonly Mock<ILogger<AlertQueriesRepository>> logger = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();

        public AlertQueriesRepositoryTest()
        {
            this.userContex.Setup(u => u.GetCurrentUser()).Returns(GetRealUser());
        }

        [Theory]
        [InlineData(AlertType.NotListedInMls)]
        [InlineData(AlertType.OrphanListings)]
        public async Task GetAsync_Success(AlertType alertType)
        {
            // Arrange
            var listings = new SaleListing[]
            {
                ListingTestProvider.GetListingEntity(),
                ListingTestProvider.GetListingEntity(),
            };
            var sut = this.GetInMemoryRepository(listings);
            var filter = new BaseAlertQueryFilter();

            // Act
            var result = await sut.GetAsync(alertType, filter);

            // Assert
            Assert.Equal(listings.Length, result.Total);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetAsync_XmlListingUpdatedWithoutRequest_Success()
        {
            // Arrange
            var listing = ListingTestProvider.GetListingEntity();
            listing.XmlRequestError = new(listing.Id, "error message");
            var listings = new SaleListing[] { listing };
            var sut = this.GetInMemoryRepository(listings);
            var filter = new BaseAlertQueryFilter();

            // Act
            var result = await sut.GetAsync(AlertType.XmlListingUpdatedWithoutRequest, filter);

            // Assert
            Assert.Equal(listings.Length, result.Total);
            Assert.NotEmpty(result.Data);
        }

        /// <summary>
        /// This method is used to build a new instance of UserContext with the values of a valid user from the targetted DB. Note: The values of the user below only apply to the dev database.
        /// </summary>
        /// <returns>Returns an instance of UserContext with the values of a valid user from the target DB.</returns>
        private static UserContext GetRealUser() => new()
        {
            IsMLSAdministrator = true,
            Id = new Guid("9AD0A512-0D81-4F40-BAAF-1E7A2AFABBD1"),
            Email = "freddy@homesusa.com",
            Name = "Freddy Zambrano",
            UserRole = UserRole.MLSAdministrator,
        };

        private AlertQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            dbContext.ListingSale.AddRange(listings);
            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new AlertQueriesRepository(
                queriesDbContext,
                this.userQueriesRepository.Object,
                this.photoService.Object,
                this.serviceSubscriptionClient.Object,
                this.logger.Object,
                this.userContex.Object);
        }
    }
}
