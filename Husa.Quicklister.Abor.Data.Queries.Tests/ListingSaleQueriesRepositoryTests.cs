namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.PhotoService.Api.Client.Interfaces;
    using Husa.Quicklister.Abor.Crosscutting;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Data.Queries.Models.QueryFilters;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;
    using ExtensionsCrosscutting = Husa.Quicklister.Extensions.Crosscutting;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]
    public class ListingSaleQueriesRepositoryTests
    {
        private readonly Mock<IUserContextProvider> userContext = new();
        private readonly Mock<ILogger<ListingSaleQueriesRepository>> logger = new();
        private readonly Mock<IUserRepository> userQueriesRepository = new();
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<IPhotoServiceClient> photoServiceClient = new();
        private readonly Mock<IOptions<ApplicationOptions>> applicationOptions = new();

        public ListingSaleQueriesRepositoryTests()
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
        public async Task GetAsync_AsSalesEmployeeReadonly_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.User, companyId: companyId);
            user.EmployeeRole = RoleEmployee.SalesEmployeeReadonly;
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var communityId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId, communityId: communityId);
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            community.Employees = new List<CommunityEmployee>()
            {
                new(user.Id, communityId, companyId),
            };
            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });
            var filter = new ListingQueryFilter()
            {
                CommunityId = communityId,
                Inactive = true,
            };

            // Act
            var result = await sut.GetAsync(filter);

            // Assert
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetAsync_AsSalesEmployeeReadonly_WithoutCommunityId_Success()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.User, companyId: companyId);
            user.EmployeeRole = RoleEmployee.SalesEmployeeReadonly;
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            var communityId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId, communityId: communityId);
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            community.Employees.Add(new CommunityEmployee(user.Id, communityId, companyId));
            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            // Act
            var result = await sut.GetAsync(new ListingQueryFilter() { Inactive = true });

            // Assert
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task GetEmailLeads_BuilderLeads()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var companyEmailLead = new CompanyResponse.EmailLead
            {
                Id = Guid.NewGuid(),
                EntityType = EmailEntityType.Sale,
                EmailPriority = EmailPriority.One,
                Email = "principal@homesusa.com",
            };
            var company = new CompanyResponse.CompanyDetail()
            {
                Id = companyId,
                EmailLeads = new List<CompanyResponse.EmailLead> { companyEmailLead },
            };

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead() { EmailLeadPrincipal = "principal@homesusa.com" };

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetCompany(It.IsAny<Guid>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();
            this.serviceSubscriptionClient.Setup(u => u.Company.GetEmailLeads(It.Is<Guid>(x => x == companyId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CompanyResponse.EmailLead> { companyEmailLead })
                .Verifiable();

            // Act
            var result = await sut.GetEmailLeads(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.EmailLeadPrincipal, companyEmailLead.Email);
        }

        [Fact]
        public async Task GetEmailLeads_CommunityLeads()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var emailLeadPrincipal = "principal@homesusa.com";

            var company = new CompanyResponse.CompanyDetail()
            {
                Id = companyId,
            };

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead() { EmailLeadPrincipal = emailLeadPrincipal };

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetCompany(It.IsAny<Guid>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            // Act
            var result = await sut.GetEmailLeads(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.EmailLeadPrincipal, emailLeadPrincipal);
        }

        [Fact]
        public async Task GetListingNotFound()
        {
            // Arrange
            this.SetupMlsAdmin();

            var listing = ListingTestProvider.GetListingEntity(Guid.NewGuid());
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId);
            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetListingSuccess()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var company = new CompanyResponse.CompanyDetail() { Id = companyId };
            this.serviceSubscriptionClient.Setup(u => u.Company.GetCompany(
                It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listingId, result.Id);
        }

        [Fact]
        public async Task GetListingWithLeadsFromCommunitySuccess()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var company = new CompanyResponse.CompanyDetail() { Id = companyId };

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead() { EmailLeadPrincipal = "principal@homesusa.com" };

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetCompany(
                It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.EmailLead.EmailLeadPrincipal, community.EmailLead.EmailLeadPrincipal);
        }

        [Fact]
        public async Task GetListingWithLeadsFromCompanySuccess()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var companyEmailLead = new CompanyResponse.EmailLead
            {
                Id = Guid.NewGuid(),
                EntityType = EmailEntityType.Sale,
                EmailPriority = EmailPriority.One,
                Email = "principal@homesusa.com",
            };
            var company = new CompanyResponse.CompanyDetail()
            {
                Id = companyId,
                EmailLeads = new List<CompanyResponse.EmailLead> { companyEmailLead },
            };

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead() { EmailLeadPrincipal = "principal@homesusa.com" };

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetCompany(
                It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.EmailLead.EmailLeadPrincipal, companyEmailLead.Email);
        }

        [Fact]
        public async Task GetLeadsFromCompanySuccess()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var leads = new List<CompanyResponse.EmailLead>();
            var companyEmailLead = new CompanyResponse.EmailLead
            {
                Id = Guid.NewGuid(),
                EntityType = EmailEntityType.Sale,
                EmailPriority = EmailPriority.One,
                Email = "principal@homesusa.com",
            };

            leads = new List<CompanyResponse.EmailLead> { companyEmailLead };

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead() { EmailLeadPrincipal = "principal@homesusa.com" };

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetEmailLeads(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leads)
                .Verifiable();

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.EmailLead.EmailLeadPrincipal, companyEmailLead.Email);
        }

        [Fact]
        public async Task GetLeadsFromCompanyEmpty()
        {
            // Arrange
            this.SetupMlsAdmin();

            var companyId = Guid.NewGuid();
            var leads = new List<CompanyResponse.EmailLead>();

            var listingId = Guid.NewGuid();
            var listing = ListingTestProvider.GetListingEntity(listingId, companyId: companyId);
            var community = CommunityTestProvider.GetCommunityEntity(listing.SaleProperty.CommunityId, companyId: companyId);
            community.EmailLead = new EmailLead();

            var sut = this.GetInMemoryRepository(
                new List<SaleListing> { listing },
                new List<CommunitySale> { community });

            this.serviceSubscriptionClient.Setup(u => u.Company.GetEmailLeads(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(leads)
                .Verifiable();

            // Act
            var result = await sut.GetListing(listingId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.EmailLead);
        }

        [Fact]
        public async Task GetLockedBySystem_ReturnsEmpty_WhenNoneLocked()
        {
            // Arrange: no listings at all
            var sut = this.GetInMemoryRepository(
                listings: new List<SaleListing>(),
                communities: new List<CommunitySale>());

            // Act
            var result = await sut.GetLockedBySystem();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLockedBySystem_ReturnsOnlyLockedBySystem()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var communityId = Guid.NewGuid();

            // create one locked and one unlocked listing
            var locked = ListingTestProvider.GetListingEntity(Guid.NewGuid(), companyId, communityId);
            locked.LockedStatus = LockedStatus.LockedBySystem;

            var unlocked = ListingTestProvider.GetListingEntity(Guid.NewGuid(), companyId, communityId);
            unlocked.LockedStatus = LockedStatus.NoLocked;

            var sut = this.GetInMemoryRepository(
                listings: new[] { locked, unlocked },
                communities: new List<CommunitySale>());

            // Act
            var results = (await sut.GetLockedBySystem()).ToList();

            // Assert
            Assert.Single(results);
            var item = results[0];

            // projection should match entity
            Assert.Equal(locked.MlsNumber, item.MlsNumber);
            Assert.Equal(locked.MlsStatus.ToString(), item.MlsStatus);
            Assert.Equal(
                locked.SaleProperty.AddressInfo.GetFormalAddress(),
                item.Address);
            Assert.Equal(locked.CompanyId, item.CompanyId);
            Assert.Equal(locked.MarketModifiedOn, item.MarketModifiedOn);
            Assert.Equal(locked.SysModifiedOn, item.SysModifiedOn);
            Assert.Equal(locked.SysModifiedBy, item.SysModifiedBy);
        }

        private void SetupMlsAdmin()
        {
            var user = TestModelProvider.GetCurrentUser(userRole: UserRole.MLSAdministrator);
            this.userContext.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
        }

        private ListingSaleQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings, IEnumerable<CommunitySale> communities)
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
