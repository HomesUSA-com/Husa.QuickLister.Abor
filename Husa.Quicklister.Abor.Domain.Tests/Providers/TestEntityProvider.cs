namespace Husa.Quicklister.Abor.Domain.Tests.Providers
{
    using System;
    using Husa.Extensions.Authorization;
    using Moq;

    public static class TestEntityProvider
    {
        public static IUserContextProvider GetIUserContextProvider()
        {
            var userContextProvider = new Mock<IUserContextProvider>();
            var userId = Guid.NewGuid();
            userContextProvider.Setup(u => u.GetCurrentUserId()).Returns(userId).Verifiable();
            userContextProvider.Setup(u => u.GetUserLocalDate()).Returns(DateTime.UtcNow).Verifiable();
            return userContextProvider.Object;
        }
    }
}
