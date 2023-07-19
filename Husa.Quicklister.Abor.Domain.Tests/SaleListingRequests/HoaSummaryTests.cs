namespace Husa.Quicklister.Abor.Domain.Tests.SaleListingRequests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Husa.Quicklister.Abor.Crosscutting.Tests;
    using Husa.Quicklister.Abor.Crosscutting.Tests.SaleListing;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Entities.Request.Records;
    using Husa.Quicklister.Extensions.Domain.ValueObjects;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class HoaSummaryTests
    {
        [Fact]
        public void GetHoaSummary_New_Success()
        {
            // Arrange
            var hoasCount = 2;
            var hoas = HoaTestProvider.GetListingSaleHoas(totalElements: hoasCount);

            // Act
            var hoasSummary = GetHoaSummary(hoas, new List<SaleListingHoa>());

            // Assert
            Assert.True(hoasSummary.Fields.Count(x => x.OldValue == null) == hoasCount);
        }

        [Fact]
        public void GetHoaSummary_Old_Success()
        {
            // Arrange
            var hoasCount = 2;
            var hoas = HoaTestProvider.GetListingSaleHoas(totalElements: hoasCount);

            // Act
            var hoasSummary = GetHoaSummary(new List<SaleListingHoa>(), hoas);

            // Assert
            Assert.True(hoasSummary.Fields.Count(x => x.NewValue == null) == hoasCount);
        }

        [Fact]
        public void GetHoaSummary_NotChanges_Success()
        {
            // Arrange
            var hoasCount = 2;
            var hoas = HoaTestProvider.GetListingSaleHoas(totalElements: hoasCount);

            // Act
            var hoasSummary = GetHoaSummary(hoas, hoas);

            // Assert
            Assert.True(hoasSummary == null);
        }

        [Fact]
        public void GetHoaSummary_Changes_Success()
        {
            // Arrange
            var newHoas = HoaTestProvider.GetListingSaleHoas(totalElements: 1);
            var equalHoas = HoaTestProvider.GetListingSaleHoas(totalElements: 2);
            var removedHoas = HoaTestProvider.GetListingSaleHoas(totalElements: 1);

            // Act
            var hoasSummary = GetHoaSummary(equalHoas.Concat(newHoas), equalHoas.Concat(removedHoas));

            // Assert
            Assert.True(hoasSummary.Fields.Count(x => x.NewValue == null) == 1);
            Assert.True(hoasSummary.Fields.Count(x => x.OldValue == null) == 1);
            Assert.True(hoasSummary.Fields.Count(x => x.OldValue != null && x.NewValue != null) == 2);
        }

        private static SummarySection GetHoaSummary(IEnumerable<SaleListingHoa> currentHoas, IEnumerable<SaleListingHoa> oldHoas)
        {
            // Arrange
            var listingId = Guid.NewGuid();

            var newRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid());
            newRequest.ListingSaleId = listingId;
            newRequest.SaleProperty.UpdateHoas(currentHoas.ToList());

            var oldRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid());
            oldRequest.ListingSaleId = listingId;
            oldRequest.SaleProperty.UpdateHoas(oldHoas.ToList());

            // Act
            var summary = newRequest.SaleProperty.GetSummarySections(oldRequest.SaleProperty);
            return summary.FirstOrDefault(s => s != null && s.Name == HoaRecord.SummarySection);
        }
    }
}
