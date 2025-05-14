namespace Husa.Quicklister.Abor.Api.Client.Tests.Authentication
{
    using System;
    using Husa.Extensions.Authorization;
    using Husa.Extensions.Authorization.Enums;
    using Husa.Extensions.Authorization.Models;
    using Moq;

    internal static class UserContextProviderMock
    {
        public static Mock<IUserContextProvider> SetupUserContext()
        {
            var user = new UserContext()
            {
                Id = Guid.NewGuid(),
                UserRole = UserRole.MLSAdministrator,
                IsMLSAdministrator = true,
            };
            var mock = new Mock<IUserContextProvider>();
            mock.Setup(u => u.GetCurrentUser()).Returns(user).Verifiable();
            mock.Setup(u => u.GetCurrentUserId()).Returns(user.Id).Verifiable();
            mock.Setup(u => u.GetUserLocalDate()).Returns(DateTime.UtcNow).Verifiable();

            return mock;
        }
    }
}
