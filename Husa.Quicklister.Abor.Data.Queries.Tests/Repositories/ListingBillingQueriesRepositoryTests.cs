namespace Husa.Quicklister.Abor.Data.Queries.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Domain.Enums;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Extensions.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using CompanyRequest = Husa.CompanyServicesManager.Api.Contracts.Request;
    using CompanyResponse = Husa.CompanyServicesManager.Api.Contracts.Response;

    public class ListingBillingQueriesRepositoryTests
    {
        protected readonly Mock<IUserRepository> userQueriesRepository = new();
        protected readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        protected readonly Mock<ILogger<ListingBillingQueriesRepository>> logger = new();

        [Fact]
        public async Task GetBillableListingsAsync_EmptyResult()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            this.SetUpCompany([GetServiceSubscription(companyId: companyId)]);
            var sut = this.GetInMemoryRepository();

            // Act
            var result = await sut.GetBillableListingsAsync(new()
            {
                From = DateTime.UtcNow.AddDays(-1),
                To = DateTime.UtcNow.AddDays(1),
            });

            // Assert
            Assert.Empty(result.Data);
            Assert.Equal(0, result.Total);
        }

        private static CompanyResponse.ServiceSubscription GetServiceSubscription(Guid companyId, ServiceCode? seviceCode = null)
        => new()
        {
            ServiceCode = seviceCode ?? ServiceCode.NewListing,
            Price = 200.0m,
            CompanyId = companyId,
        };

        private ListingBillingQueriesRepository GetInMemoryRepository(IEnumerable<SaleListing> listings = null)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString(), new InMemoryDatabaseRoot())
                 .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            if (listings != null)
            {
                dbContext.ListingSale.AddRange(listings);
            }

            dbContext.SaveChanges();

            var queriesDbContext = new ApplicationQueriesDbContext(optionsBuilder.Options);
            return new ListingBillingQueriesRepository(
                 queriesDbContext,
                 this.logger.Object,
                 this.userQueriesRepository.Object,
                 this.serviceSubscriptionClient.Object);
        }

        private void SetUpCompany(IEnumerable<CompanyResponse.ServiceSubscription> serviceSubscription = null)
        {
            var subscription = serviceSubscription ?? Array.Empty<CompanyResponse.ServiceSubscription>();
            var companyServicesResponse = new DataSet<CompanyResponse.ServiceSubscription>(subscription, subscription.Count());

            this.serviceSubscriptionClient.Setup(x => x.CompanyService.GetAsync(It.IsAny<CompanyRequest.FilterCompanyServiceRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyServicesResponse);
        }
    }
}
