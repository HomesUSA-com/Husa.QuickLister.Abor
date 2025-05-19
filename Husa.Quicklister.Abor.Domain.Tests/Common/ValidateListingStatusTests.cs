namespace Husa.Quicklister.Abor.Domain.Tests.Common
{
    using System;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Husa.Quicklister.Abor.Domain.Tests.Providers;
    using Xunit;
    public class ValidateListingStatusTests
    {
        [Fact]
        public void ActiveUnderContractValidations_NoErrors_Test()
        {
            // Arrange
            var statusFields = new StatusFieldsRecord { PendingDate = DateTime.Now.AddDays(-5), EstimatedClosedDate = DateTime.Now.AddDays(2) };

            // Act
            var sut = new ValidateListingStatus<StatusFieldsRecord>(TestEntityProvider.GetIUserContextProvider());
            var result = sut.GetErrors(MarketStatuses.ActiveUnderContract, statusFields);

            // Assert
            Assert.Null(result);
        }
    }
}
