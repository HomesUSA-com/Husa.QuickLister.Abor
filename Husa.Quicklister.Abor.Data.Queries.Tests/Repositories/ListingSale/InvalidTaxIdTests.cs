namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories.ListingSale
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;
    using ExtensionsCrosscutting = Husa.Quicklister.Extensions.Crosscutting;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]
    public class InvalidTaxIdTests
    {
        private readonly Mock<IUserContextProvider> userContext = new();
        private readonly Mock<ILogger<ListingSaleQueriesRepository>> logger = new();
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IPhotoServiceClient> photoServiceClient = new();
        private readonly Mock<IOptions<ApplicationOptions>> applicationOptions = new();

        public InvalidTaxIdTests()
        {
            this.applicationOptions.SetupGet(o => o.Value).Returns(new ApplicationOptions
            {
                FeatureFlags = new ExtensionsCrosscutting.FeatureFlags
                {
                    FindEmailLeads = true,
                },
            });
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_Success()
        {
            // Arrange
            var sut = this.Setup(taxId: "N/A");

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_NoInvalidTaxIds_ReturnsEmptyList()
        {
            // Arrange
            var sut = this.Setup(taxId: "123456");

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_MultipleInvalidTaxIds_ReturnsAllInvalid()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var communityId = Guid.NewGuid();

            // Create first listing with invalid tax ID
            var listingId1 = Guid.NewGuid();
            var listing1 = ListingTestProvider.GetListingEntity(listingId1, companyId: companyId, communityId: communityId, mlsNumber: "1");
            listing1.SaleProperty.PropertyInfo.TaxId = "N/A";

            // Create third listing with valid tax ID
            var listingId2 = Guid.NewGuid();
            var listing3 = ListingTestProvider.GetListingEntity(listingId2, companyId: companyId, communityId: communityId, mlsNumber: "3");
            listing3.SaleProperty.PropertyInfo.TaxId = "123456";

            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing1, listing3 },
                new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Single(result);
            Assert.Contains(result, r => r.Id == listingId1);
            Assert.DoesNotContain(result, r => r.Id == listingId2);
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_NullTaxId_NotReturned()
        {
            // Arrange
            var sut = this.Setup(taxId: null);

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_EmptyTaxId_Returned()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var sut = this.Setup(listingId, taxId: string.Empty);

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Single(result);
            Assert.Equal(listingId, result.Select(x => x.Id).First());
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_ListingWithoutMlsNumber_NotReturned()
        {
            // Arrange
            var sut = this.Setup(mlsNumber: null, taxId: string.Empty);

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(MarketStatuses.Closed)]
        [InlineData(MarketStatuses.Canceled)]
        [InlineData(MarketStatuses.Expired)]
        public async Task GetListingsWithInvalidTaxId_ListingWithTerminalStatus_NotReturned(MarketStatuses marketStatus)
        {
            // Arrange
            var sut = this.Setup(taxId: string.Empty, marketStatus: marketStatus);

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(MarketStatuses.Active)]
        [InlineData(MarketStatuses.ActiveUnderContract)]
        [InlineData(MarketStatuses.Pending)]
        public async Task GetListingsWithInvalidTaxId_ListingWithActiveStatus_Returned(MarketStatuses marketStatus)
        {
            // Arrange
            var sut = this.Setup(taxId: string.Empty, marketStatus: marketStatus);

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetListingsWithInvalidTaxId_ListingWithABCHomes_NotReturned()
        {
            // Arrange
            var sut = this.Setup(taxId: string.Empty, ownerName: "ABC Homes");

            // Act
            var result = await sut.GetListingsWithInvalidTaxId();

            // Assert
            Assert.Empty(result);
        }

        private ListingSaleQueriesRepository Setup(Guid? listingId = null, string mlsNumber = "123456", string taxId = null, MarketStatuses? marketStatus = null, string ownerName = null)
        {
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var communityId = Guid.NewGuid();
            var listId = listingId ?? Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(
                listId,
                companyId: companyId,
                communityId: communityId,
                mlsNumber: mlsNumber,
                marketStatuses: marketStatus,
                ownerName: ownerName);
            listing.SaleProperty.PropertyInfo.TaxId = taxId;
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            return this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });
        }

        private ListingSaleQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings, IEnumerable<CommunitySale> communities)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
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

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new ListingSaleQueriesRepository(
                queriesDbContext,
                this.userContext.Object,
                this.logger.Object,
                this.userQueriesRepository.Object,
                this.serviceSubscriptionClient.Object,
                this.photoServiceClient.Object,
                this.applicationOptions.Object);
        }
    }
}
