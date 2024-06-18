namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Entities.Request;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Xunit;
    public class ValidateListingStatusTests
    {
        [Fact]
        public void ActiveUnderContractValidations_NoErrors_Test()
        {
            // Arrange
            var statusFields = new StatusFieldsRecord { PendingDate = DateTime.Now.AddDays(-5), EstimatedClosedDate = DateTime.Now.AddDays(2) };

            // Act
            var result = ValidateListingStatus<StatusFieldsRecord>.GetErrors(MarketStatuses.ActiveUnderContract, statusFields);

            // Assert
            Assert.Null(result);
        }
    }
}
