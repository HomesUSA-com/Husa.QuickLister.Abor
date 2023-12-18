namespace Husa.Quicklister.Abor.Api.Client.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests.Community;
    using Husa.Quicklister.Abor.Data;
    using Husa.Quicklister.Abor.Domain.Entities.Community;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Api.Client.Tests")]
    public class CommunityClientResourceTest
    {
        private readonly CustomWebApplicationFactory<TestStartup> customWebApplicationFactory;
        private readonly QuicklisterAborClient quicklisterAborClient;

        public CommunityClientResourceTest(CustomWebApplicationFactory<TestStartup> customWebApplicationFactory)
        {
            this.customWebApplicationFactory = customWebApplicationFactory ?? throw new ArgumentNullException(nameof(customWebApplicationFactory));
            var client = customWebApplicationFactory.GetClient();
            var loggerFactory = this.customWebApplicationFactory.Services.GetRequiredService<ILoggerFactory>();
            this.quicklisterAborClient = new QuicklisterAborClient(loggerFactory, client);
        }

        [Fact]
        public async Task GetCommunityEmployeesSuccess()
        {
            // Arrange
            var communityId = Guid.NewGuid();
            var companyId = Guid.NewGuid();
            var dbContext = this.customWebApplicationFactory.GetService<ApplicationDbContext>();
            var community = CommunityTestProvider.GetCommunityEntity(communityId: communityId, companyId: companyId);
            community.Employees.Add(new CommunityEmployee(Factory.UserId, communityId, companyId));
            dbContext.Community.Add(community);
            dbContext.SaveChanges();

            // Act
            var result = await this.quicklisterAborClient.SaleCommunity.GetEmployeesAsync(communityId);

            // Assert
            Assert.Equal(1, result.Total);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetCommunityEmployeesEmpty()
        {
            // Arrange
            var communityId = Guid.NewGuid();

            // Act
            var result = await this.quicklisterAborClient.SaleCommunity.GetEmployeesAsync(communityId);

            // Assert
            Assert.Equal(0, result.Total);
            Assert.Empty(result.Data);
        }
    }
}
