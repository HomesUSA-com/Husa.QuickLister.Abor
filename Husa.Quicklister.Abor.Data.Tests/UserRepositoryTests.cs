namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Husa.CompanyServicesManager.Api.Client.Interfaces;
    using Husa.CompanyServicesManager.Api.Contracts.Request;
    using Husa.Extensions.Cache;
    using Husa.Extensions.Common.Classes;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Moq;
    using Xunit;
    using User = Husa.CompanyServicesManager.Api.Contracts.Response.User;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]

    public class UserRepositoryTests
    {
        private readonly Mock<IServiceSubscriptionClient> serviceSubscriptionClient = new();
        private readonly Mock<ICache> cache = new();

        [Fact]
        public async Task GetUsersById_NoUserId_Success()
        {
            // Arrange
            var userIds = new List<Guid>();
            var sut = this.GetSut();

            // Act
            var users = await sut.GetUsersById(userIds);

            // Assert
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUsersById_EmptyChache_Success()
        {
            // Arrange
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var userIds = new List<Guid>() { userId1, userId2 };
            var user1 = TestModelProvider.GetUser(userId1);
            var user2 = TestModelProvider.GetUser(userId2);
            var userReturned = new List<User>() { user1, user2 };
            var dataSetQueryResult = new DataSet<User>(userReturned, 2);

            this.serviceSubscriptionClient
                .Setup(s => s.User.GetAsync(It.IsAny<UserRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dataSetQueryResult);
            var sut = this.GetSut();

            // Act
            var users = await sut.GetUsersById(userIds);

            // Assert
            Assert.NotEmpty(users);
            this.serviceSubscriptionClient.Verify(ss => ss.User.GetAsync(It.IsAny<UserRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            var result = Assert.IsAssignableFrom<IEnumerable<User>>(users);
            Assert.Contains(result, u => u.Id == userId1);
        }

        private UserRepository GetSut() => new(this.cache.Object, this.serviceSubscriptionClient.Object);
    }
}
