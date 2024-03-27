namespace Husa.Quicklister.Abor.Domain.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Extensions.Common.Enums;
    using Husa.Extensions.Document.Extensions;
    using Husa.Extensions.Document.ValueObjects;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
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
            var result = SummaryExtensions.GetFieldSummary(addressInfo, oldAddressInfo, excludeFields: filteredFields);

            // Assert
            Assert.NotNull(result);
            var summaryFields = Assert.IsAssignableFrom<IEnumerable<SummaryField>>(result);
            Assert.Equal(2, summaryFields.ToList().Count);
        }
    }
}
