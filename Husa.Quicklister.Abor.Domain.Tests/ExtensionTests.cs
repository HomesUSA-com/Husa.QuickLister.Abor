namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Extensions;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class ExtensionTests
    {
        [Fact]
        public void GetFieldSummaryWithChangesSuccess()
        {
            // Arrange
            var addressInfo = TestModelProvider.GetFullSalePropertyValueObject().AddressInfo;
            var oldAddressInfo = TestModelProvider.GetFullSalePropertyValueObject().AddressInfo;
            string[] filteredFields = { "City" };
            oldAddressInfo.County = Counties.Bexar;
            oldAddressInfo.State = States.SouthDakota;

            // Act
            var result = SummaryExtensions.GetFieldSummary(addressInfo, oldAddressInfo, true, filteredFields);

            // Assert
            Assert.NotNull(result);
            var summaryFields = Assert.IsAssignableFrom<IEnumerable<SummaryField>>(result);
            Assert.Equal(2, summaryFields.ToList().Count);
        }
    }
}
