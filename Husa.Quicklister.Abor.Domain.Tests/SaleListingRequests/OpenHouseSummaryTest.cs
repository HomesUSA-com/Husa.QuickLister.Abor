namespace Husa.Quicklister.Abor.Domain.Tests.SaleListingRequests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Husa.Quicklister.Abor.Domain.Comparers;
    using Husa.Quicklister.Abor.Domain.Entities.Listing;
    using Husa.Quicklister.Abor.Domain.Enums.Domain;
    using Husa.Quicklister.Extensions.Domain.Enums;
    using Xunit;

    [ExcludeFromCodeCoverage]
    [Collection("Husa.Husa.Quicklister.Abor.Domain.Test")]
    public class OpenHouseSummaryTest
    {
        [Fact]
        public void OpenHouseComparer_Equals_ReturnsTrueForEqualObjects()
        {
            var propertyId = Guid.NewGuid();
            var date = new DateTime(2022, 11, 7, 0, 0, 0, DateTimeKind.Utc);

            var openHouse1 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Beverages, Refreshments.Lunch });

            var openHouse2 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Lunch, Refreshments.Beverages });

            var comparer = new OpenHouseComparer();

            bool result = comparer.Equals(openHouse1, openHouse2);

            Assert.True(result);
        }

        [Fact]
        public void OpenHouseComparer_Equals_ReturnsFalseForDifferentObjects()
        {
            var propertyId = Guid.NewGuid();
            var date = new DateTime(2022, 11, 7, 0, 0, 0, DateTimeKind.Utc);

            var openHouse1 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Beverages, Refreshments.Lunch });

            var openHouse2 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Tuesday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Lunch, Refreshments.Beverages });

            var comparer = new OpenHouseComparer();

            bool result = comparer.Equals(openHouse1, openHouse2);

            Assert.False(result);
        }

        [Fact]
        public void OpenHouseComparer_GetHashCode_ReturnsSameHashCodeForEqualObjects()
        {
            var propertyId = Guid.NewGuid();
            var date = new DateTime(2022, 11, 7, 0, 0, 0, DateTimeKind.Utc);

            var openHouse1 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Beverages, Refreshments.Lunch });

            var openHouse2 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Lunch, Refreshments.Beverages });

            var comparer = new OpenHouseComparer();

            int hashCode1 = comparer.GetHashCode(openHouse1);
            int hashCode2 = comparer.GetHashCode(openHouse2);

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void OpenHouseComparer_GetHashCode_ReturnsDifferentHashCodeForDifferentObjects()
        {
            var propertyId = Guid.NewGuid();
            var date = new DateTime(2022, 11, 7, 0, 0, 0, DateTimeKind.Utc);

            var openHouse1 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Monday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Beverages, Refreshments.Lunch });

            var openHouse2 = new SaleListingOpenHouse(
            propertyId,
            type: OpenHouseType.Tuesday,
            startTime: date.AddHours(8).TimeOfDay,
            endTime: date.AddHours(18).TimeOfDay,
            refreshments: new List<Refreshments> { Refreshments.Lunch, Refreshments.Beverages });

            var comparer = new OpenHouseComparer();

            int hashCode1 = comparer.GetHashCode(openHouse1);
            int hashCode2 = comparer.GetHashCode(openHouse2);

            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
