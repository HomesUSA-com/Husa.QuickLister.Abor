namespace Husa.Quicklister.Abor.Domain.Tests.Common
{
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Common;
    using Husa.Quicklister.Abor.Domain.Enums;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class CommonFieldsValidationTests
    {
        [Theory]
        [InlineData(MarketStatuses.Pending, false)]
        [InlineData(MarketStatuses.Expired, false)]
        [InlineData(MarketStatuses.Canceled, false)]
        [InlineData(MarketStatuses.Closed, false)]
        public void IsAllowedStatusForXmlRequest(MarketStatuses status, bool espectedResult)
        {
            // Act
            var result = status.IsAllowedStatusXmlForRequest();

            // Assert
            Assert.Equal(espectedResult, result);
        }
    }
}
