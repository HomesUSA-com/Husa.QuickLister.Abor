namespace Husa.Quicklister.Abor.Data.Queries.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Data.Queries.Repositories;
    using Husa.Quicklister.Extensions.Data.Queries.Interfaces;
    using Moq;
    using Xunit;
    using User = Husa.CompanyServicesManager.Api.Contracts.Response.User;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Quicklister.Abor.Data.Queries.Test")]

    public class UserRepositoryTests
    {
        private readonly Mock<IUserCacheRepository> userCacheRepository = new();

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

            this.userCacheRepository
                .Setup(s => s.GetUsers(It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(userReturned);
            var sut = this.GetSut();

            // Act
            var users = await sut.GetUsersById(userIds);

            // Assert
            Assert.NotEmpty(users);
            var result = Assert.IsAssignableFrom<IEnumerable<User>>(users);
            Assert.Contains(result, u => u.Id == userId1);
        }

        private UserRepository GetSut() => new(this.userCacheRepository.Object);
    }
}
