namespace Husa.Quicklister.Abor.Domain.Tests.Common
{
    using System;
    using Husa.Extensions.Authorization;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Moq;
    using Xunit;
    public class ValidateListingStatusTests
    {
        [Fact]
        public void ActiveUnderContractValidations_NoErrors_Test()
        {
            // Arrange
            var statusFields = new StatusFieldsRecord { PendingDate = DateTime.Now.AddDays(-5), EstimatedClosedDate = DateTime.Now.AddDays(2) };

            // Act
            var sut = new ValidateListingStatus<StatusFieldsRecord>(GetIUserContextProvider());
            var result = sut.GetErrors(MarketStatuses.ActiveUnderContract, statusFields);

            // Assert
            Assert.Null(result);
        }

        private static IUserContextProvider GetIUserContextProvider()
        {
            var userContextProvider = new Mock<IUserContextProvider>();
            var userId = Guid.NewGuid();
            userContextProvider.Setup(u => u.GetCurrentUserId()).Returns(userId).Verifiable();
            userContextProvider.Setup(u => u.GetUserLocalDate()).Returns(DateTime.UtcNow).Verifiable();
            return userContextProvider.Object;
        }
    }
}
