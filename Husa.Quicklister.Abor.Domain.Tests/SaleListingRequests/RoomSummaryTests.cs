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
    public class RoomSummaryTests
    {
        [Fact]
        public void GetRoomSummary_New_Success()
        {
            // Arrange
            var roomsCount = 2;
            var rooms = RoomTestProvider.GetListingSaleRooms(totalElements: roomsCount);

            // Act
            var roomsSummary = GetRoomSummary(rooms, new List<ListingSaleRoom>());

            // Assert
            Assert.True(roomsSummary.Fields.Count(x => x.OldValue == null) == roomsCount);
        }

        [Fact]
        public void GetRoomSummary_Old_Success()
        {
            // Arrange
            var roomsCount = 2;
            var rooms = RoomTestProvider.GetListingSaleRooms(totalElements: roomsCount);

            // Act
            var roomsSummary = GetRoomSummary(new List<ListingSaleRoom>(), rooms);

            // Assert
            Assert.True(roomsSummary.Fields.Count(x => x.NewValue == null) == roomsCount);
        }

        [Fact]
        public void GetRoomSummary_NotChanges_Success()
        {
            // Arrange
            var roomsCount = 2;
            var rooms = RoomTestProvider.GetListingSaleRooms(totalElements: roomsCount);

            // Act
            var roomsSummary = GetRoomSummary(rooms, rooms);

            // Assert
            Assert.True(roomsSummary == null);
        }

        [Fact]
        public void GetRoomSummary_Changes_Success()
        {
            // Arrange
            var newRooms = RoomTestProvider.GetListingSaleRooms(totalElements: 1);
            var equalRooms = RoomTestProvider.GetListingSaleRooms(totalElements: 2);
            var removedRooms = RoomTestProvider.GetListingSaleRooms(totalElements: 1);

            // Act
            var roomsSummary = GetRoomSummary(equalRooms.Concat(newRooms), equalRooms.Concat(removedRooms));

            // Assert
            Assert.True(roomsSummary.Fields.Count(x => x.NewValue == null) == 1);
            Assert.True(roomsSummary.Fields.Count(x => x.OldValue == null) == 1);
            Assert.True(roomsSummary.Fields.Count(x => x.OldValue != null && x.NewValue != null) == 2);
        }

        private static SummarySection GetRoomSummary(IEnumerable<ListingSaleRoom> currentRooms, IEnumerable<ListingSaleRoom> oldRooms)
        {
            // Arrange
            var listingId = Guid.NewGuid();

            var newRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid(), currentRooms);
            newRequest.ListingSaleId = listingId;
            newRequest.SaleProperty.UpdateRooms(currentRooms.ToList());

            var oldRequest = TestModelProvider.GetListingSaleRequestEntity(Guid.NewGuid(), oldRooms);
            oldRequest.ListingSaleId = listingId;
            oldRequest.SaleProperty.UpdateRooms(oldRooms.ToList());

            // Act
            var summary = newRequest.SaleProperty.GetSummarySections(oldRequest.SaleProperty);
            return summary.FirstOrDefault(x => x != null && x.Name == RoomRecord.SummarySection);
        }
    }
}
